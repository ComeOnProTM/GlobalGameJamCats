using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SmallFireScript : MonoBehaviour
{
    [SerializeField] private GameObject FireParticle;
    public float FireTimeA = 0.5f;
    private GameObject SpawnedParticle;
    public float FireTimeB = 1.5f;
    [SerializeField] private float radius = 3f;
    public LayerMask layermask = 1 << 6;
    private IEnumerator Fire()
    {
        Debug.Log("Entered Fire Coroutine");

        //while (true)
        //{
        float randomValue = Random.value;

        // Check if the random value is greater than 0.5
        if (randomValue > 0.2f)
        {
            // Your code here (this code will be skipped 50% of the time)
            yield return new WaitForSeconds(Random.Range(FireTimeA, FireTimeB));
        }
        else
        {
            // This block will be executed when the random value is less than or equal to 0.5
            Debug.Log("This line of code is skipped.");
        }


        Collider[] _hitColliders = Physics.OverlapSphere(transform.position, radius, layermask);
        List<Collider> _hitCollidersList = new List<Collider>();
        foreach (Collider _collider in _hitColliders)
        {
            _hitCollidersList.Add(_collider);
        }
        // anything with a collider on the fire layer is affected

        for (int i = 0; i < _hitCollidersList.Count; i++)
        {
            Collider randomCollider = _hitColliders[Random.Range(0, _hitColliders.Length)];
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
        

        /*foreach (Collider hitCollider in hitColliders)
        {

            if (!hitCollider.TryGetComponent<SmallFireScript>(out _) && hitCollider.TryGetComponent(out Burnable burnable) && burnable.Flamability < Random.Range(0f, 1f))
            {

                SmallFireScript spawnedFire = hitCollider.gameObject.AddComponent<SmallFireScript>();
                spawnedFire.FireParticle = FireParticle;
                continue;
            }
        }*/
        Debug.Log("Loop", context: this);
        //}
    }

    private void OnDestroy()
    {
        Destroy(SpawnedParticle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Start()
    {
        StartCoroutine(Fire());

        if (FireParticle != null)
        {
            SpawnedParticle = Instantiate(original: FireParticle, position: transform.position, rotation: transform.rotation, transform);
            SpawnedParticle.transform.localScale = Vector3.one;
        }
    }
}
