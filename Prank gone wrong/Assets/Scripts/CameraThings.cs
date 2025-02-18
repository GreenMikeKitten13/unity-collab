using UnityEngine;
using Mirror;

public class FollowPlayer : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        FindLocalPlayer();
    }

    void Update()
    {
        if (player == null) FindLocalPlayer();
        if (player != null) transform.position = player.position + new Vector3(0.8f, 3, -0.5f);
    }

    void FindLocalPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            if (p.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                player = p.transform;
                break;
            }
        }
    }
}
