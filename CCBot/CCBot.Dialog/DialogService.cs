using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;

namespace CCBot.Dialog
{
    public class DialogService<T>
        where T : Dialog
    {
        protected readonly Dialog Dialog;
        protected readonly BotState ConversationState;
        protected readonly BotState UserState;

        public DialogService(
            ConversationState conversationState,
            UserState userState,
            T dialog
        )
        {
            ConversationState = conversationState;
            UserState = userState;
            Dialog = dialog;
        }
    }
}
