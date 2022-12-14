using UnityEngine;

public class PlaneObject : MonoBehaviour
{
    [SerializeField] PlanePhysicsFlightController _planePhysicsFlightController;
    [SerializeField] PlaneStartPushController _planeStartPushController;
    [SerializeField] float _startPushDuration;
    [SerializeField] Transform _craneAttachPoint;
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] AudioSource _baseSoundAudioSource;
    [SerializeField] AudioSource _splashSoundAudioSource;
    [SerializeField] AudioSource _domeHitSoundAudioSource;
    [SerializeField] GameObject _splashParticleSystemPrefab;
    public Transform craneAttachPoint => _craneAttachPoint;
    public event System.Action hitWater;
    public AudioSource baseSoundAudioSource => _baseSoundAudioSource;

    private bool _startingPushInProgress = false;
    private bool _flightInProgress = false;
    private float _pushStartTime;
    private bool _underwater = false;
    private bool _hitDome = false;
    private float _baseDrag;
    private float _baseAngularDrag;

    protected void Start()
    {
        _baseDrag = _rigidbody.drag;
        _baseAngularDrag = _rigidbody.angularDrag;
    }

    protected void Update()
    {
        if (_startingPushInProgress)
        {
            _planeStartPushController.ProcessFlight();
            if ((Time.time - _pushStartTime) >= _startPushDuration)
            {
                _startingPushInProgress = false;
                _flightInProgress = true;
                Debug.Log("Initial push ended");
            }
        }
        else if(_flightInProgress)
        {
            _planePhysicsFlightController.ProcessFlight(!_hitDome && !_underwater);
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        var skyDomeLayerMask = LayerMask.GetMask("Sky");
        if (!_hitDome && skyDomeLayerMask == (skyDomeLayerMask | (1 << collision.gameObject.layer)))
        {
            _hitDome = true;
            _domeHitSoundAudioSource.Play();
            hitWater?.Invoke();
            _rigidbody.velocity = Vector3.zero;
        }
    }
    protected void OnTriggerEnter(Collider other)
    {
        var waterLayerMask = LayerMask.GetMask("Water");
        if (!_underwater && !_hitDome && waterLayerMask == (waterLayerMask | (1 << other.gameObject.layer)))
        {
            _underwater = true;
            _splashSoundAudioSource.Play();
            _planePhysicsFlightController.SetWaterMultipliers();
            hitWater?.Invoke();
            var splash = Instantiate(_splashParticleSystemPrefab);
            var newPos = transform.position;
            newPos.y = 0;
            splash.transform.position = newPos;
        }
    }

    public void ResetInternalState()
    {
        _underwater = false;
        _hitDome = false;
        _flightInProgress = false;
        _startingPushInProgress = false;
        _rigidbody.drag = _baseDrag;
        _rigidbody.angularDrag = _baseAngularDrag;
    }

    public void StartPush()
    {
        _pushStartTime = Time.time;
        _startingPushInProgress = true;
    }

    public void DisableMovement()
    {
        print("disabling movenet");
        //TODO
        _rigidbody.isKinematic = true;
    }

    public void DisableColliders()
    {
        foreach (var collider in gameObject.GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
    }

    public void EnableMovement()
    {
        //TODO
        _rigidbody.isKinematic = false;
    }

    public void EnableAllColliders()
    {
        foreach (var collider in gameObject.GetComponentsInChildren<Collider>())
        {
            collider.enabled = true;
        }
    }

    public void EnablePlaneCollider()
    {
        var collider = GetComponent<Collider>();
        collider.enabled = true;
    }
}
