// 4. EnemyExample.cs
using UnityEngine;

/// An example showing how an object interacts with the system without needing a reference to it.

public class EnemyExample : MonoBehaviour
{
    public void Die()
    {
        // Broadcast to the void that a slime died.
        // The enemy DOES NOT know or care if the AchievementManager exists.
        GameEvents.OnProgressMade?.Invoke("enemy_slime_killed", 1);
        
        Destroy(gameObject);
    }
}