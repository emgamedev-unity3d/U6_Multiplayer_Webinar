using System.Linq;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(MoveNetworkObjectLinearlyInOneDirection))]
public class ZombieChasePlayer : NetworkBehaviour
{
    [SerializeReference]
    private MoveNetworkObjectLinearlyInOneDirection m_moveNetworkObjLinearly;

    private Transform m_playerTransformToChase;

    private Vector2 zombieToPlayerDirection = Vector2.zero;
    private Vector2 zombieToPlayerNormalized = Vector2.zero;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsServer)
            return;

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

    private void Update()
    {
        if (!IsOwner || !NetworkObject.IsSpawned)
            return;

        UpdateZombieToPlayerDirection();

        UpdateZombieRotationToFacePlayer();

        UpdateMoveLinearlyComponent();
    }

    private void UpdateZombieToPlayerDirection()
    {
        zombieToPlayerDirection.x =
            m_playerTransformToChase.position.x - transform.position.x;

        zombieToPlayerDirection.y =
            m_playerTransformToChase.position.z - transform.position.z;

        zombieToPlayerNormalized = zombieToPlayerDirection.normalized;
    }

    private void UpdateZombieRotationToFacePlayer()
    {
        //// Calculate the angle between the zombie's forward vector and
        //// the direction vector pointing towards the player
        //float newYaxisAngle =
        //    Mathf.Atan2(zombieToPlayerNormalized.x, zombieToPlayerNormalized.y)
        //    * Mathf.Rad2Deg;

        //// Set the zombie's rotation around the Y-axis
        //transform.rotation = Quaternion.Euler(0f, newYaxisAngle, 0f);

        transform.LookAt(m_playerTransformToChase.position);
    }

    private void UpdateMoveLinearlyComponent()
    {
        m_moveNetworkObjLinearly.direction.x = zombieToPlayerNormalized.x;
        m_moveNetworkObjLinearly.direction.z = zombieToPlayerNormalized.y;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawLine(
            transform.position,
            new Vector3(
                transform.position.x + zombieToPlayerNormalized.x,
                -0.5f,
                transform.position.z + zombieToPlayerNormalized.y));
    }
#endif
}
