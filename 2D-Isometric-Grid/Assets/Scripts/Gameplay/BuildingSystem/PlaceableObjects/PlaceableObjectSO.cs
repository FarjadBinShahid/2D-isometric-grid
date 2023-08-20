using UnityEngine;

namespace core.gameplay.buildingsystem.placeobjects
{
    [CreateAssetMenu(fileName = "Placeable", menuName = "Scriptable Objects/PlaceableScriptableObject")]
    public class PlaceableObjectSO : ScriptableObject
    {
        public string NameString;
        public Transform Prefab;
        public Transform Visual;
        public BoundsInt Area;
    }
}