using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileSystemBrowser_WebApi_Angular.Models
{
    public class FileSystemObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ParentPath { get; set; }
        public bool IsDirectory { get; set; }
        public int FilesLess10 { get; set; }
        public int NotMore50 { get; set; }
        public int MoreThan100 { get; set; }
    }
}