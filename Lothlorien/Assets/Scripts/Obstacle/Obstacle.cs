using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum obstacleTypes
    {
        Bounce,
        Slow,
        BoostPickup,
        BoostDisable,
        AirDragThorns,
        BearCannon,
        HawkGrab,
        CashSquirrel,
        None
    };

    public enum animalType
    {
        Frog,
        Hawk,
        Hedgehog,
        BearCannon,
        Owl,
        Beaver,
        RabbitWhite,
        RabbitBrown,
        SleepingBear,
        SmallBirdBlue,
        SmallBirdRed,
        SquirrelFlying,
        SquirrelRunning,
        Other
    };

    public obstacleTypes obstacleType = obstacleTypes.Bounce;
    public obstacleTypes secondaryObstacleType = obstacleTypes.None;
    public animalType animal = animalType.Other;

    public float velocityMultiplierPercent;
    public float velocityFixedBoost;
    public float yToXPercentageIncrease;

    public float velocitySlowPercent;

    public int boostAmount;

    public float boostDisableTime;

    public float speed;
    public float minHeight;
    public float maxHeight;

    public float speedLossPercentPerSecond;
    public float airDragThornsDuration;

    public float cannonMinMultiplier;
    public float cannonMaxMultiplier;
    public float cannonMaxValueHitBonus;
    public float cannonMinFixedBonus;
    public float cannonMaxFixedBonus;
    public float cannonMaxValueHitFixedBonus;
    public float cannonDuration;
    public GameObject cannonArrow;
    public GameObject powerBar;
    public GameObject throwablePos;

    public float hawkFlightAngle;
    public float hawkAccelerationPercentage;
    public float hawkAccelerationFixed;
    public float hawkDuration;

    public int minCash;
    public int maxCash;
    public GameObject cashExplosion;

    public GameObject backgroundManager;
    public GameObject player;
    private Animator anim;
    //public Animation move;
    //bool movingAnimated;
    float frogMovement;

    int type;
    int secondaryType;
    public int typeAnimal;
    bool oneTime = false;
    public BackgroundManager manager;

    bool timeWarped;

    // Start is called before the first frame update
    void Start()
    {
        if (speed != 0)
        {
            float speedSpread = Random.Range(-1, 1);
            speed += speedSpread;
        }
        player = PlayerTest.player;

        if (gameObject.GetComponent<Animator>() != null)
        {
            anim = gameObject.GetComponent<Animator>();
            //anim.SetBool("Move", true);
        }
        type = (int)obstacleType;
        secondaryType = (int)secondaryObstacleType;
        typeAnimal = (int)animal;

        velocityMultiplierPercent /= 100;
        yToXPercentageIncrease /= 100;

        velocitySlowPercent = 1 - (velocitySlowPercent / 100);

        if (backgroundManager != null)
            manager = backgroundManager.GetComponent<BackgroundManager>();

        speedLossPercentPerSecond /= 100;

        cannonMinMultiplier /= 100;
        cannonMaxMultiplier /= 100;
        cannonMaxValueHitBonus /= 100;

        hawkAccelerationPercentage /= 100;

        if (typeAnimal == (int)animalType.Frog)
        {
            frogMovement = speed;
            speed = 0;
            StartFrogLeap();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(speed + " Speed");
        if ((!oneTime || typeAnimal == (int)animalType.Hedgehog || typeAnimal == (int)animalType.BearCannon || typeAnimal == (int)animalType.Beaver || typeAnimal == (int)animalType.SleepingBear || typeAnimal == (int)animalType.SquirrelFlying) && !Camera.main.GetComponent<FingerSling>().enabled)
            transform.parent.Translate((manager.xSpeed * Time.deltaTime * 1f) + (speed * Time.deltaTime), 0, 0);
        if (!oneTime && player != null && (transform.position.x <= player.transform.position.x - 30 && !oneTime || transform.position.x >= player.transform.position.x + 50))// && transform.parent.parent == null || transform.position.x >= transform.parent.parent.position.x + 5 && !oneTime && transform.parent.parent != null)
        {
            DestroySelf();
        }
    }

    Vector2 getCurrentSpeedVector()
    {
        Vector2 currentSpeedVector;
        currentSpeedVector.y = player.GetComponent<Rigidbody2D>().velocity.y;
        currentSpeedVector.x = -manager.xSpeed;
        return currentSpeedVector;
    }

    void SetCurrentSpeedVector(Vector2 speed)
    {
        manager.xSpeed = -speed.x;
        speed.x = 0;
        player.GetComponent<Rigidbody2D>().velocity = speed;
    }

    public bool ObstacleHit(bool ignore, GameObject _player)
    {
        if (ignore)
            return false;

        player = _player;
        oneTime = true;

        switch (type)
        {
            case 0:
                ObstacleBounce();
                //Final version will probably play a small few frame animation and the animation calls DestroySelf funtion with a keyframe at the end
                DestroySelf();
                break;
            case 1:
                ObstacleSlow();
                DestroySelf();
                break;
            case 2:
                ObstacleBoostPickup();
                DestroySelf();
                break;
            case 3:
                ObstacleBoostDisable();
                DestroySelf();
                break;
            case 4:
                AirDragThorns();
                DestroySelf();
                //Invoke("DestroySelf", 0.2f);
                break;
            case 5:
                BearCannon();
                //BearCannon will play its animations and destroy itself in Cannon script
                break;
            case 6:
                HawkGrabber();
                break;
            case 7:
                CashSquirrel();
                DestroySelf();
                break;

        }

        switch (secondaryType)
        {
            case 0:
                ObstacleBounce();
                break;
            case 1:
                ObstacleSlow();
                break;
            case 2:
                ObstacleBoostPickup();
                break;
            case 3:
                ObstacleBoostDisable();
                break;
            case 4:
                AirDragThorns();
                break;
        }

        switch (typeAnimal)
        {
            case (int)animalType.Frog:
                AudioManager.PlaySound("porky_boing");
                break;
            case (int)animalType.Hawk:      //Ääni tulee soimaan silloin kuin nappaa kiinni
                AudioManager.PlaySound("porky_boing_owl");
                break;
            case (int)animalType.Hedgehog:
                AudioManager.PlaySound("porky_boing_slowdown");
                break;
            //case (int)animalType.BearCannon:
            //   break;
            case (int)animalType.Owl:       //Ääni tulee soimaan silloin kuin nappaa kiinni
                AudioManager.PlaySound("porky_boing_owl");
                break;
            case (int)animalType.Beaver:
                AudioManager.PlaySound("porky_boing");
                break;
            case (int)animalType.RabbitWhite:
                AudioManager.PlaySound("porky_boing");
                break;
            case (int)animalType.RabbitBrown:
                AudioManager.PlaySound("porky_boing");
                break;
            case (int)animalType.SleepingBear:
                AudioManager.PlaySound("porky_boing_bear_sleeping");
                break;
            case (int)animalType.SmallBirdBlue:
                AudioManager.PlaySound("porky_boing_bird");
                break;
            case (int)animalType.SmallBirdRed:
                AudioManager.PlaySound("porky_boing_bird");
                break;
            case (int)animalType.SquirrelFlying:
                AudioManager.PlaySound("porky_boing");
                break;
            case (int)animalType.SquirrelRunning:
                AudioManager.PlaySound("porky_boing");
                break;
            case (int)animalType.Other:
                AudioManager.PlaySound("porky_boing");
                break;
        }
        return true;
    }

    void stopTimeSlow()
    {
        //Debug.Log("STOP TIME SLOW");
        if (timeWarped)
        {
            timeWarped = Camera.main.GetComponent<TimeWarp>().ToggolePause();
            //Camera.main.GetComponent<TimeWarp>().slowMultiplier = Camera.main.GetComponent<TimeWarp>().originalSlowMultiplier;
        }
    }

    bool ObstacleBounce()
    {
        //Debug.Log("BOUNCE");
        Vector2 currentSpeedVector = getCurrentSpeedVector();
        Vector2 oldSpeedVector = getCurrentSpeedVector();
        currentSpeedVector.y = Mathf.Abs(currentSpeedVector.y);
        //currentSpeedVector.y = Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.y);
        //currentSpeedVector.x = -manager.xSpeed;
        //Debug.Log(currentSpeedVector);
        //float magnitude = currentSpeedVector.magnitude;
        //currentSpeedVector = Vector3.Normalize(currentSpeedVector);
        //currentSpeedVector *= ((magnitude * (1 + velocityMultiplierPercent)) + velocityFixedBoost);
        currentSpeedVector.y += Mathf.Clamp((currentSpeedVector.y * velocityMultiplierPercent + velocityFixedBoost) * (1 + yToXPercentageIncrease), velocityFixedBoost * 3, oldSpeedVector.y + (velocityFixedBoost * 5) + 10);
        currentSpeedVector.x += (currentSpeedVector.x * velocityMultiplierPercent + velocityFixedBoost);
        SetCurrentSpeedVector(currentSpeedVector);

        if (typeAnimal == (int)animalType.Beaver || typeAnimal == (int)animalType.SleepingBear)
        {
            //Shake.shaker.StartShake(2f, 1f);
            if (!timeWarped)
            {
                //Camera.main.GetComponent<TimeWarp>().slowMultiplier = 1/50f;
                timeWarped = Camera.main.GetComponent<TimeWarp>().ToggolePause();
                Invoker.InvokeDelayed(stopTimeSlow, 0.2f);
            }
        }

        //Debug.Log(currentSpeedVector);

        return true;
    }
    bool ObstacleSlow()
    {
        Debug.Log("SLOW");
        backgroundManager.GetComponent<AnimationManager>().ChangeHurtSprite();
        Vector2 currentSpeedVector = getCurrentSpeedVector();
        currentSpeedVector.y = Mathf.Abs(currentSpeedVector.y);
        //currentSpeedVector.y = Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.y);
        //currentSpeedVector.x = -manager.xSpeed;
        float magnitude = currentSpeedVector.magnitude;
        currentSpeedVector = Vector3.Normalize(currentSpeedVector);
        currentSpeedVector *= (magnitude * velocitySlowPercent);
        currentSpeedVector.y = Mathf.Clamp(currentSpeedVector.y, 3, Mathf.Infinity);
        SetCurrentSpeedVector(currentSpeedVector);
        //manager.xSpeed = -currentSpeedVector.x;
        //currentSpeedVector.x = 0;
        //player.GetComponent<Rigidbody2D>().velocity = currentSpeedVector;
        return true;
    }
    bool ObstacleBoostPickup()
    {
        PickupBoost.Pickup();
        Debug.Log("BOOSTPICKUP");
        AirBoost.airBoost.boostAmount = Mathf.Clamp(AirBoost.airBoost.boostAmount + 1, 0, AirBoost.airBoost.maxBoostAmount);
        //Camera.main.GetComponent<AirBoost>().boostAmount += boostAmount;
        return true;
    }
    bool ObstacleBoostDisable()
    {
        Debug.Log("BOOSTDISABLE");
        Camera.main.GetComponent<AirBoost>().setDisabled(boostDisableTime);
        return true;
    }

    bool AirDragThorns()
    {
        Debug.Log("SPIKES");
        player.AddComponent<DragThorns>().enabled = false;
        player.GetComponent<DragThorns>().drag = speedLossPercentPerSecond;
        player.GetComponent<DragThorns>().duration = airDragThornsDuration;
        player.GetComponent<DragThorns>().enabled = true;
        return true;
    }

    bool BearCannon()
    {
        anim.SetBool("Grabbed", true);
        float prevMag = getCurrentSpeedVector().magnitude;
        GameObject tempPowerBar = Instantiate(powerBar, transform);
        GameObject tempArrow = Instantiate(cannonArrow, transform);
        gameObject.AddComponent<Cannon>().enabled = false;
        //gameObject.GetComponent<Cannon>().manager = backgroundManager;
        gameObject.GetComponent<Cannon>().minForceMultiplier = cannonMinMultiplier;
        gameObject.GetComponent<Cannon>().maxForceMultiplier = cannonMaxMultiplier;
        gameObject.GetComponent<Cannon>().maxBonus = cannonMaxValueHitBonus;
        gameObject.GetComponent<Cannon>().minFixedForceBonus = cannonMinFixedBonus;
        gameObject.GetComponent<Cannon>().maxFixedForceBonus = cannonMaxFixedBonus;
        gameObject.GetComponent<Cannon>().maxFixedBonus = cannonMaxValueHitFixedBonus;
        gameObject.GetComponent<Cannon>().duration = cannonDuration;
        gameObject.GetComponent<Cannon>().arrow = tempArrow;
        gameObject.GetComponent<Cannon>().throwable = player;
        gameObject.GetComponent<Cannon>().powerBar = tempPowerBar;
        gameObject.GetComponent<Cannon>().prevMagnitude = prevMag;
        gameObject.GetComponent<Cannon>().enabled = true;
        ObstacleSpawner.obstacleSpawner.PauseSpawner();
        return true;
    }

    bool HawkGrabber()
    {
        anim.SetBool("Grabbed", true);
        Quaternion rotation = Quaternion.AngleAxis(hawkFlightAngle, Vector3.forward);
        Vector2 dirVector = rotation * Vector2.right;

        player.AddComponent<HawkGrabber>().enabled = false;
        player.GetComponent<HawkGrabber>().dirVector = dirVector;
        player.GetComponent<HawkGrabber>().magnitude = getCurrentSpeedVector().magnitude;
        player.GetComponent<HawkGrabber>().grabber = gameObject;
        player.GetComponent<HawkGrabber>().accelerationPercentage = hawkAccelerationPercentage;
        player.GetComponent<HawkGrabber>().fixedAcceleration = hawkAccelerationFixed;
        player.GetComponent<HawkGrabber>().duration = hawkDuration;
        player.GetComponent<HawkGrabber>().enabled = true;

        return true;
    }

    bool CashSquirrel()
    {
        int cashGained = Random.Range(minCash, maxCash);
        backgroundManager.GetComponent<GameManager>().IncreaseCurrency(cashGained);
        Debug.Log(cashGained + " CASH GAINED!");
        GameObject cashPop = Instantiate(cashExplosion, transform.position, transform.rotation, null);
        cashPop.GetComponent<CashExplosion>().amount = cashGained;
        cashPop.GetComponent<CashExplosion>().absorber = player;
        return true;
    }

    public void DestroySelf()
    {
        StopAllCoroutines();
        //Debug.Log(typeAnimal + " DESTROY");
        if (GetComponent<BoxCollider2D>() != null)
            GetComponent<BoxCollider2D>().enabled = false;
        //CancelInvoke();
        if (typeAnimal != (int)animalType.SquirrelFlying)
            speed = 0;
        Vector2 popVector = new Vector2(-0.3f, 1);
        //Debug.Log(GetComponent<Rigidbody2D>());
        if (anim != null)
            anim.SetTrigger("Hit");       //make a kill frame anim for all animators
        if (transform.parent.GetComponent<Rigidbody2D>() == null && typeAnimal != (int)animalType.Hedgehog && typeAnimal != (int)animalType.BearCannon && typeAnimal != (int)animalType.Beaver && typeAnimal != (int)animalType.SleepingBear && typeAnimal != (int)animalType.SquirrelFlying)
            transform.parent.gameObject.AddComponent<Rigidbody2D>();
        if (transform.parent.GetComponent<Rigidbody2D>() != null)
        {
            transform.parent.GetComponent<Rigidbody2D>().AddForce(popVector * 8f, ForceMode2D.Impulse);
            transform.parent.GetComponent<Rigidbody2D>().AddTorque(5, ForceMode2D.Impulse);
            transform.parent.GetComponent<Rigidbody2D>().gravityScale = 2;
        }
        //transform.parent.parent = null;
        oneTime = true;
        Destroy(transform.parent.gameObject, 2);
        //gameObject.SetActive(false);
        //transform.position = new Vector2(-100, -100);
    }

    public void StartFrogLeap()
    {
        //Debug.Log("START");
        speed = frogMovement;
        anim.SetBool("Move", true);
    }
    Coroutine frogLeap;
    public void StopFrogLeap()
    {
        //Debug.Log("STOP");
        speed = 0;
        anim.SetBool("Move", false);
        //Invoke("StartFrogLeap", 0.15f);
        frogLeap = StartCoroutine(ExecuteAfterTime(0.15f));
    }
    IEnumerator ExecuteAfterTime(float time)
    {
        //Debug.Log(time + " TIMESTUFF");
        yield return new WaitForSeconds(time);
        StartFrogLeap();

    }

    public void LaunchBearCannon()
    {
        if (GetComponent<Cannon>() != null)
            GetComponent<Cannon>().Launch();
        ObstacleSpawner.obstacleSpawner.ResumeSpawner();
        AudioManager.PlaySound("porky_boing_bear");
    }

    public void BearCannonTimesUpSwing()
    {
        anim.SetTrigger("Swing");
        if (GetComponent<Cannon>() != null)
        {
            GetComponent<Cannon>().stage1 = true;
            GetComponent<Cannon>().stage2 = true;
        }
    }
}
