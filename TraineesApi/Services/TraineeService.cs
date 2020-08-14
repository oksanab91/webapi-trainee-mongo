using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using TraineesApi.Models;

namespace TraineesApi.Services
{
    public class TraineeService
    {
        private readonly IMongoCollection<Trainee> _trainees;

        public TraineeService(ITraineesDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _trainees = database.GetCollection<Trainee>(settings.TraineesCollectionName);
        }

        public List<Trainee> Get() =>
            _trainees.Find(trainee => true).ToList();

        public Trainee Get(string id) =>
            _trainees.Find<Trainee>(trainee => trainee.Id == id).FirstOrDefault();

        public Trainee Create(Trainee trainee)
        {
            _trainees.InsertOne(trainee);
            return trainee;
        }

        public void Update(string id, Trainee traineeUpdate) =>
            _trainees.ReplaceOne(trainee => trainee.Id == id, traineeUpdate);

        public void Remove(Trainee traineeRemove) =>
            _trainees.DeleteOne(trainee => trainee.Id == traineeRemove.Id);

        public void Remove(string id) =>
            _trainees.DeleteOne(trainee => trainee.Id == id);
    }
}
