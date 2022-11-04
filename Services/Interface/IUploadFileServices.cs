using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Services.Interface
{
    public interface IUploadFileServices
    {
        Task<string> UploadFileAsync(IFormFile fileUpload);
        Task<string> UploadMultipleFileAsync(List<IFormFile> files);
        string DeleteFile(string filePath);
    }
}
