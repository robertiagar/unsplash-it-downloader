using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Unsplash_It_Downloader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Downloading images...");

            DoWork().Wait();
            Console.WriteLine("Done...");
            Console.ReadKey();
        }

        public static async Task DoWork()
        {
            var client = new HttpClient();
            var jsonString = await client.GetStringAsync("https://unsplash.it/list");

            var jsonList = JsonConvert.DeserializeObject<IList<dynamic>>(jsonString);
            var max = jsonList.Count;
            int i = 1;
            foreach (var json in jsonList)
            {
                var link = (json.post_url + "/download").ToString();
                var stream = await client.GetStreamAsync(link);
                var filename = json.filename.ToString();
                UpdateCount(i, max, string.Format("Getting image {0}",filename));

                using (var fileStream = File.Create("D:\\pics\\" + filename))
                {
                    stream.CopyTo(fileStream);
                }

                i++;
            }
        }

        public static void UpdateCount(int count, int max, string message)
        {
            Console.Write("\r{0} {1} of {2}   ", message, count, max);
        }
    }
}
