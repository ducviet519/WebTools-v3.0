using System.Collections.Generic;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface IDanhMucBHTNServices
    {
        public Task<List<DanhMucBHTN>> GetDanhMucBHTN(string loai);
    }
}
