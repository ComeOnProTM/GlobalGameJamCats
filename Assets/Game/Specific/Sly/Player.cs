using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float speed;

    [SerializeField]
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        //Set running time to 1
        Time.timeScale = 1f;
    }

    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement * 1.5f * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collision with other objects
        Debug.Log("Collision Detected");
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(0, 0);
        
        if(Input.GetKey(KeyCode.W)) {
            movement.y = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement.x = -1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement.y = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement.x = 1;
        }
    }
}
