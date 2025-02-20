using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ZombieSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject m_zombiePrefab;

    [Range(2f, 10f)]
    [SerializeField]
    private float m_spawnCooldown = 5f;

    [SerializeField]
    private List<Transform> m_spawnPositions = new();

    private bool m_gameStarted = false;

    private float m_currentTime = 0f;

    private int m_zombieSpawnCount = 0;

    private const int MAX_ZOMBIES_TO_SPAWN = 10;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += OnNetworkManagerServerStarted;
    }

    private void FixedUpdate()
    {
        // only spawn new zombies if you're the host device
        if (!m_gameStarted || !CanStillSpawn() || !IsHost)
            return;

        m_currentTime += Time.deltaTime;

        if(m_currentTime > m_spawnCooldown)
        {
            m_currentTime = 0f;

            SpawnZombie();

            Debug.Log($"Spawned Zombie #{m_zombieSpawnCount}!");
        }
    }

    private void OnNetworkManagerServerStarted()
    {
        Debug.Log("Server connected, game has started!");

        m_gameStarted = true;
    }

    private bool CanStillSpawn()
    {
        return m_zombieSpawnCount < MAX_ZOMBIES_TO_SPAWN;
    }

    private void SpawnZombie()
    {
        // Note: We're not using ".Count - 1" because the int Random.Range(...)
        //   function's max parameter is exclusive.
        // 
        // Ex.Random.Range(0,2) excludes 2, so max value included is 1
        int spawnPositionIndex = Random.Range(0, m_spawnPositions.Count);
        var spawnPosition = m_spawnPositions.Count == 0 ?
            Vector3.zero :
            m_spawnPositions[spawnPositionIndex].position;

        m_zombieSpawnCount++;

        NetworkObjectSpawner.SpawnNewNetworkObject(m_zombiePrefab, spawnPosition);
    }
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        NetworkManager.Singleton.OnServerStarted -= OnNetworkManagerServerStarted;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (m_spawnPositions == null || m_spawnPositions.Count == 0)
            return;

        foreach(var spawnPosition in m_spawnPositions)
        {
            Gizmos.DrawWireSphere(spawnPosition.position, 1.5f);
        }
    }
#endif
}