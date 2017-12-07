using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;
using TopDogBot.Luis;
using TopDogBot.Services;
using System.Linq;

namespace TopDogBot.Dialogs
{
    [Serializable]
    public class AnimalsListDialog : IDialog<object>
    {
        [NonSerialized]
        private ILuisService _luis;
        [NonSerialized]
        private ITopDogApi _topDogApi;

        public AnimalsListDialog()
        {
            _luis = new LuisService();
            _topDogApi = new TopDogApi();
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            var luisResponse = GetLuisResponse(activity.Text);
            var entities = string.Join(",", luisResponse.entities.Select(e => $"{e.type} = {e.entity}"));

            await context.PostAsync($"Intent: {luisResponse.topScoringIntent.intent} - {entities}");

            context.Wait(MessageReceivedAsync);
        }

        private LuisResponse GetLuisResponse(string messageText)
        {
            var luisResponse = _luis.GetResponse(messageText);

            return luisResponse;
        }
    }
}