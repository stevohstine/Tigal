using System.Security.Cryptography.X509Certificates;

namespace Tigal.Shared.Models.Houses
{
    public class Houses
    {
        public int Id { get; set; }
        public string OwnerUsername{get;set;} = "";
        public string OwnerImage{get;set;} = "";
        public string ImageOrVideo{get;set;} = "";
        public bool ImageAvailable{get;set;} = false;
        public string ContactPhone{get;set;}  = "";
        public string AlternativeContactPhone{get;set;} = "";
        public string BusinessCategory{get;set;} = "";
        public string SubCategory{get;set;} = "";
        public string PropertyDescription{get;set;} = "";
        public string PropertyNature{get;set;} = "";
        public string PropertyTitle{get;set;} = "";
        public string PropertyType{get;set;} = "";
        public string PropertyMaterialBuild{get;set;} = "";
        public string PropertyTypeFunction{get;set;} = "";
        public string PropertyUse{get;set;} = "";
        public string Location{get;set;} = "";
        public string PropertyPrice{get;set;} = "";
        public string Currency{get;set;} = "";
        public string PropertySetup{get;set;} = "";
        public string State{get;set;} = "";
        public string Betdrooms{get;set;} = "";
        public string Bathrooms{get;set;} = "";
        public string Kitchen{get;set;} = "";
        public bool Parking{get;set;} = false;
        public string PropertyAddress{get;set;}  = "";
        public string EstateVillageName{get;set;} = "";
        public string PropertyCondition{get;set;} = "";
        public string PropertyFurnishing{get;set;} = "";
        public string PropertySize{get;set;} = "";
        public bool ServiceCharge{get;set;} = false;
        public string GuestCapacity{get;set;} = "";
        public bool Wifi{get;set;} = false;
        public bool Balcony{get;set;} = false;
        public bool Wardrobe{get;set;} = false;
        public bool BackupPowerGenerator{get;set;} = false;
        public bool PetsAllowed{get;set;} = false;
        public bool GPSCoordinates{get;set;} = false;
        public string Latitude{get;set;} = "";
        public string Longitude{get;set;} = "";
        public int Likes{get;set;} = 0;
        public int Comments{get;set;} = 0;
        public int Impressions{get;set;} = 0;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}