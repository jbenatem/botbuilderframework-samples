using CCBot.Domain.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CCBot.Core.Services
{
    public class OrchestratorService : IOrchestratorService
    {
        private readonly IAnswersService _answersService;

        public OrchestratorService(
            IAnswersService answersService
        )
        {
            _answersService = answersService;
        }

        public async Task SendBotGreeting(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            await _answersService.Greeting(membersAdded, turnContext, cancellationToken);
        }
        public async Task SendBotAnswer(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await _answersService.SendResponse(turnContext.Activity.Text, turnContext, cancellationToken);
        }
    }
}
