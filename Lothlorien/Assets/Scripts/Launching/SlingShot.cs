using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    public StartMenu startMenu;
    GameObject go = null;

    public bool grabbed = false;
    bool thrown = false;
    GameObject throwingObject;
    Vector2 startPosition;
    Vector2 launchVector;

    Vector2 angleComparison = new Vector2(1, 0);
    float angle;

    [Header("Release settings")]
    [Tooltip("Max angle when the ball can be released. The angles go to 180° and -180°")]
    public float maxAngle = 90f;
    [Tooltip("Min angle when the ball can be released. The angles go to 180° and -180°")]
    public float minAngle = 0f;


    [Header("Slingshot settings")]
    [Tooltip("Force multiplier")]
    public float forceMultiplier = 10;
    [Tooltip("Slingshot power stroke")]
    public float distance = 3f;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(grabbed);
        if (Input.GetButton("Fire1") && !thrown && startMenu.inMainMenu)
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.transform.CompareTag("Throwable") && !grabbed)
            {
                if (startPosition == Vector2.zero)
                    startPosition = hit.transform.position;
                hit.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                hit.transform.gameObject.GetComponent<Rigidbody2D>().drag = 100;
                hit.transform.gameObject.GetComponent<Collider2D>().enabled = false;
                throwingObject = hit.transform.gameObject;
                grabbed = true;
                if (hit.transform.GetComponent<DistanceJoint2D>() == null)
                {
                    hit.transform.gameObject.AddComponent<DistanceJoint2D>();
                    hit.transform.GetComponent<DistanceJoint2D>().autoConfigureDistance = false;
                    hit.transform.GetComponent<DistanceJoint2D>().maxDistanceOnly = true;
                    hit.transform.GetComponent<DistanceJoint2D>().distance = distance;
                    hit.transform.GetComponent<DistanceJoint2D>().connectedAnchor = hit.transform.position;
                }
            }
            if (go != null)
            {
                go.transform.position = hit.point;
                
            }
        }
        else if (grabbed)
        {
            launchVector.x = startPosition.x - throwingObject.transform.position.x;
            launchVector.y = startPosition.y - throwingObject.transform.position.y;
            angle = Vector2.SignedAngle(angleComparison, launchVector);
            Debug.Log(angle);
            if (angle > minAngle && angle < maxAngle)
            {
                grabbed = false;
                throwingObject.GetComponent<Rigidbody2D>().drag = 0;
                Destroy(throwingObject.GetComponent<DistanceJoint2D>());
                throwingObject.GetComponent<Rigidbody2D>().AddForce(launchVector * 20, ForceMode2D.Impulse);
                Destroy(throwingObject.GetComponent<DistanceJoint2D>());
                Destroy(go);
                go = null;
                gameObject.GetComponent<CameraMovement>().StartTracking();
                gameObject.GetComponent<SlingShot>().enabled = false;
                
            }
            else
            {
                throwingObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                throwingObject.transform.position = startPosition;
            }
            throwingObject.transform.gameObject.GetComponent<Collider2D>().enabled = true;

        }

        /*if (thrown && throwingObject.transform.position.x > followThreshold)
        {
        }*/

        if (grabbed && go == null)
        {
            go = new GameObject("Rigidbody dragger");
            go.AddComponent<Rigidbody2D>();
            go.AddComponent<SpringJoint2D>();
            go.GetComponent<Rigidbody2D>().isKinematic = true;

            go.GetComponent<SpringJoint2D>().connectedBody = throwingObject.GetComponent<Rigidbody2D>();
            go.GetComponent<SpringJoint2D>().autoConfigureDistance = false;
            go.GetComponent<SpringJoint2D>().distance = 0;
            go.GetComponent<SpringJoint2D>().frequency = 3;
        }
    }
}
