using Microsoft.AspNetCore.Mvc;
using Modellfabrik.Models;

namespace Modellfabrik.Controllers
{
    /// <summary>
    /// Der SettingsController regelt die Abläufe auf der Einstellungsseite
    /// </summary>
    public class SettingsController : Controller
    {

        /// <summary>
        /// Objekt vom Typ ServiceClassModel
        /// </summary>
        [BindProperty]
        public ServiceClassModel ServiceClass { get; set; }

        /// <summary>
        /// Die Methode gibt uns die View von Setting wieder, in dem Fall die Einstellungen
        /// </summary>
        /// <returns>IActionResult</returns>
        public IActionResult Setting()
        {
            return View();
        }

        /// <summary>
        /// Die Methode gibt uns die View von HandControl wieder, in dem Fall die Manuelle Steuerung
        /// </summary>
        /// <returns>IActionResult</returns>
        public IActionResult HandControl()
        {
            return View();
        }

        /// <summary>
        /// Startet die weitere Ausführung des Programms über die Mauelle Steuerung
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public IActionResult ExecuteManualCommand(string value)
        {
            ServiceClass.Logic.ExecuteManualCommand(value);
            return RedirectToAction(nameof(HandControl)); 
        }

        /// <summary>
        /// Ändern der Stimme von der Modellfabrik
        /// </summary>
        /// <param name="voiceName">Name der Stimme</param>
        [HttpPost]
        public void ChangeVoiceGender(string voiceName)
        {
            if (voiceName.Equals("Female"))                                                 //... wenn der Name der Stimme gleich "Female" ist     
                ServiceClass.Logic.SpeechManager.ChangeSynthesisVoice("de-DE-KatjaNeural"); // ... dann änder diese Konfiguration auf die Weibliche
            
            if (voiceName.Equals("Male"))                                                    //... wenn der Name der Stimme gleich "Male" ist
                ServiceClass.Logic.SpeechManager.ChangeSynthesisVoice("de-DE-ConradNeural"); // ... dann änder diese Konfiguration auf die Männliche
        }
    }
}
