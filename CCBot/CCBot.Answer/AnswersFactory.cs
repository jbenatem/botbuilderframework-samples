using CCBot.Domain.Interfaces;
using CCBot.Domain.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CCBot.Answer
{
    public class AnswersFactory : IAnswersFactory
    {
        public async Task SendWebChatResponseAsync(string code, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            using (var stream = GetType().Assembly.GetManifestResourceStream("CCBot.Answer.Resources.Responses.WebChatResponses.json"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var reply = MessageFactory.Text("");
                    string botResponsesJson = reader.ReadToEnd();
                    var allBotResponses = JObject.Parse(botResponsesJson);
                    IList<JToken> botResponsesList = allBotResponses["responses"].Children().ToList();
                    List<string> answers = new List<string>();
                    foreach (JToken botResponse in botResponsesList)
                    {
                        if (botResponse["code"].ToString().Trim() == code)
                        {
                            IList<JToken> messages = botResponse["messages"].Children().ToList();
                            foreach (JToken message in messages)
                            {
                                BotResponse response = JsonConvert.DeserializeObject<BotResponse>(message.ToString());
                                if (message["type"].ToString().Trim() == "quick-replies")
                                {
                                    reply = await CreateQuickRepliesAnswer(response);
                                }
                                else if (message["type"].ToString().Trim() == "carousel")
                                {
                                    reply = await CreateCarouselAnswer(response);
                                }
                                else if (message["type"].ToString().Trim() == "simple")
                                {
                                    reply = await CreateSimpleAnswer(response);
                                }
                                await turnContext.SendActivityAsync(reply, cancellationToken);
                            }
                        }
                    }
                }
            }
        }

        private async Task<Activity> CreateSimpleAnswer(BotResponse botResponse)
        {
            int random = new Random().Next(botResponse.Text.Count);
            var reply = MessageFactory.Text(botResponse.Text[random]);
            return reply;
        }

        private async Task<Activity> CreateQuickRepliesAnswer(BotResponse botResponse)
        {
            int random = new Random().Next(botResponse.Text.Count);
            var reply = MessageFactory.Text(botResponse.Text[random]);
            var Actions = new List<CardAction>();
            foreach (string quickReply in botResponse.QuickReplies)
            {
                Actions.Add(new CardAction() { Title = quickReply, Type = ActionTypes.ImBack, Value = quickReply });
            }
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = Actions
            };
            return reply;
        }

        private async Task<Activity> CreateCarouselAnswer(BotResponse botResponse)
        {
            var reply = MessageFactory.Text("");
            foreach (Card card in botResponse.Cards)
            {
                List<CardAction> buttons = new List<CardAction>();
                foreach (Button button in card.buttons)
                {
                    if (button.Type == ActionTypes.OpenUrl)
                    {
                        buttons.Add(new CardAction() { Title = button.Title.Trim(), Type = button.Type, Value = button.Value.Trim() });
                    }
                    else if (button.Type == ActionTypes.ImBack)
                    {
                        buttons.Add(new CardAction() { Title = button.Title.Trim(), Type = button.Type, Value = button.Value.Trim() });
                    }
                }
                var heroCard = new HeroCard
                {
                    Title = card.Title,
                    Images = new List<CardImage> { new CardImage(card.ImageUrl.Trim()) },
                    Subtitle = card.Subtitle,
                    Buttons = buttons
                };
                reply.Attachments.Add(heroCard.ToAttachment());                
            }
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            return reply;
        }
    }
}
