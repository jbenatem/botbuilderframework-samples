﻿using Jarvis.Domain.Interfaces;
using Jarvis.Domain.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using System.Threading;
using System.Threading.Tasks;

namespace Jarvis.Cognitive
{
    public class LuisService : ILuisService
    {
        public LuisRecognizer luisRecognizer { get; private set; }

        public LuisService(
            LuisApplication luisApplication
        )
        {
            luisRecognizer = new LuisRecognizer(luisApplication);
        }

        public async Task<LuisResult> GetLuisResult(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            LuisResult luis_result = new LuisResult();

            // The actual call to LUIS
            var recognizerResult = await luisRecognizer.RecognizeAsync(turnContext, cancellationToken);

            var (intent, score) = recognizerResult.GetTopScoringIntent();
            luis_result.TopIntent = intent;
            luis_result.TopIntentScore = score;

            //luis_result.Email = recognizerResult.Entities["email"]?.FirstOrDefault()?.ToString().ToLower();
            //luis_result.pais = recognizerResult.Entities["pais"]?.FirstOrDefault()?.ToString().ToLower();

            return luis_result;
        }
    }
}
