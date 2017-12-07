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
                var dogs = Get<IEnumerable<Animal>>($"dogs?code=IRTUwEKH7VTTZvt5Sq7zCO6PLgRuzYrRIJUqHSkmvRyry/k09amaqw==");

                return dogs;
            }
        }

        public IEnumerable<Animal> GetDogsByName(string name)
        {
            using (var client = CreateClient())
            {
                var dogs = Get<IEnumerable<Animal>>($"dogname?code=UgNNgWdZe/xk6K85zbgJq0XADRW8PX5wsbF2K6vXPCQcWa9aj3ILSw==&name={name}");

                return dogs;
            }
        }
    }
}