using UnityEngine;
using UnityEngine.XR;

public class PlayerColliderFollow : MonoBehaviour
{
    public Transform cameraTransform; // Assign your XR Camera here
    public CapsuleCollider cap;   // Assign your cap Collider here

    void Update()
    {
        // Keep the collider centered horizontally with the camera
        Vector3 newCenter = cap.center;
        Vector3 localHeadPos = transform.InverseTransformPoint(cameraTransform.position);
        newCenter.x = localHeadPos.x;
        newCenter.z = localHeadPos.z;

        cap.center = newCenter;
    }
}
