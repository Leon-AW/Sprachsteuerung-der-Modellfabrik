using System;
using System.Collections.Generic;
using System.IO;

namespace Modellfabrik.Components
{
    /// <summary>
    /// Im Datamanager werden die benoetigten Commands bereitgestellt
    /// </summary>
    public class DataManager
    {
        /// <summary>
        /// Liste, welche die Sprachbefehle beinhaltet
        /// </summary>
        public List<string> SpeechCommands { get; } = new();
        /// <summary>
        /// Liste, welche die Topics beinhaltet
        /// </summary>
        public List<string> MqttTopics { get; } = new();

        /// <summary>
        /// Liste, welche die MQTT-Befehle beinhaltet
        /// </summary>
        public List<string> MqttMessages { get; } = new();
        /// <summary>
        /// Liste, welche die Antwortbefehle beinhaltet
        /// </summary>
        public List<string> ResponseCommands { get; } = new();

        /// <summary>
        /// Test ob die Text Datei eingelesen wurde
        /// </summary>
        public bool TestAllLinesRead;

        public DataManager()
        {
            GetCommandData("Components/Ressource-Files/Commands.txt");
        }

        /// <summary>
        /// Durch Angabe des Text-Datei-Pfads werden hier die einzelnen lines der Textdateien in die jeweiligen Listen gespeichert
        /// </summary>
        private void GetCommandData(string commandFilePath)
        {
             
            var lines = ReadTextFile(commandFilePath); //einlesen der Befehle

            for (int i = 0; i < lines.Length; i+=5) //Die Commands.txt ist so aufgebaut:
            {
                SpeechCommands.Add(lines[i]);       // 1. Line: Sprachbefehl (i = 0 + 5n)
                MqttTopics.Add(lines[i +1]);        // 2. Line: MQTT Topic   (i = 1 + 5n)
                MqttMessages.Add(lines[i+2]);       // 3. Line: MQTT Message (i = 2 + 5n)
                ResponseCommands.Add(lines[i +3]);  // 4. Line: Antwort      (i = 3 + 5n)
                                                    // 5. Line: Leerzeile    (wird nicht weiterverarbeitet)
            }
        }

        /// <summary>
        /// Liest Textfiles aus
        /// </summary>
        /// <param name="path">Der Pfad der Datei</param>
        /// <returns>Ein Array, wo ein Element gleich einer Line der Textdatei entspricht.</returns>
        private string[] ReadTextFile(string path)
        {
            try
            {
                var lines = File.ReadAllLines(path); //liest alle Lines der txt
                TestAllLinesRead = true;
                return lines;
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Exception: " + e.Message);
                TestAllLinesRead = false;
                return Array.Empty<string>();
            }

        }
    }
}
