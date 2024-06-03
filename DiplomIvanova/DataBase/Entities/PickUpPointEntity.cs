namespace DiplomIvanova.DataBase.Entities
{
    public class PickUpPointEntity:EntityBase
    {
        public string Name { get; set; } = "";
        public string Adress { get; set; } = "";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        public Location GetLocation() => new(Latitude, Longitude);
    }
}
