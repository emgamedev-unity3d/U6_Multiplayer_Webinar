using Unity.Netcode;
using UnityEngine;

public class ClientAuthoritativeMoveAndRotate : NetworkBehaviour
{
    [SerializeField]
    private float rotationSpeed = 100f;

    [SerializeField]
    private float movementSpeed = 5f;

    void Update()
    {
        if (!IsOwner)
            return;

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

        // Movement
        if (Input.GetKey(KeyCode.A))
        {
            //transform.position += -multiplier * transform.right;
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //transform.position += multiplier * transform.right;
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
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
