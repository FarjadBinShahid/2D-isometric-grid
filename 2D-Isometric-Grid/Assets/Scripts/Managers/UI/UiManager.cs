using core.architecture;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using core.general.datamodels;
using core.popups;
using core.views;

namespace core.managers.ui
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Popups")]
        [SerializeField] List<PopupBase> popups;

        [Header("Views")]
        [SerializeField] List<ViewBase> views;

        private PopupBase previouslyEnabledPopup;



        public Dictionary<PopupsType, PopupBase> AllPopups { get; set; }
        public Dictionary<ViewsType, ViewBase> AllViews { get; set; }


        protected override void Awake()
        {
            base.Awake();
            AllPopups = popups.ToDictionary(x => x.PopupType, y => y);
            AllViews = views.ToDictionary(x => x.ViewType, y => y); 
        }

        private void Start()
        {
        }

        #region Views

        public void ShowView(ViewsType viewType)
        {
            DisableAllViews();
            AllViews[viewType].gameObject.SetActive(true);
        }

        public void HideView(ViewsType viewType)
        {
            AllViews[viewType].gameObject.SetActive(false);
        }

        public void DisableAllViews()
        {
            for (int i = 0; i < AllViews.Count; i++)
            {
                AllViews.ElementAt(i).Value.gameObject.SetActive(false);
            }
        }

        #endregion

        #region Popups

        public void ShowPopup(PopupsType popupType, string heading, string message)
        {
            if(previouslyEnabledPopup != null)
            {
                previouslyEnabledPopup.gameObject.SetActive(false);
            }

            if (AllPopups.ContainsKey(popupType))
            {
                AllPopups[popupType].UpdateUIElements(heading, message);
                AllPopups[popupType].gameObject.SetActive(true);
                previouslyEnabledPopup = AllPopups[popupType];
            }

        }
        public void ShowPopup(PopupsType popupType)
        {
            if (previouslyEnabledPopup != null)
            {
                previouslyEnabledPopup.gameObject.SetActive(false);
            }
            if (AllPopups.ContainsKey(popupType))
            {
                AllPopups[popupType].gameObject.SetActive(true);
                previouslyEnabledPopup = AllPopups[popupType];
            }
        }

        public void HidePopup(PopupsType popupType)
        {
            if (AllPopups.ContainsKey(popupType))
            {
                AllPopups[popupType].gameObject.SetActive(false);
            }
        }

        public void DisableAllPopups()
        {
            for (int i = 0; i < AllPopups.Count; i++)
            {
                AllPopups.ElementAt(i).Value.gameObject.SetActive(false);
            }
        }
        #endregion

        #region API Loader

        public void ShowAPILoader()
        {
            ShowPopup(PopupsType.Loading);
        }


        public void DestroyAPILoader()
        {
            HidePopup(PopupsType.Loading);
        }
        #endregion
    }
}