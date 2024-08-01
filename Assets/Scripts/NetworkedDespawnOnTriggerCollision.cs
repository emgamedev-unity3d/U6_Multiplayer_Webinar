using Unity.Netcode;
using UnityEngine;

public class NetworkedDespawnOnTriggerCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[{name}] Collided with {collision.gameObject.name}.");

        NetworkObjectDespawner.DespawnNetworkObject(GetComponent<NetworkObject>());
        var zombieChasePlayer = collision.gameObject.GetComponent<ZombieChasePlayer>();
        if (zombieChasePlayer)
        {
            NetworkObjectDespawner.DespawnNetworkObject(zombieChasePlayer.NetworkObject);
            Debug.Log($"[{name}] Killed zombie!");
        }
    }
}
