using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(MoveNetworkObjectLinearlyInOneDirection))]
public class ZombieChasePlayer : NetworkBehaviour
{
    [SerializeReference]
    private MoveNetworkObjectLinearlyInOneDirection m_moveNetworkObjLinearly;

    private Transform m_playerTransformToChase;

    private Vector2 zombieToPlayerDirection = Vector2.zero;

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

        zombieToPlayerDirection.x =
            m_playerTransformToChase.position.x - transform.position.x;

        zombieToPlayerDirection.y =
            m_playerTransformToChase.position.z - transform.position.z;

        Vector2 zombieToPlayerNormalized = zombieToPlayerDirection.normalized;

        // Calculate the angle between the zombie's forward vector and the direction
        // vector pointing towards the player
        float angle =
            Mathf.Atan2(zombieToPlayerNormalized.x, zombieToPlayerNormalized.y)
            * Mathf.Rad2Deg;

        // Set the zombie's rotation around the Y-axis
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        m_moveNetworkObjLinearly.direction.x = zombieToPlayerNormalized.x;
        m_moveNetworkObjLinearly.direction.z = zombieToPlayerNormalized.y;
    }
}
