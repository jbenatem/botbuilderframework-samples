﻿using Microsoft.Bot.Builder;
using System.Threading;
using System.Threading.Tasks;

namespace Jarvis.Domain.Interfaces
{
    public interface IAnswersFactory
    {
        Task SendWebChatResponseAsync(string answerType, ITurnContext turnContext, CancellationToken cancellationToken);
    }
}
