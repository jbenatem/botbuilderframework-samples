using Microsoft.Bot.Builder;
using System.Threading;
using System.Threading.Tasks;

namespace CCBot.Domain.Interfaces
{
    public interface IAnswersFactory
    {
        Task SendBotAnswerWebChatMessengerAsync(string answerType, ITurnContext turnContext, CancellationToken cancellationToken);
    }
}
