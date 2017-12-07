using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;
using TopDogBot.Services;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using TopDogBot.Extensions;

namespace TopDogBot.Dialogs
{
    [Serializable]
    [LuisModel("59832cdf-c413-4060-ab6f-a03429b535a4", "9170b329cac7443f88ff78fc9514ff16")]
    public class AnimalsDialog : LuisDialog<object>
    {
        [LuisIntent("GetAnimals")]
        public async Task GetAnimal(IDialogContext context, LuisResult result)
        {
            var reply = context.MakeMessage();

            var animals = GetAnimals(result.Entities);

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetAttachmentsForAnimals(animals);

            await context.PostAsync(reply);

            context.Wait(MessageReceived);
        }

        [LuisIntent("GetAnimalDetails")]
        public async Task GetAnimalDetails(IDialogContext context, LuisResult result)
        {
            var reply = context.MakeMessage();

            var animals = GetAnimal(result.Entities);
            var animal = animals.FirstOrDefault();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetAttachmentsForAnimals(animals.Take(1), true);

            if (animal != null)
            {
                var childFriendly = (animal.IsDogFriendly.StartsWith("A -")) ? "I love kids" : "I'm not so keen on children";
                var genderDesc = (animal.Gender.ToLower() == "female") ? "she's" : "he's";
                DateTime date;
                DateTime.TryParse(animal.Age, out date);
                DateTime zeroTime = new DateTime(1, 1, 1);
                TimeSpan span = DateTime.Now - date;
                int yearsOld = (zeroTime + span).Year - 1;


                reply.Text = $"Hi, I'm {animal.Name}. I'm a {yearsOld} year old {animal.Gender.ToLower()} {animal.Breed} & {childFriendly}";
            }
                
            await context.PostAsync(reply);

            context.Wait(MessageReceived);
        }

        private IList<Attachment> GetAttachmentsForAnimals(IEnumerable<Animal> animals, bool hero = false)
        {
            var attachments = new List<Attachment>();

            foreach(var animal in animals.Take(5))
            {
                if (hero)
                {
                    attachments.Add(GetHeroCard(animal.Name,
                        animal.Breed,
                        animal.Gender,
                        new CardImage(url: animal.ImageUrl),
                        new CardAction(ActionTypes.OpenUrl, "Learn more", value: animal.Path)));
                }
                else
                {
                    attachments.Add(GetThumbnailCard(animal.Name,
                        animal.Breed,
                        animal.Gender,
                        new CardImage(url: animal.ImageUrl),
                        new CardAction(ActionTypes.OpenUrl, "Learn more", value: animal.Path)));
                }

            }

            return attachments;
        }

        private IEnumerable<Animal> GetAnimals(IList<EntityRecommendation> entities)
        {
            var topDogApi = new TopDogApi();

            if (entities.IsDog())
                return topDogApi.GetCats();

            if (entities.IsCat())
                return topDogApi.GetDogs();

            return Enumerable.Empty<Animal>();
        }

        private IEnumerable<Animal> GetAnimal(IList<EntityRecommendation> entities)
        {
            var topDogApi = new TopDogApi();

            if (entities.HasName())
                return topDogApi.GetDogsByName(entities.GetName());

            return Enumerable.Empty<Animal>();
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