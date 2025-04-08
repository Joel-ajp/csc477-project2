using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private float raiseDistance = 2f;
    [SerializeField] private float raiseSpeed = 1f;

    private bool doorIsRaised = false;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    public void RaiseDoor()
    {
        if (!doorIsRaised)
        {
            // One option: simple coroutine that lerps the door upward
            StartCoroutine(RaiseDoorRoutine());
            doorIsRaised = true;
        }
    }

    private System.Collections.IEnumerator RaiseDoorRoutine()
    {
        Vector3 targetPos = startPos + Vector3.up * raiseDistance;
        float elapsed = 0f;
        float duration = raiseDistance / raiseSpeed;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // Make sure it’s exactly at the top
        transform.position = targetPos;
    }
}
