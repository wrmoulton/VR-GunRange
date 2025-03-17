using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class TacticalTargetManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> breakables; // List of all "Breakable" targets

    public void ActivateRoundTargets(int quota)
    {
        if (breakables == null || breakables.Count == 0)
        {
            Debug.LogWarning("No breakables assigned to TacticalTargetManager.");
            return;
        }

        // Shuffle the list to get random targets
        List<GameObject> shuffledBreakables = new List<GameObject>(breakables);
        for (int i = 0; i < shuffledBreakables.Count; i++)
        {
            int randomIndex = Random.Range(0, shuffledBreakables.Count);
            GameObject temp = shuffledBreakables[i];
            shuffledBreakables[i] = shuffledBreakables[randomIndex];
            shuffledBreakables[randomIndex] = temp;
        }

        // Activate the specified number of targets
        int activatedCount = 0;
        foreach (GameObject breakable in shuffledBreakables)
        {
            if (activatedCount >= quota) break;

            if (breakable != null)
            {
                Damageable damageable = breakable.GetComponent<Damageable>();
                if (damageable != null)
                {
                    if (!breakable.activeSelf)
                    {
                        // Activate the target
                        breakable.SetActive(true);
                    }
                    else
                    {
                        // Toggle the respawn property, wait, then untoggle
                        Debug.Log("Toggled Respawn");
                        
                        // Spawn object
                        // if (damageable.SpawnOnDeath != null) {
                        //     var go = GameObject.Instantiate(damageable.SpawnOnDeath);
                        //     go.transform.position = transform.position;
                        //     go.transform.rotation = transform.rotation;
                        // }

                        damageable.Health = damageable.getStartingHealth();
                        damageable.resetDestroyed();
                        // Deactivate
                        foreach (var go in damageable.ActivateGameObjectsOnDeath) {
                            go.SetActive(false);
                        }

                        // Re-Activate
                        foreach (var go in damageable.DeactivateGameObjectsOnDeath) {
                            go.SetActive(true);
                        }
                        foreach (var col in damageable.DeactivateCollidersOnDeath) {
                            col.enabled = true;
                        }
                        Rigidbody rigid = damageable.getRigid();
                        if (rigid) {
                            rigid.isKinematic = damageable.initalKinematic();
                        }
                    }

                    activatedCount++;
                }
                else
                {
                    Debug.LogWarning($"Breakable {breakable.name} does not have a Damageable component.");
                }
            }
        }
    }
    public int getBreakablesCount()
    {
        return breakables.Count;
    }

    // private IEnumerator ToggleRespawnTemporarily(Damageable damageable)
    // {
    //     damageable.Respawn = !damageable.Respawn; // Toggle Respawn
    //     yield return new WaitForSeconds(0.0055f);  // Wait for 0.005 seconds
    //     damageable.Respawn = !damageable.Respawn; // Untoggle Respawn
    // }
}
