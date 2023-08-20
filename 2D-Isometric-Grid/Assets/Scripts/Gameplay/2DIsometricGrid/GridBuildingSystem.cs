using core.architecture;
using core.general.datamodels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;
using UnityEngine.EventSystems;
using core.constants;
using CodeMonkey.Utils;
using core.gameplay.buildingsystem.placeobjects;

namespace core.gameplay.isometricgrid2d
{
    public class GridBuildingSystem : Singleton<GridBuildingSystem>
    {

        public GridLayout GridLayout;
        [SerializeField] Tilemap mainTilemap;
        [SerializeField] private Tilemap tempTilemap;

        public static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();


        private PlacedObject placeableObjectSO;
        private Vector3 prevPos;
        private BoundsInt prevArea;
        private BoundsInt buildingArea;

        private Camera cam ;

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
            if(!placeableObjectSO)
            {
                return;
            }

            if(Input.GetMouseButton(0))
            {
                if(EventSystem.current.IsPointerOverGameObject(0))
                {
                    return;
                }

                if(!placeableObjectSO.Placed)
                {
                    Vector2 touchPos = cam.ScreenToWorldPoint(Input.mousePosition);
                    Vector3Int cellPos = GridLayout.LocalToCell(touchPos);

                    if(prevPos != cellPos)
                    {
                        placeableObjectSO.transform.position = GridLayout.CellToLocalInterpolated(cellPos + new Vector3(0.5f,0.5f,0f));
                        prevPos = cellPos;
                        FollowBuilding();
                    }

                }
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(placeableObjectSO.CanBePlaced())
                {
                    placeableObjectSO.CreatePlacedObject();
                }
            }else if(Input.GetKeyDown(KeyCode.Escape))
            {
                ClearArea();
                Destroy(placeableObjectSO.gameObject);
            }
        }
        #endregion

        #region Tilemap Management

        private static TileBase[] GetTileBlock(BoundsInt area, Tilemap tilemap)
        {
            TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
            int counter = 0;

            foreach(var v in area.allPositionsWithin)
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
            for(int i =0;i< arr.Length;i++)
            {
                arr[i] = tileBases[type];
            }
        }



        #endregion
        #region Building Placement

        public void InitWithBuilding(GameObject building)
        {
            Vector2 screenMidPos = new Vector2(Screen.width /2 , Screen.height/2);
            Vector2 ghostInitPos = cam.ScreenToWorldPoint(screenMidPos);
            placeableObjectSO = Instantiate(building, ghostInitPos, Quaternion.identity).GetComponent<PlacedObject>();
            buildingArea = placeableObjectSO.Area;
            FollowBuilding();
        }

        private void ClearArea()
        {
            TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
            FillTiles(toClear, TileType.Empty);
            tempTilemap.SetTilesBlock(prevArea, toClear);
        }

        private void FollowBuilding()
        {
            ClearArea();
            placeableObjectSO.Area.position = GridLayout.WorldToCell(placeableObjectSO.gameObject.transform.position);
            buildingArea = placeableObjectSO.Area;
            TileBase[] baseArray = GetTileBlock(buildingArea, mainTilemap);

            int size = baseArray.Length;

            TileBase[] tileArray = new TileBase[size];

            for(int i =0;i< baseArray.Length;i++) 
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

        public bool CanTakeArea(BoundsInt area)
        {
            TileBase[] baseArray = GetTileBlock(area,mainTilemap);
            foreach(var tilemap in baseArray)
            {
                if(tilemap != tileBases[TileType.White])
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

       

        #endregion
    }
}