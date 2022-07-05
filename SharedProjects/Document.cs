using System;

namespace SharedProjects
{
    public class MyDocuments
    {  
        public string Base64stringFile { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}
