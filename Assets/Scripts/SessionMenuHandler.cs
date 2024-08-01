using Unity.Netcode;
using UnityEngine;

public class SessionMenuHandler : MonoBehaviour
{
    public Canvas UICanvas;
    private NetworkManager m_NetworkManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_NetworkManager = GetComponent<NetworkManager>();
        m_NetworkManager.OnClientStarted += OnClientStarted;
    }

    private void OnClientStarted()
    {
        m_NetworkManager.OnClientStarted -= OnClientStarted;
        m_NetworkManager.OnClientStopped += OnClientStopped;
        UICanvas.gameObject.SetActive(false);
    }

    private void OnClientStopped(bool isHost)
    {
        m_NetworkManager.OnClientStarted += OnClientStarted;
        m_NetworkManager.OnClientStopped -= OnClientStopped;
        UICanvas.gameObject.SetActive(true);
    }

    private void Update()
    {
        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        //{
        //    Debug.Log("Forward");
        //}
        //if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        //{
        //    Debug.Log("Backward");
        //}
        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //{
        //    Debug.Log("Rotate Left");
        //}
        //if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        //{
        //    Debug.Log("Rotate Right");
        //}
    }
}
