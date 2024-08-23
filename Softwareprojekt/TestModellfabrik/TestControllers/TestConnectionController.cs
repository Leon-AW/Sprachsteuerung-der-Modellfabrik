using Modellfabrik.Components;
using Modellfabrik.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;

namespace TestModellfabrik.TestControllers
{
    /// <summary>
    /// Klasse zum Testen der Funktionalität von ConnectionController
    /// </summary>
    [TestFixture]
    public class TestConnectionController
    {
        private ConnectionController _connectionController;
        private MqttCommandManager _commandManager;


        [SetUp]
        public void Setup()
        {
            _connectionController = new ConnectionController();
            _connectionController.ServiceClass = new Modellfabrik.Models.ServiceClassModel();
            _commandManager = _connectionController.ServiceClass.Logic.MqttCommandManager;

        }

        /// <summary>
        /// Prüft mit String die CheckInternetConnection-Methode, ob sie wie erwartet die Internet-Verbindung prüft
        /// </summary>
        [Test]
        public void TestCheckInternetConnection()
        {
            //arrange
            var expected = "true";

            //act
            var jsonResult = _connectionController.CheckInternetConnection();
            var connection = JsonConvert.SerializeObject(jsonResult.Value);
            
            //assert
            Assert.AreEqual(expected, connection);
        }

        /// <summary>
        /// Prüft mit String die IsConnectedToBroker-Methode, ob sie wie erwartet die Verbindung zum MQTT-Broker prüft
        /// </summary>
        [Test]
        public void TestIsConnectedToBroker()
        {
            //arrange
            var expected = "true";

            //acts
            _connectionController.ConnectToBroker("test.mosquitto.org");
            var jsonResult = _connectionController.IsConnectedToBroker();
            var isConnected = JsonConvert.SerializeObject(jsonResult.Value);
          
            //assert
            Assert.AreEqual(expected, isConnected);
        }

        /// <summary>
        /// Prüft mit Boolean die ConnectToBroker-Methode, ob sie die Verbindung zum Broker aufbauen kann.
        /// </summary>
        [Test]
        public void TestConnectToBroker()
        {
            //act
            _connectionController.ConnectToBroker("test.mosquitto.org");

            //assert
            Assert.AreEqual(true, _commandManager.IsConnectedToBroker());
        }

        /// <summary>
        /// Prüft mit Boolean die DisconnectBroker-Methode, ob sie die Verbindung zum Broker aufheben kann.
        /// </summary>
        [Test]
        public void TestDisconnectBroker()
        {
            //act
            _connectionController.ConnectToBroker("test.mosquitto.org");
            _connectionController.DisconnectBroker();

            //assert
            Assert.AreEqual(true, _commandManager.BrokerIsDisconnected);
        }

        /// <summary>
        /// Prüft mit Boolean die TestPublish-Methode, ob sie einen Publish-Befehl durchführen kann.
        /// </summary>
        [Test]
        public void TestOfTestPublish()
        {
            //act
            _connectionController.ConnectToBroker("test.mosquitto.org");
            _connectionController.TestPublish();

            //assert
            Assert.AreEqual(true, _commandManager.TestPublishSent);
        }
    }
}
