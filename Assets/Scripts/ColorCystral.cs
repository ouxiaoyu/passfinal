using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCystral : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "MovableCube")
        {
            gameObject.SetActive(false);
        }
    }
}
