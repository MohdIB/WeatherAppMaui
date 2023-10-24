using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnGetWeatherClicked(object sender, EventArgs e)
        {
            try
            {
                var latitude = Convert.ToDouble(LatitudeEntry.Text);
                var longitude = Convert.ToDouble(LongitudeEntry.Text);

                string apiKey = "ZDPeCXqylGZWEYlxA5v9XraNAoyJZTXL"; // Replace with your AccuWeather API key
                string url = $"https://dataservice.accuweather.com/forecasts/v1/minute?q={latitude}%2C{longitude}&apikey={apiKey}&language=en-us";

                using (HttpClient client = new())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    //Console.WriteLine(response);


                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        List<WeatherData> weatherDataList = ParseWeatherData(jsonResponse);

                        Console.WriteLine(jsonResponse);
                        
                        Console.WriteLine($"{weatherDataList[0].SummaryPhrase}" + " aDDITION");
                        Console.WriteLine("Hghgh old" + "123");


                        SummaryPhraseLabel.Text = $"Summary Phrase:{weatherDataList[0].SummaryPhrase}";
                        MobileLinkLabel.Text = $"Mobile Link: {weatherDataList[0].MobileLink}";
                        LinkLabel.Text = $"Link: {weatherDataList[0].Link}";

                        Console.WriteLine(SummaryPhraseLabel.Text);



                        //WeatherListView.ItemsSource = weatherDataList;

                        /*
                        foreach (var parameter in weatherDataList)
                        {
                            Label label = new Label
                            {
                                Text = $"{parameter.SummaryPhrase}",
                                FontSize = 20
                            };

                            Label startMinuteLabel = new Label
                            {
                                Text = $"Start Minute: {parameter.StartMinute}"
                            };

                            Label endMinuteLabel = new Label
                            {
                                Text = $"End Minute: {parameter.EndMinute}"
                            };

                            // Add more labels as needed for other properties

                            ResponseStackLayout.Children.Add(label);
                            ResponseStackLayout.Children.Add(startMinuteLabel);
                            ResponseStackLayout.Children.Add(endMinuteLabel);
                            // Add more labels as needed for other properties
                        }
                        */

                    }
                    else
                    {
                        Console.WriteLine("Failed");

                        throw new Exception("Failed to retrieve weather data.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., display an error message)
            }
        }

        /*
        private async Task<Dictionary<string, string>> GetWeatherDataAsync(double latitude, double longitude)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiKey = "Ajj9lj34CiTpWDGQcCqJ0ys6Qj1YGUi0"; // Replace with your AccuWeather API key

                string url = $"http://dataservice.accuweather.com/forecasts/v1/minute/1hour/{latitude},{longitude}?apikey={apiKey}";

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Dictionary<string, string> weatherData = ParseWeatherData(jsonResponse);
                    return weatherData;
                }
                else
                {
                    throw new Exception("Failed to retrieve weather data.");
                }
            }
        }
        */
        private static List<WeatherData> ParseWeatherData(string jsonResponse)
        {
            List<WeatherData> weatherDataList = new();

            try
            {
                // Deserialize the JSON response
                var root = JsonDocument.Parse(jsonResponse).RootElement;
                var summary = root.GetProperty("Summary");
                var summaries = root.GetProperty("Summaries");

                foreach (var summaryItem in summaries.EnumerateArray())
                {
                    WeatherData weatherData = new WeatherData
                    {
                        SummaryPhrase = summary.GetProperty("Phrase").GetString(),
                        //SummaryType = summary.GetProperty("Type").GetString(),
                        //SummaryTypeId = summary.GetProperty("TypeId").GetString(),
                        //StartMinute = summaryItem.GetProperty("StartMinute").GetInt32(),
                        //EndMinute = summaryItem.GetProperty("EndMinute").GetInt32(),
                        //CountMinute = summaryItem.GetProperty("CountMinute").GetInt32(),
                        MinuteText = summaryItem.GetProperty("MinuteText").GetString(),
                        //SummariesType = summaryItem.GetProperty("Type").GetString(),
                        //SummariesTypeId = summaryItem.GetProperty("TypeId").GetString(),
                        MobileLink = root.GetProperty("MobileLink").GetString(),
                        Link = root.GetProperty("Link").GetString()
                    };

                    weatherDataList.Add(weatherData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Inside Parse Catch");
                // Handle exception (e.g., log or display an error message)
                Console.WriteLine(ex.Message);

            }

            return weatherDataList;
        }

        // Assuming you have a class to represent the weather data
        public class WeatherData
        {
            public string SummaryPhrase { get; set; }
            public string SummaryType { get; set; }
            public string SummaryTypeId { get; set; }
            public int StartMinute { get; set; }
            public int EndMinute { get; set; }
            public int CountMinute { get; set; }
            public string MinuteText { get; set; }
            public string SummariesType { get; set; }
            public string SummariesTypeId { get; set; }
            public string MobileLink { get; set; }
            public string Link { get; set; }
        }
    }
}