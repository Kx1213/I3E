using UnityEngine;

public class DoorBehaviour1 : MonoBehaviour
{
    public Transform door; 
    public float openAngle = 270f; // door rotates 90 degree when open
    public float closedAngle = 180f; // door close
    public float rotationSpeed = 2f; // speed of rotation

    private bool isOpen = false;
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

    // when player enters trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            Debug.Log("Player near door. Press E to open/close.");
        }
    }

    // when player leaves trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            Debug.Log("Player left door area.");
        }
    }
}
