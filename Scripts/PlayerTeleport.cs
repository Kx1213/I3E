using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerTeleport : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.transform.position = new Vector3(-0.07f, 0.35f, -7.16f);
            Debug.Log("TELE LOL");
        }
    }
}
