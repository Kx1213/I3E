using UnityEngine;
using StarterAssets; 

public class MonsterBehaviour : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;//put the ingredients here
    [SerializeField] int coinCount = 1;
    [SerializeField] float spawnSpread = 0.5f;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] Transform player;//put the player here
    [SerializeField] Transform respawnPoint;//after player died teleport to here

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;//monster walk towards player
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))//shoots the monster
        {
            SpawnCoins();//spawn ingredients
            Destroy(collision.gameObject);//then die
            Destroy(gameObject);
        }
        else if (collision.transform.root.CompareTag("Player"))//attacked by monster
        {
            Transform rootPlayer = collision.transform.root;//you die

            if (respawnPoint == null)
            {
                Debug.LogError("Respawn point not assigned!");
                return;
            }

            FirstPersonController controller = rootPlayer.GetComponent<FirstPersonController>();//send you back to the spawnpoint
            if (controller != null)
            {
                controller.RequestTeleport(respawnPoint.position);
            }
            else
            {
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
