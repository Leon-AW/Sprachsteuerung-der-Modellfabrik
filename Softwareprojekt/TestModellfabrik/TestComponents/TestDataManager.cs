using Modellfabrik.Components;
using NUnit.Framework;

namespace TestModellfabrik.TestComponents
{
    /// <summary>
    /// Klasse zum Testen der Funktionalität von TestDataManager
    /// </summary>
    [TestFixture]
    public class TestDataManager
    {
        /// <summary>
        /// Testet, ob die Commands.txt File eingelesen werden kann von dem DataManager Objekt.
        /// </summary>
        [Test]
        public void TestReadTextFile()
        {
            //act
            var datamanager = new DataManager();

            //assert
            Assert.AreEqual(true, datamanager.TestAllLinesRead);
        }
    }
}