using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerSling : MonoBehaviour
{
    public ParticleSystem boostParticle;
    public StartMenu startMenu;
    public bool grabbed = false;
    bool thrown = false;
    GameObject go = null;
    GameObject throwingObject;
    RaycastHit2D hit;
    Vector2 angleComparison = new Vector2(1, 0);
    float angle;
    public GameObject throwBoostTimer;
    float timerOrignalScale;
    GameObject oscillatingCircle;
    float direction;
    float boostTimerDisableDuration = 0.5f;
    float boostTimerDisableTimer = 0;
    float boostTimerEnableDuration = 1f;
    float boostTimerEnableTimer = 0;
    float boostTimerSensitivity = 3f;
    float forceMultiplier;
    public static bool throwBoostActive;

    public static FingerSling fingerSling;


    [Header("Release settings")]
    [Tooltip("Max angle when the ball can be released. The angles go to 180° and -180°")]
    public float maxAngle = 90f;
    [Tooltip("Min angle when the ball can be released. The angles go to 180° and -180°")]
    public float minAngle = 0f;

    [Header("Sling settings")]
    [Tooltip("This sets how 'loose' the sling is. 0 is probably the best for this")]
    public float distance = 0f;
    [Tooltip("Maximum distance the sling is allowed to stretch. bigger number means potentially stringer throws")]
    public float maxDistance = 10f;
    [Tooltip("0 < value < 5. 5 is a really stiff sling. This affects how hard the ball can be thrown")]
    public float springStrength = 1f;
    [Tooltip("Force multiplier for the throw. This affects how hard the ball can be thrown")]
    public float throwForceMultiplier = 1f;
    [Tooltip("Bonus percent on timed throw")]
    public float forceBonusPercentBoost = 20f;
    [Tooltip("How much the object resists movement. This affects how hard the ball can be thrown. Less is more powerful but eventually makes the sling slightly more wobbly at rest")]
    public float drag = 1f;
    /*[Tooltip("How fast the timer oscillates")]
    public float timerSpeed;
    [Tooltip("The minimun scale for the oscillating circle. This has to be less than 1")]
    public float OscillateMinSize;*/

    // Start is called before the first frame update
    void Start()
    {
        throwBoostActive = false;
        fingerSling = this;
        forceBonusPercentBoost /= 100;
        //oscillatingCircle = throwBoostTimer.transform.GetChild(0).gameObject;
        //timerOrignalScale = oscillatingCircle.transform.localScale.x;
        forceMultiplier = throwForceMultiplier;
        AudioManager.PlaySound("music_stop");
    }

    // Update is called once per frame
    void Update()
    {
        if (throwBoostTimer.activeSelf)
        {
            /*if (oscillatingCircle.transform.localScale.x >= timerOrignalScale)
            {
                direction = -timerSpeed;
            }
            else if (oscillatingCircle.transform.localScale.x <= OscillateMinSize)
            {
                direction = timerSpeed;
            }
            oscillatingCircle.transform.localScale *= (1 + direction);*/
            if (throwBoostActive)
            {
                //oscillatingCircle.GetComponent<SpriteRenderer>().color = Color.red;
                forceMultiplier = throwForceMultiplier + forceBonusPercentBoost;
            }
            else
            {
                //oscillatingCircle.GetComponent<SpriteRenderer>().color = Color.white;
                forceMultiplier = throwForceMultiplier;
            }
        }
        else
        {
            forceMultiplier = throwForceMultiplier;
        }


        if (throwingObject != null && throwingObject.transform.GetComponent<Rigidbody2D>().velocity.magnitude >= boostTimerSensitivity)
        {
            if (boostTimerDisableTimer != 0)
            {
                //AudioManager.PlaySound("porky_sling");
            }
            boostTimerDisableTimer = 0;
            boostTimerEnableTimer += Time.deltaTime;

            //Debug.Log(throwingObject.transform.GetComponent<Rigidbody2D>().velocity.magnitude + " MAG " + boostTimerEnableTimer);
            if (throwBoostTimer.activeSelf || boostTimerEnableTimer >= boostTimerEnableDuration)
            {
                if (!throwBoostTimer.activeSelf)
                {
                    throwBoostTimer.SetActive(true);
                }
                boostTimerEnableTimer = 0;

            }
        }
        else
        {
            boostTimerEnableTimer = 0;
            boostTimerDisableTimer += Time.deltaTime;
            if (boostTimerDisableTimer >= boostTimerDisableDuration)
            {
                //oscillatingCircle.transform.localScale = new Vector3(timerOrignalScale, timerOrignalScale, timerOrignalScale);
                throwBoostTimer.SetActive(false);
                boostTimerDisableTimer = 0;
            }
        }

        if (Input.GetButton("Fire1") && !thrown && startMenu.inMainMenu)
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.transform.CompareTag("Throwable") && !grabbed)
            {
                Debug.Log("HIT");
                hit.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                hit.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                hit.transform.gameObject.GetComponent<Rigidbody2D>().drag = drag;
                hit.transform.gameObject.GetComponent<Collider2D>().enabled = false;
                throwingObject = hit.transform.gameObject;
                //minHeight = groundLevel.transform.position.y + throwingObject.transform.GetChild(0).GetComponent<Renderer>().bounds.extents.y;
                grabbed = true;
            }
            //While button is held, object is being dragged
            if (go != null)
            {
                go.transform.position = hit.point;

                //Vector2 tempVector = throwingObject.transform.position;
                //tempVector.y = Mathf.Clamp(tempVector.y, minHeight, Mathf.Infinity);
                //throwingObject.transform.position = tempVector;
            }
        }
        else if (grabbed)       //When button is released, destroy spring anchor
        {
            angle = Vector2.SignedAngle(angleComparison, throwingObject.GetComponent<Rigidbody2D>().velocity);
            //Debug.Log(throwingObject.GetComponent<Rigidbody2D>().velocity + " " + angle);
            if (angle > minAngle && angle < maxAngle)
            {
                if (throwBoostActive)
                    boostParticle.GetComponent<ParticleSystem>().Play();
                grabbed = false;
                Destroy(go);
                go = null;
                throwingObject.GetComponent<Rigidbody2D>().drag = 0;
                throwingObject.GetComponent<Rigidbody2D>().velocity *= forceMultiplier;
                gameObject.GetComponent<CameraMovement>().StartTracking();
                throwingObject.transform.gameObject.GetComponent<Collider2D>().enabled = true;
                throwingObject.transform.GetChild(2).gameObject.SetActive(true);
                throwBoostTimer.SetActive(false);
                Debug.Log(throwingObject.transform.GetComponent<Rigidbody2D>().velocity.magnitude + " THROWN");
                throwingObject = null;
                ObstacleSpawner.obstacleSpawner.spawning = true;
                gameObject.GetComponent<FingerSling>().enabled = false;
                AudioManager.PlaySound("music_gameplay");
                
            }
            //else if (throwingObject.GetComponent<Rigidbody2D>().velocity.magnitude == 0)
        }

        if (grabbed && go == null)
        {
            go = new GameObject("Rigidbody dragger");
            go.AddComponent<Rigidbody2D>();
            go.AddComponent<SpringJoint2D>();
            go.AddComponent<DistanceJoint2D>();
            go.GetComponent<Rigidbody2D>().isKinematic = true;

            go.GetComponent<SpringJoint2D>().connectedBody = hit.rigidbody;
            go.GetComponent<DistanceJoint2D>().connectedBody = hit.rigidbody;

            go.GetComponent<SpringJoint2D>().autoConfigureDistance = false;
            go.GetComponent<SpringJoint2D>().distance = distance;
            go.GetComponent<SpringJoint2D>().frequency = springStrength;

            go.GetComponent<DistanceJoint2D>().autoConfigureDistance = false;
            go.GetComponent<DistanceJoint2D>().maxDistanceOnly = true;
            go.GetComponent<DistanceJoint2D>().distance = maxDistance;
        }
    }
}
