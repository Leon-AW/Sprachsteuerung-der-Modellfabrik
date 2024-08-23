using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using Modellfabrik.Models;

namespace Modellfabrik.Controllers
{
    /// <summary>
    /// Der ConnectionController regelt die Abläufe für die Verbindungen zum Internet und zum Broker
    /// </summary>
    public class ConnectionController : Controller
    {
       
        /// <summary>
        /// Objekt vom Typ ServiceClassModel
        /// </summary>
        [BindProperty]
        public ServiceClassModel ServiceClass { get; set; }


        /// <summary>
        /// Prüft die Internetverbindung, in dem google angepingt wird
        /// </summary>
        /// <returns>true falls eine Verbindung besteht, sonst false</returns>
        [HttpPost]
        public JsonResult CheckInternetConnection()
        {
            
            var ping = new Ping();
            try
            {
                var reply = ping.Send("www.google.de", 3500);
                return Json(reply is { Status: IPStatus.Success });
            }
            catch
            {
                return Json(false);
            }
        }

        /// <summary>
        /// Gibt den Status vom Broker wieder, ob zu ihm eine Verbindung aufgebaut werden konnte oder nicht.
        /// </summary>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult IsConnectedToBroker()
        {
            return Json(ServiceClass.Logic.MqttCommandManager.IsConnectedToBroker());
        }

        /// <summary>
        /// Startet den Broker mit IP
        /// </summary>
        /// <param name="brokerIp">Broker IP</param>
        [HttpPost]
        public void ConnectToBroker(string brokerIp)
        {
            ServiceClass.Logic.MqttCommandManager.StartBroker(brokerIp);
        }

        /// <summary>
        /// Trennt die Verbindung zum Broker
        /// </summary>
        [HttpPost]
        public void DisconnectBroker()
        {
            ServiceClass.Logic.MqttCommandManager.DisconnectBroker();
        }

        /// <summary>
        /// Führt die Testdummy Daten für publishen aus.
        /// Nach Sprint 2 wird diese wieder enfernt und die dazugehörigen Verweise. 
        /// </summary>
        [HttpPost]
        public void TestPublish()
        {
            ServiceClass.Logic.MqttCommandManager.TestPublish();
        }


    }
}
