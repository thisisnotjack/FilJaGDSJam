using UnityEngine;

public class PhysicsAffector : MonoBehaviour
{
    [SerializeField] Transform _centerOfGravity;
    [SerializeField] float _weight;
    [SerializeField] float _resistance;

    public Vector3 position => _centerOfGravity.position;
    public float weight => _weight;
    public float resistance => _resistance;
}
