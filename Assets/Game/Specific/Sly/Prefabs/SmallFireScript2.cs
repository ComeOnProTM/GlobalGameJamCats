using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SmallFireScript2 : MonoBehaviour
{
    [SerializeField]private GameObject FireParticle;
    public float FireTimeA = 5;
    public float FireTimeB = 10;
    [SerializeField]private float radius = 3f;
    public LayerMask layermask = 1 << 6;
    private IEnumerator Fire()
    {
        Debug.Log("Entered Fire Coroutine");

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(FireTimeA, FireTimeB));

            var hitColliders = Physics.OverlapSphere(transform.position, radius, layermask);
            // anything with a collider on the fire layer is affected
            foreach (var hitCollider in hitColliders)
            {
                if (!hitCollider.TryGetComponent<SmallFireScript>(out _))
                {
                    if (hitCollider.TryGetComponent(out Burnable burnable))
                    {
                        
                        if (burnable.Flamability < Random.Range(0f,1f))
                        {
                            SmallFireScript spawnedFire = hitCollider.gameObject.AddComponent<SmallFireScript>();
                           // spawnedFire.FireParticle = FireParticle;
                        }
                    }
                }
            }
            
            Debug.Log("Loop", context: this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Start()
    {
        StartCoroutine(Fire());

        if(FireParticle != null)
        {
            Instantiate(original: FireParticle, position: transform.position, rotation: transform.rotation);
        }
    }
}
