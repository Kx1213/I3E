using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    private bool isOpen = false;
    private float openAngle = 90f;
    private float closedAngle = 0f;

    public void Interact()
    {
        Vector3 doorRotation = transform.eulerAngles;

        if (!isOpen)
        {
            doorRotation.y += openAngle;
        }
        else
        {
            doorRotation.y -= openAngle;
        }

        transform.eulerAngles = doorRotation;
        isOpen = !isOpen;
    }
}
