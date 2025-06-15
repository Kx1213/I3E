using UnityEngine;
using TMPro;

public class PlayerBehaviour : MonoBehaviour
{
    int score = 0; //Ingredients collected
    int currentHealth = 100;
    int maxHealth = 100;

    bool canInteract = false;
    CoinBehaviour currentCoin;//valuue for ingredients
    DoorBehaviour currentDoor;

    [SerializeField] GameObject projectile;//put the bullets here
    [SerializeField] Transform spawnPoint;//put the spawnpoint of ur bullets here
    [SerializeField] float FireStrength = 0f;
    [SerializeField] float interactionDistance = 6f;
    [SerializeField] TextMeshProUGUI scoreText;//text for score
    [SerializeField] TextMeshProUGUI victoryText; //text for victory

    void Start()
    {
        scoreText.text = "Ingredients Collected: " + score;//count the score
        victoryText.gameObject.SetActive(false);  //hide it first
    }

    void Update()
    {
        HandleRaycast();

        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[Update] 'E' pressed");
            OnInteract();
        }
    }

    void OnFire()
    {
        Debug.Log("[OnFire] Firing projectile");

        if (projectile == null || spawnPoint == null)
        {
            Debug.LogWarning("[OnFire] Missing projectile or spawnPoint!");
            return;
        }

        GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        Vector3 fireForce = spawnPoint.forward * FireStrength;
        Rigidbody rb = newProjectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(fireForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("[OnFire] No Rigidbody on projectile!");
        }
    }

    void HandleRaycast()
    {
        canInteract = false;
        currentCoin = null;
        currentDoor = null;

        Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.green);

        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out RaycastHit hitInfo, interactionDistance))
        {
            GameObject hitObj = hitInfo.collider.gameObject;
            Debug.Log("[Raycast] Hit: " + hitObj.name);

            if (hitObj.CompareTag("Collectible"))
            {
                currentCoin = hitObj.GetComponent<CoinBehaviour>();
                if (currentCoin != null)
                {
                    canInteract = true;
                }
            }
            else if (hitObj.CompareTag("Door"))
            {
                currentDoor = hitObj.GetComponent<DoorBehaviour>();
                if (currentDoor != null)
                {
                    canInteract = true;
                }
            }
        }
    }

    void OnInteract()//yes you can tell i really struggles to make the door works somehow
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
        }
        else
        {
            Debug.Log("[Interact] Nothing to interact with");
        }
    }

    public void ModifyScore(int amount)
    {
        score += amount;
        scoreText.text = "Ingredients Collected: " + score;
        Debug.Log("[Score] Modified. New score: " + score);

        if (score >= 10)
        {
            victoryText.text = "You made an Egg Fried Rice!";
            victoryText.gameObject.SetActive(true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("[CollisionEnter] Collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Hazard"))
        {
            currentHealth -= 100;
            Debug.Log("[CollisionEnter] Hit hazard. Health: " + currentHealth);
            if (currentHealth < 1)
            {
                Debug.Log("[CollisionEnter] Player is dead");
            }
        }
    }

    void OnCollisionStay(Collision collision)//didnt use it but you know if it aint wrong dont touch it
    {
        if (collision.gameObject.CompareTag("HealingArea") && currentHealth < maxHealth)
        {
            currentHealth++;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            Debug.Log("[Healing] Health restored. Current health: " + currentHealth);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("[TriggerEnter] Entered: " + other.gameObject.name);

        if (other.CompareTag("Collectible"))
        {
            currentCoin = other.GetComponent<CoinBehaviour>();
            canInteract = currentCoin != null;
        }
        else if (other.CompareTag("Door"))
        {
            currentDoor = other.GetComponent<DoorBehaviour>();
            canInteract = currentDoor != null;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("[TriggerExit] Exited: " + other.gameObject.name);

        if (other.CompareTag("Collectible") && currentCoin != null && other.gameObject == currentCoin.gameObject)
        {
            currentCoin = null;
            canInteract = false;
        }
        else if (other.CompareTag("Door") && currentDoor != null && other.gameObject == currentDoor.gameObject)
        {
            currentDoor = null;
            canInteract = false;
        }
    }
}
