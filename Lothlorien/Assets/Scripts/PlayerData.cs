using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    //public GameObject[] shopObjects;
    public static PlayerData playerData;
    //public Shop[] shops;
    public int currency = 0;
    public float bestDistance;
    public int[] boughtUpgrades;
    public int[] boughtUpgradeLevels;
    public bool doneOnce = false;

    //public Shop[] shops;
    //public static int currency = DataManager.status.currency;
    //public static float bestDistance = DataManager.status.bestDistance;
    //public static int[] boughtUpgrades = DataManager.status.boughtUpgrades;

    public int shopsLenght = 0;

    void Awake()
    {
        if (playerData == null)
        {
            DontDestroyOnLoad(gameObject);
            playerData = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        doneOnce = false;
        /*
        //Find all the shops in scene
        shopObjects = GameObject.FindGameObjectsWithTag("Shop");
        shops = new Shop[shopObjects.Length];
        for(int i = 0; i < shopObjects.Length; i++)
        {
            shops[i] = shopObjects[i].GetComponent<Shop>();
        }

        //Gets the lenght of all shopItems in all shops
        for (int i = 0; i < shops.Length; i++)
        {
            shopsLenght += shops[i].shopItem.Length;
        }
        */

        //Makes a list full of -1
        

        DataManager.status.Load();

        //Load Currence
        if (DataManager.status.currency != 0)
        {
            currency = DataManager.status.currency;
        }

        //Load BestDistance
        if (DataManager.status.bestDistance != 0)
        {
            bestDistance = DataManager.status.bestDistance;
        }
    }
    private void Update()
    {
        if(shopsLenght != 0 && !doneOnce)
        {
            boughtUpgrades = new int[shopsLenght];
            boughtUpgradeLevels = new int[shopsLenght];
            for (int i = 0; i < boughtUpgrades.Length; i++)
            {
                boughtUpgrades[i] = -1;
                boughtUpgradeLevels[i] = -1;
            }
            if(DataManager.status.boughtUpgrades.Length != 0)
            {
                //Load Upgrades
                for (int i = 0; i < boughtUpgrades.Length; i++)
                {
                    /*
                    Debug.Log("DataManager Lenght: " + DataManager.status.boughtUpgrades.Length);
                    Debug.Log("Local Lenght: " + boughtUpgrades.Length);
                    Debug.Log("DataManager Levels Lenght: " + DataManager.status.boughtUpgradeLevels.Length);
                    Debug.Log("Local Levels Lenght: " + boughtUpgradeLevels.Length);
                    */
                    if (DataManager.status.boughtUpgrades[i] != -1)
                    {
                        boughtUpgrades[i] = DataManager.status.boughtUpgrades[i];
                        boughtUpgradeLevels[i] = DataManager.status.boughtUpgradeLevels[i];
                    }
                }
            }
            doneOnce = true;
        }
        
    }

    public void PushBoughtUpgrade(int index, int level)
    {
        for (int i = 0; i < boughtUpgrades.Length; i++)
        {
            if(boughtUpgrades[i] == index)
            {
                //Debug.Log("Upgrading an already bought item with index " + index);
                boughtUpgradeLevels[i] = level;
                return;
            }
        }
        for (int i = 0; i < boughtUpgrades.Length; i++)
        {
            if (boughtUpgrades[i] == -1)
            {
                //Debug.Log("Pushing bought upgrade with index of " + i);
                boughtUpgradeLevels[i] = level;
                boughtUpgrades[i] = index;
                break;
            }
        }
    }

    public void LoadBoughtUpgrades()
    {
        //Load bought upgrades from cloud and fill local boughtUpgrades from the beginning
        for (int i = 0; i < boughtUpgrades.Length; i++)
        {
            //boughtUpgradeLevels[i] = file.BoughtUpgradeLevels[i];
            //boughtUpgrades[i] = file.BoughtUpgrades[i];
        }
    }

    public void UpdateInfo()
    {
        if (GameManager.getDistance() > playerData.bestDistance)
        {
            bestDistance = GameManager.getDistance();
            
        }

        currency = PlayerData.playerData.currency;
        boughtUpgrades=PlayerData.playerData.boughtUpgrades;
        boughtUpgradeLevels = PlayerData.playerData.boughtUpgradeLevels;
        Debug.Log("Save");
        DataManager.status.Save();
    }
}
