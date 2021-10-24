using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public bool populated = false;
    bool doneOnce = false;
    public int shopNumber;
    public ShopItem[] shopItem;
    public int upgradeLevel = 0;
    public Upgrades upgrades;
    [SerializeField] private Transform shopContainer;
    [SerializeField] private GameObject shopItemPrefab;
    //[SerializeField] private PlayerData playerData;
    [SerializeField] int indexForFirstUpgrade;

    private void Awake()
    {
        
    }
    private void Start()
    {
        PlayerData.playerData.shopsLenght += shopItem.Length;
    }
    private void Update()
    {
        if(PlayerData.playerData.doneOnce && !doneOnce)
        {
            PopulateShop();
            doneOnce = true;
            if(shopNumber != 0)
            {
                gameObject.SetActive(false);
            }
        }
        
    }


    private void PopulateShop()
    {
        for(int i = 0; i < shopItem.Length; i++)
        {
            if(shopNumber == 0)
            {
                upgrades.shopCosts[i] = shopItem[i].costs[0];
            }
            else if(shopNumber == 1)
            {
                upgrades.shopCosts2[i] = shopItem[i].costs[0];
            }
            
            ShopItem si = shopItem[i];
            GameObject itemObject = Instantiate(shopItemPrefab, shopContainer);
            bool bought = false;

            //Go through the list of bought items and compare, if current item is already bought
            indexForFirstUpgrade = (PlayerData.playerData.boughtUpgrades.Length - shopItem.Length) * shopNumber;
            int tmp = i + indexForFirstUpgrade;
            for (int j = 0; j< PlayerData.playerData.boughtUpgrades.Length; j++)
            {
                
                if(PlayerData.playerData.boughtUpgrades[j] == tmp)
                {
                    upgradeLevel = PlayerData.playerData.boughtUpgradeLevels[j];
                    bought = true;
                    break;
                }
            }
            if(bought)
            {
                for(int j = 0; j <= upgradeLevel; j++)
                {
                    upgrades.LoadUpgrades(itemObject, si, tmp, j, shopNumber);
                }

            }
            else
            {
                itemObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => upgrades.BuyUpgrade(itemObject, si, tmp, 0, shopNumber));
                //Debug.Log("Create " + si.name + " with index of " + i);
                itemObject.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = si.sprites[0];
                itemObject.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = si.costs[0].ToString();
                itemObject.transform.GetChild(0).GetChild(4).GetComponent<Image>().sprite = si.stars[0];

                
            }
            if (shopNumber == 0)
            {
                upgrades.shopExclamations1[i] = itemObject.transform.GetChild(0).GetChild(5).GetComponent<Image>();
            }
            else if (shopNumber == 1)
            {
                upgrades.shopExclamations2[i] = itemObject.transform.GetChild(0).GetChild(5).GetComponent<Image>();
            }

            //itemObject.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = si.itemNames[0];
            populated = true;
        }
    }
}
