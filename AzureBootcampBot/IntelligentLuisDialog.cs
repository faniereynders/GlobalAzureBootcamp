using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBootcampBot
{
    [Serializable]
    [LuisModel("c413b2ef-382c-45bd-8ff0-f76d60e2a821", "8369d13268d14267a0218c223c1e61f7")]
    public class IntelligentLuisDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            var message = $"Sorry I did not understand: " + string.Join(", ", result.Intents.Select(i => i.Intent));
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("builtin.intent.weather.question_weather")]
        public async Task WeatherQuestion(IDialogContext context, LuisResult result)
        {
            var message = $"You've asked a question about the weather";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }
        
        [LuisIntent("builtin.intent.places.get_reviews")]
        [LuisIntent("builtin.intent.places.find_place")]
        public async Task GetReviews(IDialogContext context, LuisResult result)
        {
            var message = $"You asked me about reviews of places";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }
    }
}
