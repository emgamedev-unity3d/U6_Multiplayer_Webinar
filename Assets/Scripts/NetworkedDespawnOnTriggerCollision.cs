using Unity.Netcode;
using UnityEngine;

public class NetworkedDespawnOnTriggerCollision : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         if(!IsOwner)
            enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        NetworkObjectDespawner.DespawnNetworkObject(NetworkObject);
    }
}
