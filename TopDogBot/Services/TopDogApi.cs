using System.Collections.Generic;

namespace TopDogBot.Services
{

    public class TopDogApi 
        : WebClientBase
        , ITopDogApi
    {
        public TopDogApi()
            :base("TopDogApiUrl")
        {

        }

        public IEnumerable<Animal> GetAnimals()
        {
            using (var client = CreateClient())
            {
                var animals = Get<IEnumerable<Animal>>($"/api/animals");

                return animals;
            }
        }


    }
}