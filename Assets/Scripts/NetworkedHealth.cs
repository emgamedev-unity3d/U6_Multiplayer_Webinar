using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class NetworkedHealth : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<int> m_health = new(3);

    [SerializeField]
    private Image m_healthImage;

    private int m_initialHealthValue;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;

        m_initialHealthValue = m_health.Value;
        m_health.OnValueChanged += OnHealthValueChanged;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer)
            return;

        m_health.OnValueChanged -= OnHealthValueChanged;
    }

    public void TakeDamage(int damage = 1)
    {
        if (!IsServer)
            return;

        int currentHealthValue = m_health.Value;

        m_health.Value = currentHealthValue - damage;

        Debug.Log($"{gameObject.name}'s health is now {m_health.Value}");

        if(m_health.Value <= 0 )
        {
            NetworkObjectDespawner.DespawnNetworkObject(NetworkObject);
        }
    }

    private void OnHealthValueChanged(int previous, int current)
    {
        float healthRatio = (float)current / m_initialHealthValue;

        m_healthImage.transform.localScale = new Vector3(healthRatio, 1f, 1f);
    }
}