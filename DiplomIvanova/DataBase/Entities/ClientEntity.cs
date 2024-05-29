namespace DiplomIvanova.DataBase.Entities
{
    public class ClientEntity:EntityBase
    {
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string? Adress { get; set; }
    }
}
