using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Vuforia.SharpZipLib.Zip;

namespace Assets.Build
{
    public class PostBuildSetup : IPostprocessBuildWithReport
    {
        private static string WINPIX_FILENAME = "WinPixEventRuntime.dll";
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            try
            {
                var directoryPath = Path.GetDirectoryName(report.summary.outputPath);
                var zip = ZipFile.Create($"F:\\Builds\\Tidehunter\\Project Anchorang {DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year} {DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}.zip");
                zip.BeginUpdate();
                foreach (var file in report.files)
                {
                    zip.Add(file.path);
                }
                zip.Add($"{directoryPath}\\{WINPIX_FILENAME}");
                zip.CommitUpdate();
                zip.Close();
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Exception during Post Process - {ex}");
            }

        }
    }
}