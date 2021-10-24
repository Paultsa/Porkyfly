using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public float timeBetweenLightFlash;
    public int flashAmount;
    public bool isHitByLightning = false;
    public float divideSpeedWhileStopping;
    public float minimumSpeedWhileStopping;
    public GameObject player;
    private Rigidbody2D playerRB;
    private PlayerTest playerScript;
    private SpriteRenderer playerSpriteRenderer;
    private BackgroundManager backgroundManager;
    private GameManager gameManager;
    public HUDScript hudScript;

    
    [SerializeField] private Sprite[] boostedSprites;
    [SerializeField] private Sprite[] spinningSprites;
    [SerializeField] private Sprite[] flyingSprites;
    [SerializeField] private Sprite[] lightningSprites;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite slidingSprite;
    [SerializeField] private Sprite stoppedSprite;
    [SerializeField] private Sprite lightningSprite;
    [SerializeField] private Sprite hurtSprite;
    [SerializeField] private Sprite bearBoostSprite;
    private bool stopping = false;
    public bool justRotated = false;
    public bool wasBoosted = false;
    public bool isBoosted = false;
    public bool wasHurt = false;
    public float boostedTime;
    float timer = 0;
    float rotateTimer = 0;
    public float rotatedTime = 0.1f;
    float hurtTimer = 0;
    public float hurtTime = 0.1f;

    float boostsUsed = 0;


    private void Start()
    {
        backgroundManager = GetComponent<BackgroundManager>();
        gameManager = GetComponent<GameManager>();
        playerRB = player.GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<PlayerTest>();
        playerSpriteRenderer = player.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(playerScript.failStop && !stopping)
        {
            stopping = true;
            StartCoroutine(failThrowStopping());
        }
        else if(playerScript.stop && !stopping)
        {
            stopping = true;
            StartCoroutine(isStopping());
        }
        if(stopping)
        {
            playerScript.playerMaterial.bounciness = 0;
            player.transform.position = new Vector2(player.transform.position.x, 1.5f);
        }

        if (isBoosted)
        {
            if (timer < boostedTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                ChangeFlyingSprite();
            }
        }
        else
        {
            timer = 0;
            boostsUsed = 0;
        }

        if (boostsUsed > 1)
        {
            boostsUsed--;
            timer = 0;
        }

        if (justRotated)
        {
            if (rotateTimer < rotatedTime)
            {
                rotateTimer += Time.deltaTime;
            }
            else
            {
                justRotated = false;
            }
        }
        else rotateTimer = 0;

        if(wasHurt)
        {
            if(hurtTimer < hurtTime)
            {
                hurtTimer += Time.deltaTime;
            }
            else
            {
                hurtTimer = 0;
                wasHurt = false;
            }
        }

        

    }

    public void ChangeDefaultSprite()
    {
        wasHurt = false;
        wasBoosted = false;
        isBoosted = false;
        playerSpriteRenderer.sprite = defaultSprite;
    }

    public void ChangeSpinningSprite()
    {
        justRotated = true;
        wasBoosted = false;
        isBoosted = false;
        if(!wasHurt)
        {
            playerSpriteRenderer.sprite = getRandomSprite(spinningSprites);
        }
    }

    public void ChangeFlyingSprite()
    {
        if(!justRotated)
        {
            wasHurt = false;
            wasBoosted = false;
            isBoosted = false;
            playerSpriteRenderer.sprite = getRandomSprite(flyingSprites);
        }
    }

    public void ChangeBoostSprite()
    {
        wasHurt = false;
        isBoosted = true;
        wasBoosted = true;
        boostsUsed++;
        playerSpriteRenderer.sprite = getRandomSprite(boostedSprites);
    }

    public void ChangeBearBoostSprite()
    {
        wasHurt = false;
        isBoosted = false;
        wasBoosted = false;
        playerSpriteRenderer.sprite = bearBoostSprite;
    }

    public void ChangeHurtSprite()
    {
        wasHurt = true;
        wasBoosted = false;
        isBoosted = false;
        playerSpriteRenderer.sprite = hurtSprite;
    }
    public void UseLightningSprite()
    {
        wasHurt = false;
        isBoosted = false;
        wasBoosted = false;
        playerSpriteRenderer.sprite = lightningSprite;

    }

    public void UseSlidingSprite()
    {
        wasHurt = false;
        wasBoosted = false;
        isBoosted = false;
        playerSpriteRenderer.sprite = slidingSprite;
    }
    public void UseStoppedSprite()
    {
        wasHurt = false;
        wasBoosted = false;
        isBoosted = false;
        playerSpriteRenderer.sprite = stoppedSprite;
    }

    public void DoLightningAnimation()
    {
        wasHurt = false;
        wasBoosted = false;
        isBoosted = false;
        StartCoroutine(hitByLightning());
    }



    IEnumerator isStopping()
    {
        int money = (int)(GameManager.distance / 15);

        if (GameManager.distance <= 100)
        {
            money = (int)(GameManager.distance / 15);
        }
        else if (GameManager.distance <= 200)
        {
            money = (int)(GameManager.distance / 12);
        }
        else if (GameManager.distance <= 500)
        {
            money = (int)(GameManager.distance / 13);
        }
        else if (GameManager.distance <= 1000)
        {
            money = (int)(GameManager.distance / 14);
        }
        else
        {
            money = (int)(GameManager.distance / 15);
        }
        
        gameManager.IncreaseCurrency(money);
        player.transform.GetChild(3).gameObject.SetActive(true);
        Camera.main.GetComponent<AirBoost>().isAvailable = false;
        Debug.Log("isStopping");
        playerRB.velocity = Vector2.zero;
        UseSlidingSprite();
        player.transform.right = Vector2.right;
        while (backgroundManager.xSpeed < -minimumSpeedWhileStopping * Time.unscaledDeltaTime)
        {
            backgroundManager.xSpeed = backgroundManager.xSpeed / divideSpeedWhileStopping;
            yield return new WaitForSeconds(0.1f);
        }
        backgroundManager.xSpeed = 0;
        UseStoppedSprite();
        //gameManager.IncreaseCurrency(60); //DEBUG -Tommi
        PlayerData.playerData.UpdateInfo(); //Distance and stuff -Tommi
        hudScript.OpenGameOverMenu();
    }

    IEnumerator failThrowStopping()
    {
        Camera.main.GetComponent<AirBoost>().isAvailable = false;
        Debug.Log("Fail throw");
        UseSlidingSprite();
        player.transform.right = Vector2.right;
        playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
        player.GetComponent<PlayerTest>().torque = 0;
        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        while (playerRB.velocity.x > minimumSpeedWhileStopping * Time.unscaledDeltaTime)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x / divideSpeedWhileStopping, 0);
            yield return new WaitForSeconds(0.1f);
        }
        playerRB.velocity = Vector2.zero;
        UseStoppedSprite();
        yield return new WaitForSeconds(2f);
        gameManager.ReloadScene();
    }

    IEnumerator hitByLightning()
    {
        Camera.main.GetComponent<AirBoost>().isAvailable = false;
        player.GetComponent<OrbitalThunderBoost>().SaveCurrentSpeedVector();
        isHitByLightning = true;
        float origGravityScale = playerRB.gravityScale;
        playerRB.velocity = Vector2.zero;
        playerRB.gravityScale = 0;
        backgroundManager.xSpeed = 0;
        for(int i = 0; i < flashAmount; i++)
        {
            playerSpriteRenderer.sprite = lightningSprites[0];
            yield return new WaitForSeconds(timeBetweenLightFlash/2);
            playerSpriteRenderer.sprite = lightningSprites[1];
            yield return new WaitForSeconds(timeBetweenLightFlash/2);
        }
        playerRB.gravityScale = origGravityScale;
        isHitByLightning = false;
        player.GetComponent<OrbitalThunderBoost>().OrbitalBoost();
        UseLightningSprite();
        AudioManager.PlaySound("porky_thunder");

    }

    private Sprite getRandomSprite(Sprite[] newSprites)
    {
        int rand = 0;
        if(newSprites.Length > 1)
        {
            rand = Random.Range(0, newSprites.Length);

            while (playerSpriteRenderer.sprite == newSprites[rand])
            {
                rand = Random.Range(0, newSprites.Length);
            }
        }
        else if (newSprites.Length == 0)
        {
            return playerSpriteRenderer.sprite;
        }
        return newSprites[rand];
    }
}
