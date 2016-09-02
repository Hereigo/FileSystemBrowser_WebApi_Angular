using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 39
                string root = @"E:\BRAIN_ACADEMy";

                string[] filesPathes = Directory.GetFiles(root, "*.*", SearchOption.AllDirectories);

                List<FileInfo> files = new List<FileInfo>();

                foreach (string filePath in filesPathes)
                {
                    files.Add(new FileInfo(filePath));
                }

                var lessThan10 = from f in files where (f.Length > (10*1024*1024) && f.Length <= (50 * 1024 * 1024)) select f;

                foreach (var item in lessThan10)
                {
                    Console.WriteLine(item.Name + " - " + (item.Length/1024/1024) );
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }

            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}
