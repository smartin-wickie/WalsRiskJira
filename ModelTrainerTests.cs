using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;

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
            ModelTrainer.TrainAndSaveModel(data, "test-model");
            var (risk, effort) = ModelTrainer.Predict("test-model", features);
            Assert.That(risk, Is.InRange(2, 8));
            Assert.That(effort, Is.InRange(7, 13));
        }
    }
}
