using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Transform playerList;

    public Transform[] spawnPoints;

    public Text roomCodeText;

    public Button startButton;

    public Text playerCountText;

    public Text infoText;

    public Text characterText;

    public Button remy;
    public Button leonard;
    public Button shae;


    // Start is called before the first frame update
    void Start()
    {
        UpdatePlayerList();
        roomCodeText.text = PhotonNetwork.CurrentRoom.Name;
        infoText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        startButton.interactable = false;
        GameObject player = PhotonNetwork.Instantiate("Player Lobby", spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].position, Quaternion.identity);
        photonView.RPC("PlayerInstantiated", RpcTarget.AllBuffered, player.GetPhotonView().ViewID, CharacterSwitcher.characterName);
    }

    [PunRPC]
    public void PlayerInstantiated(int viewID, string character)
    {
        var characterToBeActivated = PhotonView.Find(viewID).transform.GetChild(1).Find(character).gameObject;
        characterToBeActivated.SetActive(true);
        characterToBeActivated.transform.SetAsFirstSibling();
    }

    private void UpdatePlayerList()
    {
        for (int i = 0; i < 5; i++)
        {
            playerList.GetChild(i).gameObject.GetComponent<Text>().text = "";
        }
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            playerList.GetChild(i).gameObject.GetComponent<Text>().text = PhotonNetwork.CurrentRoom.GetPlayer(i + 1).NickName;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                startButton.interactable = true;
                infoText.gameObject.SetActive(true);
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        startButton.interactable = false;
        infoText.gameObject.SetActive(false);
    }

    void UpdateCharacter()
    {
        characterText.text = "Current Character: " + CharacterSwitcher.characterName;
        remy.interactable = false;
        leonard.interactable = false;
        shae.interactable = false;
        switch (CharacterSwitcher.characterName)
        {
            case "Remy":
                remy.interactable = true;
                break;
            case "Leonard":
                leonard.interactable = true;
                break;
            case "Shae":
                shae.interactable = true;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerList();
        UpdateCharacter();
        if (Input.GetKeyDown(KeyCode.Return) && startButton.interactable)
        {
            StartGame();
        }
        playerCountText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    public void StartGame()
    {
        startButton.interactable = false;
        photonView.RPC("StartGameForAllPlayers", RpcTarget.All);
    }

    [PunRPC]
    public void StartGameForAllPlayers()
    {
        PhotonNetwork.LoadLevel("Main");
    }
}
