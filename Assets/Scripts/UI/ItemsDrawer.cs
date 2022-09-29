using System.Collections;
using System.Collections.Generic;
using Additional;
using Manager;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

namespace UI
{
    public class ItemsDrawer : MonoBehaviour
    {
        [SerializeField] private RectTransform item, holder;
        [SerializeField] private ObjectsContainer objects;

        [SerializeField] private Camera camera;

        public void Draw()
        {
            StartCoroutine(RenderObject());
        }



        IEnumerator RenderObject()
        {
            foreach (var obj in objects.Objects)
            {
                var it = Instantiate(item, holder).GetComponent<RawImage>();
                it.gameObject.SetActive(true);

                yield return null;

                var spawned = Instantiate(obj, camera.transform.position + camera.transform.forward * 2f, Quaternion.Euler(0, 45, 0));
                ChangeLayer(spawned.transform);
                var rn = new RenderTexture(512, 512, 32, GraphicsFormat.R16G16B16A16_SNorm);
                yield return null;
                camera.targetTexture = rn;
                camera.Render();
                it.texture = rn;
                yield return null;

                Destroy(spawned.gameObject);

            }
        }

        public void ChangeLayer(Transform child)
        {
            child.gameObject.layer = LayerMask.NameToLayer("HideRenderer");
            foreach (Transform c in child)
            {
                ChangeLayer(c);
            }
        }
    }
}
