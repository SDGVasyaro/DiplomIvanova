using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomIvanova.DataBase.Entities
{
    public class RouteEntity:EntityBase
    {
        //public List<TripRequestEntity>? TripRequests { get; set; }
        //public bool IsLoading { get; set; }
        public Guid? DeparturePointId { get; set; }
        public Guid? ArrivalPointId { get; set; }
        public PickUpPointEntity? DeparturePoint { get; set; }
        public PickUpPointEntity? ArrivalPoint { get; set; }
        public List<PickUpPointEntity>? IntermediatePoints { get; set; }
        public TimeSpan? Duration { get; set; }
        //[NotMapped]
        //public double? Weight => TripRequests?.Sum(x=>x.Products?.Sum(x => x.Weight));
    }
}
