using CCBot.Domain.Interfaces;
using CCBot.Domain.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CCBot.Core
{
    public class OrchestratorService<T> : IOrchestratorService
        where T : Dialog
    {
        private readonly IAnswersService _answersService;
        private readonly ILuisService _luisService;
        private readonly IQnAService _qnaService;

        protected readonly Dialog Dialog;
        protected readonly BotState ConversationState;
        protected readonly BotState UserState;

        public OrchestratorService(
            IAnswersService answersService,
            ILuisService luisService,
            IQnAService qnaService, 
            ConversationState conversationState, 
            UserState userState, 
            T dialog
        )
        {
            _answersService = answersService;
            _luisService = luisService;
            _qnaService = qnaService;

            ConversationState = conversationState;
            UserState = userState;
            Dialog = dialog;
        }

        public async Task SendBotGreeting(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            await _answersService.SendGreetingAsync(membersAdded, turnContext, cancellationToken);
        }
        public async Task SendBotAnswer(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            LuisResult luisResult = await _luisService.GetLuisResult(turnContext, cancellationToken);
            if (luisResult.TopIntent == "None" || luisResult.TopIntentScore <= 0.50)
            {
                //await _qnaService.SendQnAResultAsync(turnContext, cancellationToken);
                await turnContext.SendActivityAsync(MessageFactory.Text("Llamando a qnamaker"), cancellationToken);
            }
            else if (luisResult.TopIntent == "agendar_reunion")
            {
                await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>("DialogState"), cancellationToken);
            }
            else
            {
                await _answersService.SendResponseAsync(luisResult.TopIntent, turnContext, cancellationToken);
            }    
        }
    }
}
