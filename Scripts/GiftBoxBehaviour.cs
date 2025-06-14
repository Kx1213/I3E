using UnityEngine;

public class GiftBoxBehaviour : MonoBehaviour
{
    [SerializeField]
    GameObject coinPrefab;

    [SerializeField]
    int coinCount = 1; // Set how many coins to spawn in the Inspector

    [SerializeField]
    float spawnSpread = 0.5f; // Optional: spacing between spawned coins

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            SpawnCoins();
            Destroy(collision.gameObject); // Destroy the projectile
            Destroy(gameObject); // Destroy the gift box
        }
    }

    void SpawnCoins()
    {
        for (int i = 0; i < coinCount; i++)
        {
            Vector3 spawnOffset = new Vector3(
                Random.Range(-spawnSpread, spawnSpread),
                0.5f,
                Random.Range(-spawnSpread, spawnSpread)
            );
            Instantiate(coinPrefab, transform.position + spawnOffset, Quaternion.identity);
        }
    }
}