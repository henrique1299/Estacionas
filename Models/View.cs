using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Npgsql.Replication.PgOutput.Messages;
using System.Collections;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Estacionas.Models
{
    public class View
    {
        public View() { }

        private static HttpClient client = new()
        {
            BaseAddress = new Uri("https://dev.virtualearth.net/REST/v1/Routes/DistanceMatrix?key=AslBwsGy-hXKFSRSLSwcCTT-j-0lnXkyDbyVlhH3wiCY-d0XkWUvdfNtA0jeHpBi"),
        };

        public async Task<string> CalculateDistance(List<Estacionamentos> est, string local)
        {
            Dictionary<string, Object> origem = new Dictionary<string, Object>();
            List<Dictionary<string, decimal>> origemAux = new List<Dictionary<string, decimal>>();
            Dictionary<string, decimal> aux;

            foreach (Estacionamentos e in est)
            {
                aux = new Dictionary<string, decimal>();
                aux.Add("latitude", e.Latitude);
                aux.Add("longitude", e.Longitude);

                origemAux.Add(aux);
            }
            origem.Add("origins",origemAux);

            List<Dictionary<string, decimal>> destino = new List<Dictionary<string, decimal>>();
            aux = new Dictionary<string, decimal>();
            aux.Add("latitude", (decimal)-23.62527275);
            aux.Add("longitude", (decimal)-46.5410614);
            destino.Add(aux);

            origem.Add("destinations", destino);
            origem.Add("travelMode", "driving");

            string json = JsonSerializer.Serialize(origem);
            json = json.Replace(@"\", "");

            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await client.PostAsync(client.BaseAddress,jsonContent);

            var responseString = response.Content.ReadAsStringAsync().Result;

            return responseString;
        }

        public async Task<string>
        GetCoordinate(string cep)
        {
            string url = "http://dev.virtualearth.net/REST/v1/Locations?countryRegion=BR&postalCode="+cep+"&key=AslBwsGy-hXKFSRSLSwcCTT-j-0lnXkyDbyVlhH3wiCY-d0XkWUvdfNtA0jeHpBi";

            using HttpResponseMessage response = await client.GetAsync(url);

            string responseString = response.Content.ReadAsStringAsync().Result;

            int ini = responseString.IndexOf("coordinates") + 14;
            responseString = responseString.Substring(ini, 11);

            return responseString;

        }

    }
}
