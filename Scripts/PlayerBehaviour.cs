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
        Debug.Log("bro");
        GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        Vector3 fireForce = spawnPoint.forward * FireStrength;
        newProjectile.GetComponent<Rigidbody>().AddForce(fireForce);
    }

    void Update()
    {
        // Raycast checking
        RaycastHit hitInfo;
        Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.green);
        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            if (hitInfo.collider.gameObject.CompareTag("Collectible"))
            {
                if (currentCoin != null && currentCoin != hitInfo.collider.GetComponent<CoinBehaviour>())
                {
                    currentCoin.UnHighlight();
                }

                canInteract = true;
                currentCoin = hitInfo.collider.gameObject.GetComponent<CoinBehaviour>();
                currentCoin.Highlight();

                Debug.Log("bro did u hit");
            }
        }
        else if (currentCoin != null)
        {
            currentCoin.UnHighlight();
            currentCoin = null;
        }

        // Interaction with coin
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            if (currentCoin != null)
            {
                currentCoin.Collect(this);
                canInteract = false;
            }
        }
    }

    public void ModifyScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Collectible"))
        {
            ++score;
            Debug.Log("Score: " + score);
        }
        else if (collision.gameObject.CompareTag("Hazard"))
        {
            currentHealth -= 35;
            Debug.Log("Health: " + currentHealth);
            if (currentHealth < 1)
            {
                Debug.Log("Player is dead");
                // Add death logic here
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
                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;

                Debug.Log("Health: " + currentHealth);
            }
        }
    }
        void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.CompareTag("Collectible"))
        {
            canInteract = true;
            currentCoin = other.GetComponent<CoinBehaviour>();
        }
        else if (other.CompareTag("Door"))
        {
            canInteract = true;
            currentDoor = other.GetComponent<DoorBehaviour>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            if (currentCoin != null && other.gameObject == currentCoin.gameObject)
            {
                canInteract = false;
                currentCoin = null;
                Debug.Log("Moved away from coin");
            }
        }
    }
    void OnInteract()
    {
        if (canInteract)
        {
            if (currentCoin != null)
            {
                Debug.Log("Interacting with coin");
                currentCoin.Collect(this);
            }
            else if (currentDoor != null)
            {
                Debug.Log("Interacting with door");
                currentDoor.Interact();
            }
        }
    }
}
