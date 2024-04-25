using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Text.Json.Nodes;

namespace Convertion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                double money;
                Console.Write("Enter countycode1 like (USD,EUR,RUB or so) ");
                string countryCode1 = Console.ReadLine();

                Console.Write("Enter countycode2 like (USD,EUR,RUB or so) ");
                string countryCode2 = Console.ReadLine();
                Console.Write("Enter count of moneey to convert: ");
                if (double.TryParse(Console.ReadLine(), out money))
                {

                    List<object> list = ConnectWithJson();


                    Convertion(ConnectWithJson(), countryCode1, money, countryCode2);
                }
                Console.ReadKey();
                Console.Clear();
            }

        }
        static List<object> ConnectWithJson()
        {
            HttpClient httpClient = new HttpClient();

            var reques = new HttpRequestMessage(HttpMethod.Get, "https://www.nbu.uz/exchange-rates/json/");
            var responce = httpClient.SendAsync(reques).Result;

            var body = responce.Content.ReadAsStringAsync().Result;


            return JsonConvert.DeserializeObject<List<object>>(body);
        }
        static void Convertion(List<object> info, string countryCode1, double money, string countryCode2)
        {
            List<object> countries = info;
            for (int i = 0; i < countries.Count; i++)
            {
                JObject tempObject1 = JObject.Parse(JsonConvert.SerializeObject(countries[i]));
                if(countryCode1 == "UZS")
                {
                    for (int j = 0; j < countries.Count; j++)
                    {
                        JObject tempObject2 = JObject.Parse(JsonConvert.SerializeObject(countries[j]));
                        if (tempObject2["code"].ToString().Equals(countryCode2))
                        {
                            Console.WriteLine($"{money /(double)tempObject2["cb_price"] } {tempObject2["code"]} ");
                            return;
                        }
                    }
                }
                else if(countryCode2 == "UZS")
                {
                    for (int j = 0; j < countries.Count; j++)
                    {
                        JObject tempObject2 = JObject.Parse(JsonConvert.SerializeObject(countries[j]));
                        if (tempObject2["code"].ToString().Equals(countryCode1))
                        {
                            Console.WriteLine($"{(double)tempObject2["cb_price"] * money} UZS");
                            return;
                        }
                    }
                }
                else if (tempObject1["code"].ToString().Equals(countryCode1))
                {
                    for (int j = 0; j < countries.Count; j++)
                    {
                        JObject tempObject2 = JObject.Parse(JsonConvert.SerializeObject(countries[j]));
                        if (tempObject2["code"].ToString().Equals(countryCode2))
                        {
                            Console.WriteLine($"{(double)tempObject1["cb_price"] / (double)tempObject2["cb_price"] * money} {tempObject2["code"]}");
                        }

                    }
                }
            }
        }
    }
}