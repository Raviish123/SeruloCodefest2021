using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class PlayerInfected : MonoBehaviourPun
{
    public float initialTimer;

    private float timer;
    private Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        timerText = FindObjectOfType<TheGameManager>().timerText;
        timer = initialTimer;
        timerText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        timerText.gameObject.SetActive(PhotonNetwork.LocalPlayer.GetScore() == 1);
        if (PhotonNetwork.LocalPlayer.GetScore() == 1)
        {
            timer -= Time.deltaTime;
            if ((Mathf.RoundToInt(timer) % 2) == 0)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = Color.white;
            }
            timerText.text = Mathf.RoundToInt(timer).ToString();
            if (timer <= 0)
            {
                FindObjectOfType<TheGameManager>().mapCam.SetActive(true);
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
        else
        {
            timer = initialTimer;
        }
    }
}
