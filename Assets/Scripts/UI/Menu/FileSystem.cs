using System.IO;
using UnityEngine;

namespace UI.Menu
{
    public class FileSystem
    {
        private string root;
        private string saves;
        private string designs;
        
        public string DesingsFolder => designs;
        public FileSystem()
        {
            root = Path.Combine(Application.dataPath, "../");
            saves = root + "/Saves/";
            designs = saves + "/Desings/";
            CreateFolders();
        }



        public void CreateFolders()
        {
            Directory.CreateDirectory(saves);
            Directory.CreateDirectory(designs);
        }
    }
}