using System.Collections.Generic;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface IDepts
    {
        public Task<List<Depts>> GetAll_DeptsAsync();
    }
}
