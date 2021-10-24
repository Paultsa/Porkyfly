using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public GameObject boostParent;
    public GameObject currencyObj;
    public GameObject optionsButton;
    public HUDScript hudScript;
    public int thisRunCurrency = 0;
    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI earnedCurrencyText;
    public GameObject pauseButton;
    bool doneOnce = false;
    bool secondOnce = false;
    //fingerSling = 1, slingShot = 0
    public int maxHeight;
    public int maxSpeed;
    public int actualMaxSpeed;
    public static float distance;
    public GameObject player;
    private Rigidbody2D playerRB;
    public BackgroundManager bgManager;
    public AnimationManager animManager;

    public float timeBetweenSlow;
    float timer = 0;
    public float maxSpeedSlowDownAmount;

    public GameObject speedSlider;

    int launchStyle;
    public GameObject mainCam;
    public GameObject arrow;
    FingerSling fingerSling;
    SlingShot slingShot;
    public GameObject mainMenu;
    public GameObject startText1;
    public GameObject startText2;

    public Shop shop;
    
    bool grabbed = false;
    //public GameObject shopOverlay;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = player.GetComponentInParent<Rigidbody2D>();
        fingerSling = mainCam.GetComponent<FingerSling>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if(shop.populated)
        {
            if (!GetComponent<Upgrades>().somethingLoadedOrBought && !secondOnce)
            {
                startText1.SetActive(true);
                secondOnce = true;
            }

            if (GetComponent<Upgrades>().somethingLoadedOrBought)
            {
                startText1.SetActive(false);
                startText2.SetActive(false);
            }
        }
        

        currencyText.text = ": " + PlayerData.playerData.currency;

        if (fingerSling.grabbed && !hudScript.openingGameOverMenu) { 
            mainMenu.SetActive(false);
            optionsButton.SetActive(false);
            Destroy(arrow); 
            pauseButton.SetActive(true);
            currencyObj.SetActive(false);
            boostParent.SetActive(true);

            if (!GetComponent<Upgrades>().somethingLoadedOrBought)
            {
                startText1.SetActive(false);
                startText2.SetActive(true);
            }

        }
        if(hudScript.openingGameOverMenu)
        {
            optionsButton.SetActive(true);
            speedSlider.SetActive(false);
            boostParent.SetActive(false);
            //currencyObj.SetActive(true);
        }
        else if(!fingerSling.enabled)
        {
            speedSlider.SetActive(true);

            if (!GetComponent<Upgrades>().somethingLoadedOrBought)
            {
                startText2.SetActive(false);
            }
        }
   
        //Debug.Log("FPS: " + 1 / Time.unscaledDeltaTime);
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            ReloadScene();
        }
        if(player.transform.position.y >= maxHeight+player.GetComponent<SpriteRenderer>().size.y && !doneOnce)
        {
            animManager.DoLightningAnimation();
            doneOnce = true;
        }
        else if(player.transform.position.y < maxHeight && doneOnce)
        {
            doneOnce = false;
        }

        if (Mathf.Abs(bgManager.xSpeed) > actualMaxSpeed/2)
        {
            bgManager.xSpeed = -actualMaxSpeed/2;
        }
        else if (Mathf.Abs(bgManager.xSpeed) > maxSpeed/2)
        {
            timer += Time.deltaTime;
            if(timer >= timeBetweenSlow)
            {
                bgManager.xSpeed += maxSpeedSlowDownAmount;
                timer = 0;
            }
        }

        if(Mathf.Abs(playerRB.velocity.y) > actualMaxSpeed/2)
        {
            if(playerRB.velocity.y > 0)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, actualMaxSpeed / 2);
            }
            else if(playerRB.velocity.y < 0)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, -(actualMaxSpeed / 2));
            }
            
        }
    }

    public void ReloadScene()
    {
        PlayerData.playerData.shopsLenght = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static float getDistance()
    {
        return distance;
    }

    public void IncreaseCurrency(int amount)
    {
        thisRunCurrency += amount;
        PlayerData.playerData.currency += amount;
        earnedCurrencyText.text = thisRunCurrency.ToString();
    }
    
}
