using CCBot.Domain.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;

namespace CCBot.Dialogs.MeetingReservation
{
    public class MeetingReservationDialog : ComponentDialog
    {
        public MeetingReservationDialog() : base(nameof(MeetingReservationDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var user = (User)stepContext.Options;
            if (user.Name == null)
            {
                var promptMessage = MessageFactory.Text("Ingresa tu nombre");
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }
            return await stepContext.NextAsync(user.Name, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var user = (User)stepContext.Options;
            if (user.ContactNumber == null)
            {
                var promptMessage = MessageFactory.Text("Ingresa tu número de contacto");
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }
            return await stepContext.NextAsync(user.ContactNumber, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Gracias por los datos"));
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
