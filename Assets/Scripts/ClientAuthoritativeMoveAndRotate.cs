using Unity.Netcode;
using UnityEngine;

public class ClientAuthoritativeMoveAndRotate : NetworkBehaviour
{
    public float rotationSpeed = 100.0f;
    public float movementSpeed = 5.0f;

    private void Start()
    {
        if (!IsOwner)
            enabled = false;
    }

    void Update()
    {
        var multiplier = movementSpeed * Time.deltaTime;

        // Rotation
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
        }

        // Old input backends are enabled.
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -multiplier * transform.right;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += multiplier * transform.right;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            transform.position += multiplier * transform.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position += -multiplier * transform.forward;
        }
    }
}
