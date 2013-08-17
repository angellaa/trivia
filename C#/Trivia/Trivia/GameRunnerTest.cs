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
    }
}