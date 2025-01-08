using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private List<GameObject> weaponPrefabs = new List<GameObject>();

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    public void SpawnWeapons()
    {
        foreach (Transform spawnPoint  in spawnPoints)
        {
            SpawnRandomWeapon(spawnPoint.position, spawnPoint.rotation);
        }
    }

    private void SpawnRandomWeapon(Vector3 position, Quaternion rotation)
    {
        int randomIndex = Random.Range(0, weaponPrefabs.Count);
        GameObject weapon = weaponPrefabs[randomIndex];

        Object.Instantiate(weapon, position, rotation);
    }
}
