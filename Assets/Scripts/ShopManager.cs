using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class ShopManager : MonoBehaviourPun
{
    public GameObject shopPanel;
    public Text currencyText;

    private PlayerResistance pr;
    private int currency;


    public Button vaccineButton;
    public Button carrotButton;
    public Button appleButton;

    // Start is called before the first frame update
    void Start()
    {
        shopPanel.SetActive(false);
        foreach (var player in FindObjectsOfType<PlayerResistance>())
        {
            if (player.gameObject.GetPhotonView().Owner == PhotonNetwork.LocalPlayer)
            {
                pr = player;
            }
        }
        currency = 0;
        InvokeRepeating("Income", 0, 10);
    }

    void Income()
    {
        ChangeCurrency(50);
    }

    public bool ChangeCurrency(int currencyToAdd)
    {
        currency += currencyToAdd;
        if (currency < 0)
        {
            currency -= currencyToAdd;
            return false;
        }
        return true;
    }

    public void BuyAndUse(string item)
    {
        var cost = ObjectDictionary.GetObjectCost(item);
        if (ChangeCurrency(cost * -1))
        {
            ObjectDictionary.UseObject(item, pr);
        }
    }

    // Update is called once per frame
    void Update()
    {

        vaccineButton.interactable = ObjectDictionary.GetObjectCost("Vaccine") <= currency && PhotonNetwork.LocalPlayer.GetScore() == 1;
        carrotButton.interactable = ObjectDictionary.GetObjectCost("Carrot") <= currency && PhotonNetwork.LocalPlayer.GetScore() == 0;
        appleButton.interactable = ObjectDictionary.GetObjectCost("Apple") <= currency && PhotonNetwork.LocalPlayer.GetScore() == 0;


        currencyText.text = "Currency: $" + currency.ToString();
        if (Input.GetKeyDown(KeyCode.X))
        {
            Cursor.lockState = shopPanel.activeSelf ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = shopPanel.activeSelf ? false : true;
            shopPanel.SetActive(!shopPanel.activeSelf);
        }
    }
}
