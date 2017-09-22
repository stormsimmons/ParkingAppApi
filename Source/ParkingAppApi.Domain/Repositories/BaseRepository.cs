using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Configuration;


namespace ParkingAppApi.Domain.Repositories
{
    public class BaseRepository
    {
        private readonly string _connectionstring;
        private readonly MongoClient _client;
        protected IMongoDatabase database;

        public BaseRepository(string connectionString)
        {
            _connectionstring = connectionString;
            _client = new MongoClient(_connectionstring);
            database = _client.GetDatabase("ParkingAppDatabase");
        }
    }
}
