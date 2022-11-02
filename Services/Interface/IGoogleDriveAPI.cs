using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface IGoogleDriveAPI
    {
        string UploadFile(string path);
        void DownloadFile(string blobId, string savePath);
        Task<List<GoogleDriveFile>> GetDriveFiles();

        Task<List<GoogleDriveFile>> SearchDriveFiles(string searchString);
    }
}
