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
        // GET api/values
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
            var dirsAndFiles = new List<FileSystemObject>();

            string driveRoot = drv + ":\\";

            if (Directory.Exists(driveRoot))
            {
                DirectoryInfo[] dirs = new DirectoryInfo(driveRoot).GetDirectories();
                FileInfo[] files = new DirectoryInfo(driveRoot).GetFiles();

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
                            ParentPath = Path.GetPathRoot(dir.FullName),
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
                        ParentPath = Path.GetPathRoot(file.FullName),
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
    }
}
