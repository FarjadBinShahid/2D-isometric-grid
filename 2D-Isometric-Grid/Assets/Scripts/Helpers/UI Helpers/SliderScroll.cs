using UnityEngine;
using UnityEngine.UI;


namespace core.helpers.ui
{
    public class SliderScroll : MonoBehaviour
    {
        [Header("Scroll UI Elements")]
        [SerializeField]
        private ScrollRect scrollRect;
        [SerializeField]
        private Slider scrollSlider;
        [SerializeField]
        private Button btn_ScrollDown;
        [SerializeField]
        private Button btn_ScrollUp;

        private void OnEnable()
        {
            AddListeners();
        }
        private void AddListeners()
        {
            scrollRect.onValueChanged.AddListener(SyncSlideraAndRect);
            btn_ScrollDown.onClick.AddListener(ScrollUp);
            btn_ScrollUp.onClick.AddListener(ScrollDown);
            scrollSlider.onValueChanged.AddListener(UpdateScrollPosition);
        }

        private void RemoveListeners()
        {
            scrollRect.onValueChanged.RemoveAllListeners();
            btn_ScrollDown.onClick?.RemoveAllListeners();
            btn_ScrollUp.onClick?.RemoveAllListeners();
            scrollSlider.onValueChanged.RemoveAllListeners();
        }

        private void ScrollUp()
        {
            scrollSlider.value += 0.1f;
        }

        private void ScrollDown()
        {
            scrollSlider.value -= 0.1f;
        }

        private void UpdateScrollPosition(float value)
        {
            scrollRect.verticalNormalizedPosition = value;
        }

        private void SyncSlideraAndRect(Vector2 position)
        {
            scrollSlider.value = scrollRect.verticalNormalizedPosition;
        }

        private void OnDisable()
        {
            RemoveListeners();
        }
    }
}