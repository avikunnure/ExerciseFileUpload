using ExerciseFileUploadAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using SharedProjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;

namespace ExerciseFileUploadAPI.Repository
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly UploadSettings _uploadSettings;
        private readonly IWebHostEnvironment _env;
        public DocumentRepository(IOptions<UploadSettings> options, IWebHostEnvironment env)
        {
            _uploadSettings = options.Value;
            _env = env;
        }

        public List<MyDocuments> GetList()
        {
            List<MyDocuments> documents = new List<MyDocuments>();
            string paths = _env.ContentRootPath;
            var uploadpath = Path.Combine(paths, _uploadSettings.UploadFolderName);
            if (Directory.Exists(uploadpath))
            {
                foreach (var item in Directory.EnumerateFiles(uploadpath))
                {
                    using var filestream = new FileStream(item, FileMode.Open);
                    byte[] buffer = new byte[filestream.Length];
                    filestream.Read(buffer);
                    filestream.Close();
                    var finfo=new FileInfo(item);
                    var provider = new FileExtensionContentTypeProvider();
                    string contentType;
                    if (!provider.TryGetContentType(item, out contentType))
                    {
                        contentType = "application/octet-stream";
                    }
                    documents.Add(new MyDocuments()
                    {
                        Content = buffer,
                        FileName = finfo.Name,
                        Base64stringFile = Convert.ToBase64String(buffer),
                        ContentType = contentType
                    });
                }
            }
            return documents;
        }

        public bool UploadDocument(MyDocuments myDocuments)
        {
            try
            {

                string paths = _env.ContentRootPath;
                var uploadpath = Path.Combine(paths, _uploadSettings.UploadFolderName);
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }
                string newFullpaths = Path.Combine(uploadpath, myDocuments.FileName);
                if (System.IO.File.Exists(newFullpaths) != true)
                {
                    System.IO.File.Delete(newFullpaths);
                }
                using var filestream = new FileStream(path: newFullpaths, FileMode.Create);
                using var memorystream = new MemoryStream(myDocuments.Content);
                memorystream.CopyTo(filestream);
                memorystream.Close();
                filestream.Close();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }
    }
}
