using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


namespace core.helpers.ui
{
    public class CutoutMaskUI : Image
    {

        protected override void Start()
        {
            ToggleMask();
        }

        public override Material materialForRendering
        {
            get
            {
                Material material = new Material(base.materialForRendering);
                material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
                return material;
            }
        }

        public void ToggleMask()
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
    }
}