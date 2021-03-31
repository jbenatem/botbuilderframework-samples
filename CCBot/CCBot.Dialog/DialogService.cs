using CCBot.Domain.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;

namespace CCBot.Dialogs
{
    public class DialogService : IDialogService
    {
        public async Task ProcessDialog(Dialog dialog, BotState conversationState, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await dialog.RunAsync(turnContext, conversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);
        }
    }
}
