using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class NetworkedHealth : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<int> m_health = new(3);

    [SerializeField]
    private Image m_healthImage;

    public UnityEvent OnHealthDepleted = new();

    private int m_initialHealthValue;

    public override void OnNetworkSpawn()
    {
        // we'd like for this code to run on both client and server side
        m_initialHealthValue = m_health.Value;

        if (!IsClient)
            return;

        // We only need react to health value changed on the client-side
        m_health.OnValueChanged += OnHealthValueChanged;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsClient)
            return;

        // We only need un-subscribe from the health value
        //   changed event on the client-side
        m_health.OnValueChanged -= OnHealthValueChanged;
    }

    private void OnHealthValueChanged(int previous, int current)
    {
        float healthRatio = (float)current / m_initialHealthValue;

        m_healthImage.transform.localScale = new Vector3(healthRatio, 1f, 1f);
    }

    public void TakeDamage(int damage = 1)
    {
        if (!IsOwner)
            return;

        // The health data is owned by the server side, so we need to handle
        //   the changes here
        int currentHealthValue = m_health.Value;

        m_health.Value = currentHealthValue - damage;

        Debug.Log($"{gameObject.name}'s health is now {m_health.Value}");

        if(m_health.Value <= 0 )
        {
            OnHealthDepleted.Invoke();

            // Despawn the object, from the server side, once the health is 0 or less
            NetworkObjectDespawner.DespawnNetworkObject(NetworkObject);
        }
    }
}