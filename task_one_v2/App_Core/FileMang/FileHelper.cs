using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using task_one_v2.App_Core.ConstString;

namespace task_one_v2.App_Core.FileHelper;

public class FileHelper
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileHelper(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> UploadFileAsync(IFormFile formFile)
    {
        if (formFile == null || formFile.Length == 0)
        {
            throw new ArgumentException("No From File Uploded");
        }

        string wwwrootPath = _webHostEnvironment.WebRootPath;
        string fileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
        string fullPath = Path.Combine(wwwrootPath, ConstantApp.imgPath, fileName);

        using (var fileStream = new FileStream(fullPath, FileMode.Create))
        {
            await formFile.CopyToAsync(fileStream);
        }

        return fileName;
    }

    
}
