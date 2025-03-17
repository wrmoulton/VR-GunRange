using UnityEngine;

public class TriggerCollisionChecker : MonoBehaviour
{
    [SerializeField] private Collider triggerCollider;

    [SerializeField] private SceneTransitionManager SceneTransitionManager;

    private void Start()
    {
        // Get the Collider component attached to the same GameObject
        triggerCollider = GetComponent<Collider>();

        // Ensure the Collider is set as a trigger
        if (triggerCollider == null)
        {
            Debug.LogError("No Collider found on this GameObject!");
        }
        else if (!triggerCollider.isTrigger)
        {
            Debug.LogWarning("Collider is not set as a trigger. Setting it to a trigger.");
            triggerCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the "Player" tag
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player entered the trigger collider: {other.name}");
            SceneTransitionManager.SceneSwap();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object that exited the trigger has the "Player" tag
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player exited the trigger collider: {other.name}");
        }
    }
}
