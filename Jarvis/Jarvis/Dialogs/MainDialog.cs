// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.12.2

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

using Jarvis.Domain.Interfaces;
using Jarvis.Domain.Model;
using Jarvis.Dialogs.MeetingReservation;

namespace Jarvis.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        protected readonly ILogger Logger;
        private readonly ILuisService _luisService;
        private readonly IAnswersService _answersService;

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(
            ILogger<MainDialog> logger,
            IAnswersService answersService,
            ILuisService luisService)
            : base(nameof(MainDialog))
        {
            _answersService = answersService;
            _luisService = luisService;
            Logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new MeetingReservationDialog());
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Use the text provided in FinalStepAsync or the default if it is the first time.
            var messageText = stepContext.Options?.ToString() ?? "¿En qué puedo ayudarte?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            LuisResult luisResult = await _luisService.GetLuisResult(stepContext.Context, cancellationToken);

            if (luisResult.TopIntent == "None" || luisResult.TopIntentScore <= 0.50)
            {
                //await _qnaService.SendQnAResultAsync(turnContext, cancellationToken);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Llamando a qnamaker"), cancellationToken);
            }
            else if (luisResult.TopIntent == "agendar_reunion")
            {
                return await stepContext.BeginDialogAsync(nameof(MeetingReservationDialog));
            }
            else
            {
                await _answersService.SendResponseAsync(luisResult.TopIntent, stepContext.Context, cancellationToken);
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Restart the main dialog with a different message the second time around
            var promptMessage = "¿Te puedo ayudar en otra cosa?";
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }
    }
}
