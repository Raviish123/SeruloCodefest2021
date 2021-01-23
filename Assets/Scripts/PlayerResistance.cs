using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class PlayerResistance : MonoBehaviourPun
{
    public Slider resistanceSlider;
    public Button infectButton;

    private TheGameManager tgm;
    private float resistance;

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) return;
        PhotonNetwork.LocalPlayer.SetScore(0);
        tgm = FindObjectOfType<TheGameManager>();
        infectButton = tgm.infectButton.GetComponent<Button>();
        resistance = 100;
        resistanceSlider = FindObjectOfType<Slider>();
        resistanceSlider.value = resistance;
        resistanceSlider.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        infectButton.gameObject.SetActive(PhotonNetwork.LocalPlayer.GetScore() == 1);
        if (PhotonNetwork.LocalPlayer.GetScore() == 1)
        {
            resistanceSlider.value = 0;
            resistance = 0;
            resistanceSlider.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.green;
            int actorNr = CheckToInfect();
            infectButton.interactable = actorNr != -1;
            if (Input.GetKeyDown(KeyCode.E) && infectButton.interactable)
            {
                Infect(actorNr);
            }
            return;
        }

        resistanceSlider.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.red;
        var multiplier = GetComponent<PlayerMovement>().isRunning ? 6 : 2;
        resistance -= Time.deltaTime * multiplier;
        resistance = Mathf.Clamp(resistance, -1f, 100f);
        resistanceSlider.value = resistance;
        if (resistance <= 0) PhotonNetwork.LocalPlayer.SetScore(1);
    }

    private int CheckToInfect()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, 10f);
        int actorNr = -1;
        foreach (var player in players)
        {
            if (player.gameObject.GetComponent<PlayerResistance>() != null && player.gameObject != gameObject)
            {
                if (player.gameObject.GetPhotonView().Owner.GetScore() == 1)
                {
                    continue;
                }
                actorNr = player.gameObject.GetPhotonView().OwnerActorNr;
            }
        }
        return actorNr;
    }

    public void IncreaseResistance(int amount)
    {
        if (PhotonNetwork.LocalPlayer.GetScore() == 1) return;
        resistance += amount;
        resistance = Mathf.Clamp(resistance, -1f, 100);
    }

    private void Infect(int actorNr)
    {
        PhotonNetwork.LocalPlayer.SetScore(0);
        resistance = 50;
        photonView.RPC("SendInfection", PhotonNetwork.CurrentRoom.GetPlayer(actorNr));
    }

    [PunRPC]
    public void SendInfection()
    {
        PhotonNetwork.LocalPlayer.SetScore(1);
    }

    public void Disinfect()
    {
        PhotonNetwork.LocalPlayer.SetScore(0);
    }
}
