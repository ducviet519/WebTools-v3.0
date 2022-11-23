using System.Collections.Generic;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface IBangKeChiPhiSevices
    {
        Task<List<BangKeChiPhi>> GetBangKeChiPhi(string id, string loai );

        Task<List<BangKeChiPhi>> XuLyDuLieu(List<BangKeChiPhi> bangke, decimal tile, decimal sotien, string loaiApDung);
    }
}
