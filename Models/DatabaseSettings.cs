namespace chat_api.Models
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DataBase { get; set; }
    }

    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DataBase { get; set; }
    }
}