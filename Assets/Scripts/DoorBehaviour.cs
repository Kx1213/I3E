using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public Transform door; 
    public float openAngle = 90f; // door rotates 90 degree when open
    public float closedAngle = 0f; // door close
    public float rotationSpeed = 2f; // how fast it rotates

    private bool isOpen = false; //add this then it wont keep turning 90degree
    private bool playerNear = false;

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen; 
        }

        float targetAngle = isOpen ? openAngle : closedAngle;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        door.rotation = Quaternion.Slerp(door.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    // when player walk near
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            Debug.Log("Player near door. Press E to open/close.");
        }
    }

    // when player walk away
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            Debug.Log("Player left door area.");
        }
    }
}
