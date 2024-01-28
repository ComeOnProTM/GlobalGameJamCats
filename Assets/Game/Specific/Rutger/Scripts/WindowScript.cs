using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowScript : MonoBehaviour
{
    private const string BREAK = "Break";
    private float radius = 1;
    [SerializeField] private LayerMask NPCLayer;
    [SerializeField] private Animator anim;

    // Update is called once per frame
    void Update()
    {
        Collider2D[] collisionNpc = Physics2D.OverlapCircleAll(transform.position, radius, NPCLayer);

        for (int i = 0; i < collisionNpc.Length; i++)
        {
            if (collisionNpc[i].gameObject.TryGetComponent<Civilian>(out Civilian _NPC))
            {
                Destroy(_NPC.gameObject);
                NpcManager.Instance.SuccesfulSaveNPC();
                anim.SetTrigger(BREAK);
            }
        }
    }
}
