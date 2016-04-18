using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBootcampBot
{
    //Demo 1.2 - Basic Dialog

    [Serializable]
    public partial class IntelligentDialog : IDialog<object>
    {
        bool isRunning = false;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(messageReceived);
        }

        #region PreProcess
        private async Task preProcessMessage(IDialogContext context, Message message)
        {
            var reply = message.CreateReplyMessage();

            switch (message.Type)
            {
                case "Message":
                    {
                        return;
                    }
                case "Ping":
                    {
                        reply.Type = "Ping";
                        await context.PostAsync(reply);
                        break;
                    }
                case "BotAddedToConversation":
                    {
                        await context.PostAsync($@"**Hello there!** I'm your friendly intelligent bot.");
                        break;

                    }
                case "DeleteUserData":
                case "BotRemovedFromConversation":
                case "UserAddedToConversation":
                case "UserRemovedFromConversation":
                case "EndOfConversation":
                default:
                    {
                        break;
                    }
            }
            context.Wait(messageReceived);
        }
        #endregion
        #region MessageReceived
        private async Task messageReceived(IDialogContext context, IAwaitable<Message> argument)
        {
            var message = await argument;

            await preProcessMessage(context, message);

            
            if (message.Attachments.Any())
            {
                await imageReceived(context, message);
                return;
            }

            if (isRunning)
            {
                await stopEmotionalAnalysis(context, message);
            }
            else
            {
                await defaultAction(context, message);
            } 

        }
        #endregion
        #region Default
        private async Task defaultAction(IDialogContext context, Message message)
        {
            if (message.Text == "start")
            {
                PromptDialog.Confirm(context, startEmotionalAnalysis,
                    "I'm about to start the process and will need to use the device's camera. (camera) Is that okay?",
                    "Didn't get that! Please try again?");
            }
            else
            {
                await context.PostAsync(@"I can analyze people's emotions in *real-time*. Just tell me when to **start**.");
                context.Wait(messageReceived);
            }

        }
        #endregion
        
        
        private async Task imageReceived(IDialogContext context, Message message)
        {
            await context.PostAsync("**Yay!** You've sent me something special! (gift)");

            var imageResult = await ImageAnalyzer.DescribeImage(message.Attachments[0].ContentUrl);

            var color = imageResult.Color.AccentColor;

            await context.PostAsync($@"I see {imageResult.Description.Captions[0].Text}.");

            
            await Task.Run(() =>
            {
                var colorValueFrmHex = ColorTranslator.FromHtml("#" + color);
                AzureIoTHub.SendMessageAsync($"{(int)colorValueFrmHex.R},{(int)colorValueFrmHex.G},{(int)colorValueFrmHex.B}");
            });
            await context.PostAsync($@"Now look how I work my IoT magic using the accent color of this image... (holidayspirit)");
            

        }
        private async Task startEmotionalAnalysis(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                isRunning = true;
                
                await Task.Run(() =>
                {
                    AzureIoTHub.SendMessageAsync($"start-track");
                });
                
                await context.PostAsync("I've initiated the emotion analysis on the device and will send you the results in the form of emoji's soon. (brb)");
            }
            else
            {
                await context.PostAsync("Okay, you're the boss! (y)");
            }
            context.Wait(messageReceived);
        }
        private async Task stopEmotionalAnalysis(IDialogContext context, Message message)
        {
            if (message.Text == "stop")
            {
                isRunning = false;
                
                await Task.Run(() =>
                        {
                            AzureIoTHub.SendMessageAsync($"stop-track");
                        });
                
                await context.PostAsync("I've stopped the emotion analysis process.");
            }
            else
            {
                await context.PostAsync($"I'm busy doing emotion analysis. (movie) Tell me when to **stop**.");
                context.Wait(messageReceived);
            }

        }
        
        
        
        
    }
}

