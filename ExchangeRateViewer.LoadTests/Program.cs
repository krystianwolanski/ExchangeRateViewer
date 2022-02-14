using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateViewer.LoadTests
{
    class Program
    {
        private const string _baseUrl = "https://localhost:44330";
        private const string _apiKey = "71a5a737-41bc-4d34-b6e7-9cd0dfb646e8";

        static async Task Main(string[] args)
        {
            await RunTest();   
        }
        public static async Task RunTest()
        {
            const int MAX_ITERATIONS = 2;
            const int MAX_PARALLEL_REQUESTS = 50;
            const int DELAY = 100;
            const int RANDOM_URLS = 25;

            string[] urls = GenerateUrls(RANDOM_URLS);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("ApiKey", _apiKey);

                for (var step = 1; step <= MAX_ITERATIONS; step++)
                {
                    Console.WriteLine($"Started iteration: {step}");

                    var tasks = new List<Task>();
                    for (int i = 0; i < MAX_PARALLEL_REQUESTS; i++)
                    {
                        var url = urls[i % urls.Length];
                        int j = i;

                        tasks.Add(Task.Run(async () =>
                        {
                            var stopWatch = Stopwatch.StartNew();
                            await httpClient.GetAsync(url);
                            Console.WriteLine($"{url}\n" +
                                     $"Step - {step}, Request - {j + 1}, Time - {stopWatch.Elapsed}");
                            stopWatch.Stop();
                        }));
                    }

                    await Task.WhenAll(tasks);
                    Console.WriteLine($"Completed Iteration: {step}");


                    await Task.Delay(DELAY);
                }
            }
        }

        private static string[] GenerateUrls(uint count)
        {
            List<string> urls = new List<string>();

            Random gen = new Random();

            var sourceCurrencies = new string[] { "PLN", "GBP" };

            for (int i = 1; i <= count; i++)
            {
                var randomDates = GetRandomDates(gen);
                var startDate = randomDates.Key.ToString("yyyy-MM-dd");
                var endDate = randomDates.Value.ToString("yyyy-MM-dd");

                urls.Add($"{_baseUrl}/api/ExchangeRate?currencyCodes[{sourceCurrencies[i % 2]}]=EUR&startDate={startDate}&endDate={endDate}");
            }

            return urls.ToArray();
        }

        private static KeyValuePair<DateTime, DateTime> GetRandomDates(Random gen)
        {
            DateTime start = new DateTime(2020, 5, 1);

            int startDateRange = (DateTime.Today - start).Days;
            var startDate = start.AddDays(gen.Next(startDateRange));

            int endDateRange = (DateTime.Today - startDate).Days;
            var endDate = startDate.AddDays(gen.Next(endDateRange));

            return new KeyValuePair<DateTime, DateTime>(startDate, endDate);
        }
    }
}
