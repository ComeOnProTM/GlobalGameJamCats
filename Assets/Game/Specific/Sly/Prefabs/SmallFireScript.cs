using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SmallFireScript : MonoBehaviour
{
    [SerializeField] private GameObject FireParticle;
    private GameObject SpawnedParticle;
    [SerializeField] private float radius = 2.5f;
    public LayerMask layermask = 1 << 6;

    [SerializeField] private float fireTimerMin = 3f;
    [SerializeField] private float fireTimerMax = 4f;
    private float timeTillNextFire;

    private void Start()
    {
        FireManager.Instance.AddFire(gameObject);

        timeTillNextFire = fireTimerMax;
        if (FireParticle != null)
        {
            SpawnedParticle = Instantiate(original: FireParticle, position: transform.position, rotation: transform.rotation, transform);
            SpawnedParticle.transform.localScale = Vector3.one;
        }
    }

    private void Update()

    {
        transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.x, this.transform.localScale.x);
        timeTillNextFire -= Time.deltaTime;

        if (timeTillNextFire < 0)
        {
            timeTillNextFire = Random.Range(fireTimerMin, fireTimerMax);
            SpawnFire();
        }
    }

    private void OnDestroy()
    {
        Destroy(SpawnedParticle);
        FireManager.Instance.RemoveFire(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void SpawnFire()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            Collider[] _hitColliders = Physics.OverlapSphere(transform.position, radius, layermask);
            List<Collider> _hitCollidersList = new List<Collider>();
            foreach (Collider _collider in _hitColliders)
            {
                _hitCollidersList.Add(_collider);
            }
            // anything with a collider on the fire layer is affected

            for (int i = 0; i < _hitCollidersList.Count; i++)
            {
                Collider randomCollider = _hitColliders[i];
                if (!randomCollider.TryGetComponent<SmallFireScript>(out SmallFireScript _))
                {
                    SmallFireScript spawnedFire = randomCollider.gameObject.AddComponent<SmallFireScript>();
                    spawnedFire.FireParticle = FireParticle;
                    break;
                }
                else
                {
                    _hitCollidersList.Remove(randomCollider);
                }
            }
        }
    }
}
