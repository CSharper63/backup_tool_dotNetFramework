using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace backup_tool_dotNetFramework.Utils
{
  class Toolbox
  {
    public Toolbox()
    {

    }

    public void Log(string message, ConsoleColor color)
    {
      Console.ForegroundColor = color;
      Console.WriteLine(message);
      Console.ResetColor();
    }

    public List<string> GetFilesDir(string dirPath)
    {
      
      DirectoryInfo dir = new DirectoryInfo(@dirPath);
      string[] filesFromDir = Directory.GetFiles(dir.FullName, "*", SearchOption.AllDirectories);

      List<string> lstFile = new List<string>();

      Log("-- Listing of " + dir.FullName, ConsoleColor.Green);
      foreach (var file in filesFromDir)
      {
        FileInfo eachFile = new FileInfo(file);

        
        lstFile.Add(eachFile.FullName);
        Log("Adding " + eachFile.FullName + " to list", ConsoleColor.Green);
      }

      Log("-- End task", ConsoleColor.Green);

      return lstFile;
    }

    static string CalculateMD5(string filename)
    {
      using (var md5 = MD5.Create())
      {
        using (var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
        {
          var hash = md5.ComputeHash(fileStream);
          return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
      }
    }

    public void Backup(List<string> lstFiles, string destDirPath)
    {
      //lstFiles = new List<string>();
      DirectoryInfo destDir = new DirectoryInfo(@destDirPath);
      Log("-- Start backup task", ConsoleColor.White);
      foreach (var file in lstFiles)
      {
        FileInfo file2Copy = new FileInfo(file);
        string destFile = Directory.GetFiles(destDir.FullName, file2Copy.Name, SearchOption.AllDirectories).FirstOrDefault();
        if (File.Exists(destFile))
        {
          if (destFile != null)
          {
            FileInfo oldFile = new FileInfo(destFile);
            var md5OldFile = CalculateMD5(oldFile.FullName);
            var md5NewFile = CalculateMD5(file2Copy.FullName);

            if (md5NewFile != md5OldFile)
            {
              Log("copying " + file2Copy.FullName + " to " + oldFile.FullName, ConsoleColor.White);
              File.Copy(file2Copy.FullName, oldFile.FullName, true);
            }
            else
            {
              Log("No change detected in " + file2Copy.FullName, ConsoleColor.White);
            }
          }
        }
        else
        {
          //check
          string lastFolderName = Path.GetFileName(Path.GetDirectoryName(file2Copy.FullName));
          Directory.CreateDirectory(destDirPath + @"\" + lastFolderName);

          Log("copying " + file2Copy.FullName + " to " + destDir.FullName + @"\" + lastFolderName + @"\" + file2Copy.Name, ConsoleColor.White);
          File.Copy(file2Copy.FullName, destDir.FullName + @"\" + lastFolderName + @"\" + file2Copy.Name, true);
        }
      }
      Log("-- End task", ConsoleColor.White);
    }
  }
}
