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

        public IEnumerable<Animal> GetCats()
        {
            using (var client = CreateClient())
            {
                var cats = Get<IEnumerable<Animal>>($"cats?code=2aSg6XnzAOmAnkzgX4OUq2ZZGWwLjvtxsXRrJUrp16zhhYgC7n96mA==");

                return cats;
            }
        }

        public IEnumerable<Animal> GetDogs()
        {
            using (var client = CreateClient())
            {

                var cats = Get<IEnumerable<Animal>>($"dogs?code=IRTUwEKH7VTTZvt5Sq7zCO6PLgRuzYrRIJUqHSkmvRyry/k09amaqw==");

                return cats;
            }
        }
    }
}