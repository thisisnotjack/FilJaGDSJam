using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance => _instance;
    private static BuildingManager _instance;
    public LayerMask _buildingLayerMask;
    public Transform _draggingParent;
    public Camera _camera;
    public float _rotateSpeed;
    public GameObject _floor;
    public Transform _planeBuildingPositionTransform;
    public bool IsDragging => _currentlyDragging != null;
    bool _inBuildingMode = false;
    public GameObject CurrentlyDragging => _currentlyDragging?.gameObject;
    private AttachableItemBody _currentlyDragging;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }
    void Start()
    {
        if (_camera == null)
            _camera = Camera.main;
        GameStateManager.instance.gameStateChanged += HandleGameStateChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_inBuildingMode)
            return;
        UpdateDraggingPos();
        if (IsDragging)
        {
            if (!Input.GetKey(KeyCode.Mouse0))
            {
                _currentlyDragging.Release();
                ReleaseObject();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                RotateGrabbed(Input.GetAxis("Mouse ScrollWheel"));
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TryGrabObject();
        }
    }

    private void UpdateDraggingPos()
    {
        
        RaycastHit[] hits;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Vector3 pos = ray.origin + ray.direction * 20f;
        hits = Physics.RaycastAll(ray, 100f, _buildingLayerMask);
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (hit.transform.gameObject == _floor)
                {
                    pos = hit.point;
                    pos.y = 1;
                    break;
                }
            }
        }
        _draggingParent.transform.position = pos;
    }

    public void ReleaseObject()
    {
        if (_currentlyDragging == null)
            return;
        _currentlyDragging = null;
    }

    private void TryGrabObject()
    {
        RaycastHit[] hits;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(ray, 100f, _buildingLayerMask);
        if (hits.Length > 0) {

            foreach (var hit in hits)
            {
                GameObject objectHit = hit.transform.gameObject;
                if (objectHit != _floor)
                {
                    var clicked = objectHit.GetComponent<AttachableItemBody>();
                    if (clicked && clicked.TryGrab(_draggingParent.transform))
                    {
                        _currentlyDragging = clicked;
                    }
                }
            }
        }
    }

    private void RotateGrabbed(float amount)
    {
        if(_draggingParent.transform.childCount > 0)
        {
            _draggingParent.transform.GetChild(0).Rotate(Vector3.right * _rotateSpeed * Time.deltaTime * amount);
        }
    }
    private void HandleGameStateChanged(GameStateManager.GameState gameState)
    {
        _inBuildingMode = false;
        if(gameState == GameStateManager.GameState.Building)
        {
            //Reset building state - position of the plane etc.
            PlaneManager.instance.MovePlaneToTransform(_planeBuildingPositionTransform);
            _inBuildingMode = true;
        }
    }
}
