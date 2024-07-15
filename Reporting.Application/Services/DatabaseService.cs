using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Reporting.Core.IService;
using Reporting.Infrastracture.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Application.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IMongoDatabase _database;

        public DatabaseService(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<List<string>> GetCollectionsAsync()
        {
            var collections = await _database.ListCollectionNames().ToListAsync();
            return collections;
        }
        public async Task<List<string>> GetCollectionColumnsAsync(string collectionName)
        {
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            var firstDocument = await collection.Find(new BsonDocument()).FirstOrDefaultAsync();

            if (firstDocument == null)
            {
                return new List<string>();
            }

            return firstDocument.Names.ToList();
        }

    }

    }

