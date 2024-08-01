using Unity.Netcode;
using UnityEngine;


public class ClientAuthoritativeMoveAndRotate : NetworkBehaviour
{
    [Range(0.01f, 1.0F)]
    public float rotationSpeed = 0.10f;
    [Range(0.1f, 5.0f)]
    public float movementSpeed = 5.0f;
    private Rigidbody m_Rigidbody;    

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.maxAngularVelocity = 4;
        m_Rigidbody.maxLinearVelocity = 15;
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Camera.main.transform.SetParent(transform, false);
        }
        base.OnNetworkSpawn();
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            Camera.main.transform.SetParent(null, false);
        }
        base.OnNetworkDespawn();
    }

    private void FixedUpdate()
    {
        if (!IsSpawned || !IsOwner)
        {
            return;
        }
        var hasMotion = false;
        var hasRotation = false;
        var motion = transform.forward;
        float rotate = 0.0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            hasMotion = true;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            hasMotion = true;
            motion *= -1.0f;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            hasRotation = true;
            rotate = -rotationSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            hasRotation = true;
            rotate = rotationSpeed;
        }

        if (hasRotation)
        {
            m_Rigidbody.angularVelocity += Vector3.up * rotate;
        }
        if (hasMotion)
        {
            motion *= movementSpeed;
            motion.y = 0.0f;
#if UNITY_2023_3_OR_NEWER
            m_Rigidbody.linearVelocity += motion;
#else
            m_Rigidbody.velocity += motion;
#endif
        }
    }
}
