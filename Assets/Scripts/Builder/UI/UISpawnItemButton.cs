using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Builder.UI
{
    public class UISpawnItemButton : UICreateButton
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private RectTransform content;
        [SerializeField] private RawImage image;

        private bool over = false;
        private Item item;

        public void Init(Item item, UICreateWindow window)
        {
            this.item = item;
            text.text = item.Name;

            if (item.Icon != null)
                image.texture = item.Icon;
            else
                image.transform.localScale /= 2;
        }


        public override void Enter()
        {
            base.Enter();
            if (over == false)
            {
                over = true;
                image.DOKill();
                StartCoroutine(Animation());
            }
        }


        public override void Exit()
        {
            base.Exit();
            image.rectTransform.DOAnchorPos(Vector2.zero, 0.2f).SetLink(image.gameObject);
            over = false;
        }

        IEnumerator Animation()
        {
            while (over)
            {
                var pos = ScreenToRectPos(Input.mousePosition, content);
                image.rectTransform.anchoredPosition = Vector2.Lerp(image.rectTransform.anchoredPosition, new Vector2(pos.x * -20, pos.y * -20), Time.deltaTime * 10f);
                yield return null;
            }
        }


        public static Vector2 ScreenToRectPos(Vector2 screen_pos, RectTransform content)
        {
            Vector2 anchorPos = screen_pos - new Vector2(content.position.x, content.position.y);
            anchorPos = new Vector2(anchorPos.x / content.lossyScale.x, anchorPos.y / content.lossyScale.y);
            return (anchorPos/content.sizeDelta) * 2f;
        }
    }
}
