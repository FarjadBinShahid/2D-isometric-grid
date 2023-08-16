
using core.general.datamodels;
using core.managers.data;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace core.helpers.general
{
    public class Tooltip : MonoBehaviour
    {
        private static Tooltip Instance;

        [Header("UI Elements")]
        [SerializeField]
        private RectTransform bgRectTransform;
        [SerializeField]
        private TMP_Text tooltipText;


        private Dictionary<TooltipMessageType, string> tooltipMessages => DataManager.Instance.MetaData.GameMetaConstants.TooltipMessages;
        private void Awake()
        {
            Instance = this;
            gameObject.SetActive(false);
        }

        private void ShowTooltip(string text, Vector3 pos)
        {
            transform.position = pos;
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            tooltipText.text = text;
            float textPadding = 4f;
            Vector2 bgSize = new Vector2(tooltipText.preferredWidth + textPadding * 2, tooltipText.preferredHeight + textPadding * 2);
            bgRectTransform.sizeDelta = bgSize;
        }

        private void ShowTooltip(TooltipMessageType type, Vector3 pos)
        {
            transform.position = pos;
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            tooltipText.text = tooltipMessages[type];
            float textPadding = 4f;
            Vector2 bgSize = new Vector2(tooltipText.preferredWidth + textPadding * 2, tooltipText.preferredHeight + textPadding * 2);
            bgRectTransform.sizeDelta = bgSize;
        }

        private void HideTooltip()
        {
            gameObject.SetActive(false);
        }

        public static void ShowTooltip_Static(string text, Vector3 pos)
        {
            Instance.ShowTooltip(text, pos);
        }
        public static void ShowTooltip_Static(TooltipMessageType type, Vector3 pos)
        {
            Instance.ShowTooltip(type, pos);
        }

        public static void HideTooltip_Static()
        {
            Instance.HideTooltip();
        }
    }
}