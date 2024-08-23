using System;
using System.Threading;
using System.Threading.Tasks;

namespace Modellfabrik.Components
{
    /// <summary>
    /// Die Service-Class enthaelt die Programmlogik für die Bedienung der Sprachsteuerung
    /// </summary>
    public sealed class ServiceClass
    {
        /// <summary>
        /// Objekt vom Typ DataManager
        /// </summary>
        private readonly DataManager _dataManager;

        /// <summary>
        /// Objekt vom Typ Speechmanager
        /// </summary>
        public readonly SpeechManager SpeechManager;

        /// <summary>
        /// Objekt vom Typ MqttCommandManager
        /// </summary>
        public readonly MqttCommandManager MqttCommandManager;

        /// <summary>
        /// Ob ein (manuelle Steuerung) Button gedrückt wurde
        /// </summary>
        private bool _manualButtonPressed;

        /// <summary>
        /// privates Feld (Singleton)
        /// </summary>
        private static readonly Lazy<ServiceClass> LogicClass = new(() => new ServiceClass());

        /// <summary>
        /// Singleton Entwurfsmuster
        /// </summary>
        public static ServiceClass Logic => LogicClass.Value;

        /// <summary>
        /// Objekt vom Typ OnOffLED
        /// </summary>
        private readonly OnOffLed _onOffLed;

        /// <summary>
        /// Das rote LED ist auf PIN 13 installiert
        /// </summary>
        private const int RedLedPin = 13;

        /// <summary>
        /// Das grüne LED ist auf PIN 6 installiert
        /// </summary>
        private const int GreenLedPin = 6;

        /// <summary>
        /// Das blaue LED ist auf PIN 26 installiert
        /// </summary>
        private const int BlueLedPin = 26;


        /// <summary>
        /// Im Konstruktor werden die Objekte instanziiert
        /// </summary>
        private ServiceClass()
        {
            _dataManager = new DataManager();
            SpeechManager = new SpeechManager(_dataManager.SpeechCommands);
            MqttCommandManager = new MqttCommandManager();
            _onOffLed = new OnOffLed();
        }


        /// <summary>
        /// Wird ausgeführt, wenn ein Button der manuellen Steuerung gedrückt wird
        /// </summary>
        /// <param name="manualCommand">Manueller Sprachbefehl</param>
        /// <returns>Task</returns>
        public async void ExecuteManualCommand(string manualCommand)
        {
            if (!_manualButtonPressed) //Wenn ein Button der manuellen Steurung nicht gedrückt wurde
            {
                _manualButtonPressed = true; //manueller Button wurde gedrückt
                SpeechManager.ValidateText(manualCommand); //Befehl wird validiert, damit er in SpeechManager gespeichert wird
                var response = ExecuteMqttCommand();
                await SpeechManager.TextToSpeech(response);
                _manualButtonPressed = false; //manueller Button wird "losgelassen"
            }
        }

        /// <summary>
        /// Sendet, Empfängt den MQTT befehl wandelt ihn in Text und dann in Sprache um (Aufruf der jeweiligen Methoden)
        /// </summary>
        /// <returns>Task</returns>
        private string ExecuteMqttCommand()
        {
            var response = "";
            if (SpeechManager.ValidText) //Wenn der Text valide ist...
            {
                var commandPosition = SpeechManager.CommandPosition; //...Befehlsindex
                var topic = _dataManager.MqttTopics[commandPosition]; //...MQTT topic
                var message = _dataManager.MqttMessages[commandPosition]; //...MQTT message
                response = _dataManager.ResponseCommands[commandPosition]; //...Antwort aus .txt-Datei

                response = MqttCommandManager.FormResponse(topic, message, commandPosition, response); //Antwort mit Information

            }

            return response;
        }

        /// <summary>
        /// Wird aufgerufen um einen Sprachbefehl auszusprechen
        /// </summary>
        /// <returns>Task</returns>
        public string SpeechOutput()
        {
            var response = ExecuteMqttCommand();
            Parallel.Invoke(
                LedFeedback,
                () => _ = SpeechManager.TextToSpeech(response)); //LED-Feedback und Sprachausgabe werden Parallel gestartet
            Console.WriteLine(response);
            return SpeechManager.Response;
        }

        /// <summary>
        /// Wird aufgerufen um einen Sprachbefehl einzusprechen
        /// </summary>
        /// <returns>Task</returns>
        public async Task<string> SpeechInput()
        {
            _onOffLed.TurnOnLED(BlueLedPin);
            var recognizedText = await SpeechManager.SpeechToText();
            SpeechManager.ValidateText(recognizedText);
            return recognizedText;
        }

        /// <summary>
        /// Gibt das Feedback ueber die LEDs wieder
        /// </summary>
        private void LedFeedback()
        {
            _onOffLed.TurnOffLED(BlueLedPin);
           
            if (SpeechManager.ValidText) //Wenn Text valide...
            {
                _onOffLed.TurnOnLED(GreenLedPin); //dann grünes Licht
                Thread.Sleep(3500);
                _onOffLed.TurnOffLED(GreenLedPin);
            }
            else
            {
                _onOffLed.TurnOnLED(RedLedPin);//sonst rotes Licht
                Thread.Sleep(2500);
                _onOffLed.TurnOffLED(RedLedPin);
            }
        }

    }
}
