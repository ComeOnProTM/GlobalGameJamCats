using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SmallFireScript : MonoBehaviour

{

    [SerializeField]private GameObject FireParticle;
    public float FireTimeA = 5;
    public float FireTimeB = 10;
    [SerializeField]private float radius = 1f;
    public LayerMask layermask = 1 << 6;
    private IEnumerator Fire()
    {
        while (true)
        {
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
                            hitCollider.gameObject.AddComponent<SmallFireScript>();
                        }
                    
                        
                    }
                       
                }
            }
            yield return new WaitForSeconds(Random.Range(FireTimeA, FireTimeB));
        }
    }
    private void Start()
    {
        Instantiate(original: FireParticle, position: transform.position, rotation: transform.rotation);
        StartCoroutine(Fire());
    }
}
