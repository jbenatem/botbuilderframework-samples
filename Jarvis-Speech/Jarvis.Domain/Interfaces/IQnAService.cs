using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace Jarvis.Domain.Interfaces
{
    public interface IQnAService
    {
        Task SendQnAResultAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken);
    }
}
