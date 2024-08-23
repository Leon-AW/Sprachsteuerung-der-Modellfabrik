using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Modellfabrik.Models;

namespace Modellfabrik.Controllers
{
    /// <summary>
    /// Der Homecontroller regelt die Abläufe auf der Startseite
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Testvariable für den Unittest, ob in die Methode rein gegeangen wird
        /// </summary>
        public bool TestOfInput;

        /// <summary>
        /// Testvariable für den Unittest, ob in die Methode rein gegeangen wird
        /// </summary>
        public bool TestOfHistory;
        /// <summary>
        /// Testvariable für den Unittest, ob in die Methode rein gegeangen wird
        /// </summary>
        public bool TestOfTime;
        /// <summary>
        /// Objekt vom Typ ServiceClassModel
        /// </summary>
        [BindProperty]
        public ServiceClassModel ServiceClass { get; set; }


        /// <summary>
        /// Die Methode gibt uns die View von Index wieder, in dem Fall die Startseite
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Die Methode gibt uns die View von Privacy wieder, in dem Fall die Lizenzseite
        /// </summary>
        /// <returns>IActionResult</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Diese Methode hört und wartet auf die Schlüsselworterkennung
        /// </summary>
        /// <returns>Task</returns>
        [HttpPost]
        public async Task ActivateKeyRecognition(bool activate)
        {
            var speechManager = ServiceClass.Logic.SpeechManager;
            if (activate)                                           // ... wenn die Seite geladen ist und der activate "true" ist
            {
                Console.WriteLine(@"Es wird auf einen Key gehört");
                await speechManager.StartKeyRecognition();                // ... dann wird auf das Schlüsselwort gehört und man kann das Schlüsselwort aussprechen

            }
            else
            {
                Console.WriteLine(@"Es wird nicht mehr auf einen Key gehört");
                await speechManager.StopKeyRecognition();         // ... ansonsten hört es auf, auf das Schlüsselwort zu hören
            }
        }

        /// <summary>
        /// Diese Methode startet die weitere Ausführung des Programm nach der Schlüsselworterkennung
        /// </summary>
        /// <returns>Task JsonResult</returns>
        public async Task<JsonResult> CallSpeechInput()
        {
            TestOfInput = true;
            var recognizedText = await ServiceClass.Logic.SpeechInput();
            return Json(new { getText = recognizedText });
        }

        public JsonResult CallSpeechOutput()
        {
            var response = ServiceClass.Logic.SpeechOutput();
            return Json(new { getText = response });
        }


        /// <summary>
        /// Speichert den Sprachverlauf in TempData, damit diese in der Textarea
        /// </summary>
        [HttpPost]
        public JsonResult GetHistory()
        {
            TestOfHistory = true;
            return Json(new { getText = ServiceClass.Logic.SpeechManager.SpeechHistory });
        }

        /// <summary>
        /// Gibt den Status vom geprüften Text wieder, ob der gültig ist oder nicht.
        /// </summary>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult IsTextValid()
        {
            return Json(ServiceClass.Logic.SpeechManager.ValidText);
        }

        /// <summary>
        /// Ruft die Zeit auf, welche von der Spracheingabe bis zur Sprachausgabe (Verarberitung) gemessen wurde.
        /// </summary>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult GetTime()
        {
            TestOfTime = true;
            return Json(ServiceClass.Logic.SpeechManager.TrackTime());
        }

        /// <summary>
        /// Fehlerseite
        /// </summary>
        /// <returns>IActionResult</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
