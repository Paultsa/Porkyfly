using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Upgrades : MonoBehaviour
{
    public GameObject[] exclamationMarks1;
    public GameObject[] exclamationMarks2;

    public Image[] shopExclamations1;
    public Image[] shopExclamations2;
    public Shop shop1;
    public Shop shop2;
    public bool somethingLoadedOrBought = false;
    public int[] shopCosts;
    public int[] shopCosts2;
    [SerializeField] int shop1Upgrades = 0;
    [SerializeField] int shop2Upgrades = 0;
    bool doOnce = false;
    /*
    [Header("Animals")]
    public GameObject frog;
    public GameObject bear;
    public GameObject eagle;
    public GameObject owl;
    public GameObject beaver;
    public GameObject squirrel;

    [Header("Obstacle scripts")]
    [SerializeField] Obstacle frogScript;
    [SerializeField] Obstacle bearScript;
    [SerializeField] Obstacle eagleScript;
    [SerializeField] Obstacle owlScript;
    [SerializeField] Obstacle beaverScript;
    [SerializeField] Obstacle squirrelScript;
    */
    [Header("Boost/Porky initializations")]
    public AirBoost airBoost;

    [Header("Other initializations")]
    public FingerSling fingerSling;
    public PlayerTest playerScript;
    public GameManager gameManager;
    public HUDScript hudScript;

    [Header("Boost/Porky values")]
    public float[] boostMultiplier;
    public float[] boostFixed;

    float aimTime;
    //public float[] aimTimeMultiplier;
    public float[] aimTimeFixed;

    public int[] extraBoosts;

    float jumpForce;
    public float[] jumpMultiplier;
    public float[] jumpFixed;

    float speed;
    public float[] maxSpeedMultiplier;
    public float[] maxSpeedFixed;

    float throwForce;
    public float[] throwMultiplier;
    //public float[] throwFixed;
    public float[] dragFixed;
    public float[] springStrength;
    public float[] forceBonusPercent;
    public float[] maxDistance;

    

    [Header("Frog values")]
    public int[] maxInstancesFrog;
    public int[] spawnIntervalsFrog;
    public int[] spawnSpreadFrog;

    [Header("Bear values")]
    public int[] maxInstancesBear;
    public int[] spawnIntervalsBear;
    public int[] spawnSpreadBear;

    [Header("Eagle values")]
    public int[] maxInstancesEagle;
    public int[] spawnIntervalsEagle;
    public int[] spawnSpreadEagle;

    [Header("Sleepy values")]
    public int[] maxInstancesSleepy;
    public int[] spawnIntervalsSleepy;
    public int[] spawnSpreadSleepy;

    [Header("Squirrel values")]
    public int[] maxInstancesSquirrel;
    public int[] spawnIntervalsSquirrel;
    public int[] spawnSpreadSquirrel;
    public int[] minCash;
    public int[] maxCash;

    [Header("Beaver values")]
    public int[] maxInstancesBeaver;
    public int[] spawnIntervalsBeaver;
    public int[] spawnSpreadBeaver;


    private void Start()
    {
        ObstacleSpawner.obstacleSpawner.SpawnObstacle(2);
        ObstacleSpawner.obstacleSpawner.SpawnObstacle(3);
        ObstacleSpawner.obstacleSpawner.SpawnObstacle(5);
        ObstacleSpawner.obstacleSpawner.SpawnObstacle(6);
        ObstacleSpawner.obstacleSpawner.SpawnObstacle(9);
        ObstacleSpawner.obstacleSpawner.SpawnObstacle(10);
        ObstacleSpawner.obstacleSpawner.SpawnObstacle(11);
        ObstacleSpawner.obstacleSpawner.SpawnObstacle(13);

        

    }
    private void Update()
    {
        if(!doOnce)
        {
            shopCosts = new int[shop1.shopItem.Length];
            shopCosts2 = new int[shop2.shopItem.Length];
            shopExclamations1 = new Image[shop1.shopItem.Length];
            shopExclamations2 = new Image[shop2.shopItem.Length];
            doOnce = true;
        }
        if(doOnce)
        {
            for (int i = 0; i < shopCosts.Length; i++)
            {
                if(shopCosts[i] != 0 && shopCosts[i] <= PlayerData.playerData.currency)
                {
                    if(shopExclamations1[i] != null)
                    {
                        shopExclamations1[i].enabled = true;
                    }
                    shop1Upgrades++;
                }
                else
                {
                    if (shopExclamations1[i] != null)
                    {
                        shopExclamations1[i].enabled = false;
                    }
                }
            }
            if(shop1Upgrades > 0)
            {
                foreach(GameObject obj in exclamationMarks1)
                {
                    obj.SetActive(true);
                }
                shop1Upgrades = 0;
            }
            else
            {
                foreach (GameObject obj in exclamationMarks1)
                {
                    obj.SetActive(false);
                }
            }

            for (int i = 0; i < shopCosts2.Length; i++)
            {
                if (shopCosts2[i] != 0 && shopCosts2[i] <= PlayerData.playerData.currency)
                {
                    if (shopExclamations2[i] != null)
                    {
                        shopExclamations2[i].enabled = true;
                    }
                    shop2Upgrades++;
                }
                else
                {
                    if (shopExclamations2[i] != null)
                    {
                        shopExclamations2[i].enabled = false;
                    }
                }
            }
            if(shop2Upgrades > 0)
            {
                foreach (GameObject obj in exclamationMarks2)
                {
                    obj.SetActive(true);
                }
                shop2Upgrades = 0;
            }
            else
            {
                foreach (GameObject obj in exclamationMarks2)
                {
                    obj.SetActive(false);
                }
            }
        }
        
        
    }

    public void BuyUpgrade(GameObject shopObject, ShopItem si, int index, int level, int shopNmbr)
    {
        if (level < (si.stars.Length - 2))
        {
            if (PlayerData.playerData.currency >= si.costs[level])
            {
                if (shopNmbr == 0)
                {
                    shopCosts[index] = si.costs[level + 1];
                }
                else if (shopNmbr == 1)
                {
                    shopCosts2[index - shopCosts.Length] = si.costs[level + 1];
                }
                Debug.Log(si.itemName + " bought");
                //playerData.PushBoughtUpgrade(index, level);
                PlayerData.playerData.PushBoughtUpgrade(index, level);
                //shopObject.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = si.sprites[level+1];
                shopObject.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = si.costs[level + 1].ToString();
                shopObject.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                shopObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => BuyUpgrade(shopObject, si, index, level + 1, shopNmbr));
                shopObject.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = si.sprites[level + 1];
                shopObject.transform.GetChild(0).GetChild(4).GetComponent<Image>().sprite = si.stars[level + 1];
                Upgrade(si, level);
                PlayerData.playerData.currency -= si.costs[level];
                PlayerData.playerData.UpdateInfo(); //Tommi
                switch (level)
                {
                    case -1: AudioManager.PlaySound("shop_unlock_charaster"); break;
                    case 0: AudioManager.PlaySound("shop_upgrade_1_star"); break;
                    case 1: AudioManager.PlaySound("shop_upgrade_2_star"); break;
                    //case 2: AudioManager.PlaySound("shop_upgrade_3_star"); break;
                }
            }
            else
            {
                Debug.Log("Not enough money!");
            }
        }
        else if (level == (si.stars.Length - 2))
        {
            if (PlayerData.playerData.currency >= si.costs[level])
            {
                if (shopNmbr == 0)
                {
                    shopCosts[index] = 0;
                }
                else if (shopNmbr == 1)
                {
                    shopCosts2[index - shopCosts.Length] = 0;
                }
                Debug.Log(si.itemName + " bought");
                PlayerData.playerData.PushBoughtUpgrade(index, level);
                shopObject.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                shopObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => BuyUpgrade(shopObject, si, index, level + 1, shopNmbr));
                PlayerData.playerData.currency -= si.costs[level];
                shopObject.transform.GetChild(0).GetComponent<Image>().color = Color.grey;
                shopObject.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = si.sprites[level + 1];
                shopObject.transform.GetChild(0).GetChild(4).GetComponent<Image>().sprite = si.stars[level + 1];
                shopObject.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = "MAXED";
                Upgrade(si, level);
                for (int k = 0; k < shopObject.transform.GetChild(0).childCount-1; k++)
                {
                    Image img = shopObject.transform.GetChild(0).GetChild(k).GetComponent<Image>();
                    if (img == null)
                    {
                        continue;
                    }
                    img.color = Color.grey;
                }
                Debug.Log("Maxed");
                PlayerData.playerData.UpdateInfo(); //Tommi
                AudioManager.PlaySound("shop_upgrade_3_star");
            }
            else
            {
                Debug.Log("Not enough money!");
                AudioManager.PlaySound("shop_buy_no_money");
            }

        }
        else if (level > (si.stars.Length - 2))
        {
            Debug.Log("Already max level");
            AudioManager.PlaySound("shop_buy_no_money");
        }


    }

    public void LoadUpgrades(GameObject shopObject, ShopItem si, int index, int level, int shopNmbr)
    {
        
        if (level < (si.stars.Length - 2))
        {
            if (shopNmbr == 0)
            {
                shopCosts[index] = si.costs[level + 1];
            }
            else if (shopNmbr == 1)
            {
                shopCosts2[index-shopCosts.Length] = si.costs[level + 1];
            }
            //Debug.Log(si.itemName + " bought from load with level of " + level);
            //shopObject.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = si.sprites[level+1];
            shopObject.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = si.sprites[level + 1];
            shopObject.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = si.costs[level + 1].ToString();
            shopObject.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            shopObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => BuyUpgrade(shopObject, si, index, level + 1, shopNmbr));
            shopObject.transform.GetChild(0).GetChild(4).GetComponent<Image>().sprite = si.stars[level + 1];
            Upgrade(si, level);
        }
        else if (level == (si.stars.Length - 2))
        {
            if (shopNmbr == 0)
            {
                shopCosts[index] = 0;
            }
            else if (shopNmbr == 1)
            {
                shopCosts2[index - shopCosts.Length] = 0;
            }
            //Debug.Log(si.itemName + " bought from load with level of " + level + " (else if)");
            //PlayerData.playerData.PushBoughtUpgrade(index, level);
            shopObject.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            shopObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => BuyUpgrade(shopObject, si, index, level + 1, shopNmbr));
            //PlayerData.playerData.currency -= si.costs[level];
            shopObject.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = si.sprites[level + 1];
            shopObject.transform.GetChild(0).GetComponent<Image>().color = Color.grey;
            shopObject.transform.GetChild(0).GetChild(4).GetComponent<Image>().sprite = si.stars[level + 1];
            shopObject.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = "MAXED";
            Upgrade(si, level);

            for (int k = 0; k < shopObject.transform.GetChild(0).childCount-1; k++)
            {
                Image img = shopObject.transform.GetChild(0).GetChild(k).GetComponent<Image>();
                if (img == null)
                {
                    continue;
                }
                img.color = Color.grey;
            }
            //Debug.Log("Maxed");
        }
        else if (level > (si.stars.Length - 2))
        {
            //Debug.Log("Already max level");
        }
    }

    public void Upgrade(ShopItem si, int level)
    {
        somethingLoadedOrBought = true;
        switch (si.itemName)
        {
            case "BoostAmount":
                Debug.Log(si.itemName);
                airBoost.boostAmount += extraBoosts[level];
                airBoost.maxBoostAmount += extraBoosts[level];
                break;

            case "BoostSpeed":
                Debug.Log(si.itemName);
                airBoost.boostMultiplier += boostMultiplier[level];
                airBoost.boostFixedStep += boostFixed[level];
                break;

            case "BoostTime":
                Debug.Log(si.itemName);
                airBoost.aimTime += aimTimeFixed[level];

                break;

            case "Jump":
                Debug.Log("upgrade");
                Debug.Log(si.itemName);
                playerScript.groundSlowDownUpward += jumpFixed[level];
                playerScript.groundSlowDownForward += jumpFixed[level];

                break;

            case "Speed":
                Debug.Log(si.itemName);
                gameManager.maxSpeed += (int)maxSpeedFixed[level];
                gameManager.actualMaxSpeed = (int)(1.5f * (float)gameManager.maxSpeed);
                hudScript.UpdateSpeed();
                
                break;

            case "Throw":
                Debug.Log(si.itemName);
                fingerSling.drag += dragFixed[level];
                fingerSling.springStrength += springStrength[level];
                fingerSling.throwForceMultiplier += throwMultiplier[level];
                fingerSling.maxDistance += maxDistance[level];
                fingerSling.forceBonusPercentBoost += forceBonusPercent[level];

                break;

            case "Bear":
                Debug.Log(si.itemName);
                if (level == 0) ObstacleSpawner.obstacleSpawner.SpawnObstacle(4);
                else
                {
                    ObstacleSpawner.obstacleSpawner.maxInstancesPerObject[4] += maxInstancesBear[level];
                    ObstacleSpawner.obstacleSpawner.spawnInterval[4] -= spawnIntervalsBear[level];
                    ObstacleSpawner.obstacleSpawner.spawnIntervalSpread[4] -= spawnSpreadBear[level];
                }

                break;

            case "Frog":
                Debug.Log(si.itemName);
                if (level == 0)
                {
                    ObstacleSpawner.obstacleSpawner.SpawnObstacle(0);
                    ObstacleSpawner.obstacleSpawner.SpawnObstacle(7);
                }
                else
                {
                    ObstacleSpawner.obstacleSpawner.maxInstancesPerObject[0] += maxInstancesFrog[level];
                    ObstacleSpawner.obstacleSpawner.spawnInterval[0] -= spawnIntervalsFrog[level];
                    ObstacleSpawner.obstacleSpawner.spawnIntervalSpread[0] -= spawnSpreadFrog[level];
                    ObstacleSpawner.obstacleSpawner.maxInstancesPerObject[7] += maxInstancesFrog[level];
                    ObstacleSpawner.obstacleSpawner.spawnInterval[7] -= spawnIntervalsFrog[level];
                    ObstacleSpawner.obstacleSpawner.spawnIntervalSpread[7] -= spawnSpreadFrog[level];
                }

                break;

            case "Eagle":
                Debug.Log(si.itemName);
                if (level == 0) ObstacleSpawner.obstacleSpawner.SpawnObstacle(1);
                else
                {
                    ObstacleSpawner.obstacleSpawner.maxInstancesPerObject[1] += maxInstancesEagle[level];
                    ObstacleSpawner.obstacleSpawner.spawnInterval[1] -= spawnIntervalsEagle[level];
                    ObstacleSpawner.obstacleSpawner.spawnIntervalSpread[1] -= spawnSpreadEagle[level];
                }

                break;

            case "Squirrel":
                Debug.Log(si.itemName);
                ObstacleSpawner.obstacleSpawner.maxInstancesPerObject[9] += maxInstancesSquirrel[level];
                ObstacleSpawner.obstacleSpawner.spawnInterval[9] -= spawnIntervalsSquirrel[level];
                ObstacleSpawner.obstacleSpawner.maxInstancesPerObject[13] += maxInstancesSquirrel[level];
                ObstacleSpawner.obstacleSpawner.spawnInterval[13] -= spawnIntervalsSquirrel[level];


                break;

            case "Sleepy":
                Debug.Log(si.itemName);
                if (level == 0) ObstacleSpawner.obstacleSpawner.SpawnObstacle(12);
                else
                {
                    ObstacleSpawner.obstacleSpawner.maxInstancesPerObject[12] += maxInstancesSleepy[level];
                    ObstacleSpawner.obstacleSpawner.spawnInterval[12] -= spawnIntervalsSleepy[level];
                    ObstacleSpawner.obstacleSpawner.spawnIntervalSpread[12] -= spawnSpreadSleepy[level];
                }

                break;

            case "Beaver":
                Debug.Log(si.itemName);
                if (level == 0) ObstacleSpawner.obstacleSpawner.SpawnObstacle(8);
                else
                {
                    ObstacleSpawner.obstacleSpawner.maxInstancesPerObject[8] += maxInstancesBeaver[level];
                    ObstacleSpawner.obstacleSpawner.spawnInterval[8] -= spawnIntervalsBeaver[level];
                    ObstacleSpawner.obstacleSpawner.spawnIntervalSpread[8] -= spawnSpreadBeaver[level];
                }
                break;
        }
    }
}
