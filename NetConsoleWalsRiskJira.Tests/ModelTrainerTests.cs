using System;
using System.Linq;
using NUnit.Framework;
using NetConsoleWalsRiskJira.Shared;
using NetConsoleWalsRiskJira;

namespace NetConsoleWalsRiskJira.Tests
{
    public class ModelTrainerTests
    {
        [Test]
        public void CanTrainAndPredict()
        {
            var features = new float[] { 0.1f, 0.2f, 0.3f };
            var data = new[]
            {
                new ModelInput { Features = features, Risk = 5, Effort = 10 },
                new ModelInput { Features = features.Select(f => f * 1.1f).ToArray(), Risk = 7, Effort = 12 },
                new ModelInput { Features = features.Select(f => f * 0.9f).ToArray(), Risk = 3, Effort = 8 }
            };
            NetConsoleWalsRiskJira.ModelTrainer.TrainAndSaveModel(data, "test-model");
            var (risk, effort) = NetConsoleWalsRiskJira.ModelTrainer.Predict("test-model", features);
            Assert.That(float.IsNaN(risk), Is.False, "Risk should not be NaN");
            Assert.That(float.IsInfinity(risk), Is.False, "Risk should not be Infinity");
            Assert.That(float.IsNaN(effort), Is.False, "Effort should not be NaN");
            Assert.That(float.IsInfinity(effort), Is.False, "Effort should not be Infinity");
        }
    }
}
