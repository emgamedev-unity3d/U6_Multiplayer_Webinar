using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChasePlayer : NetworkBehaviour
{
    [SerializeField]
    private NavMeshAgent m_NavMeshAgent;

    private Transform m_playerTransformToChase;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            // Since the zombies are owned server side, we choose our target here

            // NOTE: We're using the network manager's internal list of connected clients
            //  to choose a random player to chase
            var connectedClients = NetworkManager.Singleton.ConnectedClients.Values.ToArray();

            // Note: We're not using ".Length - 1" because the int Random.Range(...)
            //   function's max parameter is exclusive.
            // 
            // Ex.Random.Range(0,2) excludes 2, so max value included is 1
            int randomIndex = Random.Range(0, connectedClients.Length);

            var randomClientToChase = connectedClients[randomIndex];

            // Note: Using the PlayerObject property works because we've assigned a player
            //   prefab to the Network Manager object, from the editor.
            m_playerTransformToChase = randomClientToChase.PlayerObject.transform;
            Debug.Log($"Spawned Zombie is chasing client #{randomClientToChase.ClientId}!");

            // Fun fact: You can leave the player prefab field empty in the editor,
            //  and assign the player object at runtime
        }
        else
        {
            // disable nav mesh agent, because we're not the owner of the zombie
            m_NavMeshAgent.enabled = false;
        }

    }

    private void Update()
    {
        if (!IsOwner || !NetworkObject.IsSpawned)
            return;

        m_NavMeshAgent.destination = m_playerTransformToChase.position;
    }
}
