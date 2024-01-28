using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquishAndStretch : MonoBehaviour
{
    public float squishAmount = 1f;   // Adjust the amount of squish
    public float stretchAmount = 1.2f;  // Adjust the amount of stretch
    public float animationSpeed = 12f;   // Adjust the speed of the animation

    private void Update()
    {
        // Calculate the scale based on a sine function to create a squish and stretch effect
        float scale = Mathf.Sin(Time.time * animationSpeed) > 0 ? stretchAmount : squishAmount;

        // Apply the scale to the object
        transform.localScale = new Vector3(scale, 0.1f, 0.1f);
    }
}
