using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    //public GameObject manager;
    // Start is called before the first frame update
    [HideInInspector]
    public bool stage1 = false;
    [HideInInspector]
    public bool stage2 = false;
    int direction = 1;
    int bonusFramesCounter;
    float timer;

    int bonusMaxPowerFrames = 3;
    float arrowRotateSpeed = 250f;
    float powerBarFillSpeed = 2.1f;

    Vector3 tempVector;
    [HideInInspector]
    public float minForceMultiplier;
    [HideInInspector]
    public float maxForceMultiplier;
    float forceMultiplier;
    [HideInInspector]
    public float duration;
    [HideInInspector]
    public GameObject powerBar;
    [HideInInspector]
    public GameObject arrow;
    [HideInInspector]
    public GameObject throwable;
    [HideInInspector]
    public float prevMagnitude;
    [HideInInspector]
    public float maxBonus;
    public float minFixedForceBonus;
    public float maxFixedForceBonus;
    public float maxFixedBonus;
    float fixedForceBonus;

    Vector3 originalScale;
    void Start()
    {
    }

    private void OnEnable()
    {
        if (throwable != null)
        {
            throwable.GetComponent<PlayerTest>().inObstacle = true;
            throwable.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            throwable.transform.position = transform.GetComponent<Obstacle>().throwablePos.transform.position;
            throwable.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            throwable.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        }
        if (powerBar != null)
        {
            powerBar.transform.localPosition = new Vector2(-3.7f, 2.7f);
            tempVector = powerBar.transform.GetChild(0).localScale;
            originalScale = powerBar.transform.GetChild(0).GetChild(0).GetChild(0).lossyScale;
        }
        if (arrow != null)
        {
            arrow.transform.localPosition = new Vector2(2.7f, 4f);
            arrow.transform.Rotate(0.0f, 0.0f, Random.Range(0.1f, 0.6f));
        }
        Camera.main.GetComponent<AirBoost>().isAvailable = false;
        gameObject.GetComponent<Obstacle>().manager.xSpeed = 0;
        timer = 0;
        bonusFramesCounter = 0;
        tempVector.y = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Input.mousePosition.x > Screen.width / 8)
        {
            LaunchStages();
        }

        /*if (timer > duration)
        {
            //stage1 = true;
            //LaunchStages();
            //Launch();
        }
        timer += Time.deltaTime;*/

        if (!stage1 && !stage2)
        {
            if (arrow.transform.rotation.z >= 0.6f)
            {
                direction = -1;
            }
            if (arrow.transform.rotation.z <= 0.1f)
            {
                direction = 1;
            }
            arrow.transform.Rotate(0.0f, 0.0f, direction * arrowRotateSpeed * Time.deltaTime, Space.Self);
        }
        if (stage1 && !stage2)
        {
            if (powerBar.transform.GetChild(0).localScale.y >= 1f)
            {
                if (bonusFramesCounter >= bonusMaxPowerFrames)
                {
                    direction = -1;
                    bonusFramesCounter = 0;
                }
                else
                {
                    bonusFramesCounter++;
                }
            }
            if (powerBar.transform.GetChild(0).localScale.y <= 0.03f)
            {
                direction = 1;
            }

            tempVector.y += 1.5f * direction * powerBarFillSpeed * Time.deltaTime;
            tempVector.y = Mathf.Clamp(tempVector.y, 0.02f, 1);
            powerBar.transform.GetChild(0).localScale = tempVector;

            SetGlobalScale(powerBar.transform.GetChild(0).GetChild(0).GetChild(0), originalScale);
        }
        //Debug.Log(tempVector.y + "POWAAAA");
    }

    public static void SetGlobalScale(Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }

    public void LaunchStages()
    {
        if (stage1)
        {

            /*Quaternion rotation = Quaternion.AngleAxis(45, Vector3.forward);
            Vector2 test = rotation * Vector2.right;
            Debug.Log(test + " ANGLE TEST");*/

            //Launch
            //Launch();
            stage2 = true;

        }
        if (!stage1)
            stage1 = true;
    }

    public void Launch()
    {
        fixedForceBonus = Mathf.Lerp(minFixedForceBonus, maxFixedForceBonus, tempVector.y);
        forceMultiplier = Mathf.Lerp(minForceMultiplier, maxForceMultiplier, tempVector.y);
        GetComponent<Obstacle>().backgroundManager.GetComponent<AnimationManager>().ChangeFlyingSprite();
        if (tempVector.y == 1)
        {
            //throwable.transform.GetChild(0).GetComponent<Shake>().StartShake(2f, 1f);
            Shake.shaker.StartShake(2f, 1f);
            gameObject.GetComponent<Obstacle>().backgroundManager.GetComponent<AnimationManager>().ChangeBearBoostSprite();
            forceMultiplier += maxBonus;
            fixedForceBonus += maxFixedBonus;
        }
        //Debug.Log(forceMultiplier);
        throwable.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        Vector2 newVelocity = arrow.transform.right * ((prevMagnitude * forceMultiplier) + fixedForceBonus);
        //Vector2 newVelocity = arrow.transform.right * ((prevMagnitude + fixedForceBonus) * forceMultiplier);
        //Debug.Log(newVelocity.magnitude + " " + prevMagnitude + " " + forceMultiplier + " " + fixedForceBonus);
        //newVelocity.x = 0;
        throwable.GetComponent<Rigidbody2D>().velocity = newVelocity;
        throwable.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        throwable.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
        throwable.transform.right = newVelocity;
        throwable.GetComponent<PlayerTest>().torque = 0;
        throwable.GetComponent<PlayerTest>().inObstacle = false;
        Camera.main.GetComponent<AirBoost>().isAvailable = true;
        Destroy(gameObject.GetComponent<Cannon>());
        gameObject.GetComponent<Obstacle>().DestroySelf();
        //Camera.main.GetComponent<CameraMovement>().StartTracking();
        //backgroundManager.moving = true;
    }
}
