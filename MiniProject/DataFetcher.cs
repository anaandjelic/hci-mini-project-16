using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MiniProject
{
    /// <summary>
    /// Fetches data from Alphavantage's API and returns a list of DataPoint objects containing Timestamp and Open, High, Low, Close values for each interval.
    /// </summary>
    class DataFetcher
    {
        //private static string _apiKey = "HB6SFOGKJEOCRMWM";
        private static string _apiKey = "1JDD8ANJY6ERMK2G";

        private static Dictionary<string, List<DataPoint>> _cachedData = new Dictionary<string, List<DataPoint>>();

        /// <summary>
        /// firstCurrency and secondCurrency: three-letter symbol from the list specified in the API docs
        /// interval:  "1min", "5min", "15min", "30min", "60min", "daily", "weekly" or "monthly"
        /// </summary>
        public static List<DataPoint> FetchData(string firstCurrency, string secondCurrency, string interval)
        {
            string cachedDataKey = $"{firstCurrency} - {secondCurrency} - {interval}";
            string queryUrlString;
            string dateTimeFormat;

            if (interval.Contains("min"))
            {
                queryUrlString = $"https://www.alphavantage.co/query?function=FX_INTRADAY&from_symbol={firstCurrency}&to_symbol={secondCurrency}&interval={interval}&datatype=csv&apikey={_apiKey}";
                dateTimeFormat = "yyyy-MM-d HH:mm:ss";
            }
            else
            {
                queryUrlString = $"https://www.alphavantage.co/query?function=FX_{interval.ToUpper()}&from_symbol={firstCurrency}&to_symbol={secondCurrency}&datatype=csv&apikey={_apiKey}";
                dateTimeFormat = "yyyy-MM-d";
            }

            if (_cachedData.ContainsKey(cachedDataKey))
                return _cachedData[cachedDataKey];
            
            using (WebClient client = new WebClient())
            {
                string response = client.DownloadString(new Uri(queryUrlString));

                List<DataPoint> dataPoints = new List<DataPoint>();
                foreach (string entry in response.Split("\r\n").Skip(1))
                {
                    if (entry == "")
                        break;

                    var tokens = entry.Split(",");

                    dataPoints.Add(new DataPoint
                    {
                        TimeStamp = DateTime.ParseExact(tokens[0], dateTimeFormat, null),
                        Open = Double.Parse(tokens[1]),
                        High = Double.Parse(tokens[2]),
                        Low = Double.Parse(tokens[3]),
                        Close = Double.Parse(tokens[4])
                    });
                }

                dataPoints.Reverse();
                _cachedData.Add(cachedDataKey, dataPoints);

                return dataPoints;
            }
        }

        public static void Flush()
        {
            _cachedData.Clear();
        }

        public static List<string> GetCacheKeys()
        {
            return _cachedData.Keys.ToList();
        }
    }
}
