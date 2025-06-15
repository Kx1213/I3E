using UnityEngine;
using StarterAssets; // Make sure this matches your namespace for FirstPersonController

public class MonsterBehaviour : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] int coinCount = 1;
    [SerializeField] float spawnSpread = 0.5f;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Transform player;
    [SerializeField] Transform respawnPoint;

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            SpawnCoins();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.transform.root.CompareTag("Player"))
        {
            Transform rootPlayer = collision.transform.root;

            if (respawnPoint == null)
            {
                Debug.LogError("Respawn point not assigned!");
                return;
            }

            // Try to get the FirstPersonController component on the root player
            FirstPersonController controller = rootPlayer.GetComponent<FirstPersonController>();
            if (controller != null)
            {
                // Use the teleport request method
                controller.RequestTeleport(respawnPoint.position);
            }
            else
            {
                // If no controller, fallback to directly setting position (less recommended)
                Rigidbody playerRb = rootPlayer.GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    playerRb.linearVelocity = Vector3.zero;
                    playerRb.angularVelocity = Vector3.zero;
                }
                rootPlayer.position = respawnPoint.position;
            }

            Debug.Log("Player touched monster and was teleported.");
        }
    }

    void SpawnCoins()
    {
        for (int i = 0; i < coinCount; i++)
        {
            Vector3 offset = new Vector3(
                Random.Range(-spawnSpread, spawnSpread),
                0.5f,
                Random.Range(-spawnSpread, spawnSpread)
            );
            Instantiate(coinPrefab, transform.position + offset, Quaternion.identity);
        }
    }
}
