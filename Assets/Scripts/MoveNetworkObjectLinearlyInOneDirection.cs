using Unity.Netcode;
using UnityEngine;

public class MoveNetworkObjectLinearlyInOneDirection : NetworkBehaviour
{
    public Vector3 direction = Vector3.right;

    public float speed = 2f;

    private Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!IsOwner || !IsSpawned)
            return;

        var target = transform.position + speed * direction;

        rigidbody.MovePosition(
            Vector3.Lerp(transform.position, target, Time.deltaTime));

        //transform.Translate(speed * Time.deltaTime * direction);
    }
}