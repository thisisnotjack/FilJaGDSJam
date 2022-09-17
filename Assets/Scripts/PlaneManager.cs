using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : Singleton<PlaneManager>
{
    [SerializeField] PlaneObject _planeObject;

    public PlaneObject planeObject => _planeObject;
}
