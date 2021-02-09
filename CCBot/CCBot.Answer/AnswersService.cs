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

        public async Task SendEcho(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var replyText = $"Echo: {turnContext.Activity.Text}";
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        public async Task SendResponse(string code, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Activity.ChannelId == "emulator" ||
                turnContext.Activity.ChannelId == "directline" ||
                turnContext.Activity.ChannelId == "webchat" ||
                turnContext.Activity.ChannelId == "messenger")
            {
                await _answerFactory.SendBotAnswerWebChatMessengerAsync(code, turnContext, cancellationToken);
            }
        }

        public async Task Greeting(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    if (turnContext.Activity.ChannelId == "emulator" ||
                        turnContext.Activity.ChannelId == "directline" ||
                        turnContext.Activity.ChannelId == "webchat" ||
                        turnContext.Activity.ChannelId == "messenger")
                    {
                        await _answerFactory.SendBotAnswerWebChatMessengerAsync("saludo_inicial", turnContext, cancellationToken);
                    }
                }
            }
        }
    }
}
