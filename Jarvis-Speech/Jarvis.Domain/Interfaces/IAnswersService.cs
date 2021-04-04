using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Jarvis.Domain.Interfaces
{
    public interface IAnswersService
    {
        Task SendEchoAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken);
        Task SendResponseAsync(string code, ITurnContext turnContext, CancellationToken cancellationToken);
        Task SendGreetingAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken);
    }
}
