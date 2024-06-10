namespace DiplomIvanova.DataBase.Entities
{
    public class CarEntity:EntityBase
    {
        public string? Number { get; set; }
        public DateTime? DateOfCommissioning { get; set; }
        public int? Mileage { get; set; }
        public double? Сapacity { get; set; }
    }
}
