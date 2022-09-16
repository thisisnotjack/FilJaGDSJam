using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance => _instance;
    private static BuildingManager _instance;
    public Transform _draggingParent;
    public Camera _camera;
    public bool IsDragging => _currentlyDragging != null;
    public GameObject CurrentlyDragging => _currentlyDragging?._itemParent;
    private AttachableItem _currentlyDragging;
    private float _grabHeight;

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
            if (Input.GetKey(KeyCode.Mouse0))
            {
                // drag object
            }
            else
            {
                ReleaseObject();
                //release object, maybe attach 
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TryGrabObject();
        }
    }

    private void UpdateDraggingPos()
    {
        if (IsDragging)
            _currentlyDragging._itemParent.gameObject.SetActive(false);

        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Vector3 pos = ray.origin + ray.direction * 5f;
        if (Physics.Raycast(ray, out hit))
        {
            pos = hit.point;
            pos.y += _grabHeight;
        }
        _draggingParent.transform.position = pos;
        if(IsDragging)
            _currentlyDragging._itemParent.gameObject.SetActive(true);
    }

    public void ReleaseObject(bool rigidbodyKinematic = false)
    {
        if (_currentlyDragging == null)
            return;
        _grabHeight = 0f;
        _currentlyDragging._itemParent.transform.parent = null;                
        _currentlyDragging._itemParent.GetComponent<Rigidbody>().isKinematic = rigidbodyKinematic;
        _currentlyDragging = null;
    }

    private void TryGrabObject()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            GameObject objectHit = hit.transform.gameObject;
            var clicked = objectHit.GetComponentInChildren<AttachableItem>();
            if (clicked)
            {
                _currentlyDragging = clicked;
                _currentlyDragging._itemParent.GetComponent<Rigidbody>().isKinematic = true;
                _currentlyDragging._itemParent.transform.parent = _draggingParent.transform;
                _grabHeight = hit.point.y;
            }
        }
    }
}
