using Unity.Cinemachine;
using Unity.Netcode;

public class ClientGetReferenceToCinemachineCam : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            return;

        base.OnNetworkSpawn();

        var cinemachineCamera = FindAnyObjectByType<CinemachineCamera>();

        if (cinemachineCamera == null)
            return;

        cinemachineCamera.Follow = transform;
    }
}