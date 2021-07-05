using ConsoleApp.bd.model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.api
{
    class API
    {
        private static HttpClient httpClient;
        private static HttpResponseMessage response;

        public async Task<CountryInf> GET(string name) 
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://restcountries.eu");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var countryInf = new List<CountryInf>();
            using (httpClient)
            {
                response = await httpClient.GetAsync("/rest/v2/name/" + name);
                if (response.IsSuccessStatusCode)
                {
                    countryInf = await response.Content.ReadAsAsync<List<CountryInf>>();                     
                }              
            }
            httpClient.Dispose();
            return countryInf[0];
        }
        void POST()
        { 

        }
        void PUT()
        { 

        }
        void DELETE() 
        {

        }
    }
}
