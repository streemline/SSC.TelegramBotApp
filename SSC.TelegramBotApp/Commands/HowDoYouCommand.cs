﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Commands
{
    public class HowDoYouCommand : Command
    {
        public override string Name => "Как дела?";

        public override async void Execute(TelegramBotClient client, Message msg)
        {
            var chatId = msg.Chat.Id;
            await client.SendTextMessageAsync(chatId, "У бота все отлично:)");
        }

        public override Task ExecuteAsync(TelegramBotClient client, Message msg)
        {
            var chatId = msg.Chat.Id;
            return client.SendTextMessageAsync(chatId, "У бота все отлично:)");
        }
    }
}