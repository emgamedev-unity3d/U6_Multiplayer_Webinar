using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkedHealth))]
public class NetworkedReduceHealthOnTrigger : NetworkBehaviour
{
    [SerializeField]
    private NetworkedHealth m_networkedHealth;

    [SerializeField]
    private int m_damageAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer)
            return;

        m_networkedHealth.TakeDamage(m_damageAmount);
    }
}