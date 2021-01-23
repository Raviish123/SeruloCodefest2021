using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterSwitcher : MonoBehaviourPun
{
    public static string characterName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchCharacter("Remy");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchCharacter("Leonard");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchCharacter("Shae");
        }
    }

    public void SwitchCharacter(string name)
    {
        characterName = name;
        photonView.RPC("ChangeCharacter", RpcTarget.AllBuffered, photonView.ViewID, name);
    }

    [PunRPC]
    public void ChangeCharacter(int viewID, string name)
    {
        var model = PhotonView.Find(viewID).transform.GetChild(1);
        for (int i = 0; i < model.childCount; i++)
        {
            model.GetChild(i).gameObject.SetActive(false);
        }
        var characterToBeActivated = model.Find(name).gameObject;
        characterToBeActivated.SetActive(true);
        characterToBeActivated.transform.SetAsFirstSibling();
    }
}
