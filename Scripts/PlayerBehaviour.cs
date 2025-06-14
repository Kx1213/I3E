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

    void Update()
    {
        Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.green);

        RaycastHit hitInfo;

        // Raycast to detect interactable objects (door or collectible)
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            GameObject hitObject = hitInfo.collider.gameObject;

            if (hitObject.CompareTag("Collectible"))
            {
                if (currentCoin != null && currentCoin != hitObject.GetComponent<CoinBehaviour>())
                {
                    currentCoin.UnHighlight();
                }

                canInteract = true;
                currentCoin = hitObject.GetComponent<CoinBehaviour>();
                currentCoin.Highlight();

                currentDoor = null;  // Clear door if previously targeted

                Debug.Log("[Raycast] Coin detected and highlighted");
            }
            else if (hitObject.CompareTag("Door"))
            {
                canInteract = true;
                currentDoor = hitObject.GetComponent<DoorBehaviour>();

                if (currentCoin != null)
                {
                    currentCoin.UnHighlight();
                    currentCoin = null;
                }

                Debug.Log("[Raycast] Door detected in front");
            }
            else
            {
                ClearInteraction();
            }
        }
        else
        {
            ClearInteraction();
        }

        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[Update] 'E' pressed");
            OnInteract();
        }
    }

    void OnFire()
    {
        Debug.Log("[OnFire] Firing projectile");
        GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        Vector3 fireForce = spawnPoint.forward * FireStrength;
        newProjectile.GetComponent<Rigidbody>().AddForce(fireForce);
    }


    void ClearInteraction()
    {
        canInteract = false;

        if (currentCoin != null)
        {
            currentCoin.UnHighlight();
            currentCoin = null;
        }

        if (currentDoor != null)
        {
            currentDoor = null;
        }
    }

    void OnInteract()
    {
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

}