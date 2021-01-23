using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MenuManager : MonoBehaviourPunCallbacks
{

    public GameObject mainMenuPanel;
    public GameObject joinOrCreatePanel;
    // public GameObject createOptionsPanel;

    public Text status;

    public InputField roomCodeField;

    public Button playButton;
    public Button joinButton;
    public Button createButton;


    private void ActivatePanel(string panelName)
    {
        mainMenuPanel.SetActive(false);
        joinOrCreatePanel.SetActive(false);
        // createOptionsPanel.SetActive(false);
        switch (panelName)
        {
            case "Main":
                mainMenuPanel.SetActive(true);
                break;
            case "JOC":
                joinOrCreatePanel.SetActive(true);
                break;
            /*case "CO":
                createOptionsPanel.SetActive(true);
                break;*/
            default:
                break;
        }
    }

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        playButton.interactable = false;
        ActivatePanel("Main");
        status.text = "";


        if (PhotonNetwork.IsConnected)
        {
            playButton.interactable = true;
            status.text = "Connected.";
        } else
        {
            status.text = "Connecting...";
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = "0.1.0";
        }
    }

    public override void OnConnectedToMaster()
    {
        playButton.interactable = true;
        status.text = "Connected.";
    }

    public void Play()
    {
        ActivatePanel("JOC");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Join()
    {
        if (roomCodeField.text == "") return;
        joinButton.interactable = false;
        createButton.interactable = false;
        PhotonNetwork.JoinRoom(roomCodeField.text);
    }

    public void Create()
    {
        joinButton.interactable = false;
        createButton.interactable = false;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(GenerateRandom6DigitNumber(), roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(GenerateRandom6DigitNumber(), roomOptions);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        status.text = "Room doesn't exist or it is full.";
        joinButton.interactable = true;
        createButton.interactable = true;
    }

    private string GenerateRandom6DigitNumber()
    {
        return Random.Range(1, 1000000).ToString("D6");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.NickName = "Player" + PhotonNetwork.LocalPlayer.ActorNumber.ToString();
        CharacterSwitcher.characterName = PhotonNetwork.IsMasterClient ? "Remy" : "Leonard";
        PhotonNetwork.LoadLevel("Lobby");
    }
}
