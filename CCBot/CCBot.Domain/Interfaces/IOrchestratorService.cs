using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CCBot.Domain.Interfaces
{
    public interface IOrchestratorService
    {
        Task SendBotAnswer(Dialog dialog, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken);
        Task SendBotGreeting(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken);
    }
}
