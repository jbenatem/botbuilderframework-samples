using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;

namespace CCBot.Domain.Interfaces
{
    public interface IDialogService
    {
        Task ProcessDialog(Dialog dialog, BotState conversationState, ITurnContext turnContext, CancellationToken cancellationToken);
    }
}
