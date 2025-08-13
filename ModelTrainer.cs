using Microsoft.ML;
using Microsoft.ML.Data;
using System.Collections.Generic;
using System.Linq;

using NetConsoleWalsRiskJira.Shared;

namespace NetConsoleWalsRiskJira
{
    public static class ModelTrainer
    {
        public static void TrainAndSaveModel(IEnumerable<ModelInput> data, string modelPath)
        {
            var mlContext = new MLContext();
            var dataView = mlContext.Data.LoadFromEnumerable(data.Select(d => new ModelInputFlat
            {
                Features = d.Features,
                Risk = d.Risk,
                Effort = d.Effort
            }));
            var pipeline = mlContext.Transforms.Concatenate("Features", nameof(ModelInputFlat.Features))
                .Append(mlContext.Transforms.CopyColumns("Label", nameof(ModelInputFlat.Risk)))
                .Append(mlContext.Regression.Trainers.Ols());
            var riskModel = pipeline.Fit(dataView);
            pipeline = mlContext.Transforms.Concatenate("Features", nameof(ModelInputFlat.Features))
                .Append(mlContext.Transforms.CopyColumns("Label", nameof(ModelInputFlat.Effort)))
                .Append(mlContext.Regression.Trainers.Ols());
            var effortModel = pipeline.Fit(dataView);
            mlContext.Model.Save(riskModel, dataView.Schema, modelPath + ".risk.zip");
            mlContext.Model.Save(effortModel, dataView.Schema, modelPath + ".effort.zip");
        }

        public static (float risk, float effort) Predict(string modelPath, float[] features)
        {
            var mlContext = new MLContext();
            var riskModel = mlContext.Model.Load(modelPath + ".risk.zip", out _);
            var effortModel = mlContext.Model.Load(modelPath + ".effort.zip", out _);
            var predEngineRisk = mlContext.Model.CreatePredictionEngine<ModelInputFlat, ModelOutputFlat>(riskModel);
            var predEngineEffort = mlContext.Model.CreatePredictionEngine<ModelInputFlat, ModelOutputFlat>(effortModel);
            var input = new ModelInputFlat { Features = features };
            var risk = predEngineRisk.Predict(input).Score;
            var effort = predEngineEffort.Predict(input).Score;
            return (risk, effort);
        }

        private class ModelInputFlat
        {
            [VectorType]
            public float[] Features { get; set; }
            public float Risk { get; set; }
            public float Effort { get; set; }
        }
        private class ModelOutputFlat
        {
            public float Score { get; set; }
        }
    }
}
