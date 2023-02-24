using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base.MapBuilder;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

namespace Builder.UI
{

    public static class ItemsUtility
    {
        public static string[] GetFolders(string path)
        {
            if (path.Last() == '/')
            {
                path = path.Remove(path.Length - 1, 1);
            }

            return path.Split('/');
        }

        public static string GetName(string fullName)
        {
            var folders = GetFolders(fullName);

            return folders.Last();
        }

    }

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

                window.RenderInFolder(folder);
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
        
        [System.Serializable]
        public class SpawnMode
        {
            public enum SpawnType
            {
                Single,
                Multispawn
            }

            [System.Serializable]
            public class TextSelection
            {
                [SerializeField] private TMP_Text text;
                [SerializeField] private Image fill;
                [SerializeField] private SpawnType type;

                public void Animate(SpawnType current)
                {
                    float height = 53;
                    float speed = Time.deltaTime * 10;
                    if (current == type)
                    {
                        text.color = Color.Lerp(text.color, Color.gray, speed);
                    }
                    else
                    {
                        text.color = Color.Lerp(text.color, Color.white, speed);
                    }

                    fill.rectTransform.sizeDelta = Vector2.Lerp(fill.rectTransform.sizeDelta, new Vector2(fill.rectTransform.sizeDelta.x, current == type ? height : 0), speed);
                }
            }


            [SerializeField] private SpawnType type;
            [SerializeField] private List<TextSelection> hotkeys;

            public SpawnType Type => type;


            public void Update()
            {
                type = SpawnType.Single;
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    type = SpawnType.Multispawn;
                }

                foreach (var key in hotkeys)
                {
                    key.Animate(Type);
                }
            }
        }

        [SerializeField] private PageDrawer pageDrawer;
        [SerializeField] private SpawnMode spawnMode;
        [SerializeField] private Camera renderCamera;
        [SerializeField] private Light light;

        private UIOpenWindow openClose;
        
        
        private Folder holder = new Folder("Holder", null);
        public Folder Holder => holder;


        public override void Init(Manager windowManager)
        {
            base.Init(windowManager);
            openClose = GetComponent<UIOpenWindow>();
            pageDrawer.Init(this);
            windowManager.StartCoroutine(CreateItems());
        }

        IEnumerator CreateItems()
        {
            light.transform.SetParent(null);
            light.enabled = false;
            
            List<Folder> allFolders = new List<Folder>(10);

            foreach (var item in Manager.ItemsService.BuildParts)
            {
                if (item.PartName.Trim().Length != 0)
                {
                    var path = ItemsUtility.GetFolders(item.PartName);

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
                    
                    

                    
                    if (lastFolder != null)
                    {
                        lastFolder.Add(new Item(ItemsUtility.GetName(item.PartName), null, item));
                    }


                }

                yield return null;
            }

            for (int i = 0; i < allFolders.Count; i++)
            {
                allFolders[i].SortItems();
            }

            MoveFolder(holder);
        }
        IEnumerator RenderObject(Camera cam, BuildPart item, Item folderItem)
        {
            yield return null;
            var obj = Instantiate(item.gameObject, Vector3.zero, Quaternion.identity);
            light.enabled = true;
            
            ChangeLayer(obj.transform);
            var bounds = new Bounds(Vector3.zero, Vector3.zero);
            var mesh = obj.GetComponentsInChildren<MeshRenderer>();
            
            if (mesh.Length != 0)
            {
                cam.targetTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.Default);
               
                for (int i = 0; i < mesh.Length; i++)
                {
                    bounds.Encapsulate(mesh[i].bounds);
                }

                cam.transform.position = bounds.center + bounds.size;
                obj.transform.position = item.PartPreview.Position;
                cam.transform.LookAt(bounds.center);
                obj.transform.rotation = Quaternion.Euler(item.PartPreview.Rotation);

                cam.Render();

                folderItem.SetTexture(cam.targetTexture);
            }
            else
            {
                cam.targetTexture = null;
            }

            light.enabled = false;
            obj.gameObject.SetActive(false);
            yield return null;
            Destroy(obj.gameObject);
        }

        IEnumerator RenderFolderItems(Folder folder)
        {
            for (int i = 0; i < folder.Items.Count; i++)
            {
                if ((folder.Items[i] is Item))
                {
                    var item = (folder.Items[i] as Item);

                    if (item.Icon == null)
                    {
                        yield return StartCoroutine(RenderObject(renderCamera, item.Part, item));
                    }
                }
            }
        }

        private void RenderInFolder(Folder folder)
        {
            StartCoroutine(RenderFolderItems(folder));
        }


        void ChangeLayer(Transform trns)
        {
            trns.gameObject.layer = LayerMask.NameToLayer("Gizmo");
            var render = trns.GetComponent<MeshRenderer>();
            if (render)
            {
                render.renderingLayerMask = (uint)LightLayerEnum.LightLayer5;
            }
            
            
            foreach (Transform child in trns)
            {
                ChangeLayer(child);
            }
        }
        
        public void MoveFolder(Folder fld)
        {
            pageDrawer.DrawPage(fld);
        }

        private void Update()
        {
            spawnMode.Update();
        }


        public void CreateItem(Item item)
        {
            var obj = Manager.PlayerService.SpawnItem(item.Prefab, item.Name);
            if (spawnMode.Type == SpawnMode.SpawnType.Single)
            {
                openClose.OpenClose();
            }
        }
    }
}
