using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerOrbCollector : MonoBehaviourPun
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Orb")
        {
            if (photonView.IsMine)
            {
                GetComponent<PlayerResistance>().IncreaseResistance(20);
                FindObjectOfType<ShopManager>().ChangeCurrency(25);
                if (PhotonNetwork.IsMasterClient) FindObjectOfType<OrbMAnager>().ReduceOrb();
                GetComponent<AudioSource>().Play();
            }

            Destroy(other.gameObject);

        }
    }
}
