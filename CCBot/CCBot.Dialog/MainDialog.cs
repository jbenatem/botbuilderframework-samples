using CCBot.Dialogs.MeetingReservation;
using CCBot.Domain.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;

namespace CCBot.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly ConversationState _conversationState;
        public MainDialog(ConversationState conversationState) : base(nameof(MainDialog))
        {
            _conversationState = conversationState;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new MeetingReservationDialog());
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                DerivateStepAsync
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> DerivateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.BeginDialogAsync(nameof(MeetingReservationDialog), cancellationToken : cancellationToken);
        }
    }
}
