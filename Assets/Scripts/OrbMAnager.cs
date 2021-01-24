using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OrbMAnager : MonoBehaviourPun
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float yPos;

    private int orbCount = 0;
    private float orbSpawnTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) Destroy(this);

        InvokeRepeating("SpawnOrb", 0, 0.25f);

    }

    public void ReduceOrb()
    {
        orbCount--;
    }

    void SpawnOrb()
    {
        if (orbCount > 900) return;
        orbCount++;
        int layerMask = ~LayerMask.GetMask("Ground");
        Vector3 orbPosition = new Vector3(Random.Range(startPos.x, endPos.x), yPos, Random.Range(startPos.z, endPos.z));
        while (Physics.OverlapSphere(orbPosition, 2.5f, layerMask).Length != 0)
        {
            orbPosition = new Vector3(Random.Range(startPos.x, endPos.x), yPos, Random.Range(startPos.z, endPos.z));
        }
        var rayPosition = orbPosition;
        rayPosition.y = -10;
        RaycastHit hit;
        Physics.Raycast(rayPosition, Vector3.up, out hit, 20f, LayerMask.GetMask("Ground"));
        orbPosition.y = hit.point.y + yPos;
        PhotonNetwork.Instantiate("Orb", orbPosition, Quaternion.identity);
    }

    IEnumerator orbRoutine()
    {
        yield return new WaitForSeconds(orbSpawnTime);
        while (true)
        {
            for (int i = 0; i < 10; i++)
            {
                SpawnOrb();
                yield return new WaitForSeconds(orbSpawnTime);
            }
            orbSpawnTime = orbSpawnTime * 1.25f;
            yield return null;
        }
    }
}
