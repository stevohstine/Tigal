namespace Tigal.Shared.Models.Houses
{
    public class PropertyComments
    {
        public int Id { get; set; }
        public int PropertyID{get;set;}
        public string OwnerUsername{get;set;}
        public string OwnerImage{get;set;} 
        public DateTime CreatedAt{get;set;}
    }
}