using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TopDogBot.Services
{
    public interface ITopDogApi
    {
        IEnumerable<Animal> GetAnimals();

        IEnumerable<Animal> GetCats();

        IEnumerable<Animal> GetDogs();
    }
}