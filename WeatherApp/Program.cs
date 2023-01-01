using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;

namespace WeatherApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            OpenWeatherMap.GetCurrentWeather();
            OpenWeatherMap.GetForecastedWeather();
        }
    }
}