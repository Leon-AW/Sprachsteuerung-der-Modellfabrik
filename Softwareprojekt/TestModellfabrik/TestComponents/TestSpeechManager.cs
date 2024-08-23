using System;
using Modellfabrik.Components;
using NUnit.Framework;

namespace TestModellfabrik.TestComponents
{
    /// <summary>
    /// Klasse zum Testen der Funktionalität von SpeechManager
    /// </summary>
    [TestFixture]
    public class TestSpeechManager
    {
        private SpeechManager _speechManager;
        private DataManager _dataManager;

        [SetUp]
        public void Setup()
        {
            _dataManager = new DataManager();
            _speechManager = new SpeechManager(_dataManager.SpeechCommands);
        }

        /// <summary>
        /// Prüft mit einem String, ob die Methode zur Änderung der Stimme wie erwartet auf weiblich geändert wird.
        /// </summary>
        [Test]
        public void TestChangeSynthesisVoiceToFemale()
        {
            Console.WriteLine(Environment.CurrentDirectory);
            //arrange
            var expected = "de-DE-KatjaNeural";

            //act
            _speechManager.ChangeSynthesisVoice("de-DE-KatjaNeural");

            //assert
            Assert.AreEqual(expected, _speechManager.SpeechConfig.SpeechSynthesisVoiceName);
        }

        /// <summary>
        /// Prüft mit einem String, ob die Methode zur Änderung der Stimme wie erwartet auf männlich geändert wird.
        /// </summary>
        [Test]
        public void TestChangeSynthesisVoiceToMale()
        {
            //arrange
            var expected = "de-DE-ConradNeural";

            //act
            _speechManager.ChangeSynthesisVoice("de-DE-ConradNeural");

            //assert
            Assert.AreEqual(expected, _speechManager.SpeechConfig.SpeechSynthesisVoiceName);
        }

        /// <summary>
        /// Prüft die StartKeyRecognition mit einem Boolean, allerdings
        /// kann man StartKeyRecognition nicht testen, da _keywordRecognizer beim Testlauf nicht gestartet wird
        /// </summary>
        [Test]
        public void TestStartKeyRecognition()
        {
            //act
            _= _speechManager.StartKeyRecognition();
            
            //assert
            Assert.AreEqual(true, _speechManager.KeyRecognitionStarted);
        }
        /// <summary>
        /// Prüft die StopKeyRecognition mit einem Boolean, allerdings
        /// kann StopKeyRecognition man nicht testen, da _keywordRecognizer beim Testlauf nicht gestartet wird
        /// </summary>
        [Test]
        public void TestStopKeyRecognition()
        {
            //act
            _ =_speechManager.StopKeyRecognition();

            //assert
            Assert.AreEqual(true, _speechManager.StopKeyRecognitionStarted);
        }
        
        /// <summary>
        /// Prüft mit Boolean, ob die ValidateText Methode, wie erwartet einen korrekten Befehl als valide erkennt.
        /// </summary>
        [Test]
        public void TestValidateTextTrue()
        {
            //act
            _speechManager.ValidateText("Bestelle ein rotes Werkstück.");

            //assert
            Assert.AreEqual(true, _speechManager.ValidText);
        }

        /// <summary>
        /// Prüft mit Boolean, ob die ValidateText Methode, wie erwartet einen falschen Befehl als Falsches erkennt.
        /// </summary>
        [Test]
        public void TestValidateTextFalse()
        {
            //act
            _speechManager.ValidateText("Falscher Befehl");

            //assert
            Assert.AreEqual(false, _speechManager.ValidText);
        }

        /// <summary>
        /// Prüft mit Boolean, ob die TextToSpeech Methode einen String/Text zu einem Sprachbefehl umwandeln kann.
        /// </summary>
        [Test]
        public void TestTextToSpeech()
        {
            //act
            _ = _speechManager.TextToSpeech(_speechManager.Response);

            //assert
            Assert.AreEqual(true, _speechManager.TextToSpeechStarted);
        }

        /// <summary>
        /// Prüft, ob die Zeit der TrackTime-Methode korrekt funktioniert und gemessen wird.
        /// </summary>
        [Test]
        public void TestTrackTime()
        {
            //arrange
            var expectedTimeSpan = 0.0f;

            //act
            var trackedTime = _speechManager.TrackTime();

            //assert
            Assert.AreEqual(expectedTimeSpan, trackedTime);
        }
    }
}
