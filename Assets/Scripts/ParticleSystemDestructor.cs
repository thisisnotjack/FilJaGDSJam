using UnityEngine;

public class ParticleSystemDestructor : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;


    void Update()
    {
        if (!_particleSystem.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
