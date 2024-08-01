using Unity.Netcode;
using UnityEngine;

public class NetworkedDespawnOnTriggerCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[{name}] Collided with {collision.gameObject.name}.");
        var localNetworkObject = GetComponent<NetworkObject>();
        if ( localNetworkObject != null && !localNetworkObject.NetworkManager.IsServer)
        {
            return;
        }
        NetworkObjectDespawner.DespawnNetworkObject(localNetworkObject);
        var zombieChasePlayer = collision.gameObject.GetComponent<ZombieChasePlayer>();
        if (zombieChasePlayer)
        {
            NetworkObjectDespawner.DespawnNetworkObject(zombieChasePlayer.NetworkObject);
            Debug.Log($"[{name}] Killed zombie!");
        }
    }
}
