// client for Weather Service 

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Weather_Client.Models;

namespace Weather_Client
{
    class Program
    {
        static async Task Run()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:52594/");                             // base URL for API Controller i.e. RESTFul service

                    // add an Accept header for JSON
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    
                    // get weather info for all cities
                    // GET ../weather/all
                    Console.WriteLine("============================\nGetAllWeatherInformation()");
                    HttpResponseMessage response = await client.GetAsync("/weather/all");
                    if (response.IsSuccessStatusCode)
                    {
                        // read results 
                        var weatherList = await response.Content.ReadAsAsync<IEnumerable<WeatherInformation>>();
                        foreach (var weather in weatherList)        
                        {
                            Console.WriteLine(weather);
                        }
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                    }
                    
                    
                    // get weather info for Dublin
                    // GET ../weather/Dublin
                    Console.WriteLine("============================\nGetWeatherInformationForCity(Dublin)");
                    WeatherInformation weatherInfo = new WeatherInformation();
                    try
                    {
                        //WeatherInformation weatherInfo = new WeatherInformation();
                        response = await client.GetAsync("/weather/city/dublin");
                        response.EnsureSuccessStatusCode();                         // throw exception if not success
                        weatherInfo = await response.Content.ReadAsAsync<WeatherInformation>();
                        Console.WriteLine(weatherInfo);
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine(e.Message);
                    }


                    // update by Put to weather/Dublin - now its sunny
                    Console.WriteLine("============================\nPutUpdateWeatherInformation(Dublin, weatherInfo)");
                    try
                    {
                        weatherInfo.City = "Dublin";
                        weatherInfo.Conditions = "Sunny";
                        response = await client.PutAsJsonAsync("weather/Dublin", weatherInfo);
                        response.EnsureSuccessStatusCode();
                        Console.WriteLine(weatherInfo);
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine(e.Message);
                    }


                    // get cities with weather warnings in place
                    // GET ../weather?warning=true
                    Console.WriteLine("============================\nGetCityNamesWithWeatherWarning(true)");
                    response = await client.GetAsync("/weather/warning/true");
                    if (response.IsSuccessStatusCode)
                    {
                        // read result 
                        var cities = await response.Content.ReadAsAsync<IEnumerable<String>>();
                        foreach (String city in cities)
                        {
                            Console.WriteLine(city);
                        }
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Main()
        {
            Run().Wait();
            Console.ReadLine();
        }
    }
}