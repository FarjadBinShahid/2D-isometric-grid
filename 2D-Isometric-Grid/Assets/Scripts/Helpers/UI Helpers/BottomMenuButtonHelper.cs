using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dope.wars.helpers
{
    public class BottomMenuButtonHelper : MonoBehaviour
    {
        

        [Header("Components")]
        [SerializeField] Image btnImage;
        [SerializeField] GameObject selectedIconImage;
        [SerializeField] GameObject btnText;

        [Header("Sprites")]
        public Sprite selectedSprite;
        public Sprite defaultSprite;

        public void SelectButton()
        {
            btnImage.sprite = selectedSprite;
            selectedIconImage.SetActive(true);
            btnText.SetActive(false);
        }

        public void DeselectButton()
        {
            btnImage.sprite = defaultSprite;
            selectedIconImage.SetActive(false);
            btnText.SetActive(true);
        }
    }
}