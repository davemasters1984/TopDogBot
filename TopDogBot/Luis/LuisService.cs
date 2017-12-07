namespace TopDogBot.Luis
{
    public class LuisService 
        : WebClientBase
        , ILuisService
    {

        public LuisService()
            :base("LuisUrl")
        {

        }
        public LuisResponse GetResponse(string message)
        {
            var response = Get<LuisResponse>($"?subscription-key=9170b329cac7443f88ff78fc9514ff16&verbose=true&timezoneOffset=0&q={message}");

            return response;
        }
    }
}