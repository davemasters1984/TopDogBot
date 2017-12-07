using System.Collections.Generic;

namespace TopDogBot.Services
{
    public interface ITopDogApi
    {
        IEnumerable<Animal> GetCats();

        IEnumerable<Animal> GetDogs();

        IEnumerable<Animal> GetDogsByName(string name);
    }
}