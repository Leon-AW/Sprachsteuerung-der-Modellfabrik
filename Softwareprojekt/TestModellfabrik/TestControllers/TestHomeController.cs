using Modellfabrik.Components;
using Modellfabrik.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;

namespace TestModellfabrik.TestControllers
{
    /// <summary>
    /// Klasse zum Testen der Funktionalität von HomeController
    /// </summary>
    [TestFixture]
    public class TestHomeController
    {
        private HomeController _homeController;
        private SpeechManager _speechManager;

        [SetUp]
        public void Setup()
        {
            _homeController = new HomeController();
            _homeController.ServiceClass = new Modellfabrik.Models.ServiceClassModel();
            _homeController.ServiceClass.Logic.MqttCommandManager.StartBroker("test.mosquitto.org");
            _speechManager = _homeController.ServiceClass.Logic.SpeechManager;
        }

        /// <summary>
        /// Prüft mit Boolean die ActivateKeyRecognition-Methode, ob Sie wie erwartet auf die Schlüsselworterkennung wartet und hört. 
        /// </summary>
        [Test]
        public void TestActivateKeyRecognition()
        {
            //arrange

            //act
            _ = _homeController.ActivateKeyRecognition(true);

            //assert
            Assert.AreEqual(true, _speechManager.KeyRecognitionStarted);
        }

        /// <summary>
        /// Die CallSpeechInput-Methode wird mit Boolean auf korrekte Ausführung nach der Schlüsselworterkennung geprüft
        /// </summary>
        [Test]
        public void TestCallSpeechInput()
        {
            //act
            _ = _homeController.CallSpeechInput();

            //assert
            Assert.AreEqual(true, _homeController.TestOfInput);
        }

        /// <summary>
        /// Die CallSpeechOutput-Methode wird mit Boolean auf korrekte Ausführung nach der Schlüsselworterkennung geprüft
        /// </summary>
        [Test]
        public void TestCallSpeechOutput()
        {
            //arrange
            var expectedCommand = "Ein rotes Werkstück wird bestellt!";

            //act
            _speechManager.CommandPosition = 0;
            _speechManager.ValidText = true;
            var jsonResult = _homeController.CallSpeechOutput();
            var jsonValue = JsonConvert.SerializeObject(jsonResult.Value);
            dynamic json = JObject.Parse(jsonValue); 
            var outputText = json.getText.ToString();

            //assert
            Assert.AreEqual(expectedCommand, outputText);
        }

        /// <summary>
        /// Prüft mit Boolean, ob der Sprachverlauf korrekt in TempData gespeichert wird.
        /// </summary>
        [Test]
        public void TestGetHistory()
        {
            //act          
            _ = _homeController.GetHistory();

            //assert
            Assert.AreEqual(true, _homeController.TestOfHistory);
        }

        /// <summary>
        /// Prüft mit Boolean, ob der Status von einem korrekten Text wiedergegeben und als gültig erkannt wird.
        /// </summary>
        [Test]
        public void TestIsTextValidTrue()
        {
            //arrange
            var expected = "true";

            //act
            _speechManager.ValidText = true;
            var jsonResult = _homeController.IsTextValid();
            var actual = JsonConvert.SerializeObject(jsonResult.Value);

            //assert 
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Prüft mit Boolean, ob der Status von einem falschen Text wiedergegeben und als inkorrekt erkannt wird.
        /// </summary>
        [Test]
        public void TestIsTextValidFalse()
        {
            //arrange
            var expected = "false";

            //act
            _speechManager.ValidText = false;
            var jsonResult = _homeController.IsTextValid();
            var actual = JsonConvert.SerializeObject(jsonResult.Value);

            //assert 
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Prüft mit Boolean, ob die Methode zur Zeitmesseung der Spracheingabe ausgeführt wird.
        /// </summary>
        [Test]
        public void TestGetTime()
        {
            //act
            _ = _homeController.GetTime();

            //assert
            Assert.AreEqual(true, _homeController.TestOfTime);
        }

    }
}
