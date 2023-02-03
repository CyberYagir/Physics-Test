using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace UnityCustom
{
    public class ModsMoverAfterBuild
    {
        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            var buildDir = Path.GetDirectoryName(pathToBuiltProject) + "/Mods/";
            if (Directory.Exists(buildDir))
            {
                Directory.Delete(buildDir);
            }

            Directory.CreateDirectory(buildDir);
            
            
            string editorMods = Application.dataPath + $"/../Mods/";
            var mods = Directory.GetFiles(editorMods);
            foreach (var mod in mods)
            {
                File.Copy(mod, buildDir + "/" + Path.GetFileName(mod));
            }
        }
    }
}
