using Microsoft.VisualStudio.TestTools.UnitTesting;
using MeDirectAssessmentUI;

namespace MeDirectAssessmentTest
{
    [TestClass]
    public class GameTest
    {
        // Test to see if the  all lights are off
        [TestMethod]
        public void AllLightOff()
        {
            var testForm = new Form1();
            testForm.lightsMatrix = new bool[5, 5];

            // Set all lights to be off
            for (int i = 0; i < testForm.lightsMatrix.GetLength(1); i++)
            {
                for (int j = 0; j < testForm.lightsMatrix.GetLength(0); j++)
                {
                    testForm.lightsMatrix[i, j] = false;
                }
            }

            // Expected the status to find that all lights are off
            bool expected = true;
            // Get actual result
            bool actual = testForm.checkStatus();
            // Assert they are the same
            Assert.AreEqual(expected, actual);
        }
    }
}
