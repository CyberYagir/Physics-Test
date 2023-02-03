using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Builder.UI
{
    public class UICreateWindow : UIController
    {
        [System.Serializable]
        public class PageDrawer
        {
            [SerializeField] private Transform holder;
            [SerializeField] private UIOpenSubFolder folderBtn, backBtn;            
            [SerializeField] private UISpawnItemButton itemBtn;

            [SerializeField] private List<GameObject> drawedItems = new List<GameObject>(20);


            private UICreateWindow window;
            private Folder selectedFolder = null;

            public void Init(UICreateWindow window)
            {
                this.window = window;
                folderBtn.gameObject.SetActive(false);
                itemBtn.gameObject.SetActive(false);
                backBtn.gameObject.SetActive(false);
            }
            public void DrawPage(Folder folder)
            {
                selectedFolder = folder;

                for (int i = 0; i < drawedItems.Count; i++)
                {
                    Destroy(drawedItems[i].gameObject);
                }
                
                drawedItems.Clear();

                if (selectedFolder.Parent != null)
                {
                    var backFolder = CreateFolder(backBtn, folder);
                    backFolder.ChangeFolder(folder.Parent);
                }
                
                for (int i = 0; i < selectedFolder.Items.Count; i++)
                {
                    if (selectedFolder.Items[i] is Folder)
                    {
                        CreateFolder(folderBtn, selectedFolder.Items[i] as Folder);
                    }
                    else
                    {
                        var it = Instantiate(itemBtn, holder);
                        it.Init(selectedFolder.Items[i] as Item, window);
                        it.gameObject.SetActive(true);
                        
                        drawedItems.Add(it.gameObject);
                    }
                }
            }

            private UIOpenSubFolder CreateFolder(UIOpenSubFolder prefab, Folder item)
            {
                var subFolder = Instantiate(prefab, holder);
                subFolder.Init(item, window);
                subFolder.gameObject.SetActive(true);
                        
                drawedItems.Add(subFolder.gameObject);

                return subFolder;
            }
        }

        [SerializeField] private PageDrawer pageDrawer;
        
        private Folder holder = new Folder("Holder", null);
        public Folder Holder => holder;


        public override void Init(UITabsManager tabsManager)
        {
            base.Init(tabsManager);
            pageDrawer.Init(this);
            StartCoroutine(CreateItems());
        }

        IEnumerator CreateItems()
        {
            Camera cam = new GameObject("Camera Items").AddComponent<Camera>();
            HDAdditionalCameraData cameraData = cam.gameObject.AddComponent<HDAdditionalCameraData>();
            cameraData.volumeLayerMask = LayerMask.GetMask();
            cam.enabled = false;
            cam.cullingMask = LayerMask.GetMask("Gizmo");
            cam.backgroundColor = Color.clear;

            Light light = new GameObject("Light Items").AddComponent<Light>();
            light.type = LightType.Directional;
            light.transform.eulerAngles = new Vector3(40, -100, 0);
            light.shadows = LightShadows.None;
            
            
            List<Folder> allFolders = new List<Folder>(10);

            foreach (var item in tabsManager.Manager.ItemsGetter.BuildParts)
            {

                light.enabled = true;
                
                if (item.PartName.Trim().Length != 0)
                {
                    var path = GetFolders(item.PartName);

                    Folder lastFolder = Holder;
                    for (int i = 0; i < path.Length-1; i++)
                    {
                        var folderName = "fl_" + path[i];
                        var folder = lastFolder.Items.Find(x => x.Name == folderName && x is Folder) as Folder;

                        if (folder == null)
                        {
                            lastFolder = lastFolder.Add(new Folder(folderName, lastFolder)) as Folder;
                            allFolders.Add(lastFolder);
                        }
                        else
                        {
                            lastFolder = folder;
                        }
                    }
                    
                    
                    var obj = Instantiate(item.gameObject, Vector3.zero, Quaternion.identity);
                    ChangeLayer(obj.transform);
                    var mesh = obj.GetComponent<MeshRenderer>();
                    if (mesh)
                    {
                        cam.targetTexture = new RenderTexture(512, 512, 16, RenderTextureFormat.Default);
                        cam.transform.position = mesh.bounds.center + mesh.bounds.size;
                        
                        obj.transform.position = item.PartPreview.Position;
                        cam.transform.LookAt(mesh.bounds.center);
                        
                        obj.transform.rotation = Quaternion.Euler(item.PartPreview.Rotation);
                        
                        cam.Render();

                    }
                    else
                    {
                        cam.targetTexture = null;
                    }
                    
                    if (lastFolder != null)
                    {
                        lastFolder.Add(new Item(path.Last(), cam.targetTexture, item.gameObject));
                    }

                    void ChangeLayer(Transform trns)
                    {
                        trns.gameObject.layer = LayerMask.NameToLayer("Gizmo");
                        foreach (Transform child in trns)
                        {
                            ChangeLayer(child);
                        }
                    }
                    Destroy(obj.gameObject);
                }

                light.enabled = false;
                yield return null;
            }

            for (int i = 0; i < allFolders.Count; i++)
            {
                allFolders[i].SortItems();
            }

            MoveFolder(holder);
        }

        public void MoveFolder(Folder fld)
        {
            pageDrawer.DrawPage(fld);
        }
        
        public string[] GetFolders(string path)
        {
            if (path.Last() == '/')
            {
                path = path.Remove(path.Length - 1, 1);
            }

            return path.Split('/');
        }
    }
}
