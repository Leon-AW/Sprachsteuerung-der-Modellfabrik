using System;
using System.Device.Gpio;

namespace Modellfabrik.Components
{
    /// <summary>
    /// Ansteuerung der LEDs
    /// </summary>
    public class OnOffLed
    {
        /// <summary>
        /// Unit Test ob LED angegangen ist
        /// </summary>
        public bool TestTurnedOn;

        /// <summary>
        /// Unit Test ob LED ausgegangen ist
        /// </summary>
        public bool TestTurnedOff;

        /// <summary>
        /// Diese Methode ermöglicht das Einschalten des LEDs
        /// </summary>
        /// <param name="pin">Der Output bin am Raspberry</param>
        public void TurnOnLED(int pin)
        {
            try
            {
                GpioController controller = new();
                controller.OpenPin(pin, PinMode.Output);
                controller.Write(pin, PinValue.High);
                TestTurnedOn = true;
            }
            catch (Exception)
            {
                TestTurnedOn = false;
                Console.WriteLine(@"Die LEDs funktionieren nur wenn das Projekt aud dem Raspberry Pi läuft");
            }
        }


        /// <summary>
        /// Diese Methode ermöglicht das Ausschalten des LEDs
        /// </summary>
        /// <param name="pin">Der Output bin am Raspberry</param>
        // ReSharper disable once InconsistentNaming
        public void TurnOffLED(int pin)
        {
            try
            {
                GpioController controller = new();
                controller.OpenPin(pin, PinMode.Output);
                controller.Write(pin, PinValue.Low);
                TestTurnedOff = true;
            }
            catch (Exception)
            {
                TestTurnedOff = false;
                Console.WriteLine(@"Die LEDs funktionieren nur wenn das Projekt aud dem Raspberry Pi läuft");
            }
        }
    }
}
