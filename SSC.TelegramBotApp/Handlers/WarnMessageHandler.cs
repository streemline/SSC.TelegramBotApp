﻿using SSC.TelegramBotApp.Extensions;
using SSC.TelegramBotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Handlers
{
    public class WarnMessageHandler : MessageHandler
    {
        public override void Handle(TelegramBotClient client, Message msg)
        {
            if (msg != null && msg.Text != null && msg.Text.IndexOf("!warn", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (msg.ReplyToMessage != null)
                {
                    var user = msg.ReplyToMessage.From;
                    _WarnUser(client, msg, user, msg.ReplyToMessage.MessageId);

                }
                else if (msg.Entities != null)
                {
                    foreach (var entity in msg.Entities)
                    {
                        var user = entity.User;
                        _WarnUser(client, msg, user, msg.MessageId);
                    }
                }

            }
            else base.Handle(client, msg);
        }

        private void _WarnUser(TelegramBotClient client, Message msg, User user, int replyMessageId)
        {
            var member = BotDbContext.Get().GetMember(msg.Chat.Id, user.Id);
            member.Warns++;
            if (member.Warns >= 3)
            {
                user.BanInChat(client, msg.Chat.Id, msg.MessageId);
            }
            else
            {
                var text = $"{user.GetMension()} предупреждение №{member.Warns}!" +
                    $"Бан при получении 3-х предупреждений!";
                client.SendTextMessageAsync(msg.Chat.Id, text, Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: replyMessageId);
                BotDbContext.Get().Entry(member).State = System.Data.Entity.EntityState.Modified;
                BotDbContext.Get().SaveChanges();
            }
        }
    }
}