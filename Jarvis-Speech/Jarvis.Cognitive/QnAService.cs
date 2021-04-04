using Jarvis.Domain.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Schema;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Jarvis.Cognitive
{
    public class QnAService : IQnAService
    {
        public QnAMaker QnaMaker { get; private set; }

        public QnAService(
            QnAMakerEndpoint endpoint
        )
        {
            QnaMaker = new QnAMaker(endpoint);
        }

        public async Task SendQnAResultAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var results = await QnaMaker.GetAnswersAsync(turnContext);
            if (results.Any())
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(results.First().Answer), cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("Lo siento pero no te entendi, ¿Puedes escribir otra consulta?"), cancellationToken);
            }
        }
    }
}
