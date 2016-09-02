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


// Recursive measure files in SubDirs !!!

//----------------------
//IEnumerable<dirRecord> GetFromPath(httpGetData? )
//-----------------------
//GetForDrive(driveLetter )
//GetForCurrDir(dir.Parent+dir.currName )
//GetForParent(dir.Parent )
//------------------------
//List<dirRecord> =
//{
//isDirectory = true
//parentPath = c:\program
//currentName = Chrome
//sizeContainedFilessizeA = 235438
//sizeContainedFilessizeB
//sizeContainedFilessizeC
//}, {