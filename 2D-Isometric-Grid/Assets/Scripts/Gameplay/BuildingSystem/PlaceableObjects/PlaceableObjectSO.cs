using core.gameplay.isometricgrid2d;
using core.general.datamodels;
using UnityEngine;

namespace core.gameplay.buildingsystem.placeobjects
{
    [CreateAssetMenu(fileName = "Placeable", menuName = "Scriptable Objects/PlaceableScriptableObject")]
    public class PlaceableObjectSO : ScriptableObject
    {
        public string NameString;
        [Tooltip("Prefab for Placeable object ie Building")]
        public Transform Prefab;
        [Tooltip("Visual of a placeable object prefab for Ghost")]
        public Transform Visual;
        [Tooltip("Area this placeable object is going to take on grid")]
        public BoundsInt Area;
        [Tooltip("Placeable object type ie house or conveyor belt")]
        public PlaceableObjectsType placeableObjectType;



        public bool CanBePlaced(Vector3 pos)
        {
            Vector3Int posInt = GridBuildingSystem.Instance.GridLayout.LocalToCell(pos);
            BoundsInt areaTemp = Area;
            areaTemp.position = posInt;
            return GridBuildingSystem.Instance.CanTakeArea(areaTemp);
        }



    }
}