using core.general.datamodels;
using UnityEngine;


namespace core.views
{
    public class ViewBase : MonoBehaviour
    {
        public ViewsType ViewType;


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

        protected virtual void OnDisable()
        {

            RemoveListeners();
        }
    }
}