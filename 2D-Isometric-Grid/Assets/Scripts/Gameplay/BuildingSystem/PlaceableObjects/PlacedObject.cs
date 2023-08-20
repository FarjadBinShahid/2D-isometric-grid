using core.gameplay.isometricgrid2d;
using UnityEngine;

namespace core.gameplay.buildingsystem.placeobjects
{

    public class PlacedObject : MonoBehaviour
    {

        public bool Placed { get; private set; }
        public BoundsInt Area;

        #region Building Methods

        public virtual bool CanBePlaced(Vector3 pos)
        {
            Vector3Int posInt = GridBuildingSystem.Instance.GridLayout.LocalToCell(pos);
            BoundsInt areaTemp = Area;
            areaTemp.position = posInt;
            return GridBuildingSystem.Instance.CanTakeArea(areaTemp);
        }

        public void TakeAreaForPlacedObject(Vector3 pos)
        {
            Vector3Int posInt = GridBuildingSystem.Instance.GridLayout.LocalToCell(pos);
            BoundsInt areaTemp = Area;
            areaTemp.position = posInt;
            Placed = true;
            GridBuildingSystem.Instance.TakeArea(areaTemp);
        }


        public static PlacedObject CreatePlacedObject(Vector3 worldPosition, PlaceableObjectSO placeableObjectSO)
        {
            Transform placedObjectTransform = Instantiate(placeableObjectSO.Prefab, worldPosition, Quaternion.identity);
            PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();
            placedObject.Area = placeableObjectSO.Area;
            placedObject.TakeAreaForPlacedObject(worldPosition);
            return placedObject;
        }

        #endregion
    }

}