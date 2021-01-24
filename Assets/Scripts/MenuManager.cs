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
    public GameObject enterNamePanel;
    public GameObject optPanel;
    public GameObject createOptsPanel;
    public GameObject howToPanel;

    public Text status;

    public Dropdown dropdown;

    public Slider volume;

    public InputField roomCodeField;
    public InputField nameField;

    public Button playButton;
    public Button joinButton;
    public Button backButton;
    public Button realCreateButton;
    public Button createButton;



    public void ActivatePanel(string panelName)
    {
        mainMenuPanel.SetActive(false);
        joinOrCreatePanel.SetActive(false);
        enterNamePanel.SetActive(false);
        optPanel.SetActive(false);
        howToPanel.SetActive(false);
        createOptsPanel.SetActive(false);
        switch (panelName)
        {
            case "Main":
                mainMenuPanel.SetActive(true);
                break;
            case "JOC":
                joinOrCreatePanel.SetActive(true);
                break;
            case "EN":
                enterNamePanel.SetActive(true);
                break;
            case "Opt":
                optPanel.SetActive(true);
                break;
            case "COP":
                createOptsPanel.SetActive(true);
                break;
            case "HTP":
                howToPanel.SetActive(true);
                break;
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
            PhotonNetwork.GameVersion = "1.0.0";
        }
    }

    public override void OnConnectedToMaster()
    {
        playButton.interactable = true;
        status.text = "Connected.";
    }

    public void Play()
    {
        ActivatePanel("EN");
    }

    public void Next()
    {
        if (nameField.text == "") return;
        PhotonNetwork.LocalPlayer.NickName = nameField.text;
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
        backButton.interactable = false;
        realCreateButton.interactable = false;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = byte.Parse(dropdown.captionText.text.Split(' ')[0]);
        PhotonNetwork.CreateRoom(GenerateRandom6DigitNumber(), roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = byte.Parse(dropdown.captionText.text.Split(' ')[0]); ;
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
        CharacterSwitcher.characterName = PhotonNetwork.IsMasterClient ? "Remy" : "Leonard";
        PhotonNetwork.LoadLevel("Lobby");
    }

    private void Update()
    {
        VolumeStorer.volume = volume.value;
        FindObjectOfType<AudioSource>().volume = VolumeStorer.volume;
    }
}
