using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FileSystemBrowser_WebApi_Angular.Models;
using System.IO;

namespace FileSystemBrowser_WebApi_Angular.Controllers
{
    public class FSObjectsController : ApiController
    {
        protected List<FileSystemObject> GetInfo(string rootPath)
        {
            var dirsAndFiles = new List<FileSystemObject>();

            if (Directory.Exists(rootPath))
            {
                DirectoryInfo[] dirs = new DirectoryInfo(rootPath).GetDirectories();
                FileInfo[] files = new DirectoryInfo(rootPath).GetFiles();

                foreach (DirectoryInfo dir in dirs)
                {
                    try
                    {
                        string[] filesPathes = Directory.GetFiles(dir.FullName, "*.*", SearchOption.AllDirectories);

                        List<FileInfo> containedFiles = new List<FileInfo>();

                        // LIST OF ALL FILES CONTAINED IN DIR WITH SUB-DIRS
                        foreach (string filePath in filesPathes) containedFiles.Add(new FileInfo(filePath));

                        dirsAndFiles.Add(new FileSystemObject
                        {
                            Name = dir.Name,
                            ParentPath = rootPath,
                            IsDirectory = true,
                            FilesLess10 = (from f in containedFiles where f.Length <= (10 * 1024 * 1024) select f).Count(),
                            NotMore50 = (from f in containedFiles
                                         where (f.Length > (10 * 1024 * 1024) && f.Length <= (50 * 1024 * 1024))
                                         select f).Count(),
                            MoreThan100 = (from f in containedFiles where f.Length >= (100 * 1024 * 1024) select f).Count(),
                        });
                    }
                    catch (Exception)
                    {
                        // TODO :
                        // POSSIBLE ACCESS IS DENIED.
                        // LOG THIS AND PROCCESS NEXT...
                    }
                }

                foreach (FileInfo file in files)
                {
                    dirsAndFiles.Add(new FileSystemObject
                    {
                        Name = file.Name,
                        ParentPath = rootPath,
                        IsDirectory = false,
                        FilesLess10 = file.Length <= (10 * 1024 * 1024) ? 1 : 0,
                        NotMore50 = (file.Length > (10 * 1024 * 1024) &&
                        file.Length <= (50 * 1024 * 1024)) ? 1 : 0,
                        MoreThan100 = file.Length >= (100 * 1024 * 1024) ? 1 : 0
                    });
                }
            }

            if (dirsAndFiles.Count < 1)
            {
                dirsAndFiles.Add(new FileSystemObject { Name = "Drive is not available or empty!" });
            }

            return dirsAndFiles;
        }


        public IEnumerable<string> Get()
        {
            var drivesList = new List<string>();

            foreach (char letter in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            {
                string drivePath = letter + ":\\";

                if (Directory.Exists(drivePath))
                {
                    drivesList.Add(drivePath);
                }
            }

            if (drivesList.Count < 1)
            {
                drivesList.Add("Available Drives Not Found!");
                drivesList.Add("Доступні диски не знайдені!");
            }

            return drivesList;
        }


        [Route("api/FSObjects/{drv}")]
        public IEnumerable<FileSystemObject> GetFromDrive(char drv)
        {
            return GetInfo(drv + ":\\");
        }


        [HttpPost]
        [Route("api/FSObjects/{drv}/SubDirs")]
        public IEnumerable<FileSystemObject> SubDirs([FromBody]FileSystemObject dirObj)
        {
            return GetInfo(dirObj.ParentPath + "\\" + dirObj.Name);
        }
    }
}
