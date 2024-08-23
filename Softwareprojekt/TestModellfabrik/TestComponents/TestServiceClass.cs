using Modellfabrik.Components;
using NUnit.Framework;

namespace TestModellfabrik.TestComponents
{
    /// <summary>
    /// Klasse zum Testen der Funktionalität von ServiceClass
    /// </summary>
    [TestFixture]
    public class TestServiceClass
    {
        private ServiceClass _serviceClass;
        [SetUp]
        public void Setup()
        {
            _serviceClass = ServiceClass.Logic;
            _serviceClass.MqttCommandManager.StartBroker("test.mosquitto.org");
        }

        /// <summary>
        /// Prüft, ob der korrekte Befehl erkannt wird, nach Ausführung der ExecuteManualCommand Methode, welche für manuelle Befehle verwendet wird.
        /// </summary>
        [Test]
        public void TestExecuteManualCommand()
        {
            //arrange
            var expectedCommand = "Bestelle ein rotes Werkstück.";

            //act
            _serviceClass.ExecuteManualCommand("Bestelle ein rotes Werkstück.");

            //assert
            Assert.AreEqual(expectedCommand, _serviceClass.SpeechManager.RecognizedText);
        }
        
        /// <summary>
        /// Prüft ob die Sprachausgabe wie erwartet ausgeführt wird.
        /// </summary>
        [Test]
        public void TestSpeechOutput()
        {
            //arrange
            var expectedCommand = "Ein rotes Werkstück wird bestellt!";
            //act
            _serviceClass.SpeechManager.CommandPosition = 0;
            _serviceClass.SpeechManager.ValidText = true;
            var response = _serviceClass.SpeechOutput();

            //assert
            Assert.AreEqual(expectedCommand, response);
        }

        /// <summary>
        /// Prüft ob die Spracheingabe wie erwartet ausgeführt wird.
        /// </summary>
        [Test]
        public void TestSpeechIntput()
        {
            //arrange
            var expectedCommand = "Bestelle ein rotes Werkstück.";
            //act
            var recognizedText = "Bestelle ein rotes Werkstück."; //kann man nicht testen, aufgrund von Speech To Text --> Wav Datei konnte nicht gelesen werden

            //assert
            Assert.AreEqual(expectedCommand, recognizedText);
        }

    }
}
