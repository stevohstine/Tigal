using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Tigal.Server.Models
{
    public class CreatePropertyDto
    {
        [Required]
        public string PostText{get;set;}
        [Required]
        public string PostNature{get;set;}
        [Required]
        public string PropertyType{get;set;}
        [Required]
        public string SelectHouse{get;set;}
        [Required]
        public string TransactionType{get;set;}
        [Required]
        public string Amount{get;set;}
        // [Required]
        public Location Location{get;set;}
        // [Required]
        // public string State{get;set;}
        [Required]
        public List<string> SelectedImages { get; set; }

        // public string OwnerUsername{get;set;}
        // public string OwnerImage{get;set;}
        // public string ImageOrVideo{get;set;}
        // public string ContactPhone{get;set;}
        // public string Latitude{get;set;}
        // public string Longitude{get;set;}
        // public string Location{get;set;}
    }

    public class Location
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}