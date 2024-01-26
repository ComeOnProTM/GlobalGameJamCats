using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private float speed;

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = new Vector2(0, 0);
        
        if(Input.GetKey(KeyCode.W)) {
            inputVector.y = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = 1;
        }
        inputVector = inputVector.normalized;
        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0);
        transform.position += moveDir * speed * Time.deltaTime;

    }
}
