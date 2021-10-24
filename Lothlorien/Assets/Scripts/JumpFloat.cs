using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFloat : MonoBehaviour
{

    public float floatFactor = 0.7f;
    public float fallMultiplier = 1.5f;
    public float peakStartVelocity = 2f;
    public float peakEndVelocity = 0.5f;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(rb.velocity.y);
        if (rb.velocity.y > -peakEndVelocity && rb.velocity.y < peakStartVelocity)
        {
            rb.velocity -= Vector2.up * Physics2D.gravity.y * (floatFactor) * Time.deltaTime;
        }
        else if (rb.velocity.y < -peakEndVelocity)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }
}
