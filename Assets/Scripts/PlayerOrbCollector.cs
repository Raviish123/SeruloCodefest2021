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
                //GetComponent<AudioSource>().Play();
            }

            Destroy(other.gameObject);

        }
    }
}
