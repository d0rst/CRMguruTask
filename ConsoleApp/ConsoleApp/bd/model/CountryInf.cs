using System;
using Newtonsoft.Json;

namespace ConsoleApp.bd.model
{
    class CountryInf
    {
        [JsonProperty("name")]
        public String name { get; set; }

        [JsonProperty("alpha2Code")]
        public String alphaCode { get; set; }

        [JsonProperty("capital")]
        public String capital { get; set; }

        [JsonProperty("area")]
        public double area { get; set; }

        [JsonProperty("population")]
        public int population { get; set; }

        [JsonProperty("region")]
        public String region { get; set; }

    }
}
