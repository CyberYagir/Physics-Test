using DG.Tweening;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Builder.UI
{
    public class UIOpenSubFolder : UICreateButton
    {
        [SerializeField] private RectTransform folderHeader;
        [SerializeField] private TMP_Text text;
        private Folder folder;
        private UICreateWindow createWindow;
        private const float upPos = 49.5f;
        private const float downPos = 56.77f;


        public void Init(Folder folder, UICreateWindow window)
        {
            this.folder = folder;
            this.createWindow = window;
            
            folderHeader.DOAnchorPosY(upPos, 0);
            
            text.text = folder.Name.Remove(0, 3) + $"[{folder.Items.Count}]";
        }

        public void Click()
        {
            createWindow.MoveFolder(folder);
            EventSystem.current.SetSelectedGameObject(null);
        }
        
        public override void Enter()
        {
            base.Enter();
            
            folderHeader.DOKill();
            folderHeader.DOAnchorPosY(downPos, 0.2f).SetLink(folderHeader.gameObject);
        }

        public override void Exit()
        {
            base.Exit();
            
            folderHeader.DOKill();
            folderHeader.DOAnchorPosY(upPos, 0.2f).SetLink(folderHeader.gameObject);
        }

        public void ChangeFolder(Folder newFolder)
        {
            folder = newFolder;
        }
    }
}
