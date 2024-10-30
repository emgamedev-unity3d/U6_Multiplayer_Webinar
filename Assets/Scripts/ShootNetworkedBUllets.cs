using UnityEngine;
using Unity.Netcode;

public class ShootNetworkedBullets : NetworkBehaviour
{
    [SerializeField]
    GameObject m_bulletPrefab;

    [SerializeField]
    Transform m_bulletSpawnPoint;

    private void Update()
    {
        if (!IsOwner)
            return;

        // once we the press space bar, tell the server we'd like to
        //   fire a bullet object
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