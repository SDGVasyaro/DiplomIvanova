namespace DiplomIvanova.DataBase.Entities
{
    public class TripRequestEntity: EntityWithStatusBase
    {
        public ClientEntity? Client {  get; set; }
        public Guid? ClientId { get; set; }
        //public string? Adress { get; set; }
        public List<ProductEntity>? Products { get; set; }
    }
}
