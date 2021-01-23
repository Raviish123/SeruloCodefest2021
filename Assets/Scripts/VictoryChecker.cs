using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VictoryChecker : MonoBehaviourPunCallbacks
{
    private bool startedChecking = false;
    private bool gameOver = false;


    public override void OnLeftRoom()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("VictoryView");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver && PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        if (FindObjectsOfType<PlayerResistance>().Length == 1 && startedChecking && !gameOver)
        {
            Victory();
        } else if (!startedChecking)
        {
            if (FindObjectsOfType<PlayerResistance>().Length == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                startedChecking = true;
            }
        }
    }

    private void Victory()
    {
        var winnerName = FindObjectOfType<PlayerResistance>().gameObject.GetPhotonView().Owner.NickName;
        photonView.RPC("GameOver", RpcTarget.All, winnerName);
    }

    [PunRPC]
    public void GameOver(string winnerName)
    {
        WInnerStorer.winnerName = winnerName;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameOver = true;
    }
}

