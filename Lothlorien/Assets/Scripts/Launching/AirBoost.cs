using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirBoost : MonoBehaviour
{

    bool isAiming;
    public bool isAvailable;
    bool slowedDown;
    float aimTimer;
    float cooldownTimer;
    float angle;
    [HideInInspector]
    public float disabledDuration = 0;
    [HideInInspector]
    public float disabledTimer = 0;

    Vector2 mousePos;
    Vector2 angleComparison = new Vector2(1, 0);
    Vector2 outOfScreen;
    public GameObject boostedObject;
    public GameObject boostParticle;
    public GameObject arrowHead;
    public GameObject arrowBody;
    public GameObject arrowTail;
    public GameObject arrowFront;
    public BackgroundManager backgroundManager;
    public Image[] boostDisabled;

    public Slider aimTimeSlider;

    [Header("Boost and aiming settings")]
    [Tooltip("How long does the player have to aim the boost. Also slows down time for the duration")]
    public float aimTime = 0.5f;
    [Tooltip("Maximum angle that can be used to boost. The angles go to 180° and -180°")]
    public float maxAngle = 90f;
    [Tooltip("Minimun angle that can be used to boost. The angles go to 180° and -180°")]
    public float minAngle = -90f;
    [Tooltip("How often can the boost be used")]
    public float cooldown = 2f;
    [Tooltip("How many times can the boost be used")]
    public int boostAmount = 2;
    [Tooltip("Max amount of boosts that can be stored")]
    public int maxBoostAmount = 5;
    [Tooltip("Maximum length for the aiming arrow")]
    public float arrowMaxLength = 3f;
    [Tooltip("Minimun length for the aiming arrow")]
    public float arrowMinLength = 1f;
    [Tooltip("Boost velocity multiplier. Boost redirects current velocity to the pointed direction then multiplies it by this")]
    public float boostMultiplier = 1f;
    [Tooltip("Fixed number to add to the velocity. The original velocity is multiplied first, then this is added to lessen the effect of the multiplier")]
    public float boostFixedStep = 20f;
    [Tooltip("Percentage of how much velocity is being redirected")]
    [Range(0, 100)] public float boostVelocityRedirectPercent = 50f;

    public static AirBoost airBoost;

    // Start is called before the first frame update
    void Start()
    {
        outOfScreen = new Vector2(-100, -100);
        aimTimeSlider.maxValue = 0;
        
        airBoost = this;
        aimTimer = 0;
        cooldownTimer = 0;
        angle = 0;
        isAiming = false;
        isAvailable = false;
        slowedDown = false;
        boostVelocityRedirectPercent /= 100;
        boostAmount = maxBoostAmount;
    }

    public void setDisabled(float duration)
    {
        disabledDuration = duration;
        disabledTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        aimTimeSlider.minValue = -aimTime;
        /*if (Input.mousePosition.x > Screen.width / 8)
        {
            print("Mouse is in boostable area");
        }
        if (Input.mousePosition.x < Screen.width / 8)
        {
            print("Not boostable");
        }*/
        //Debug.Log(isAiming + " " + slowedDown + " " + isAvailable);
        if (!isAiming && slowedDown)
        {
            //Debug.Log("PING");
            HideTimer();
            slowedDown = transform.GetComponent<TimeWarp>().ToggoleSlowTime();
            arrowHead.transform.position = outOfScreen;
            arrowBody.transform.position = outOfScreen;
            arrowTail.transform.position = outOfScreen;
            arrowFront.transform.position = outOfScreen;
        }

        if (transform.GetComponent<FingerSling>().enabled)
        {
            isAiming = false;
            HideTimer();
            return;
        }
        if (boostAmount <= 0)
        {
            isAiming = false;
            HideTimer();
            return;
        }
        if (cooldownTimer < cooldown)
        {
            isAiming = false;
            HideTimer();
            cooldownTimer += Time.deltaTime;
            foreach (Image img in boostDisabled)
            {
                img.enabled = true;
            }

            return;
        }
        if (disabledTimer < disabledDuration)
        {
            isAiming = false;
            HideTimer();
            disabledTimer += Time.deltaTime;
            foreach(Image img in boostDisabled)
            {
                img.enabled = true;
            }
            
            return;
        }
        else
        {
            disabledDuration = 0;
            disabledTimer = 0;
        }
        foreach (Image img in boostDisabled)
        {
            img.enabled = false;
        }


        if (!isAvailable || boostedObject.GetComponent<PlayerTest>().inObstacle)
        {
            isAiming = false;
            HideTimer();
            return;
        }

        if (isAiming)
        {
            aimTimer += Time.unscaledDeltaTime;
            if (aimTimer >= aimTime)
            {
                isAiming = false;
                cooldownTimer = 0;
                aimTimer = 0;
            }
            Aim();
        }
        if (Input.GetButtonDown("Fire1") && Input.mousePosition.x > Screen.width / 8)
        {
            if (isAvailable && !slowedDown)
            {
                slowedDown = transform.GetComponent<TimeWarp>().ToggoleSlowTime();
                isAiming = true;
            }
            else
                isAiming = false;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (slowedDown && isAiming)
            {
                slowedDown = transform.GetComponent<TimeWarp>().ToggoleSlowTime();
                Boost();
                HideTimer();
                cooldownTimer = 0;
                arrowHead.transform.position = outOfScreen;
                arrowBody.transform.position = outOfScreen;
                arrowTail.transform.position = outOfScreen;
                arrowFront.transform.position = outOfScreen;
            }
            aimTimer = 0;
            isAiming = false;

        }
        //boostAmount = Mathf.Clamp(boostAmount, 0, maxBoostAmount);

    }

    Vector2 lastValidAngle;
    Vector2 lastValidPos;
    Vector2 invalidAngleVector;
    float distanceBetween;
    float vectorX;
    float vectorY;
    private void Aim()
    {
        DisplayTimer();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Vector2.SignedAngle(angleComparison, mousePos - AsVector2(boostedObject.transform.position));
        //Debug.Log(angle);
        if (angle > minAngle && angle < maxAngle)
        {
            //arrowHead.transform.position = mousePos;
            lastValidAngle = mousePos;
            lastValidPos = AsVector2(boostedObject.transform.position);
            //objectToMouse = mousePos - AsVector2(boostedObject.transform.position);
            Strech(arrowBody, arrowHead, boostedObject.transform.position, mousePos, false);
        }
        else if (angle > 0)          //aiming at upper part of the screen
        {
            //New way calculated vector based on the min/max angle. nothing hardcoded. works better with low fps.
            distanceBetween = Vector2.Distance(AsVector2(boostedObject.transform.position), mousePos);
            vectorX = distanceBetween * Mathf.Cos((Mathf.PI / 180) * maxAngle);
            vectorY = distanceBetween * Mathf.Sin((Mathf.PI / 180) * maxAngle);
            invalidAngleVector = new Vector2(vectorX, vectorY);
            invalidAngleVector += AsVector2(boostedObject.transform.position);
            Strech(arrowBody, arrowHead, boostedObject.transform.position, invalidAngleVector, false);
            //Debug.Log(vectorX + " " + vectorY + " " + maxAngle + " " + distanceBetween);

            //old way calculated with the last know good angle. if no good angle was available defaulted to a hardcoded value.
            /*if (lastValidAngle == Vector2.zero)
            {
                Strech(arrowBody, arrowHead, boostedObject.transform.position, AsVector2(boostedObject.transform.position) + (new Vector2(0, 1) * distanceBetween), false);
            }
            else
            {
                tempAngleVector = lastValidAngle - lastValidPos;
                tempAngleVector = Vector3.Normalize(tempAngleVector);
                tempAngleVector *= distanceBetween;
                tempAngleVector = AsVector2(boostedObject.transform.position) + tempAngleVector;
                Strech(arrowBody, arrowHead, boostedObject.transform.position, tempAngleVector, false);
            }*/

        }
        else if (angle < 0)         //aiming at lower part of the screen
        {
            distanceBetween = Vector2.Distance(AsVector2(boostedObject.transform.position), mousePos);
            vectorX = distanceBetween * Mathf.Cos((Mathf.PI / 180) * minAngle);
            vectorY = distanceBetween * Mathf.Sin((Mathf.PI / 180) * minAngle);
            invalidAngleVector = new Vector2(vectorX, vectorY);
            invalidAngleVector += AsVector2(boostedObject.transform.position);
            Strech(arrowBody, arrowHead, boostedObject.transform.position, invalidAngleVector, false);
            //Debug.Log(vectorX + " " + vectorY + " " + maxAngle + " " + distanceBetween);
        }
        //lastValidAngle = Vector2.zero;
    }

    void DisplayTimer()
    {
        aimTimeSlider.gameObject.SetActive(true);
        aimTimeSlider.value = -aimTimer;
    }

    void HideTimer()
    {
        aimTimeSlider.gameObject.SetActive(false);
    }

    Vector2 currentSpeedVector;
    Vector2 boostVector;
    private void Boost()
    {
        AudioManager.PlaySound("porky_airboost");
        boostAmount--;
        currentSpeedVector.y = boostedObject.GetComponent<Rigidbody2D>().velocity.y;
        currentSpeedVector.x = -backgroundManager.xSpeed;
        //Debug.Log("Current vector " + currentSpeedVector.magnitude);

        //Ver.1 Completely redirects velocity to wanted angle and boosts it
        /*boostVector = Vector3.Normalize(fullDirection);
        boostVector *= currentSpeedVector.magnitude + boostFixedStep;
        boostVector *= boostMultiplier;
        backgroundManager.xSpeed = -boostVector.x;
        boostVector.x = 0;
        boostedObject.GetComponent<Rigidbody2D>().velocity = boostVector;*/
        //End of Ver.1

        //Ver. 2 Redirects x% of velocity to wanted angle and boosts it
        float currentSpeed = currentSpeedVector.magnitude;
        currentSpeedVector = Vector3.Normalize(currentSpeedVector);     //Boost current velocity with set values
        currentSpeedVector *= currentSpeed + boostFixedStep;
        currentSpeedVector *= boostMultiplier;
        //Debug.Log("Current boosted vector " + currentSpeedVector.magnitude);

        boostVector = Vector3.Normalize(fullDirection);
        boostVector *= currentSpeedVector.magnitude;
        boostVector.x *= boostVelocityRedirectPercent;
        boostVector.y *= boostVelocityRedirectPercent;
        //Debug.Log("Redirected vector " + boostVector.magnitude);

        currentSpeedVector.x *= 1 - boostVelocityRedirectPercent;     //Take the percentage off of the original velocity
        currentSpeedVector.y *= 1 - boostVelocityRedirectPercent;
        backgroundManager.xSpeed = -currentSpeedVector.x - boostVector.x;     //Add redirected velocity back
        boostVector.x = 0;
        currentSpeedVector.x = 0;
        boostedObject.GetComponent<Rigidbody2D>().velocity = currentSpeedVector + boostVector;

        boostedObject.GetComponent<PlayerTest>().torque = 0;
        boostedObject.transform.right = new Vector3(-backgroundManager.xSpeed, boostedObject.GetComponent<Rigidbody2D>().velocity.y, 0);        //This maybe should be moved to a different script

        boostedObject.transform.GetChild(0).GetComponent<Shake>().StopShake();
        gameObject.GetComponent<CameraMovement>().backgroundManager.GetComponent<AnimationManager>().ChangeBoostSprite();
        boostParticle.transform.right = -boostedObject.transform.right;
        boostParticle.GetComponent<ParticleSystem>().Play();
        /*Vector2 newSpeedVector;
        newSpeedVector.y = boostedObject.GetComponent<Rigidbody2D>().velocity.y;
        newSpeedVector.x = -backgroundManager.xSpeed;
        Debug.Log("New boosted vector " + newSpeedVector.magnitude);*/
        //End of Ver.2
    }

    public static Vector2 AsVector2(Vector3 _v)
    {
        return new Vector2(_v.x, _v.y);
    }

    float distanceFromFinger = 1f;
    float distanceFromObject = 0.5f;
    float pointDistanceFromBody = 0.2f;

    Vector2 fullDirection;
    Vector2 direction;
    Vector3 scale;
    public void Strech(GameObject sprite, GameObject point, Vector2 initialPosition, Vector2 finalPosition, bool mirrorZ)
    {
        fullDirection = finalPosition - initialPosition;
        direction = Vector3.Normalize(fullDirection);
        sprite.transform.position = initialPosition + (direction * distanceFromObject);
        sprite.transform.right = direction;
        if (mirrorZ) sprite.transform.right *= -1f;
        scale = new Vector3(1, 1, 1);
        scale.x = Vector3.Distance(initialPosition, finalPosition - (direction * distanceFromFinger));
        scale.x = Mathf.Clamp(scale.x, arrowMinLength, arrowMaxLength);
        sprite.transform.localScale = scale;
        point.transform.position = AsVector2(sprite.transform.position) + (direction * scale.x) + (direction * pointDistanceFromBody);
        point.transform.right = direction;
        arrowTail.transform.position = initialPosition + (direction * distanceFromObject);
        arrowTail.transform.right = direction;
        arrowFront.transform.position = AsVector2(sprite.transform.position) + (direction * scale.x);
        arrowFront.transform.right = direction;
    }

}

