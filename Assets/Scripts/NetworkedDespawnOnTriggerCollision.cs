using Unity.Netcode;
using UnityEngine;

public class NetworkedDespawnOnTriggerCollision : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner)
            return;

        NetworkObjectDespawner.DespawnNetworkObject(NetworkObject);
    }
}
