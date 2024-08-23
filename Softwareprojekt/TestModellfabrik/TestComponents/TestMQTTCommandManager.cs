using Modellfabrik.Components;
using NUnit.Framework;

namespace TestModellfabrik.TestComponents
{
    /// <summary>
    /// Klasse zum Testen der Funktionalität von MqttCommandManager
    /// </summary>
    [TestFixture]
    public class MqttCommandManagerTest
    {
        private MqttCommandManager _commandManager;

        [SetUp]
        public void Setup()
        {
            _commandManager = new MqttCommandManager();
        }
        /// <summary>
        /// Prüft mit Boolean, ob der Broker gestartet werden kann.
        /// </summary>
        [Test]
        public void TestStartBroker()
        {
            //Act
            _commandManager.StartBroker("test.mosquitto.org");

            //Assert
            Assert.AreEqual(true, _commandManager.IsConnectedToBroker());
        }

        /// <summary>
        /// Prüft mit Boolean, ob die Verbindung abgebrochen werden kann zum test.mosquitto.org Broker.
        /// </summary>
        [Test]
        public void TestDisconnectBroker()
        {

            //Act
            _commandManager.StartBroker("test.mosquitto.org");
            _commandManager.DisconnectBroker();

            //Assert
            Assert.AreEqual(true, _commandManager.BrokerIsDisconnected); 
        }

        /// <summary>
        /// Prüft mit Boolean, ob die Verbindung zum test.mosquitto.org Broker hergestellt werden kann.
        /// </summary>
        [Test]
        public void TestIsConnectedToBroker()
        {
            //Act  
            _commandManager.StartBroker("test.mosquitto.org");
            bool actual = _commandManager.IsConnectedToBroker();

            //Assert
            Assert.AreEqual(true, actual); 
        }

        /// <summary>
        /// Prüft mit einem String ob, die Antwort von einem abonnierten MQTT Topic mit der erwarteten Antwort übereinstimmt.
        /// </summary>
        [Test]
        public void TestFormResponse()
        {
            //Arange
            var topic = "i/bme680";
            var message = "dummymessage";
            var commandPosition = 6;
            var response = "Die Umgebungstemperatur der Modellfabrik beträgt (X) °C!";
            var expectedResponse = "Die Umgebungstemperatur der Modellfabrik beträgt (kein Messwert) °C!"; 

            //Act
            var actual = _commandManager.FormResponse(topic, message, commandPosition, response);

            //Assert
            Assert.AreEqual(expectedResponse, actual);
        }

        /// <summary>
        /// Prüft mit Boolean, ob ein Publish-Befehl über MQTT durchgeführt wird.
        /// </summary>
        [Test]
        public void TestPublishTest()
        {
            //Arange
            _commandManager.StartBroker("test.mosquitto.org");

            //Act
            _commandManager.TestPublish();

            //Assert
            Assert.AreEqual(true,_commandManager.TestPublishSent);
        }
    }
}