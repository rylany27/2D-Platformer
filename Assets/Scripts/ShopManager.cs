using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public int[,] shopItems = new int[5, 5];
    public float coins;
    public Text coinsText;
    void Start()
    {
        coinsText.text = "Coins: " + coins.ToString();

        shopItems[1,1] = 1;
        shopItems[1,2] = 2;
        shopItems[1,3] = 3;
        shopItems[1,4] = 4;

        shopItems[2,1] = 10;
        shopItems[2,2] = 10;
        shopItems[2,3] = 10;
        shopItems[2,4] = 10;

        shopItems[3,1] = 0;
        shopItems[3,2] = 0;
        shopItems[3,3] = 0;
        shopItems[3,4] = 0;
    }

    public void Buy()
    {
        GameObject buttonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ButtonInfo item = buttonRef.GetComponent<ButtonInfo>();
        float price = shopItems[2, item.itemID];
        if (coins >= price)
        {
            coins -= price;
            shopItems[3, item.itemID] += 1;
            coinsText.text = "Coins: " + coins.ToString();
            item.quantityText.text = shopItems[3, item.itemID].ToString();
        }
    }
}
