using UnityEngine;
using Unity.Netcode;

public class ShootNetworkedBullets : NetworkBehaviour
{
    [SerializeField]
    GameObject m_bulletPrefab;

    [SerializeField]
    Transform m_bulletSpawnPoint;

    private void Start()
    {
        // only spawn bullets on the server, since it will own them
        if (!IsOwner)
            enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireNewBulletServerRpc();
        }
    }

    [Rpc(SendTo.Server)]
    private void FireNewBulletServerRpc()
    {
        var newBullet = Instantiate(
            m_bulletPrefab,
            m_bulletSpawnPoint.position,
            Quaternion.identity);

        var moveLinearlyComp = newBullet.GetComponent<MoveNetworkObjectLinearlyInOneDirection>();
        moveLinearlyComp.direction = transform.forward;

        if (!newBullet.TryGetComponent(out NetworkObject networkObject))
            return;

        networkObject.Spawn(true);
    }
}