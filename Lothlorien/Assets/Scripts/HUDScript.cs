using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HUDScript : MonoBehaviour
{
    public int boostsAvailable = 0;
    public AirBoost airBoost;
    bool boostUsed = false;
    public Image[] boostImages;
    public GameObject boostParent;
    public GameObject currencyImage;
    public bool openingGameOverMenu = false;
    [SerializeField] private Transform playerTransform;
    private Vector2 origPosition;

    private TextMeshProUGUI heightText;
    private TextMeshProUGUI lenghtText;
    [SerializeField] private TextMeshProUGUI gameOverDistanceText;
    private TextMeshProUGUI speedText;
    public BackgroundManager bgManager;
    public AnimationManager animManager;
    public GameManager gameManager;
    float speed = 0;
    public float speedSliderFillTime;
    int height = 0;
    int printDistance;
    int printSpeed;
    bool moving = false;

    [SerializeField] private RectTransform sliderBackgroundTransform;
    [SerializeField] private RectTransform fillAreaTransform;
    private Vector3 origFillArea;
    [Tooltip("Never let speed  get to this point")]

    [SerializeField] Sprite backgroundOrigSprite;
    [SerializeField] Sprite backgroundOverFlowSprite;
    [SerializeField] Image backgroundImage;
    [SerializeField] Image lid;
    [SerializeField] Slider speedMeter;

    [SerializeField] RectTransform gameOverMenu;
    [SerializeField] GameObject optionsButton;
    [SerializeField] GameObject pauseButton;
    private float overflowAmount;
    void Start()
    {
        origPosition = playerTransform.position;
        lenghtText = transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        heightText = transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        //speedText = transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        //speedMeter = speedText.transform.GetChild(0).GetComponent<Slider>();
        speedMeter.maxValue = gameManager.actualMaxSpeed/2;
        //overflowAmount = gameManager.actualMaxSpeed / gameManager.maxSpeed * (int)sliderBackgroundTransform.rect.width - (int)sliderBackgroundTransform.rect.width;
        overflowAmount = ((float)gameManager.actualMaxSpeed/2) / ((float)gameManager.maxSpeed / 2);
        float temp = sliderBackgroundTransform.rect.width * overflowAmount;
        //Debug.Log("Overflow Amount: " + overflowAmount);
        // Debug.Log("Background width: " + fillAreaTransform.localScale);
        origFillArea = fillAreaTransform.localScale;
        fillAreaTransform.localScale = new Vector2(fillAreaTransform.localScale.x*overflowAmount, fillAreaTransform.localScale.y);

        for(int i = 0; i < boostParent.transform.childCount; i++)
        {
            boostImages[i] = boostParent.transform.GetChild(i).transform.GetComponent<Image>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (airBoost.maxBoostAmount)
        {
            case 5:
                boostImages[0].gameObject.SetActive(true);
                boostImages[1].gameObject.SetActive(true);
                boostImages[2].gameObject.SetActive(true);
                boostImages[3].gameObject.SetActive(true);
                boostImages[4].gameObject.SetActive(true);
                break;
            case 4:
                boostImages[0].gameObject.SetActive(false);
                boostImages[1].gameObject.SetActive(true);
                boostImages[2].gameObject.SetActive(true);
                boostImages[3].gameObject.SetActive(true);
                boostImages[4].gameObject.SetActive(true);
                break;
            case 3:
                boostImages[0].gameObject.SetActive(false);
                boostImages[1].gameObject.SetActive(false);
                boostImages[2].gameObject.SetActive(true);
                boostImages[3].gameObject.SetActive(true);
                boostImages[4].gameObject.SetActive(true);
                break;
            case 2:
                boostImages[0].gameObject.SetActive(false);
                boostImages[1].gameObject.SetActive(false);
                boostImages[2].gameObject.SetActive(false);
                boostImages[3].gameObject.SetActive(true);
                boostImages[4].gameObject.SetActive(true);
                break;
            case 1:
                boostImages[0].gameObject.SetActive(false);
                boostImages[1].gameObject.SetActive(false);
                boostImages[2].gameObject.SetActive(false);
                boostImages[3].gameObject.SetActive(false);
                boostImages[4].gameObject.SetActive(true);
                break;
            case 0:
                boostImages[0].gameObject.SetActive(false);
                boostImages[1].gameObject.SetActive(false);
                boostImages[2].gameObject.SetActive(false);
                boostImages[3].gameObject.SetActive(false);
                boostImages[4].gameObject.SetActive(false);
                break;
        }

        switch (airBoost.boostAmount)
        {
            case 5:
                boostImages[0].color = Color.white;
                boostImages[1].color = Color.white;
                boostImages[2].color = Color.white;
                boostImages[3].color = Color.white;
                boostImages[4].color = Color.white;
                break;
            case 4:
                boostImages[0].color = new Color(150f / 255f, 150f / 255f, 150f / 255f, 100f / 255f);
                boostImages[1].color = Color.white;
                boostImages[2].color = Color.white;
                boostImages[3].color = Color.white;
                boostImages[4].color = Color.white;
                break;
            case 3:
                boostImages[1].color = new Color(150f/255f,150f/255f,150f/255f, 100f/255f);
                boostImages[2].color = Color.white;
                boostImages[3].color = Color.white;
                boostImages[4].color = Color.white;
                break;
            case 2:
                boostImages[2].color = new Color(150f / 255f, 150f / 255f, 150f / 255f, 100f / 255f);
                boostImages[3].color = Color.white;
                boostImages[4].color = Color.white;
                break;
            case 1:
                boostImages[3].color = new Color(150f / 255f, 150f / 255f, 150f / 255f, 100f / 255f);
                boostImages[4].color = Color.white;
                break;
            case 0:
                boostImages[4].color = new Color(150f / 255f, 150f / 255f, 150f / 255f, 100f / 255f);
                break;
        }
        


        height = (int)(playerTransform.position.y - origPosition.y);
        if(bgManager.xSpeed == 0 && !moving)
        {
            GameManager.distance = (playerTransform.position.x - origPosition.x);
        }
        else
        {
            moving = true;
            GameManager.distance += (-bgManager.xSpeed * Time.deltaTime);
        }
        printDistance = (int)GameManager.distance;
        printSpeed = (int)speed;

        if(bgManager.moving || bgManager.movingY)
        {
            lenghtText.text = ": " + printDistance.ToString();
            gameOverDistanceText.text = printDistance.ToString();
            //heightText.text = height.ToString();
            //speedText.text = "Speed: " + printSpeed.ToString();
            
            if(!animManager.isHitByLightning)
            {
                speedMeter.value = Mathf.Lerp(speedMeter.value, speed, Time.deltaTime * speedSliderFillTime);
                if (speedMeter.value > gameManager.maxSpeed / 2)
                {
                    //speedText.color = Color.red;
                    backgroundImage.sprite = backgroundOverFlowSprite;
                    lid.gameObject.SetActive(true);
                    fillAreaTransform.GetChild(0).GetComponent<Image>().color = new Color(255f / 255f, 0, 0, 255f / 255f);//145f/255f);
                }
                else
                {
                    //speedText.color = Color.green;
                    backgroundImage.sprite = backgroundOrigSprite;
                    lid.gameObject.SetActive(false);
                    fillAreaTransform.GetChild(0).GetComponent<Image>().color = new Color(0, 255f / 255f, 0, 255f / 255f);// 145f/255f);
                }
            }
            
        }
        if(bgManager.outOfBounds)
        {
            speed = -bgManager.xSpeed;//Mathf.Lerp(speed,Mathf.Sqrt(Mathf.Pow(bgManager.xSpeed, 2) + Mathf.Pow(playerTransform.gameObject.GetComponent<Rigidbody2D>().velocity.y, 2)),Time.deltaTime*10);
        }
        else
        {
            speed = playerTransform.gameObject.GetComponent<Rigidbody2D>().velocity.x;//Mathf.Lerp(speed, Mathf.Sqrt(Mathf.Pow(playerTransform.gameObject.GetComponent<Rigidbody2D>().velocity.x, 2) + Mathf.Pow(playerTransform.gameObject.GetComponent<Rigidbody2D>().velocity.y, 2)),Time.deltaTime*10);
        }

        if(openingGameOverMenu)
        {
            gameOverMenu.offsetMax = Vector2.Lerp(gameOverMenu.offsetMax, new Vector2(gameOverMenu.offsetMax.x, 0f), Time.unscaledDeltaTime * 5);
            gameOverMenu.offsetMin = Vector2.Lerp(gameOverMenu.offsetMin, new Vector2(gameOverMenu.offsetMin.x, 0f), Time.unscaledDeltaTime * 5);
        }

    }
    public void OpenGameOverMenu()
    {
        openingGameOverMenu = true;
        currencyImage.SetActive(true);
        pauseButton.SetActive(false);
        optionsButton.SetActive(true);
        lenghtText.gameObject.SetActive(false);
        AudioManager.PlaySound("music_stop");
        AudioManager.PlaySound("gameover_menu_open");
        Camera.main.GetComponent<TimeWarp>(). ToggolePause();
    }

    public void ReturnToMainMenu()
    {
        Camera.main.GetComponent<TimeWarp>().ToggolePause();
    }

    public void UpdateSpeed()
    {
        speedMeter.maxValue = gameManager.actualMaxSpeed / 2;
        overflowAmount = ((float)gameManager.actualMaxSpeed / 2) / ((float)gameManager.maxSpeed / 2);
        float temp = sliderBackgroundTransform.rect.width * overflowAmount;
        fillAreaTransform.localScale = new Vector2(origFillArea.x * overflowAmount, origFillArea.y);
    }
}
