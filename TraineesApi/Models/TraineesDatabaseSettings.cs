
namespace TraineesApi.Models
{
    public class TraineesDatabaseSettings: ITraineesDatabaseSettings
    {
        public string TraineesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ITraineesDatabaseSettings
    {
        string TraineesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
