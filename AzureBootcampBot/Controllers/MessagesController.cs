using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Microsoft.Bot.Builder.Dialogs;

namespace AzureBootcampBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        #region WebHook
        public class WebHookMessage
        {
            public string Type { get; set; }
            public dynamic Data { get; set; }
        }

        private string resolveEmoji(string emotion)
        {
            switch (emotion)
            {
                case "Angry":
                    return ":@";
                case "Contempt":
                    return "(mm)";
                case "Disgusted":
                    return "(puke)";
                case "Fearful":
                    return ":S";
                case "Happy":
                    return "(happy)";
                case "Sad":
                    return ":(";
                case "Surprised":
                    return ":o";
            }
            return "(wtf)";
        }

        [Route("~/api/messages/hook")]
        [HttpPost]
        public async Task<IHttpActionResult> WebHook([FromBody]WebHookMessage message)
        {
            if (message.Type == "EmotionUpdate")
            {
                const string fromBotAddress = "<Skype Bot ID here>";
                const string toBotAddress = "<Destination Skype name here>";
                var text = resolveEmoji(message.Data);

                using (var client = new ConnectorClient())
                {
                    var outMessage = new Message
                    {
                        To = new ChannelAccount("skype", address: toBotAddress , isBot: false),
                        From = new ChannelAccount("skype", address: $"8:{fromBotAddress}", isBot: true),
                        Text = text,
                        Language = "en",

                    };

                    await client.Messages.SendMessageAsync(outMessage);
                }
            }
            return Ok();
        }
        #endregion


        public async Task<Message> Post([FromBody]Message message)
        {

            return await Conversation.SendAsync(message, () => new IntelligentDialog());

            //Dialog with LUIS integration:
            //return await Conversation.SendAsync(message, () => new IntelligentLuisDialog());

        }
    }
}