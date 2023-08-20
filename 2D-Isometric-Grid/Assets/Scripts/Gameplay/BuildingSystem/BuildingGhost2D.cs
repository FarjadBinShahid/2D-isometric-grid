using core.gameplay.buildingsystem.placeobjects;
using core.gameplay.isometricgrid2d;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace core.gameplay.buildingsystem
{
    public class BuildingGhost2D : MonoBehaviour
    {
        private Transform visual;
        private PlaceableObjectSO placeableObjectSO;

        private Vector3 prevPos;

        private Camera cam;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            cam = Camera.main;
        }

        private void OnEnable()
        {
            RefreshVisual();
        }

        private void Start()
        {
            GridBuildingSystem.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
        }

        private void Update()
        {
            if (visual == null)
            {
                return;
            }

            // move placeable object visual
            if (Input.GetMouseButton(0))
            {
                if (EventSystem.current.IsPointerOverGameObject(0))
                {
                    return;
                }
                Vector3 touchPos = cam.ScreenToWorldPoint(Input.mousePosition);
                touchPos.z = 0f;
                if (prevPos != touchPos)
                {
                    _transform.position = touchPos;//Vector3.Lerp(transform.position, touchPos, Time.deltaTime * 15f);
                    prevPos = _transform.position;
                    GridBuildingSystem.Instance.FollowBuilding(_transform.position);
                }
            }

            // snap the building visual to tile 
            if(Input.GetMouseButtonUp(0))
            {
                Vector3Int cellPos = GridBuildingSystem.Instance.GridLayout.LocalToCell(_transform.position);
                _transform.position = GridBuildingSystem.Instance.GridLayout.CellToLocalInterpolated(cellPos + new Vector3(0.5f, 0.5f, 0f));
            }
        }

        private void Instance_OnSelectedChanged()
        {
            RefreshVisual();
        }

        private void RefreshVisual()
        {
            if (visual != null)
            {
                Destroy(visual.gameObject);
                visual = null;
            }

            PlaceableObjectSO placedObjectTypeSO = GridBuildingSystem.Instance.GetPlacedObjectTypeSO();

            // sets the position to middle of the screen
            Vector2 screenMidPos = new Vector2(Screen.width / 2, Screen.height / 2);
            Vector2 ghostInitPos = cam.ScreenToWorldPoint(screenMidPos);
            _transform.position = ghostInitPos;


            if (placedObjectTypeSO != null)
            {
                visual = Instantiate(placedObjectTypeSO.Visual, Vector3.zero, Quaternion.identity);
                visual.parent = _transform;
                visual.localPosition = Vector3.zero;
                visual.localEulerAngles = Vector3.zero;

                Vector3Int cellPos = GridBuildingSystem.Instance.GridLayout.LocalToCell(_transform.position);
                _transform.position = GridBuildingSystem.Instance.GridLayout.CellToLocalInterpolated(cellPos + new Vector3(0.5f, 0.5f, 0f));
                GridBuildingSystem.Instance.FollowBuilding(_transform.position);
            }
        }


        private void OnDisable()
        {
            Destroy(visual.gameObject);
            visual = null;
        }
    }
}