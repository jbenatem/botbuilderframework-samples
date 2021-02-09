using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CCBot.Domain.Interfaces
{
    public interface IAnswersService
    {
        Task SendEcho(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken);
        Task SendResponse(string code, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken);
        Task Greeting(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken);
    }
}
