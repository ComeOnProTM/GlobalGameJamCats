using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SmallFireScript : MonoBehaviour
{
    [SerializeField] private GameObject FireParticle;
    public float FireTimeA = 5;
    public float FireTimeB = 10;
    [SerializeField] private float radius = 3f;
    public LayerMask layermask = 1 << 6;
    private IEnumerator Fire()
    {
        Debug.Log("Entered Fire Coroutine");

        while (true)
        {
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
            

            var hitColliders = Physics.OverlapSphere(transform.position, radius, layermask);
            // anything with a collider on the fire layer is affected
            foreach (var hitCollider in hitColliders)
            {
                if (!hitCollider.TryGetComponent<SmallFireScript>(out _))
                {
                    if (hitCollider.TryGetComponent(out Burnable burnable))
                    {

                        if (burnable.Flamability < Random.Range(0f, 1f))
                        {
                            SmallFireScript spawnedFire = hitCollider.gameObject.AddComponent<SmallFireScript>();
                            spawnedFire.FireParticle = FireParticle;
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

        if (FireParticle != null)
        {
            Instantiate(original: FireParticle, position: transform.position, rotation: transform.rotation);
        }
    }
}
