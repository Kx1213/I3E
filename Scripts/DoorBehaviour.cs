using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] private Transform doorMesh; 

    private bool isOpen = false;
    private float openAngle = 90f;
    private float closedAngle = 0f;

    public void Interact()
    {
        Debug.Log("[DoorBehaviour] Interact called!");

        Vector3 doorRotation = doorMesh.localEulerAngles;

        if (!isOpen)
        {
            doorRotation.y = closedAngle + openAngle;
        }
        else
        {
            doorRotation.y = closedAngle;
        }

        doorMesh.localEulerAngles = doorRotation;
        isOpen = !isOpen;

        Debug.Log("[DoorBehaviour] New door rotation: " + doorMesh.localEulerAngles);
    }
}
