using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Crawler
{
    internal class Program

    {
        public static async Task Main(string[] args)

        {

            if (args[0] == null)
                throw new ArgumentNullException("Please provide the url");

            string url = args[0];

            if (!(Uri.IsWellFormedUriString(url, UriKind.Absolute)))
                throw new ArgumentException("Pass correct URL address");

            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {

                    string site = await response.Content.ReadAsStringAsync();

                    Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);

                    MatchCollection matchCollection = emailRegex.Matches(site);

                    HashSet<string> strings = new HashSet<string>();

                    foreach (Match match in matchCollection)
                        strings.Add(match.Value);

                    if (strings.Count > 0)
                        foreach (var s in strings)
                            Console.WriteLine(s);
                    else
                        Console.WriteLine("I did not find the e-mails");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while downloading the site" + e.Message);
            }
            finally {
                client.Dispose();
            }
        }
    }
}
