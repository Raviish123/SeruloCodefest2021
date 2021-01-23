using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class TheGameManager : MonoBehaviourPun
{
    public Transform[] spawnPoints;
    public Text timerText;
    public GameObject infectButton;
    public GameObject mapCam;

    // Start is called before the first frame update
    void Start()
    {
        mapCam.SetActive(false);
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].position, Quaternion.identity);
        photonView.RPC("PlayerInstantiatedLobby", RpcTarget.AllBuffered, player.GetPhotonView().ViewID, CharacterSwitcher.characterName);
    }

    [PunRPC]
    public void PlayerInstantiatedLobby(int viewID, string character)
    {
        var characterToBeActivated = PhotonView.Find(viewID).transform.GetChild(1).Find(character).gameObject;
        characterToBeActivated.SetActive(true);
        characterToBeActivated.transform.SetAsFirstSibling();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
