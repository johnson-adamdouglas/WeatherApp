using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;

namespace WeatherApp
{
    public class OpenWeatherMap
    {
        public static void GetCurrentWeather()
        {
            string key = System.IO.File.ReadAllText("appsettings.json");
            string APIkey = JObject.Parse(key).GetValue("DefaultKey").ToString();
            Console.WriteLine("Enter your zip code for current weather conditions in your area.");
            var zipCode = int.Parse(Console.ReadLine());
            var client = new HttpClient();
            var owmURL = $"https://api.openweathermap.org/data/2.5/weather?zip={zipCode}&units=imperial&,&appid={APIkey}";
            var response = client.GetStringAsync(owmURL).Result;
            CurrentWeatherInfo.Root Info = JsonConvert.DeserializeObject<CurrentWeatherInfo.Root>(response);
            var city = Info.Name;
            var currentDate = DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            var currentTime = DateTime.Now.ToString("h:mm tt");
            var description = Info.Weather[0].Description;
            var temp = Math.Round(Info.Main.Temp);
            var feelsLike = Math.Round(Info.Main.FeelsLike);
            var windSpeed = Math.Round(Info.Wind.Speed);
            static string DegreesToCardinal(double degrees)
            {
                string[] caridnals = { "N", "NE", "E", "SE", "S", "SW", "W", "NW", "N" };
                return caridnals[(int)Math.Round(((double)degrees % 360) / 45)];
            }
            var windDirection = DegreesToCardinal(Info.Wind.Deg);
            var sunRise = Info.Sys.Sunrise;
            DateTime sunRiseTranslated = new DateTime(1970, 1, 1).AddSeconds(sunRise).ToLocalTime();
            var sunSet = Info.Sys.Sunset;
            DateTime sunSetTranslated = new DateTime(1970, 1, 1).AddSeconds(sunSet).ToLocalTime();

            Console.WriteLine();
            Console.WriteLine($"Here are the current conditions in {city}:");
            Console.WriteLine();
            Console.WriteLine($"Today's date is {currentDate}");
            Console.WriteLine();
            Console.WriteLine($"The time is {currentTime}");
            Console.WriteLine();
            Console.WriteLine($"Sky: {description}");
            Console.WriteLine();
            Console.WriteLine($"Temperature: {temp} degrees");
            Console.WriteLine();
            Console.WriteLine($"Feels like: {feelsLike} degrees");
            Console.WriteLine();
            Console.WriteLine($"Wind: {windSpeed}mph {windDirection}");
            Console.WriteLine();
            Console.WriteLine($"Sunrise: {sunRiseTranslated.ToString("h:mm tt")}");
            Console.WriteLine();
            Console.WriteLine($"Sunset: {sunSetTranslated.ToString("h:mm tt")}");
            
        }

        public static void GetForecastedWeather()
        {
            string key = System.IO.File.ReadAllText("appsettings.json");
            string APIkey = JObject.Parse(key).GetValue("DefaultKey").ToString();
            Console.WriteLine("Enter your zip code for the 3-hour weather forecast in your area.");
            var zipCode = int.Parse(Console.ReadLine());
            var client = new HttpClient();
            var owmURL = $"https://api.openweathermap.org/data/2.5/forecast?zip={zipCode}&units=imperial&,&appid={APIkey}";
            var response = client.GetStringAsync(owmURL).Result;
            //Console.WriteLine(response);
            ForecastedWeatherInfo.Root Info = JsonConvert.DeserializeObject<ForecastedWeatherInfo.Root>(response);
            var city = Info.City.Name;
            Console.WriteLine(city);
            for (var i = 0; i < Info.List.Count; i++)
            {
                var dt = Info.List[i].Dt;
                DateTime dtTranslated = new DateTime(1970, 1, 1).AddSeconds(dt).ToLocalTime();
                var dtDay = dtTranslated.DayOfWeek;
                var time = dtTranslated.ToString("h:mm tt");
                var description = Info.List[i].Weather[0].Description;
                var temp = Math.Round(Info.List[i].Main.Temp);
                var windSpeed = Math.Round(Info.List[i].Wind.Speed);
                static string DegreesToCardinal(double degrees)
                {
                    string[] caridnals = { "N", "NE", "E", "SE", "S", "SW", "W", "NW", "N" };
                    return caridnals[(int)Math.Round(((double)degrees % 360) / 45)];
                }
                var windDirection = DegreesToCardinal(Info.List[i].Wind.Deg);

                Console.WriteLine();
                Console.WriteLine($"{dtDay} {time}");
                Console.WriteLine($"Sky: {description}");
                Console.WriteLine($"Temperature: {temp}");
                Console.WriteLine($"Wind: {windSpeed}mph {windDirection}");
                Console.WriteLine();
                Console.WriteLine();
            }


            
        } 
    }
}
