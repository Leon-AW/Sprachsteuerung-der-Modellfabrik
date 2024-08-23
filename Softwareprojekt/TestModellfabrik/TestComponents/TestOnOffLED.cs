using Modellfabrik.Components;
using NUnit.Framework;

namespace TestModellfabrik.TestComponents
{
    /// <summary>
    /// Klasse zum Testen der Funktionalität von TestOnOffLED
    /// </summary>
    [TestFixture]
    public class TestOnOffLed
    {
        /// <summary>
        /// Prüft mit Boolean, ob die Methode die LED einschaltet
        /// </summary>
        [Test]
        public void TestTurnOnLED()
        {
            var onOffLed = new OnOffLed();
            //Act
            onOffLed.TurnOnLED(26);

            //Assert
            Assert.AreEqual(false, onOffLed.TestTurnedOn);
        }

        /// <summary>
        /// Prüft mit Boolean, ob die Methode die LED ausschaltet
        /// </summary>
        [Test]
        // ReSharper disable once InconsistentNaming
        public void TestTurnOffLED()
        {
            var onOffLed = new OnOffLed();
            //Act
            onOffLed.TurnOffLED(26);

            //Assert
            Assert.AreEqual(false, onOffLed.TestTurnedOn);
        }

    }
}
