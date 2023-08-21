using core.architecture;
using core.general.datamodels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;
using core.constants;
using CodeMonkey.Utils;
using core.gameplay.buildingsystem.placeobjects;
using core.gameplay.buildingsystem;

namespace core.gameplay.isometricgrid2d
{
    public class GridBuildingSystem : Singleton<GridBuildingSystem>
    {
        [Header("Grid Settings")]
        [Tooltip("Grid Layout")]
        public GridLayout GridLayout;
        [Tooltip("Main tilemap, tilemap where buildings are going to be placed")]
        [SerializeField] Tilemap mainTilemap;
        [Tooltip("Temp tilemap, tilemap to display if buildings are placeable green, red")]
        [SerializeField] private Tilemap tempTilemap;

        [Header("Placeable Objects")]
        [Tooltip("All placeable objects scriptable objects")]
        [SerializeField] private List<PlaceableObjectSO> placeableObjectSOList;

        [Header("Ghost")]
        [Tooltip("BuildingGhost2D script reference")]
        [SerializeField ]private BuildingGhost2D ghost2D;



        public static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

        private PlaceableObjectSO placeableObjectSO;
       // private PlacedObject placedObject;
        private Vector3 prevPos;
        private BoundsInt prevArea;
        private BoundsInt buildingArea;

        private Camera cam;


        public event Action OnSelectedChanged;


        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {

            tileBases = Resources.LoadAll<TileBase>(GameConstants.TilesFolderPath)
                .ToDictionary(x => Enum.Parse<TileType>(x.name), y => y);
            tileBases.Add(TileType.Empty, null);
            cam = Camera.main;
        }

        private void Update()
        {

            if (!placeableObjectSO)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                BuildPlaceableObject();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopGhosting();
            }
        }

        #endregion

        #region Tilemap Management

        private static TileBase[] GetTileBlock(BoundsInt area, Tilemap tilemap)
        {
            TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
            int counter = 0;

            foreach (var v in area.allPositionsWithin)
            {
                Vector3Int pos = new Vector3Int(v.x, v.y, 0);
                array[counter] = tilemap.GetTile(pos);
                counter++;
            }
            return array;
        }

        private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
        {
            int size = area.size.x * area.size.y * area.size.z;
            TileBase[] tileArray = new TileBase[size];
            FillTiles(tileArray, type);
            tilemap.SetTilesBlock(area, tileArray);
        }

        private static void FillTiles(TileBase[] arr, TileType type)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = tileBases[type];
            }
        }



        #endregion
        #region Building Placement

        public void InitWithBuilding(GameObject building)
        {
            placeableObjectSO = placeableObjectSOList[0];
            ghost2D.gameObject.SetActive(true);
            //OnSelectedChanged?.Invoke();
            /*Vector2 screenMidPos = new Vector2(Screen.width / 2, Screen.height / 2);
            Vector2 ghostInitPos = cam.ScreenToWorldPoint(screenMidPos);
            placedObject = Instantiate(building, ghostInitPos, Quaternion.identity).GetComponent<PlacedObject>();
            buildingArea = placedObject.Area;
            FollowBuilding();*/
        }

        private void ClearArea()
        {
            TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
            FillTiles(toClear, TileType.Empty);
            tempTilemap.SetTilesBlock(prevArea, toClear);
        }

        public void FollowBuilding(Vector3 ghostPos)
        {
            ClearArea();
            buildingArea = placeableObjectSO.Area;
            buildingArea.position = GridLayout.WorldToCell(ghostPos);

            TileBase[] baseArray = GetTileBlock(buildingArea, mainTilemap);

            int size = baseArray.Length;

            TileBase[] tileArray = new TileBase[size];

            for (int i = 0; i < baseArray.Length; i++)
            {
                if (baseArray[i] == tileBases[TileType.White])
                {
                    tileArray[i] = tileBases[TileType.Green];
                }
                else
                {
                    FillTiles(tileArray, TileType.Red);
                    break;
                }
            }

            tempTilemap.SetTilesBlock(buildingArea, tileArray);
            prevArea = buildingArea;
        }

        private void BuildPlaceableObject()
        {
            if (placeableObjectSO.CanBePlaced(ghost2D.transform.position))
            {
                ghost2D.gameObject.SetActive(false);
                PlacedObject.CreatePlacedObject(ghost2D.transform.position, placeableObjectSO);
                placeableObjectSO = null;
            }

        }

        public bool CanTakeArea(BoundsInt area)
        {
            TileBase[] baseArray = GetTileBlock(area, mainTilemap);
            foreach (var tilemap in baseArray)
            {
                if (tilemap != tileBases[TileType.White])
                {
                    UtilsClass.CreateWorldTextPopup(ErrorConstants.CannotPlacebuildingError, area.position, 15);
                    return false;
                }
            }
            return true;
        }


        public void TakeArea(BoundsInt area)
        {
            SetTilesBlock(area, TileType.Empty, tempTilemap);
            SetTilesBlock(area, TileType.Green, mainTilemap);
        }

        public void StopGhosting()
        {
            ClearArea();
            ghost2D.gameObject.SetActive(false);
            placeableObjectSO = null;
        }

        public void FreeArea(BoundsInt area)
        {
            SetTilesBlock(area, TileType.White, mainTilemap);
        }

        public void DemolisPlaceObject(BoundsInt area)
        {
            StopGhosting();
            FreeArea(area);
        }

        public PlaceableObjectSO GetPlacedObjectTypeSO()
        {
            return placeableObjectSO;
        }
        #endregion

    }
}