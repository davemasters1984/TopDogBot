using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TopDogBot.Extensions
{
    public static class EntitiesExtensions
    {
        public static bool IsDog(this IList<EntityRecommendation> entities)
        {
            return entities.Any(e => e.Type.ToLower() == "animaltype" && e.Entity.ToLower().Contains("dog"));
        }

        public static bool IsCat(this IList<EntityRecommendation> entities)
        {
            return entities.Any(e => e.Type.ToLower() == "animaltype" && e.Entity.ToLower().Contains("cat"));
        }

        public static bool HasName(this IList<EntityRecommendation> entities)
        {
            return entities.Any(e => e.Type.ToLower() == "animalname");
        }

        public static string GetName(this IList<EntityRecommendation> entities)
        {
            return entities.Where(e => e.Type.ToLower() == "animalname")
                    .Select(e => e.Entity)
                    .FirstOrDefault();
        }
    }
}