using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TopDogBot.Luis
{
    public interface ILuisService 
    {
        LuisResponse GetResponse(string message);
    }
}