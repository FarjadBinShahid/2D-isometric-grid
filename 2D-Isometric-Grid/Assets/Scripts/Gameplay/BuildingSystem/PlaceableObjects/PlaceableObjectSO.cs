using core.gameplay.isometricgrid2d;
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


        public bool CanBePlaced(Vector3 pos)
        {
            Vector3Int posInt = GridBuildingSystem.Instance.GridLayout.LocalToCell(pos);
            BoundsInt areaTemp = Area;
            areaTemp.position = posInt;
            return GridBuildingSystem.Instance.CanTakeArea(areaTemp);
        }



    }
}