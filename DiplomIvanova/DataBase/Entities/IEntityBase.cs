using DiplomIvanova.Enums;

namespace DiplomIvanova.DataBase.Entities
{
    public interface IEntityBase
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
    }
    public abstract class EntityBase:IEntityBase
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
    }
    public abstract class EntityWithStatusBase : EntityBase
    {
        public TripStatus Status { get; set; }
    }
    
}
