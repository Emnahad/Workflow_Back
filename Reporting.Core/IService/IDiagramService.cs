using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Reporting.Core.Entities;


namespace Reporting.Core.IService
{
    public interface IDiagramService
    {
        Task SaveDiagramAsync(Diagram diagram);
        Task<List<Diagram>> GetDiagramsAsync();
    }
}
