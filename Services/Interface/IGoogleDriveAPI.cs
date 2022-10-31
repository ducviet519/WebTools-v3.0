using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Services.Interface
{
    public interface IGoogleDriveAPI
    {
        string UploadFile(IFormFile fileUpload);
        void DownloadFile(string blobId, string savePath);
    }
}
