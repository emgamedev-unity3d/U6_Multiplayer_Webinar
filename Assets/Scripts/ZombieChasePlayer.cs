using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(MoveNetworkObjectLinearlyInOneDirection))]
public class ZombieChasePlayer : NetworkBehaviour
{
    [SerializeReference]
    private MoveNetworkObjectLinearlyInOneDirection m_moveNetworkObjLinearly;

    private Transform m_playerTransformToChase;

    private void Start()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }

        // NOT EFFICIENT CODE, JUST FOR PRESENTATION PURPOSES ONLY
        var players = FindObjectsByType<ClientAuthoritativeMoveAndRotate>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None);

        m_playerTransformToChase = players[Random.Range(0, players.Length - 1)].transform;
    }

    private void Update()
    {
        if (!NetworkObject.IsSpawned)
            return;

        Vector3 zombieToPlayerDir = Vector3.Normalize(
            m_playerTransformToChase.position - transform.position);

        // Calculate the angle between the zombie's forward vector and the direction
        // vector pointing towards the player
        float angle = Mathf.Atan2(zombieToPlayerDir.x, zombieToPlayerDir.z) * Mathf.Rad2Deg;

        // Set the zombie's rotation around the Y-axis
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        m_moveNetworkObjLinearly.direction = zombieToPlayerDir;
    }
}
