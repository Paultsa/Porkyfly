using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerTest : MonoBehaviour
{
    //For AnimationManager
    [HideInInspector] public bool rotating = false;


    [HideInInspector] public bool inObstacle = false;
    bool doneOnce = false;

    [HideInInspector]
    public bool minHeightActive = true;
    [HideInInspector]
    public bool orbitalCrash = false;

    private Quaternion origRotation;
    public float turnTimer;

    public float maximumTorque;
    public float minimumTorque;
    public float torqueSlowMultiplier;
    [HideInInspector] public float torque;
    [SerializeField] private float torqueMultiplier;
    [SerializeField] private float torqueSlowDownTime;
    private float torqueTimer = 0;

    public float maxStayOnGroundTime;
    private float timer = 0;
    public float slowDownMinimumSpeed;
    public float slowDownSpeedDivider;
    public bool stop = false;
    public bool failStop = false;
    public float minimiPomppu;
    Vector2 startPos;
    bool triggered = false;
    public bool obstacleTrigger = false;
    public BackgroundManager backgroundManager;
    public GameManager gameManager;
    public AnimationManager animationManager;
    public float shotForce;
    public float groundSlowDownForward;
    public float groundSlowDownUpward;
    public float gravityScale;
    public float bounce;
    public float origBounce;
    public float bounceAndSlideThreshold;
    public FingerSling fingerSling;
    public GameObject groundLevel;
    private Rigidbody2D rb;
    float minHeight;

    public PhysicsMaterial2D playerMaterial;
    public float physicsBounce;

    public static GameObject player;

    private void Awake()
    {
        player = this.gameObject;

    }
    void Start()
    {
        playerMaterial.bounciness = physicsBounce;
        minHeight = groundLevel.transform.position.y + transform.GetChild(0).GetComponent<Renderer>().bounds.extents.y;
        Debug.Log(minHeight);
        origRotation = transform.rotation;
        startPos = transform.position;
        origBounce = bounce;
        rb = GetComponent<Rigidbody2D>();

    }
    private void FixedUpdate()
    {
        if (failStop)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }
    private void Update()
    {

        //If camera is moving along with Porky and also is not in stopping animation
        if ((backgroundManager.moving || backgroundManager.movingY) && !stop)
        {
            if (!doneOnce && torque == 0)
            {
                transform.right = rb.velocity;
                animationManager.ChangeFlyingSprite();
                doneOnce = true;
            }

            transform.Rotate(new Vector3(0, 0, torque * (torqueMultiplier * Time.deltaTime)));
            //Start slowing down rotation automatically
            if (Mathf.Abs(torque) > minimumTorque)
            {
                rotating = true;
                torqueTimer += Time.deltaTime;
                if (torqueTimer >= torqueSlowDownTime)
                {
                    torque *= torqueSlowMultiplier;
                    torqueTimer = 0;
                }
                if (animationManager.isHitByLightning)
                {
                    torque = 0;
                }
            }
            else if (Mathf.Abs(torque) <= minimumTorque)
            {
                Vector3 currentSpeedVector = Vector3.zero;
                if (backgroundManager.outOfBounds)
                {
                    currentSpeedVector.y = rb.velocity.y;
                    currentSpeedVector.x = -backgroundManager.xSpeed;
                }
                else
                {
                    currentSpeedVector.y = rb.velocity.y;
                    currentSpeedVector.x = rb.velocity.x;
                }
                float angle = Mathf.Atan2(currentSpeedVector.y, currentSpeedVector.x) * Mathf.Rad2Deg;
                if (!animationManager.isHitByLightning)
                {
                    if (torque == 0)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), turnTimer * Time.deltaTime);
                    }
                    else if (Vector2.Angle(transform.right, currentSpeedVector) < 15)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), turnTimer * Time.deltaTime);
                        if (rotating)
                        {
                            if (!animationManager.wasBoosted)
                            {
                                animationManager.ChangeFlyingSprite();
                                animationManager.wasBoosted = false;
                            }
                            rotating = false;
                        }
                        torque = 0;
                    }
                }
                else
                {
                    transform.right = Vector2.right;
                }

            }
            //Maximum torque
            if (Mathf.Abs(torque) > maximumTorque)
            {
                torque = -maximumTorque;
            }
        }

        if (minHeightActive)
        {
            Vector2 tempVector = transform.position;
            tempVector.y = Mathf.Clamp(tempVector.y, minHeight, Mathf.Infinity);
            transform.position = tempVector;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            transform.GetChild(0).GetComponent<Shake>().StopShake();

            if (!orbitalCrash)
            {
                if (!triggered && backgroundManager.outOfBounds && !stop)
                {
                    Debug.Log("OutOfBounds collision");
                    backgroundManager.xSpeed *= groundSlowDownForward;
                    if (backgroundManager.outOfBounds)
                    {
                        torque = backgroundManager.xSpeed;
                    }
                    else
                    {
                        torque = -rb.velocity.x;
                    }
                    animationManager.ChangeSpinningSprite();
                    if (backgroundManager.xSpeed * Time.unscaledDeltaTime >= -bounceAndSlideThreshold)
                    {
                        if (Mathf.Abs(rb.velocity.y) <= minimiPomppu)
                        {
                            if (!inObstacle)
                            {
                                stop = true;
                            }
                        }
                        else
                        {
                            animationManager.ChangeSpinningSprite();
                            Debug.Log("PUOLITA1");
                            Vector2 tempVector = new Vector2(rb.velocity.x, rb.velocity.y /* -1*/ * groundSlowDownUpward / 2);
                            rb.velocity = tempVector;
                        }
                    }
                    else
                    {
                        Vector2 tempVector = new Vector2(rb.velocity.x, rb.velocity.y /* -1*/ * groundSlowDownUpward);
                        tempVector.y = Mathf.Clamp(tempVector.y, 2, Mathf.Infinity);
                        rb.velocity = tempVector;
                    }


                    AudioManager.PlaySound("porky_bounce");

                }
                else if (!backgroundManager.outOfBounds && !stop && rb.velocity.x <= bounceAndSlideThreshold && !triggered)
                {
                    Debug.Log("fail");
                    playerMaterial.bounciness = 0;
                    transform.position = new Vector2(transform.position.x, 1.5f);
                    failStop = true;
                    stop = true;

                }
                else if (!stop && !triggered)
                {
                    animationManager.ChangeSpinningSprite();
                    torque = -rb.velocity.x;
                    Vector2 tempVector = new Vector2(rb.velocity.x, rb.velocity.y /* -1*/ * groundSlowDownUpward);
                    tempVector.y = Mathf.Clamp(tempVector.y, 2, Mathf.Infinity);
                    rb.velocity = tempVector;
                }
            }
            else
            {
                //Change sprite on crash. may be some different one?
                animationManager.ChangeSpinningSprite();
                torque = backgroundManager.xSpeed;
                GetComponent<OrbitalThunderBoost>().ThunderBound();
                Camera.main.GetComponent<AirBoost>().isAvailable = true;
                Camera.main.GetComponent<AirBoost>().setDisabled(1.5f);
                AudioManager.PlaySound("porky_boing");
            }
            triggered = true;
            orbitalCrash = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            timer = 0;
            triggered = false;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            obstacleTrigger = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 && backgroundManager.moving && !stop && !inObstacle)
        {
            timer += Time.deltaTime;
            if (timer >= maxStayOnGroundTime)
            {
                if (backgroundManager.outOfBounds)
                {
                    stop = true;
                }
                else
                {
                    failStop = true;
                    stop = true;

                }
            }

        }

    }

    public void ObstacleTrigger(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            if (!obstacleTrigger && backgroundManager.moving && !stop && !orbitalCrash)
            {
                collision.GetComponent<Obstacle>().ObstacleHit(false, gameObject);
                animationManager.ChangeSpinningSprite();
                if (backgroundManager.xSpeed < 0)
                {
                    torque = backgroundManager.xSpeed;
                }

            }
        }
    }

    public void UpdateBounciness()
    {
        playerMaterial.bounciness = physicsBounce;
    }
}