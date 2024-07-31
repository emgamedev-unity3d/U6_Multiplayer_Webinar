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
        if (!m_gameStarted || !CanStillSpawn())
            return;

        m_currentTime += Time.deltaTime;

        if(m_currentTime > m_spawnCooldown)
        {
            m_currentTime = 0f;

            Debug.Log("Spawning Zombie!");
            SpawnZombie();
        }
    }

    private void OnNetworkManagerServerStarted()
    {
        // only spawn enemies on the server or host
        if (!IsServer && !IsHost)
        {
            enabled = false;
            return;
        }

        Debug.Log("Server connected, game has started!");

        m_gameStarted = true;
    }

    private bool CanStillSpawn()
    {
        return m_zombieSpawnCount < MAX_ZOMBIES_TO_SPAWN;
    }

    private void SpawnZombie()
    {
        int spawnPositionIndex = Random.Range(0, m_spawnPositions.Count - 1);
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
}