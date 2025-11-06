using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollHandler : MonoBehaviour
{
    private List<Rigidbody> _rigidbodies;
    private List<Collider> _colliders;
    public void Initialize()
    {
        _rigidbodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        _colliders = new List<Collider>(GetComponentsInChildren<Collider>());
        Disable();
    }

    // Update is called once per frame
    public void Enable()
    {
        foreach (Rigidbody rigidbody in _rigidbodies)
            rigidbody.isKinematic = false;
        foreach (Collider collider in _colliders)
            collider.enabled = true;
    }

    public void Disable()
    {
        foreach (Rigidbody rigidbody in _rigidbodies)
            rigidbody.isKinematic = true;
        foreach (Collider collider in _colliders)
            collider.enabled = false;
    }
}
 