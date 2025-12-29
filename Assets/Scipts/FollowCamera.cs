using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    void LateUpdate()
    {
        transform.position = cameraTransform.position;
    }
}
