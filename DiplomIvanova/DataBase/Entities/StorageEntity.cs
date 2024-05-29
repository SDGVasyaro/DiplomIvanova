namespace DiplomIvanova.DataBase.Entities
{
    public class StorageEntity:EntityBase
    {
        public Guid RouteId { get; set; }
        public Guid PickUpPointId { get; set; }
        //public required string Name { get; set; }
        //public string? Adress { get; set; }
    }
}
