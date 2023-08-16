using core.general.datamodels;
using core.managers.ui;
using TMPro;
using UnityEngine;


namespace core.popups
{
    public class PopupBase : MonoBehaviour
    {
        public PopupsType PopupType;

        [Header("UI Elements")]
        [SerializeField]
        protected TMP_Text headingText;

        [SerializeField]
        protected TMP_Text msgText;


        protected virtual void OnEnable()
        {
            AddListeners();
        }

        protected virtual void AddListeners()
        {

        }
        protected virtual void RemoveListeners()
        {


        }

        public virtual void UpdateUIElements(string heading, string msg)
        {
            headingText.text = heading;
            msgText.text = msg;
        }

        protected virtual void DismissPopup()
        {
            UIManager.Instance.HidePopup(PopupType);
        }


        protected virtual void OnDisable()
        {

            RemoveListeners();
        }

    }
}