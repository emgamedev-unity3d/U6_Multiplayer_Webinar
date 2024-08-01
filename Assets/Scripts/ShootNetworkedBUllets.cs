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
        newBullet.transform.forward = transform.forward;
        var moveLinearlyComp = newBullet.GetComponent<MoveNetworkObjectLinearlyInOneDirection>();
        moveLinearlyComp.direction = transform.forward;
        // Always shoot bullets slightly faster than the player is already moving
        moveLinearlyComp.speed += GetComponent<Rigidbody>().linearVelocity.magnitude * 1.15f;
        var moverRigidbody = moveLinearlyComp.GetComponent<Rigidbody>();
        moverRigidbody.isKinematic = false;
        moverRigidbody.angularVelocity = Vector3.up * 60.0f;

        if (!newBullet.TryGetComponent(out NetworkObject networkObject))
            return;

        networkObject.Spawn(true);
    }
}