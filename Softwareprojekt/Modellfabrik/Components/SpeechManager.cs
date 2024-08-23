using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace Modellfabrik.Components
{
    /// <summary>
    /// Regelt die Umwandlung von Text zu Sprache und umgekehrt und prüft die Eingabe durch eine Textvalidierung
    /// </summary>
    public class SpeechManager
    {
        /// <summary>
        /// Erkannter Text
        /// </summary>
        public string RecognizedText { get; set; }

        /// <summary>
        /// Ob der Text ein korrekter Befehl ist
        /// </summary>
        public bool ValidText { get; set; }

        /// <summary>
        /// Der Index an welcher der Speechcommand in der Liste steht
        /// </summary>
        public int CommandPosition { get; set; }

        /// <summary>
        /// Antwort Text
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Speichert den Sprachbverlauf zwischen Factory und dem Benutzer
        /// </summary>
        public string SpeechHistory { get; set; }
        /// <summary>
        /// Speichert die SpeechConfig öffentlich verfügbar
        /// </summary>
        public SpeechConfig SpeechConfig;

        /// <summary>
        /// Beinhaltet die vordefinierten Sprachkommandos aus dem DataManager
        /// </summary>
        private readonly List<string> _speechCommands;

        /// <summary>
        /// Speichert die SpeechConfig
        /// </summary>
        private SpeechConfig _speechConfig;

        /// <summary>
        /// Audioeinstellung (Mikrofonauswahl)
        /// </summary>
        private AudioConfig _audioConfig;

        /// <summary>
        /// Beinhaltet Information über das Schlüsselwort
        /// </summary>
        private KeywordRecognitionModel _keywordModel;

        /// <summary>
        /// Speichert den KeywordRecognizer
        /// </summary>
        private KeywordRecognizer _keywordRecognizer;

        /// <summary>
        /// Speichert die gestoppte Zeit für die Verarbeitung eines Befehls
        /// </summary>
        private readonly Stopwatch _stopwatch = new();

        /// <summary>
        /// Testvariable für den Unittest, ob in die Methode rein gegeangen wird
        /// </summary>
        public bool KeyRecognitionStarted;

        /// <summary>
        /// Testvariable für den Unittest, ob in die Methode rein gegeangen wird
        /// </summary>
        public bool StopKeyRecognitionStarted;

        /// <summary>
        /// Testvariable für den Unittest, ob in die Methode rein gegeangen wird
        /// </summary>
        public bool SpeechToTextStarted;

        /// <summary>
        /// Testvariable für den Unittest, ob in die Methode rein gegeangen wird
        /// </summary>
        public bool TextToSpeechStarted;

        /// <summary>
        /// Initialisiert die Komponenten des SpeechManagers
        /// </summary>
        /// <param name="speechCommands">Spracheingabe Kommandos</param>
        public SpeechManager(List<string> speechCommands)
        {
            _speechCommands = speechCommands;
            SetUp();
        }

        /// <summary>
        /// Bereitet den Speechmanager vor
        /// </summary>
        private void SetUp()
        {
            CreateSpeechConfig();
            CreateKeyWordModel("Components/Ressource-Files/factory.table");
        }


        /// <summary>
        /// Erstellt die Config (Audio, Keyword, Speech) für den SpeechService
        /// </summary>
        private void CreateSpeechConfig()
        {
            _speechConfig = SpeechConfig.FromSubscription("28f37f3d177d4417a8817e660f6bc7ef", "eastus");
            _speechConfig.SpeechRecognitionLanguage = "de-DE";
            _speechConfig.SpeechSynthesisLanguage = "de-DE";
            _speechConfig.SpeechSynthesisVoiceName = "de-DE-KatjaNeural";
            SpeechConfig = _speechConfig;
            CreateKeyWordRecognizer();
        }

        /// <summary>
        /// Erstellt den KeywordRecognizer anhand der Audioconfig
        /// </summary>
        private void CreateKeyWordRecognizer()
        {
            try
            {
                using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
                _audioConfig = audioConfig;
                _keywordRecognizer = new KeywordRecognizer(_audioConfig);
            }
            catch (Exception)
            {
                Console.WriteLine(@"Kein Audioinputgerät gefunden");
            }

        }

        /// <summary>
        /// Wechselt die Stimme (Female, Male)
        /// </summary>
        /// <param name="voiceName"></param>
        public void ChangeSynthesisVoice(string voiceName)
        {
            _speechConfig.SpeechSynthesisVoiceName = voiceName;
        }

        /// <summary>
        /// Lädt das Keywordmodell (für die Schlüsselworterkennung)
        /// </summary>
        private void CreateKeyWordModel(string path)
        {
            _keywordModel = KeywordRecognitionModel.FromFile(path);
        }

        /// <summary>
        /// Schlüsselworterkennung
        /// </summary>
        /// <returns>Task</returns>
        public async Task StartKeyRecognition()
        {
            try
            {
                KeyRecognitionStarted = true;
                await _keywordRecognizer.RecognizeOnceAsync(_keywordModel);
            }
            catch (Exception)
            {
                Console.WriteLine(@"Konnte das Schlüsselworterkennen nicht starten!");
            }
        }

        /// <summary>
        /// Stoppt Schlüsselworterkennung
        /// </summary>
        /// <returns>Task</returns>
        public async Task StopKeyRecognition()
        {
            try
            {
                StopKeyRecognitionStarted = true;
                await _keywordRecognizer.StopRecognitionAsync();
            }
            catch (Exception)
            {
                Console.WriteLine(@"Konnte das Schlüsselworterkennen nicht stoppen!");
                throw;
            }
        }

        /// <summary>
        /// Wandelt Sprache in Text um
        /// </summary>
        /// <returns>Task</returns>
        public async Task<string> SpeechToText()
        {
            Console.WriteLine(@"Sprechen Sie ins Mikrofon:");

            try
            {
                SpeechToTextStarted = true;
                using var recognizer = new SpeechRecognizer(_speechConfig, _audioConfig);
                var result = await recognizer.RecognizeOnceAsync();

                _stopwatch.Start(); //Stoppuhr wird gestartet
                return result.Text;
            }
            catch (Exception)
            {
                Console.WriteLine(@"Speech-To-Text fehlgeschlagen!");
                return "";
            }
        }

        /// <summary>
        /// Validiert den Text anhand der vorhanden Sprachbefehle
        /// </summary>
        /// <param name="recognizedText">Eingabetext</param>
        public void ValidateText(string recognizedText)
        {
            RecognizedText = recognizedText;
            Console.WriteLine(@"Erkannt wurde: " + recognizedText);
            for (var index = 0; index < _speechCommands.Count; index++) //Die SpeechCommands werden durchlaufen
            {
                var speechCommand = _speechCommands[index];
                if (Similar(ToLowerAndTrim(recognizedText), ToLowerAndTrim(speechCommand))) //Überprüft ob recognizedText und speechCommand ähnlich sind
                {
                    CommandPosition = index;    //Befehlsindex wird sich gemerkt
                    ValidText = true;           //Text ist Valide
                    return;                     //zurück
                }
            }
            ValidText = false; //Wenn keine Ähnlichkeiten gefunden
        }

        /// <summary>
        /// Wandelt Text in Sprache um
        /// </summary>
        public async Task TextToSpeech(string response)
        {
            if (!ValidText)                                                     // ... wenn der ValidText "false" ist, dann ist die Rückanwort folgendes:
                response = "Entschuldigung, ich habe es nicht verstanden.";

            Response = response;
            FormSpeechHistory();
            Console.WriteLine(@"Antwort: " + response);
            _stopwatch.Stop();  //Stoppuhr wird gestoppt

            TextToSpeechStarted = true;
            using var synthesizer = new SpeechSynthesizer(_speechConfig);
            await synthesizer.SpeakTextAsync(response);
        }

        /// <summary>
        /// Formt den Sprachverlauf
        /// </summary>
        private void FormSpeechHistory()
        {
            SpeechHistory = "Factory: " + Response + "\n\n" + SpeechHistory;
            SpeechHistory = "Ich: " + RecognizedText + "\n" + SpeechHistory;
        }

        /// <summary>
        /// Berechnet die Zeit vom Gesprochenen bis zur Antwort
        /// </summary>
        public float TrackTime()
        {
            var timeSpan = MathF.Round(_stopwatch.ElapsedMilliseconds / 1000f, 2);
            _stopwatch.Reset(); //Stoppuhr wird resetet
            return timeSpan;
        }


        /// <summary>
        /// Überprüft ob der erkannte Text ähnlich zum erwarteten Text ist
        /// </summary>
        /// <param name="scannedSource">erkannter Text</param>
        /// <param name="expectedSource">erwarteter Text</param>
        /// <returns>Ob es ähnlich ist</returns>
        private bool Similar(string scannedSource, string expectedSource)
        {
            var textDistance = CalculateDistance(scannedSource, expectedSource) <= 3; //Unterschied der beiden Text darf nicht mehr als 3 sein
            var contains = scannedSource.Contains(expectedSource); //scannedSource enthält expectedSource?

            return textDistance || contains; //Unterschied <= 3 oder enthält
        }

        /// <summary>
        /// Macht alle Buchstaben klein und entfernt "!" "?" "," "."
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>bearbeiteter Text</returns>
        private static string ToLowerAndTrim(string text)
        {
            char[] trimChars = { '?', '.', ',', '!' };
            return text.ToLower().Trim(trimChars);
        }

        /// <summary>
        ///  Vorerst der folgende Code dieser Methode wurde aus folgendener Quelle entnommen bzw. angeguckt:
        /// https://gist.github.com/Davidblkx/e12ab0bb2aff7fd8072632b396538560
        /// 
        /// Mit der CalculateDistance Methode wird die Differenz bzw. Unterschied zwischen zwei string verglichen.
        /// Dies geschieht mit hilfe der „Levenshtein Distanz“. Dies ist ein Algorithmus der char für char die jeweiligen
        /// beiden string durchgeht und guckt, wie sie sich jeweils unterscheiden.
        /// Am Ende erhält man eine Distanz (Zahl), die dafür steht wie viele Operationen man benötigt um den einen string wie den anderen darstellen zulassen.
        /// Diese sind replace, delete ode insert
        /// </summary>
        /// <param name="scannedSource">Eingabetext</param>
        /// <param name="expectedSource">Zieltext</param>
        /// <returns>Minimale Anzahl an Operationen die benoetigt werden um vom Eingabetext auf den Zieltext zukomme mithilge von Entfernen, Hinzufügen oder ersetzen von Zeichen, </returns>
        private int CalculateDistance(string scannedSource, string expectedSource)
        {

            var scannedSourceLength = scannedSource.Length;
            var expectedSourceLength = expectedSource.Length;

            var matrix = new int[scannedSourceLength + 1, expectedSourceLength + 1];

            // Überprüft ob der erste string leer ist, wenn ja ist die Distanz die länge des zweiten strings.
            if (scannedSourceLength == 0)
                return expectedSourceLength;

            // Überprüft ob der zweite string leer ist, wenn ja ist die Distanz die länge des ersten strings.
            if (expectedSourceLength == 0)
                return scannedSourceLength;


            // Initialisierung  der Zeilenanzahl des ersten string in der Matrix. Jedes char des stings, ist dabei eine Zeile.
            for (var row = 0; row <= scannedSourceLength; row++)
            {
                matrix[row, 0] = row;
            }
            // Initialisierung  der Spaltenanzahl des zweiten string in der Matrix. Jedes char des stings, ist dabei eine Sapalte.
            for (var column = 0; column <= expectedSourceLength; column++)
            {
                matrix[0, column] = column;
            }

            // Hier wird die Distanz berechnet bzw. rausgefunden d.h. hier beginnt die eigetnliche Suche nach den Unterschieden.
            // Dabei wird immer das Minimum innerhalb der temporären/ aktuellen matrix gesucht. Dies geschieht bis man an der Stelle n x m ankommt 
            for (var i = 1; i <= scannedSourceLength; i++)
            {
                for (var j = 1; j <= expectedSourceLength; j++)
                {
                    int substitutionCount;
                    if (expectedSource[j - 1] == scannedSource[i - 1])      // Unterschied der zwei chars, wenn die gleich sind,
                    {
                        substitutionCount = 0;                              // dann ist die Differenz 0
                    }
                    else
                    {
                        substitutionCount = 1;                              // ansonsten 1.
                    }

                    matrix[i, j] = Math.Min(Math.Min(matrix[i - 1, j] + 1,                          // delete
                                                     matrix[i, j - 1] + 1),                         // insert
                                                     matrix[i - 1, j - 1] + substitutionCount);     // replace

                }
            }
            // Gibt die Distanz wieder zurück.
            return matrix[scannedSourceLength, expectedSourceLength];
        }
    }
}
