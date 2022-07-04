using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tadrebat.Services
{
    public static class ServiceHelper
    {
        public static void Log(string Message)
        {
            //Environment.CurrentDirectory, 
            string docPath = @"C:\_NRG\IIS Projects\TadrebatAPT-Testing\_Logs\";
            if (!Directory.Exists(docPath))
                Directory.CreateDirectory(docPath);
            
            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "Logs" + DateTime.Now.ToString("yyyyMMdd hhmmss") + ".txt")))
            {
                    outputFile.WriteLine(Message);
            }
        }
    }
}
