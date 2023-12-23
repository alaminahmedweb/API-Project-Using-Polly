namespace API_Project_With_Polly
{
    public interface IWeatherServie
    {
        Task<string> Get(string cityName);
    }

    public class WeatherService : IWeatherServie
    {
        private HttpClient _httpClient;
        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Get(string cityName)
        {
            string apiKey = "9cd92637851c4d84a16195332232312";
            string apiUrl=$"?key={apiKey}&q={cityName}";
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();

        }
    }
}
