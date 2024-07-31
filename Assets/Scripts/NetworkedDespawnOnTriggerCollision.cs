using Unity.Netcode;
using UnityEngine;

public class NetworkedDespawnOnTriggerCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{collision.gameObject.name} TRIGGERED with me!");

        var zombieChasePlayer = GetComponent<ZombieChasePlayer>();
        if (zombieChasePlayer == null)
            return;

        NetworkObjectDespawner.DespawnNetworkObject(
            zombieChasePlayer.NetworkObject);

        var networkObject = GetComponent<NetworkObject>();

        NetworkObjectDespawner.DespawnNetworkObject(networkObject);
    }

    private void OnTriggerEnter(Collider other)
    {
    }
}
