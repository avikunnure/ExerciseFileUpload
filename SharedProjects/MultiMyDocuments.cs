using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProjects
{
    public class MultiMyDocuments
    {
        public List<MyDocuments> Documents { get; set; }
        public MultiMyDocuments()
        {
            Documents = new List<MyDocuments>();
        }
    }
}
