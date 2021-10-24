using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragThorns : MonoBehaviour
{
    // Start is called before the first frame update
    float timer;
    public float duration;
    public float drag;
    public float animationTime;

    //GameObject parent;
    Vector2 currentSpeedVector;
    float magnitude;
    void Start()
    {
        //parent = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > duration)
        {
            DeThorn();
        }
        timer += Time.deltaTime;
        currentSpeedVector.x = -gameObject.GetComponent<PlayerTest>().backgroundManager.xSpeed;
        currentSpeedVector.y = gameObject.GetComponent<Rigidbody2D>().velocity.y;
        magnitude = currentSpeedVector.magnitude * (1-(drag*Time.deltaTime));
        currentSpeedVector = Vector3.Normalize(currentSpeedVector);
        currentSpeedVector *= magnitude;

        gameObject.GetComponent<PlayerTest>().backgroundManager.xSpeed = -currentSpeedVector.x;
        currentSpeedVector.x = 0;
        gameObject.GetComponent<Rigidbody2D>().velocity = currentSpeedVector;

        /*if (timer >= 1)
        {
            temp2 *= (1 - drag);
            Debug.Log("SECOND");
            Debug.Log("1: " + temp + " 2: " + temp2);
            timer = 0;
        }
        timer += Time.deltaTime;
        temp *= (1 - (drag * Time.deltaTime));*/
    }

    void DeThorn()
    {
        drag = 0;
        Destroy(gameObject.GetComponent<DragThorns>(), animationTime);
    }
}
