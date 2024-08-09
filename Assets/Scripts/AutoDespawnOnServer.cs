using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class AutoDespawnOnServer : NetworkBehaviour
{
    [Min(0f)]
    [SerializeField]
    [Header("Time alive in seconds (s)")]
    private float m_autoDestroyTime;

    private void Update()
    {
        if (!IsServer)
            return;

        m_autoDestroyTime -= Time.deltaTime;

        if (m_autoDestroyTime <= 0f)
        {
            NetworkObjectDespawner.DespawnNetworkObject(NetworkObject);
        }
    }
}