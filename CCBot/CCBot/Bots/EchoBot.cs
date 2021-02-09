// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.11.1

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CCBot.Domain.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace CCBot.Bots
{
    public class EchoBot : ActivityHandler
    {
        private readonly IOrchestratorService _orchestratorService;
        public EchoBot(
            IOrchestratorService orchestratorService
        )
        {
            _orchestratorService = orchestratorService;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await _orchestratorService.SendBotAnswer(turnContext, cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            await _orchestratorService.SendBotGreeting(membersAdded, turnContext, cancellationToken);
        }
    }
}
