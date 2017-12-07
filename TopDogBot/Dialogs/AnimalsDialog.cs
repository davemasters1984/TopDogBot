using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;
using TopDogBot.Services;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace TopDogBot.Dialogs
{
    [Serializable]
    [LuisModel("59832cdf-c413-4060-ab6f-a03429b535a4", "9170b329cac7443f88ff78fc9514ff16")]
    public class AnimalsDialog : LuisDialog<object>
    {
        [LuisIntent("GetAnimals")]
        public async Task MessageReceivedAsync(IDialogContext context, LuisResult result)
        {
            var reply = context.MakeMessage();

            var animals = GetAnimals(result.Entities);

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetAttachmentsForAnimals(animals);

            await context.PostAsync(reply);

            context.Wait(MessageReceived);
        }

        private IList<Attachment> GetAttachmentsForAnimals(IEnumerable<Animal> animals)
        {
            var attachments = new List<Attachment>();

            foreach(var animal in animals.Take(5))
            {
                attachments.Add(GetHeroCard(animal.Name,
                    animal.Breed,
                    animal.Gender,
                    new CardImage(url: animal.ImageUrl),
                    new CardAction(ActionTypes.OpenUrl, "Learn more", value: animal.Path)));
            }

            return attachments;
        }

        private IEnumerable<Animal> GetAnimals(IList<EntityRecommendation> entities)
        {
            var topDogApi = new TopDogApi();

            if (entities.Any(e => e.Type.ToLower() == "animaltype" && e.Entity.ToLower().Contains("cat")))
                return topDogApi.GetCats();

            if (entities.Any(e => e.Type.ToLower() == "animaltype" && e.Entity.ToLower().Contains("dog")))
                return topDogApi.GetDogs();

            return null;
        }

        private static Attachment GetHeroCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }

        private static Attachment GetThumbnailCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new ThumbnailCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }
    }
}