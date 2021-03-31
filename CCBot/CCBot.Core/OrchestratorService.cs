using CCBot.Domain.Interfaces;
using CCBot.Domain.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CCBot.Core
{
    public class OrchestratorService : IOrchestratorService
    {
        private readonly IAnswersService _answersService;
        private readonly ILuisService _luisService;
        private readonly IQnAService _qnaService;
        private readonly IDialogService _dialogService;
        private BotState _conversationState;
        public OrchestratorService(
            IAnswersService answersService,
            ILuisService luisService,
            IQnAService qnaService,
            IDialogService dialogService,
            ConversationState conversationState
        )
        {
            _answersService = answersService;
            _luisService = luisService;
            _qnaService = qnaService;
            _dialogService = dialogService;
            _conversationState = conversationState;
        }

        public async Task SendBotGreeting(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            await _answersService.SendGreetingAsync(membersAdded, turnContext, cancellationToken);
        }
        public async Task SendBotAnswer(Dialog dialog, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            LuisResult luisResult = await _luisService.GetLuisResult(turnContext, cancellationToken);

            var conversationStateAccessors = _conversationState.CreateProperty<LuisResult>(nameof(LuisResult));
            var conversationData = await conversationStateAccessors.GetAsync(turnContext, () => luisResult);
            await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);

            if (luisResult.TopIntent == "None" || luisResult.TopIntentScore <= 0.50)
            {
                //await _qnaService.SendQnAResultAsync(turnContext, cancellationToken);
                await turnContext.SendActivityAsync(MessageFactory.Text("Llamando a qnamaker"), cancellationToken);
            }
            else if (luisResult.TopIntent == "agendar_reunion")
            {
                await _dialogService.ProcessDialog(dialog, _conversationState, turnContext, cancellationToken);
            }
            else
            {
                await _answersService.SendResponseAsync(luisResult.TopIntent, turnContext, cancellationToken);
            }    
        }

        private async Task BeginDialog(Microsoft.Bot.Builder.Dialogs.Dialog dialog, BotState conversationState, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {

        }
    }
}
