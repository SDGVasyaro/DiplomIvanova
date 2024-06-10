namespace DiplomIvanova.DataBase.Entities
{
    public class ProductEntity:EntityBase
    {
        public double? Weight { get; set; }
        public string? Description { get; set; }
        public Guid? ClientId { get; set; }
        public Guid TripId { get; set; }
        public TripEntity? Trip { get; set; }
    }
}
