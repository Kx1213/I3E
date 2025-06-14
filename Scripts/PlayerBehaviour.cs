using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    int score = 0;
    int currentHealth = 10;
    int maxHealth = 100;

    bool canInteract = false;
    CoinBehaviour currentCoin;
    DoorBehaviour currentDoor;

    [SerializeField] GameObject projectile;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float FireStrength = 0f;
    [SerializeField] float interactionDistance = 3f;

    void OnFire()
    {
        Debug.Log("[OnFire] Firing projectile");
        GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        Vector3 fireForce = spawnPoint.forward * FireStrength;
        newProjectile.GetComponent<Rigidbody>().AddForce(fireForce);
    }

    void Update()
    {
        // Debug raycast line
        Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.green);

        // Raycast check
        RaycastHit hitInfo;
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            Debug.Log("[Raycast] Hit object: " + hitInfo.collider.gameObject.name);

            if (hitInfo.collider.gameObject.CompareTag("Collectible"))
            {
                if (currentCoin != null && currentCoin != hitInfo.collider.GetComponent<CoinBehaviour>())
                {
                    currentCoin.UnHighlight();
                }

                canInteract = true;
                currentCoin = hitInfo.collider.gameObject.GetComponent<CoinBehaviour>();
                currentCoin.Highlight();

                Debug.Log("[Raycast] Coin detected and highlighted");
            }
        }
        else if (currentCoin != null)
        {
            Debug.Log("[Raycast] No collectible in view, unhighlighting");
            currentCoin.UnHighlight();
            currentCoin = null;
        }

        // Interaction input
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[Update] 'E' pressed");
            OnInteract();
        }
    }

    public void ModifyScore(int amount)
    {
        score += amount;
        Debug.Log("[Score] Modified. New score: " + score);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("[CollisionEnter] Collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Collectible"))
        {
            ++score;
            Debug.Log("[CollisionEnter] Collected item. Score: " + score);
        }
        else if (collision.gameObject.CompareTag("Hazard"))
        {
            currentHealth -= 35;
            Debug.Log("[CollisionEnter] Hit hazard. Health: " + currentHealth);
            if (currentHealth < 1)
            {
                Debug.Log("[CollisionEnter] Player is dead");
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("HealingArea"))
        {
            if (currentHealth < maxHealth)
            {
                currentHealth++;
                if (currentHealth > maxHealth) currentHealth = maxHealth;
                Debug.Log("[Healing] Health restored. Current health: " + currentHealth);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("[TriggerEnter] Entered: " + other.gameObject.name);

        if (other.CompareTag("Collectible"))
        {
            canInteract = true;
            currentCoin = other.GetComponent<CoinBehaviour>();
            Debug.Log("[TriggerEnter] Coin in range: " + currentCoin?.name);
        }
        else if (other.CompareTag("Door"))
        {
            canInteract = true;
            currentDoor = other.GetComponent<DoorBehaviour>();
            Debug.Log("[TriggerEnter] Door in range: " + currentDoor?.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("[TriggerExit] Exited: " + other.gameObject.name);

        if (other.CompareTag("Collectible"))
        {
            if (currentCoin != null && other.gameObject == currentCoin.gameObject)
            {
                canInteract = false;
                currentCoin = null;
                Debug.Log("[TriggerExit] Left collectible range");
            }
        }
        else if (other.CompareTag("Door"))
        {
            if (currentDoor != null && other.gameObject == currentDoor.gameObject)
            {
                canInteract = false;
                currentDoor = null;
                Debug.Log("[TriggerExit] Left door range");
            }
        }
    }

    void OnInteract()
    {
        Debug.Log("[Interact] Attempting interaction");

        if (currentCoin != null)
        {
            Debug.Log("[Interact] Interacting with coin");
            currentCoin.Collect(this);
        }
        else if (currentDoor != null)
        {
            Debug.Log("[Interact] Interacting with door");
            currentDoor.Interact();
        }
        else
        {
            Debug.Log("[Interact] Nothing to interact with");
        }
    }
}

