using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeCounter;
using System.IO;
using DevProjects;
using DataHelpers;
using BusinessObjects;

namespace TestCodeCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            // test DevProjects
            var mp = new MaintainProject();
            //mp.UpdateSLNPathInDevProjects();
            mp.PopulateSyncTableFromDevProjects();

            
            
            
            
            var path = @"C:\VS2019 Projects\DevTracker\.git\config";
            
            DHMisc hlpr = new DHMisc();
            List<NotableFileExtension> notableFileExtensions = hlpr.GetNotableFileExtensions();
            // Tuple<string, string, string> tuple = mp.GetProjectFromGitConfigSaved(path, notableFileExtensions);

            path = @"C:\VS2019 Projects\DevTracker\Classes\FileWatcher.cs";


            //Tuple<string, string, string> tuple = mp.GetProjectFromDevFileActivity(path, notableFileExtensions, "cs");
           
            var url = mp.GetGitURLFromConfigFile(@"C:\GitRepo\hell-world\.git\config");
            url = mp.GetGitURLFromPath(@"C:\GitRepo\hell-world\helloworld.csproj");
            Console.WriteLine(url);

            // test fileanalyzer
            //var cc = new CodeCounter.FileLineCounter();
            //var ret = cc.Process(@"C:\VS2019 Projects\DevTracker\Forms\MiscContainer.cs");
            string projectPath = string.Empty;
            string devProjectName = GetProjectPath(@"C:\DevTrkr DLLs\MaintainDevProject\Properties\AssemblyInfo.cs", out projectPath, "csproj");
            Console.WriteLine("Project: " + devProjectName + "Path: " + projectPath);
            Console.ReadLine();
        }
        private static string GetProjectPath(string fileFullPath, out string projectName, string projFileExt)
        {
            if (string.IsNullOrWhiteSpace(Path.GetFileName(fileFullPath)))
            {
                projectName = string.Empty;
                return string.Empty;
            }

            var fullPath = fileFullPath;
            var dirName = string.Empty;

            while (!string.IsNullOrWhiteSpace(fullPath))
            {
                fullPath = Path.GetDirectoryName(fullPath);
                if (string.IsNullOrWhiteSpace(fullPath))
                {
                    projectName = string.Empty;
                    return string.Empty;
                }

                dirName = new DirectoryInfo(fullPath).Name;
                // Vb and c# newly created projects will have the following folders
                // that have to be bypassed, they are not interesting
                if ("bin_obj_properties_my project_debug_release_temppe_".Contains(dirName.ToLower()))
                    continue; 

                object prj = /*Globals.ProjectList.Find(x => x.DevProjectName.ToLower() == dirName.ToLower())*/ null;

                // we think we have a project path
                if (prj != null)
                {
                    projectName = dirName;
                    // we think we have a project path, chek for a xxproj file in the path
                    // projFileExt can have multiple delimited extensions
                    // mainly b/c c++ proj extension changed in later version
                    string[] extensions = projFileExt.Split('|');
                    for (int i = 0; i < extensions.Length; i++)
                    {
                        var projFile = projectName + "." + extensions[i];
                        if (File.Exists(Path.Combine(fullPath, projFile)))
                        {
                            projectName = dirName;
                            return fullPath;
                        }
                    }
                    continue;
                }
            }

            projectName = string.Empty;
            return string.Empty; // did not find a project file in the supposed project directory
        }


        //public string GetProjectPath(string fileName, bool isIDE)
        //{
        //    const string slash = @"\";

        //    var idxLast = fileName.LastIndexOf(slash);
        //    var fullPath = fileName;

        //    // strip the filename and last slash if extant
        //    if (fullPath.Length > idxLast + 1)
        //        fullPath = fullPath.Substring(0, idxLast);

        //    // now we have the fullPath like 
        //    //C:\VS2019 Projects\DataLayerUtility\DataLayerUtility\Forms
        //    // since this is an ide, strip all paths from the end != ProjectPath
        //    var name = string.Empty;
        //    while (true)
        //    {
        //        idxLast = fullPath.LastIndexOf(slash);
        //        if (idxLast < 0)
        //            break;
        //        name = fullPath.Substring(idxLast + 1);
        //        if (name.ToLower() == ProjectName.ToLower())
        //            return fullPath;
        //        fullPath = fullPath.Substring(0, idxLast);
        //    }
        //    return fullPath;
        //}

    }
}
