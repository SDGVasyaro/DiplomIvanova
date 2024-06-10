namespace DiplomIvanova.DataBase.Entities
{
    public class TripEntity: EntityWithStatusBase
    {
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public CarEntity? Car{ get; set; }
        public string? CarName { get; set; }
        public Guid? CarId { get; set; }
        public DriverEntity? Driver { get; set; }
        public string? DriverName { get; set; }
        public Guid? DriverId { get; set; }
        public Guid? RouteId { get; set; }
        public RouteEntity? Route { get; set; }
        public string? Graphic { get; set;}
        //public int Durration => (DateTime.Now-StartAt).Days;
    }
}
