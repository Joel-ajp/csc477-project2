using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    [SerializeField] private float raiseDistance = 2f;
    [SerializeField] private float raiseSpeed = 1f;
    
    [Header("Audio")]
    [SerializeField] private AudioClip doorOpenSound;
    
    private AudioSource audioSource;
    private bool doorIsRaised = false;
    private Vector3 startPos;
    
    private void Start()
    {
        startPos = transform.position;
        
        // Add AudioSource component if it doesn't exist
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1.0f; // Make sound fully 3D
        }
    }
    
    public void RaiseDoor()
    {
        if (!doorIsRaised)
        {
            // Play door opening sound
            if (doorOpenSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(doorOpenSound);
            }
            
            // Start the door movement
            StartCoroutine(RaiseDoorRoutine());
            doorIsRaised = true;
        }
    }
    
    private IEnumerator RaiseDoorRoutine()
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
        
        // Make sure it's exactly at the top
        transform.position = targetPos;
    }
}