using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Reporting.Core.IService
{
    public interface IDatabaseService
    {
        Task<List<string>> GetCollectionsAsync();
        Task<List<string>> GetCollectionColumnsAsync(string collectionName);

    }
}
