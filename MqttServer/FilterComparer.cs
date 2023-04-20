namespace MqttServer;

public static class FilterComparer
{
    private const char LevelSeparator = '/';
    private const char MultiLevelWildcard = '#';
    private const char SingleLevelWildcard = '+';
    private const char ReservedTopicPrefix = '$';

    private static int _filterOffset;
    private static int _filterLength;
    private static int _topicOffset;
    private static int _topicLength;
    private static List<char> _topicPointer;
    private static List<char> _filterPointer;
    private static bool _isMultiLevelFilter;

    // Standard comparing
    private static bool IsFilterLongerThanTopic()
    {
        if (_filterLength <= _topicLength) return false;

        // It is impossible to create a filter which is longer than the actual topic.
        // The only way this can happen is when the last char is a wildcard char.
        // sensor/7/temperature >> sensor/7/temperature = Equal
        // sensor/+/temperature >> sensor/7/temperature = Equal
        // sensor/7/+           >> sensor/7/temperature = Shorter
        // sensor/#             >> sensor/7/temperature = Shorter
        var lastFilterChar = _filterPointer[_filterLength - 1];

        return lastFilterChar != MultiLevelWildcard && lastFilterChar != SingleLevelWildcard;
    }

    private static bool IsReservedTopic()
    {
        bool isReservedTopic = _topicPointer[0] == ReservedTopicPrefix;

        switch (isReservedTopic)
        {
            case true when _filterLength == 1 && _isMultiLevelFilter:
            // It is not allowed to receive i.e. '$SYS/monitor/Clients' with filter '+/monitor/Clients'.
            case true when _filterPointer[0] == SingleLevelWildcard:
                // It is not allowed to receive i.e. '$foo/bar' with filter '#'.
                return true;
            default:
                return false;
        }
    }

    private static bool IsProFilter()
    {
        return _filterLength == 1 && _isMultiLevelFilter;
        // Filter '#' matches basically everything.
    }

    // Comparing when on same place in list
    private static bool IfInvalidMultiLevel()
    {
        // Check if the current char is a multi level wildcard. The char is only allowed
        // at the very last position.
        return _filterPointer[_filterOffset] == MultiLevelWildcard && _filterOffset != _filterLength - 1;
    }

    private static bool CompareShortHash()
    {
        if (_topicOffset != _topicLength - 1) return false;
        // Check for e.g. "foo" matching "foo/#"
        if (_filterOffset == _filterLength - 3 &&
            _filterPointer[_filterOffset + 1] == LevelSeparator &&
            _isMultiLevelFilter)
        {
            return true;
        }

        // Check for e.g. "foo/" matching "foo/#"
        if (_filterOffset == _filterLength - 2 &&
            _filterPointer[_filterOffset] == LevelSeparator &&
            _isMultiLevelFilter)
        {
            return true;
        }

        return false;
    }

    private static FilterCompareResult? CompareLastWord()
    {
        if (_topicOffset != _topicLength ||
            _filterOffset != _filterLength - 1 ||
            _filterPointer[_filterOffset] != SingleLevelWildcard) return null;

        if (_filterOffset > 0 && _filterPointer[_filterOffset - 1] != LevelSeparator)
        {
            return FilterCompareResult.FilterInvalid;
        }

        return FilterCompareResult.IsMatch;
    }

    private static FilterCompareResult? CompareExact()
    {
        if (CompareShortHash()) return FilterCompareResult.IsMatch;

        _filterOffset++;
        _topicOffset++;

        // Check if the end was reached and i.e. "foo/bar" matches "foo/bar"
        if (_filterOffset == _filterLength && _topicOffset == _topicLength) return FilterCompareResult.IsMatch;

        if (CompareLastWord() is { } lastWordResult) return lastWordResult;

        return null;
    }

    // Comparing when not necessary on same place in list
    private static FilterCompareResult? CompareNotExact()
    {
        switch (_filterPointer[_filterOffset])
        {
            // Check for invalid "+foo" or "a/+foo" subscription
            case SingleLevelWildcard
                when _filterOffset > 0 && _filterPointer[_filterOffset - 1] != LevelSeparator:
            // Check for bad "foo+" or "foo+/a" subscription
            case SingleLevelWildcard
                when _filterOffset < _filterLength - 1 && _filterPointer[_filterOffset + 1] != LevelSeparator:
                return FilterCompareResult.FilterInvalid;
            case SingleLevelWildcard:
            {
                _filterOffset++;
                while (_topicOffset < _topicLength && _topicPointer[_topicOffset] != LevelSeparator) _topicOffset++;

                if (_topicOffset == _topicLength && _filterOffset == _filterLength) return FilterCompareResult.IsMatch;

                break;
            }
            case MultiLevelWildcard
                when _filterOffset > 0 && _filterPointer[_filterOffset - 1] != LevelSeparator:
            case MultiLevelWildcard 
                when _filterOffset + 1 != _filterLength:
                return FilterCompareResult.FilterInvalid;
            case MultiLevelWildcard:
                return FilterCompareResult.IsMatch;
            default:
            {
                // Check for e.g. "foo/bar" matching "foo/+/#".
                if (_filterOffset > 0 && _filterOffset + 2 == _filterLength && _topicOffset == _topicLength &&
                    _filterPointer[_filterOffset - 1] == SingleLevelWildcard &&
                    _filterPointer[_filterOffset] == LevelSeparator && _isMultiLevelFilter)
                {
                    return FilterCompareResult.IsMatch;
                }

                return FilterCompareResult.NoMatch;
            }
        }

        return null;
    }

    // Main comparing method
    public static FilterCompareResult Compare(string topic, string filter)
    {
        if (string.IsNullOrEmpty(topic)) return FilterCompareResult.TopicInvalid;

        if (string.IsNullOrEmpty(filter)) return FilterCompareResult.FilterInvalid;

        _filterOffset = 0;
        _filterLength = filter.Length;
        _topicOffset = 0;
        _topicLength = topic.Length;
        _topicPointer = new List<char>(topic);
        _filterPointer = new List<char>(filter);
        _isMultiLevelFilter = _filterPointer[_filterLength - 1] == MultiLevelWildcard;

        if (IsFilterLongerThanTopic()) return FilterCompareResult.NoMatch;
        if (IsReservedTopic()) return FilterCompareResult.NoMatch;
        if (IsProFilter()) return FilterCompareResult.IsMatch;

        // Go through the filter char by char.
        while (_filterOffset < _filterLength && _topicOffset < _topicLength)
        {
            if (IfInvalidMultiLevel()) return FilterCompareResult.FilterInvalid;
            
            if (_filterPointer[_filterOffset] == _topicPointer[_topicOffset])
            {
                if (CompareExact() is { } resultExact) return resultExact;
            }
            else
            {
                if (CompareNotExact() is { } resultNotExact) return resultNotExact;
            }
        }

        return FilterCompareResult.NoMatch;
    }
}