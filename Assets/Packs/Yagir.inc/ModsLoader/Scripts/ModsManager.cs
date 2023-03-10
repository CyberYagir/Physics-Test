using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;


public class ModsManager : MonoBehaviour
{
    public static ModsManager Instance;
    public string modsFolder { get; private set; }
    public string modsChainFolder { get; private set; }
    public ModsLoader modLoader { get; private set; }
    public ModAssemblyLoader modAssemblyLoader { get; private set; }

    private void Awake()
    {
        Singleton();
    }

    public void Singleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        if (Instance == this)
        {
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Init();
    }

    public void ReloadMods()
    {
        modLoader.UnloadBundles();
        Process.Start(Application.dataPath + "/../" + Application.productName + ".exe"); 
        Application.Quit();
    }

    public void Init()
    {
        modsFolder =  Application.dataPath + $"/../Mods/";
        Directory.CreateDirectory(modsFolder);
        modsChainFolder = modsFolder + "loader.chain";

        modAssemblyLoader = new ModAssemblyLoader();
        
        modLoader = new ModsLoader();
        modLoader.Init(modsFolder, modsChainFolder, modAssemblyLoader);
    }

    private void OnApplicationQuit()
    {
        for (int i = 0; i < modLoader.mods.Count; i++)
        {
            modLoader.mods[i].bundle?.Unload(true);
            modLoader.mods[i].scenesBundle?.Unload(true);
        }
    }
}
