using core.gameplay.isometricgrid2d;
using UnityEngine;

namespace core.gameplay.buildingsystem.placeobjects
{

    public class PlacedObject : MonoBehaviour
    {

        public BoundsInt Area;

        #region Building Methods

        #region Create Building Methods
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
            GridBuildingSystem.Instance.TakeArea(areaTemp);
        }


        public static PlacedObject CreatePlacedObject(Vector3 worldPosition, PlaceableObjectSO placeableObjectSO)
        {
            Transform placedObjectTransform = Instantiate(placeableObjectSO.Prefab, worldPosition, Quaternion.identity);
            PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();
            placedObject.Area = placeableObjectSO.Area;
            placedObject.Area.position = GridBuildingSystem.Instance.GridLayout.WorldToCell(worldPosition);
            placedObject.TakeAreaForPlacedObject(worldPosition);
            return placedObject;
        }

        #endregion

        #region Demolish Building Methods

        private void OnMouseDown()
        {
            if(!GridBuildingSystem.Instance.IsInBuildingMode)
                DemolisPlacedObject();
        }

        public void DemolisPlacedObject()
        {
            GridBuildingSystem.Instance.DemolisPlaceObject(Area);
            Destroy(gameObject);
        }


        #endregion




        #endregion
    }

}