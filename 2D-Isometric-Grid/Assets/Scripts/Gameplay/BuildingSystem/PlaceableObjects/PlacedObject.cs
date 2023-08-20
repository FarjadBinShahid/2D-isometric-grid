using core.gameplay.isometricgrid2d;
using UnityEngine;

namespace core.gameplay.buildingsystem.placeobjects
{

    public class PlacedObject : MonoBehaviour
    {

        public bool Placed { get; private set; }
        public BoundsInt Area;

        private PlaceableObjectSO placeableObjectSO;

        #region Building Methods

        public virtual bool CanBePlaced()
        {
            Vector3Int posInt = GridBuildingSystem.Instance.GridLayout.LocalToCell(transform.position);
            BoundsInt areaTemp = Area;
            areaTemp.position = posInt;
            return GridBuildingSystem.Instance.CanTakeArea(areaTemp);
        }

        public void CreatePlacedObject()
        {
            Vector3Int posInt = GridBuildingSystem.Instance.GridLayout.LocalToCell(transform.position);
            BoundsInt areaTemp = Area;
            areaTemp.position = posInt;
            Placed = true;
            GridBuildingSystem.Instance.TakeArea(areaTemp);
        }

/*
        public static PlacedObject CreatePlaceableObject(Vector3 worldPosition, Vector2Int origin, PlaceableObjectSO placeableObjectSO)
        {

            Transform placedObjectTransform = Instantiate(placeableObjectSO.Prefab, worldPosition, Quaternion.identity);

            PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();
            placedObject.placeableObjectSO = placeableObjectSO;

            return placedObject;

        }*/

        #endregion
    }

}