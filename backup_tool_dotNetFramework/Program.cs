using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backup_tool_dotNetFramework.Utils;

namespace backup_tool_dotNetFramework
{
  class Program
  {
    static void Main(string[] args)
    {
      Toolbox tool = new Toolbox();

      if (args.Length > 0)
      {
        var _args = new CommandLineArgs(args);

        if (_args[Defs.AppDefs.ArgsSourceDir] != null && _args[Defs.AppDefs.ArgsDestDir] != null)
        {
          var files2Backup = tool.GetFilesDir(@_args[Defs.AppDefs.ArgsSourceDir]);
          try
          {
            tool.Backup(files2Backup, @_args[Defs.AppDefs.ArgsDestDir]);
          }
          catch (Exception e)
          {
            tool.Log(e.Message, ConsoleColor.Red);
            throw;
          }
        }
      }
      else
      {
        tool.Log("Missing params", ConsoleColor.Red);
      }
    }
  }
}
