using System;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using uPLibrary.Networking.M2Mqtt.Messages;
using MqttClient = uPLibrary.Networking.M2Mqtt.MqttClient;


namespace Modellfabrik.Components
{
    /// <summary>
    /// MqttCommand-Class
    /// </summary>
    public class MqttCommandManager
    {
        /// <summary>
        /// Temperatur
        /// </summary>
        private string _temperature = "(kein Messwert)";
        /// <summary>
        /// Luftdruck
        /// </summary>
        private string _pressure = "(kein Messwert)";
        /// <summary>
        /// Weiße Werkstücke
        /// </summary>
        private string _whiteWorkpieceCount = "(keine Angabe)";
        /// <summary>
        /// Rote Werkstücke
        /// </summary>
        private string _redWorkpieceCount = "(keine Angabe)";
        /// <summary>
        /// Blaue Werkstücke
        /// </summary>
        private string _blueWorkpieceCount = "(keine Angabe)";
        /// <summary>
        /// Position des Werkstücks
        /// </summary>
        private string _orderPosition = "(keine Angabe)";
        /// <summary>
        /// Ob eine Bestellung unterwegs ist
        /// </summary>
        private string _orderState = "init";
        /// <summary>
        /// Ob eine Bestellung unterwegs ist
        /// </summary>
        private string _orderedWorkpieceColor;
        /// <summary>
        /// Ob eine Bestellung unterwegs ist
        /// </summary>
        private string _workpieceLocation;
        /// <summary>
        /// Mqtt Client
        /// </summary>
        private MqttClient _client;
        
        /// <summary>
        /// Der Broker ist nicht verbunden (Unittest)
        /// </summary>
        public bool BrokerIsDisconnected;

        /// <summary>
        /// Für den Testpublish (Unittest)
        /// </summary>
        public bool TestPublishSent;

        /// <summary>
        /// Versucht Verbindung zum Broker herzustellen und Subscribed Topics
        /// </summary>
        public void StartBroker(string brokerIp)
        {
            try
            {
                ConnectToBroker(brokerIp); //In der Modellfabrikumgebung ist der brokerHostName: 192.168.0.10
                SubscribeTopics();
            }
            catch (Exception)
            {
                Console.WriteLine(@"Konnte sich nicht mit Broker verbinden " + brokerIp); //Wenn keine Verbindung möglich ist
            }
        }

        /// <summary>
        /// Prüft ob eine Verbindung zum Broker besteht
        /// </summary>
        /// <returns>Verbindung zum Broker</returns>
        public bool IsConnectedToBroker()
        {
            try
            {
                return _client is { IsConnected: true };
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Versucht die Verbindung zum Broker zu entkoppeln
        /// </summary>
        public void DisconnectBroker()
        {
            try
            {
                _client.Disconnect();
                BrokerIsDisconnected = true;
            }
            catch (Exception)
            {
                Console.WriteLine(@"Disconnect fehlgeschlagen!");
            }
        }

        /// <summary>
        /// Verbindet sich mit Broker
        /// <param name="brokerIp">broker IP</param>
        /// </summary>
        private void ConnectToBroker(string brokerIp)
        {
            _client = new MqttClient(brokerIp);
            var clientId = Guid.NewGuid().ToString();
            _client.Connect(clientId); //verbindet sich mit Broker
        }

        /// <summary>
        /// Subsribed alle gebrauchten Topics
        /// </summary>
        private void SubscribeTopics()
        {
            _client.MqttMsgPublishReceived += client_MqttPublishReceived; //Event wird registriert (wenn published message empfangen)
            Subscribe("i/bme680"); //Topics subscriben
            Subscribe("f/i/stock");

            Subscribe("f/i/state/sld");
            Subscribe("f/i/state/mpo");
            Subscribe("f/i/state/dso");
            Subscribe("f/i/order");
        }

        /// <summary>
        /// Formt die Antwort auf die Anfrage und Publisht bei Bestell anfragen
        /// </summary>
        /// <param name="topic">MQTT topic</param>
        /// <param name="message">MQTT message</param>
        /// <param name="commandPosition">Befehlsindex(SpeechCommands.txt)</param>
        /// <param name="response">Antwort</param>
        /// <returns>Modellfabrik-Antwort</returns>
        public string FormResponse(string topic, string message, int commandPosition, string response)
        {
            if (topic.Equals("f/o/order")) //Wenn eine Bestellung gemacht wurde...
            {
                Publish(topic, message); //... dann Bestellung publishen
            }
            else
            {
                var replacingString = ReplacingString(commandPosition); //... ansonsten Information herausfinden (z.B. Temperatur oder Luftdruck)
                response = response.Replace("(X)", replacingString); //Antwort formen, in dem das (X) durch die Information ersetzt wird
            }

            return response;
        }

        /// <summary>
        /// Anhand des Befehlsindex wird die gebrauchte Information aus der Modellfabrik herausgefunden
        /// </summary>
        /// <param name="commandPosition">Befehlsindex (SpeechCommands.txt)</param>
        /// <returns>Information der Modelfabrik</returns>
        private string ReplacingString(int commandPosition)
        {
            var replacingString = commandPosition switch
            {
                3 => _redWorkpieceCount,
                4 => _whiteWorkpieceCount,
                5 => _blueWorkpieceCount,
                6 => _temperature.Replace(".", ","), //ersetzt Punkt durch Komma
                7 => _pressure.Replace(".", ","),
                8 => _orderPosition,
                _ => "" //default
            };

            return replacingString;
        }

        /// <summary>
        /// Zählt rote, weiße und blaue Werkstücke in der message
        /// </summary>
        /// <param name="message">MQTT message</param>
        private void GetStockDataFromJson(string message)
        {
            _whiteWorkpieceCount = Regex.Matches(message, "WHITE").Count.ToString();
            _blueWorkpieceCount = Regex.Matches(message, "BLUE").Count.ToString();
            _redWorkpieceCount = Regex.Matches(message, "RED").Count.ToString();

        }

        /// <summary>
        /// Wird ausgeführt wenn das Event "MqttMsgPublishReceived" eintritt
        /// Message wird empfangen und anhand des Topics weiterverarbeitet
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">MqttMsgPublishEventArgs</param>
        private void client_MqttPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Message); //message wird gelesen
            switch (e.Topic) //Wenn das Topic...
            {
                case "i/bme680": //... ist, dann Umweltdaten der Modellfabrik bekommen
                    GetEnvironmentDataFromJson(message);
                    break;
                case "f/i/stock": //... ist, dann Lagerdaten der Modellfabrik bekommen
                    GetStockDataFromJson(message);
                    break;
                case "f/i/order":
                    OrderState(message);
                    break;
                default:
                    OrderPosition(message); //Wird ausgeführt wenn die anderen Fälle nicht eintreffen und state im Topping enthalten ist. (If Abfrage eigentlich nicht nötig, nur falls beim Subscriben erweitert werden sollte)
                    break;

            } //kann bei Bedarf mit weiteren Befehlen erweitert werden
        }

