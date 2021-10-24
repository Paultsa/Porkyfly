using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalThunderBoost : MonoBehaviour
{

    public GameObject manager;
    public float fallAngle;
    public float fallSpeed;
    public float bounceAngle;
    public float bounceSpeedFixedBonus;
    public float bounceSpeedPercentBonus;
    public float minBounceSpeed;
    public float maxBounceSpeed;

    public float shakeMagnitude;
    public float shakeDuration;

    Vector2 currentVector;
    Quaternion fallQuaternion;
    Quaternion bounceQuaternion;
    Vector2 fallDirVector;
    Vector2 bounceDirVector;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bounceSpeedPercentBonus /= 100;
    }

    // Update is called once per frame
    /*void Update()
    {

    }*/
    public void SaveCurrentSpeedVector()
    {
        currentVector = getCurrentSpeedVector();
    }
    public void OrbitalBoost()
    {
        //Debug.Log("ORBITAL DEATH RAY ACTIVATED");
        //Probably obsolete
        Camera.main.GetComponent<AirBoost>().isAvailable = false;

        //Calculating direction vectors
        fallQuaternion = Quaternion.AngleAxis(fallAngle, Vector3.forward);
        bounceQuaternion = Quaternion.AngleAxis(bounceAngle, Vector3.forward);
        fallDirVector = fallQuaternion * Vector2.right;
        bounceDirVector = bounceQuaternion * Vector2.right;
        //Shake and turn
        transform.GetChild(0).GetComponent<Shake>().StartShake(shakeDuration, shakeMagnitude);
        transform.right = fallDirVector;
        //Fall
        SetCurrentSpeedVector(fallDirVector * fallSpeed);
        GetComponent<PlayerTest>().orbitalCrash = true;

    }

    public void ThunderBound()
    {
        float magnitude = currentVector.magnitude;
        Debug.Log(magnitude + " ONE");
        magnitude = Mathf.Clamp((magnitude * (1 + bounceSpeedPercentBonus)) + bounceSpeedFixedBonus, minBounceSpeed, maxBounceSpeed);
        Debug.Log(magnitude);
        SetCurrentSpeedVector(bounceDirVector * magnitude);

    }

    Vector2 getCurrentSpeedVector()
    {
        Vector2 currentSpeedVector;
        currentSpeedVector.y = GetComponent<Rigidbody2D>().velocity.y;
        currentSpeedVector.x = -manager.GetComponent<BackgroundManager>().xSpeed;
        return currentSpeedVector;
    }

    void SetCurrentSpeedVector(Vector2 speed)
    {
        manager.GetComponent<BackgroundManager>().xSpeed = -speed.x;
        speed.x = 0;
        GetComponent<Rigidbody2D>().velocity = speed;
    }
}
