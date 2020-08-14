using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using TraineesApi.Models;
using System;
using MongoDB.Bson;

namespace TraineesApi.Services
{
    public class TraineeService
    {
        private readonly IMongoCollection<Trainee> _trainees;
        private MongoClient Client { get; set; }
        private IMongoDatabase Database { get; set; }

        public TraineeService(ITraineesDatabaseSettings settings)
        {
            Client = new MongoClient(settings.ConnectionString);
            Database = Client.GetDatabase(settings.DatabaseName);
            _trainees = Database.GetCollection<Trainee>(settings.TraineesCollectionName);

            Seed(settings.TraineesCollectionName);
        }

        // Init collection, fill it and create an index
        public void Seed(string collectionName)
        {            
            Database.DropCollection(collectionName);
            Database.CreateCollection(collectionName);

            var trainees = new List<Trainee>
            {
                new Trainee
                {
                    Name = "Lucas Norton",
                    Company = "Skype Inc",
                    Position = "Sales manager",
                    Image = "boy.jpg",
                    Phone = "(08)7073268"
                },

                new Trainee
                {
                    Name = "Olivia Pratt",
                    Company = "Twitter Inc",
                    Position = "Sales manager",
                    Image = "girl-red.jpg",
                    Phone = "(+972)547073268"
                },

                new Trainee
                {
                    Name = "Benjamin Carlton",
                    Company = "Google Inc",
                    Position = "Developer",
                    Image = "man-mustaches.jpg",
                    Phone = "(+972)553331055"
                }
            };

            _trainees.InsertMany(trainees);

            CreateIndex();
        }

        private string CreateIndex()
        {
            IndexKeysDefinition<Trainee> keys = "{ Name: 1 }";
            var indexModel = new CreateIndexModel<Trainee>(keys);

            var ind = _trainees.Indexes.CreateOne(indexModel);

            return ind;
        }

        // use aggregation for filtering the collection
        public Task<List<Trainee>> GetByName(string name)
        {
            var match = new BsonDocument
                {
                    {
                        "$match",
                        new BsonDocument
                        {
                            {"Name", name}
                        }
                    }
                };

            var pipeline = new[] { match };
            var result = _trainees.Aggregate<Trainee>(pipeline);

            return result.ToListAsync();
        }

        public List<Trainee> Get() =>
            _trainees.Find(trainee => true).SortBy(e => e.Name)
                .ToList();

        public async Task<List<Trainee>> GetAsync() =>
            await _trainees.Find(trainee => true).SortBy(e => e.Name)
            .ToListAsync();

        public Trainee Get(string id) =>
            _trainees.Find<Trainee>(trainee => trainee.Id == id).FirstOrDefault();

        public Trainee Create(Trainee trainee)
        {
            _trainees.InsertOne(trainee);
            return trainee;
        }

        public void Update(string id, Trainee traineeUpdate) =>
            _trainees.ReplaceOne(trainee => trainee.Id == id, traineeUpdate);        
        
        // Example of using db Transaction
        public async Task<bool> UpdateInTransaction(string id, Trainee traineeUpdate)
        {
            using var session = await Client.StartSessionAsync();
            session.StartTransaction();

            try
            {
                _trainees.ReplaceOne(session, trainee => trainee.Id == id, traineeUpdate);
                await session.CommitTransactionAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing to MongoDB: " + e.Message);
                await session.AbortTransactionAsync();
                return false;
            }
        }

        public void Remove(Trainee traineeRemove) =>
            _trainees.DeleteOne(trainee => trainee.Id == traineeRemove.Id);
        
        public void Remove(string id) =>
            _trainees.DeleteOne(trainee => trainee.Id == id);                    
    }
}
