using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkGrabber : MonoBehaviour
{
    float timer;
    public float duration;
    public float animationTime;
    public Vector2 dirVector;
    public float magnitude;
    public GameObject grabber;
    public float accelerationPercentage;
    public float fixedAcceleration;
    float originalMagnitude;

    //GameObject parent;
    Vector2 currentSpeedVector;
    Vector2 pigPosOffset;
    void Start()
    {
    }

    private void OnEnable()
    {
        timer = 0;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        gameObject.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        Camera.main.GetComponent<AirBoost>().isAvailable = false;
        if (grabber != null)
            pigPosOffset = grabber.transform.GetChild(0).position - grabber.transform.position;
        originalMagnitude = magnitude;
    }

    bool once = false;
    // Update is called once per frame
    void Update()
    {
        if (timer > duration && grabber != null)
        {
            Release();
            once = true;
            return;
        }
        timer += Time.deltaTime;
        magnitude *= (1 + (accelerationPercentage * Time.deltaTime));
        magnitude += (fixedAcceleration * Time.deltaTime);
        currentSpeedVector = dirVector * magnitude;
        //Debug.Log(currentSpeedVector + " " + dirVector + " " + originalMagnitude + " MAG " + magnitude);

        gameObject.GetComponent<PlayerTest>().backgroundManager.xSpeed = -currentSpeedVector.x;
        currentSpeedVector.x = 0;
        gameObject.GetComponent<Rigidbody2D>().velocity = currentSpeedVector;
        //Debug.Log(grabber.transform.GetChild(0).position + " " + transform.position);

        grabber.transform.parent.position = AsVector2(transform.position) - pigPosOffset;
        grabber.transform.parent.right = dirVector;
    }


    void Release()
    {
        //Debug.Log("RELEASE");
        Camera.main.GetComponent<AirBoost>().isAvailable = true;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        gameObject.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
        //Debug.Log("RELEASE 1");
        grabber.GetComponent<Obstacle>().DestroySelf();
        //Debug.Log("RELEASE 2");
        Destroy(this);
    }

    public static Vector2 AsVector2(Vector3 _v)
    {
        return new Vector2(_v.x, _v.y);
    }
}
