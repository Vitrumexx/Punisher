using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotate : MonoBehaviour
{
    public Transform headTarget;
    private Vector3 normalPos = new Vector3(-0.015f, 0f, 1.6f);

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HeadRotation(other.transform);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetHeadRotation();
        }
    }

    void HeadRotation(Transform player)
    {
        headTarget.position = new Vector3(player.position.x, player.position.y + 4, player.position.z);
    }

    void ResetHeadRotation()
    {
        headTarget.position = normalPos;
    }
}
