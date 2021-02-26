using CCBot.Domain.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CCBot.Answer
{
    public class AnswersService : IAnswersService
    {
        private readonly IAnswersFactory _answerFactory;
        public AnswersService
        (
            IAnswersFactory answerFactory
        )
        {
            _answerFactory = answerFactory;
        }

        public async Task SendEchoAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var replyText = $"Echo: {turnContext.Activity.Text}";
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        public async Task SendResponseAsync(string code, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            switch (turnContext.Activity.ChannelId)
            {
                case "facebook":
                    break;
                default:
                    await _answerFactory.SendWebChatResponseAsync(code, turnContext, cancellationToken);
                    break;
            }
        }

        public async Task SendGreetingAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {                
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    switch (turnContext.Activity.ChannelId)
                    {
                        case "facebook":
                            break;
                        default:
                            await _answerFactory.SendWebChatResponseAsync("saludo_inicial", turnContext, cancellationToken);
                            break;
                    }
                }
            }
        }
    }
}
