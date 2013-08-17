using System;
using System.IO;
using NUnit.Framework;

namespace Trivia
{
    [TestFixture]
    public class GameRunnerTest
    {
        [Test]
        public void CharacterizationTest()
        {
            foreach (var fullFileName in Directory.GetFiles(@".\TestData\"))
            {
                var outputWriter = new TestOutputWriter();
                var fileName = Path.GetFileName(fullFileName);

                GameRunner.Main(new[] { fileName }, outputWriter);

                Assert.That(outputWriter.Output, Is.EqualTo(File.ReadAllText(fullFileName)));
            }
        }

        [Test]
        public void Test_CalculateNewPlace_Simple()
        {
            int currentPlace = 0;
            int roll = 1;
            int numberOfPlaces = 10;
            var newPlaceCalculator = new NewPlaceCalculator();

            var result = newPlaceCalculator.CalculateNewPlace(currentPlace, roll, numberOfPlaces);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void Test_CalculateNewPlace_Rollover()
        {
            int currentPlace = 0;
            int roll = 12;
            int numberOfPlaces = 10;
            var newPlaceCalculator = new NewPlaceCalculator();

            var result = newPlaceCalculator.CalculateNewPlace(currentPlace, roll, numberOfPlaces);

            Assert.AreEqual(2, result);
        }

        [Test]
        public void Test_CalculateNewPlace_LastPlace()
        {
            int currentPlace = 3;
            int roll = 6;
            int numberOfPlaces = 10;

            var newPlaceCalculator = new NewPlaceCalculator();
            var result = newPlaceCalculator.CalculateNewPlace(currentPlace, roll, numberOfPlaces);

            Assert.AreEqual(9, result);
        }

        [Test]
        public void Test_CalculateNewPlace_RolloverToFirstPlace()
        {
            int currentPlace = 3;
            int roll = 7;
            int numberOfPlaces = 10;
            var newPlaceCalculator = new NewPlaceCalculator();

            var result = newPlaceCalculator.CalculateNewPlace(currentPlace, roll, numberOfPlaces);

            Assert.AreEqual(0, result);
        }
    }
}