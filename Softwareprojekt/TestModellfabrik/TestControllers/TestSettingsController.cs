using Modellfabrik.Components;
using Modellfabrik.Controllers;
using NUnit.Framework;

namespace TestModellfabrik.TestControllers
{
    /// <summary>
    /// Klasse zum Testen der Funktionalität von SettingsController
    /// </summary>
    [TestFixture]
    public class TestSettingsController
    {
        private SettingsController _settingsController;
        private SpeechManager _speechManager;

        [SetUp]
        public void Setup()
        {
            _settingsController = new SettingsController();
            _settingsController.ServiceClass = new Modellfabrik.Models.ServiceClassModel();
            _speechManager = _settingsController.ServiceClass.Logic.SpeechManager;
        }

        /// <summary>
        /// Prüft ob, die Ausführung eines Befehls über die Manuelle Steuerung korrekt ausgeführt wird.
        /// </summary>
        [Test]
        public void TestExecuteManualCommand()
        {
            //arrange
            var expectedCommand = "Bestelle ein rotes Werkstück.";

            //act
            _settingsController.ExecuteManualCommand("Bestelle ein rotes Werkstück.");

            //assert
            Assert.AreEqual(expectedCommand, _speechManager.RecognizedText);
        }

        /// <summary>
        /// Prüft, ob die Stimme der Modellfabrik auf weiblich geändert werden kann.
        /// </summary>
        [Test]
        public void TestChangeVoiceGenderFemale()
        {
            //arrange
            var expectedVoice = "de-DE-KatjaNeural";

            //act
            _settingsController.ChangeVoiceGender("Female");

            //assert
            Assert.AreEqual(expectedVoice, _speechManager.SpeechConfig.SpeechSynthesisVoiceName);
        }

        /// <summary>
        /// Prüft, ob die Stimme der Modellfabrik auf männlich geändert werden kann.
        /// </summary>
        [Test]
        public void TestChangeVoiceGenderMale()
        {
            //arrange
            var expectedVoice = "de-DE-ConradNeural";

            //act
            _settingsController.ChangeVoiceGender("Male");

            //assert
            Assert.AreEqual(expectedVoice, _speechManager.SpeechConfig.SpeechSynthesisVoiceName);
        }

    }
}