        /// <summary>
        /// Ermittelt Status und Farbe des Werkstücks
        /// </summary>
        /// <param name="message">Message von TXT Controller der Modellfabrik</param>
        private void OrderState(string message)
        {
            try
            {
                dynamic json = JObject.Parse(message); //JsonObject
                _orderState = json.state; //liest den Status der Bestellung
                switch (json.type.ToString()) //Type des Werkstückes
                {
                    case "BLUE": //blau
                        _orderedWorkpieceColor = "blaue Werkstück";
                        break;
                    case "WHITE": //weiß
                        _orderedWorkpieceColor = "weiße Werkstück";
                        break;
                    case "RED": //rot
                        _orderedWorkpieceColor = "rote Werkstück";
                        break;
                    default:
                        _orderedWorkpieceColor = "";
                        break;
                }

            }
            catch (Exception)
            {
                Console.WriteLine(@"Beim Lesen des MQTT Befehls (OrderState) ist ein Fehler aufgestreten");
            }
        }

        /// <summary>
        /// Ermittelt wo die Bestellung grade ist.
        /// </summary>
        /// <param name="message">Message von TXT Controller der Modellfabrik</param>
        private void OrderPosition(string message)
        {
            try
            {
                dynamic json = JObject.Parse(message); //JsonObject
                var active = (byte)json.active; //liest ob das Modul aktiv ist
                var stationName = json.station.ToString(); //liest Stationennamen

                if (_orderState.Equals("ORDERED")) //Wenn Bestellung bestellt ist
                {
                    _workpieceLocation = " wird vom Lager zur Multi-Bearbeitungsstation befördert."; //dann Location
                }

                if (_orderState.Equals("IN_PROCESS")) //Wenn in Process ist (Multibearbeitungsstation)
                {
                    if (active == 1) //dann --> wenn Modul aktiv ist
                    {
                        _workpieceLocation = GetWorkstationNameOf(stationName); //Stationname herausfinden
                    }
                }
                if (_orderState.Equals("SHIPPED") || _orderState.Equals("WAITING_FOR_ORDER")) //Wenn Bestellung ferig ist
                {
                    _orderPosition = "Es ist keine Bestellung in Auftrag."; //dann keine Bestellung in Auftrag
                }
                else
                {
                    _orderPosition = "Das " + _orderedWorkpieceColor + _workpieceLocation; //Ansonsten Antwort formen
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Beim Lesen des MQTT Befehls (OrderPosition) ist ein Fehler aufgestreten" + e);
            }
        }


        /// <summary>
        /// Anhand vom Modulkürzel wird ein String geformt, welcher angibt wo sich die Bestellung befindet
        /// </summary>
        /// <param name="station">Modulkürzel</param>
        /// <returns>Wo sich die Bestellung gerade befindet</returns>
        private string GetWorkstationNameOf(string station)
        {
            return station switch
            {
                "sld" => " befindet sich am Sortierband.",
                "mpo" => " befindet sich an der Multi-Bearbeitungsstation.",
                "dso" => " befindet sich am Warenausgang.",
                _ => "fehler"
            };
        }


        /// <summary>
        /// Wandelt den Message-String in ein Json-Objekt um und speichert die Temperatur und den Luftdruck
        /// </summary>
        /// <param name="message">MQTT Message</param>
        private void GetEnvironmentDataFromJson(string message)
        {
            try
            {
                dynamic environment = JObject.Parse(message); //JsonObject
                _temperature = environment.t; //liest Temperatur
                _pressure = environment.p; //liest Luftdruck
            }
            catch (Exception)
            {
                Console.WriteLine(@"Beim Lesen des MQTT Befehls (Environment) ist ein Fehler aufgestreten");
            }
        }

        /// <summary>
        /// Subscribed ein Topic
        /// </summary>
        /// <param name="topic">MQTT Topic</param>
        private void Subscribe(string topic)
        {
            _client.Subscribe(new[] { topic }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        /// <summary>
        /// Publishe eine Message an ein Topic
        /// </summary>
        /// <param name="topic">MQTT Topic</param>
        /// <param name="message">MQTT message</param>
        private void Publish(string topic, string message)
        {
            var date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            message = message.Replace("YYYY-MM-DDThh:mm:ss.fff", date);
            Console.WriteLine(message);
            _client.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        }

        /// <summary>
        /// Simuliert das Senden von MQTT-Befehlen der Modellfabrik.
        /// Diese Methode dient zum testen des MQTT-Teils, sodass man es nicht selber über die cmd machen muss.
        /// In der Modellfabrik wird diese Methode eigentlich nicht gebraucht, da man dort die Nachrichten automatisch bekommt
        /// </summary>
        public void TestPublish()
        {
            Console.WriteLine(@"Testdaten werden gepublisht");

            _client.Publish("f/i/order", Encoding.UTF8.GetBytes("{\"ts\":\"YYYY-MM-DDThh:mm:ss.fffZ\", \"state\":\"ORDERED\", \"type\":\"BLUE\"}"));
            _client.Publish("f/i/state/mpo", Encoding.UTF8.GetBytes("{\"ts\":\"YYYY-MM-DDThh:mm:ss.fffZ\", \"station\":\"vgr\", \"code\":0, \"description\":\"text\", \"active\":1, \"target\":\"hbw\"}"));
            _client.Publish("i/bme680", Encoding.UTF8.GetBytes("{ \"ts\":\"YYYY-MM-DDThh:mm:ss.fffZ\", \"t\":18.1, \"rt\":25.01, \"h\":40.0, \"rh\":38.01, \"p\":1000.15, \"iaq\":200, \"aq\":3, \"gr\":161000 }"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);   //Es wird eine Nachricht an das Topic bme680 gesendet.
            _client.Publish("f/i/stock", Encoding.UTF8.GetBytes("{\"stockItems\" :[{\"location\" : \"A1\",\"workpiece\" :{\"id\" : \"0462a64a616080\",\"state\" : \"RAW\",\"type\" : \"WHITE\"}}," +
                                                                "{\"location\" : \"A2\",\"workpiece\" : {\"id\" : \"0482a64a616080\",\"state\" : \"RAW\",\"type\" : \"BLUE\"}}," +
                                                                "{\"location\" : \"A3\",\"workpiece\" :{\"id\" : \"04cea54a616080\",\"state\" : \"RAW\",\"type\" : \"WHITE\"}}," +
                                                                "{\"location\" : \"B1\",\"workpiece\" :{\"id\" : \"041ea64a616081\",\"state\" : \"RAW\",\"type\" : \"BLUE\"}}," +
                                                                "{\"location\" : \"B2\",\"workpiece\" :{\"id\" : \"0428a74a616081\",\"state\" : \"RAW\",\"type\" : \"BLUE\"}}," +
                                                                "{\"location\" : \"B3\",\"workpiece\" :{\"id\" : \"04bfa44a616080\",\"state\" : \"RAW\",\"type\" : \"RED\"}}," +
                                                                "{\"location\" : \"C1\",\"workpiece\" :{\"id\" : \"045ca44a616081\",\"state\" : \"RAW\",\"type\" : \"RED\"}}," +
                                                                "{\"location\" : \"C2\",\"workpiece\" :{\"id\" : \"0461a64a616080\",\"state\" : \"RAW\",\"type\" : \"WHITE\"}}," +
                                                                "{\"location\" : \"C3\",\"workpiece\" :{\"id\" : \"044ca4a616081\",\"state\" : \"RAW\",\"type\" : \"RED\"}}],\"ts\" : \"2020-08-04T08:34:00.533Z\"}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true); //Es wird eine Nachricht an das Topic stock gesendet.
            TestPublishSent = true;
        }

    }
}
