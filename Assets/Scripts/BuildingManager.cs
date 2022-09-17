using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance => _instance;
    private static BuildingManager _instance;
    public Transform _draggingParent;
    public Camera _camera;
    public float _rotateSpeed;
    public GameObject _floor;
    public bool IsDragging => _currentlyDragging != null;
    public GameObject CurrentlyDragging => _currentlyDragging?._itemParent;
    private AttachableItem _currentlyDragging;

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
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDraggingPos();
        if (IsDragging)
        {
            if (!Input.GetKey(KeyCode.Mouse0))
            {
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
        Vector3 pos = ray.origin + ray.direction * 5f;
        hits = Physics.RaycastAll(ray, 100f);
        if (hits.Length > 0)
        {
            var floorHit = hits[0];
            foreach (var hit in hits)
            {
                if (hit.transform.gameObject == _floor)
                {
                    floorHit = hit;
                }
            }
            pos = floorHit.point;
        }
        _draggingParent.transform.position = pos;
    }

    public void ReleaseObject(bool rigidbodyKinematic = false)
    {
        if (_currentlyDragging == null)
            return;
        _currentlyDragging.Detach(AttachableItem.ItemState.Neutral);
        _currentlyDragging = null;
    }

    private void TryGrabObject()
    {
        RaycastHit[] hits;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(ray, 100f);
        if (hits.Length > 1) {
            GameObject objectHit = hits[1].transform.gameObject;
            if (objectHit != _floor)
            {
                var clicked = objectHit.GetComponentInChildren<AttachableItem>();
                if (clicked && clicked.TryGrab(_draggingParent.transform))
                {
                    _currentlyDragging = clicked;
                }
            }
        }
    }

    private void RotateGrabbed(float amount)
    {
        _draggingParent.transform.localEulerAngles += Vector3.up * (_rotateSpeed * Time.deltaTime) * amount;
    }
}
