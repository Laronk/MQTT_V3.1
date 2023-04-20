# MQTT_V3.1

This is a project for purposes of college design patterns course, which me and my friends [Krzysiek](https://github.com/CleverLord) and [Janek](https://github.com/Laronk) realized during our 5th study semester.

The goal of this project was to implement a library in C# that supports MQTT protocol. 
To ensure that our system was robust during the project implementation, we applied various design patterns, including:
- Command,
- Observer,
- Builder,
- Factory,
- Publisher-Subscriber

One of the key takeaways from the project was the importance of separating the coding and design processes.
Overall, I'm proud of the code style we achieved and the successful completion of our project.

---

# 1. Architecture:
- MqttDataStructures - In this module, we implemented our DTO classes which
all derive from abstract Message class. Messages contains information about
the packet being sent by our SrvClient or 3rd party that uses MQTT protocol.
Every message has a fixed header. Additionally some Messages have
variable header and payload. This module also contains the interfaces
IOptions and IBuilder<T> as well as their implementations.
We used those classes for testing purposes while we developed our
MqttClient.

- MessageConverter - this module, handles converting bytes to messages.
Classes from this module are used by the TransmissionManager class which
is responsible for receiving messages over tcp tunnels.

- MqttClient - This is the module of our project where we implemented
ClientCore functionalities, TransmissionManager and Command Abstraction
Layer. <br/> <br/> Additionally we can find here a packet named ClientExtension in which we
stored implementation of the client and its commands. MqttClient can be used
as an independent library. We developed it solely for testing purposes.

- MqttServer - This module implements the Broker which is responsible for
maintaining connected clients.
SvrClient derives from ClientCore and extends its functionalities by adding
server-specific behavior.
In this module there is also the TopicPublisher class, which serves as
publisher in the Observer pattern.
A few Utils classes can also be found here.

- TesterAppClient - It is just a small console app module, where we tested our
MqttClient. The aim of the project was to test developed MQTT server.

- TesterAppServer - Another small console app module, where we start our
MqttServer

# 2. Used patterns:

- Factory - We decided to use this pattern, because in future
development of this project it will help us handle commands with QoS level 1
or 2. As for now, 6 classes are derived from Command, and 2 more concrete
classes derive after them. Additional abstraction layer will be very very helpful
then.

<div id="factory" align="center">
  <img src="https://user-images.githubusercontent.com/72869986/233464905-0e142969-60fa-4df5-a11d-e91a2c142027.png" width="800" alt="Factory"/>
</div>

<br/><br/>
- Command - This is the biggest pattern in our project. Commands handle
Messages and pass the requests to the client who passes them to the broker.
It is an amazing pattern with lots of abstraction and it helped us a lot. We
implemented it first for the client side, and then later after tests with remote
mqtt broker, we implemented it on our server

<div id="command" align="center">
  <img src="https://user-images.githubusercontent.com/72869986/233464913-1ad91648-dfb2-4896-8701-66e443a4e95f.png" width="800" alt="command"/>
</div>

<br/><br/>
- Observer - we decided to implement an observer as a pattern used on the
broker side of our project. Every client can make a subscription and then it
automatically makes a topic publisher if none with specified topicFilter exist.
Each time a client publishes a message, the server informs relevant
observers. about this message. Then our observer notifies all of his
subscribers.

<div id="observer" align="center">
  <img src="https://user-images.githubusercontent.com/72869986/233465283-7690c4be-2730-4884-a5a9-60be82d1cd07.png" width="400" alt="observer"/>
</div>

---

# 3. Class diagrams

<div id="classDiagrams" align="center">
  <img src="https://user-images.githubusercontent.com/72869986/233465313-401608bd-0b81-404c-908e-e86cf3e7cec2.png" width="800" alt="classDiagrams"/>
</div>

