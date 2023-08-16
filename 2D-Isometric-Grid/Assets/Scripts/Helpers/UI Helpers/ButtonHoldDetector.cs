
using core.helpers.interfaces.ui;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dope.wars.helpers
{
    public class ButtonHoldDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [Header("Botton Hold Settings")]

        [SerializeField]
        private float pointerDownThreshhold = 0.75f;
        [SerializeField]
        private float maxChangeTime = 0.2f;
        [SerializeField]
        private float minChangeTime = 0.05f;
        [SerializeField]
        private bool isIncreaseButton;
        [SerializeField]
        private float time;

        [Header("Parent Object")]
        [SerializeField]
        private GameObject parent;


        private Button btn;
        private IAmountChangerOnHold amountChangerOnHold;
        private bool isHolding;


        private void Awake()
        {
            amountChangerOnHold = parent.GetComponent<IAmountChangerOnHold>();
            btn = gameObject.GetComponent<Button>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!btn.interactable)
                return;

            if (isIncreaseButton)
            {
                amountChangerOnHold.IncreaseAmount();
            }
            else
            {
                amountChangerOnHold.DecreaseAmount();
            }
            isHolding = true;
            StartCoroutine(IncreaseOrDecreaseAmount());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHolding = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isHolding = false;
        }


        private IEnumerator IncreaseOrDecreaseAmount()
        {
            time = maxChangeTime;
            yield return new WaitForSeconds(pointerDownThreshhold);
            while (isHolding && btn.interactable)
            {
                if (isIncreaseButton)
                {
                    amountChangerOnHold.IncreaseAmount();
                }
                else
                {
                    amountChangerOnHold.DecreaseAmount();
                }
                if (time > minChangeTime)
                {
                    time -= Time.deltaTime;
                }
                else
                {
                    time = minChangeTime;
                }

                yield return new WaitForSeconds(time);

            }

            Debug.Log("End");


            //yield return new WaitForSeconds(1f);
        }

    }
}