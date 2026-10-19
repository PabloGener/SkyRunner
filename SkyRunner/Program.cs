using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.FlightSimulator.SimConnect;

namespace SkyRunner
{
    internal static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string jsonPath = SyncJsonFile("SkyRunner", "airports.json");



            Application.Run(new frmMain(jsonPath));


        }


        static string SyncJsonFile(string appName, string fileName) {
            string srcFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", fileName);

            string appFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                appName);
            string destFile = Path.Combine(appFolder, fileName);

            if (!File.Exists(srcFile))
                throw new FileNotFoundException(
                    $"Shipped JSON not found next to the .exe. Expected: {srcFile}", srcFile);

            Directory.CreateDirectory(appFolder);

            bool needCopy = !File.Exists(destFile)
                         || File.GetLastWriteTimeUtc(srcFile) > File.GetLastWriteTimeUtc(destFile)
                         || new FileInfo(srcFile).Length != new FileInfo(destFile).Length;

            if (needCopy) {
                string tmp = destFile + ".tmp";
                File.Copy(srcFile, tmp, overwrite: true);
                File.Copy(tmp, destFile, overwrite: true);
                File.Delete(tmp);
            }

            return destFile;
        }

    }
}
