
using core.general.datamodels;
using core.helpers.general;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dope.wars.helpers
{
    public class ShowTooltipEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        public TooltipMessageType MessageType;
        public Transform TooltipPositionTransform;

        private Vector3 tooltipPos;

        private void Start()
        {
            if (TooltipPositionTransform != null)
            {
                tooltipPos = TooltipPositionTransform.position;
            }
            else
            {
                tooltipPos = gameObject.transform.position;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Tooltip.ShowTooltip_Static(MessageType, tooltipPos);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.HideTooltip_Static();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Tooltip.HideTooltip_Static();
        }
    }
}