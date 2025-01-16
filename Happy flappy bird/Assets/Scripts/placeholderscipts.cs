using UnityEngine;

public class Placeholderscripts : MonoBehaviour
{
    public Transform controllerTransform; // Koppla till antingen LeftHandAnchor eller RightHandAnchor

    void Update()
    {
        if (controllerTransform != null)
        {
            transform.position = controllerTransform.position;
            transform.rotation = controllerTransform.rotation;
        }
    }
}
