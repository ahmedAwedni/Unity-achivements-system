// 3. AchievementManager.cs
using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    // Event for your UI to listen to
    public static event Action<AchievementData> OnAchievementUnlocked;

    [Header("Database")]
    public List<AchievementData> allAchievements;

    // Runtime Tracking (Easy to serialize for a Save System later)
    private Dictionary<string, int> achievementProgress = new Dictionary<string, int>();
    private HashSet<string> unlockedAchievements = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        // Subscribe to the global event bus
        GameEvents.OnProgressMade += HandleProgress;
        GameEvents.OnInstantUnlock += UnlockAchievement;
    }

    private void OnDisable()
    {
        // Always unsubscribe to prevent memory leaks
        GameEvents.OnProgressMade -= HandleProgress;
        GameEvents.OnInstantUnlock -= UnlockAchievement;
    }

    private void HandleProgress(string eventID, int amount)
    {
        // Find all achievements that care about this specific event
        foreach (AchievementData achievement in allAchievements)
        {
            if (achievement.progressEventID == eventID && !unlockedAchievements.Contains(achievement.achievementID))
            {
                // Initialize progress tracking if it doesn't exist yet
                if (!achievementProgress.ContainsKey(achievement.achievementID))
                {
                    achievementProgress[achievement.achievementID] = 0;
                }

                // Add progress
                achievementProgress[achievement.achievementID] += amount;

                // Check for completion
                if (achievementProgress[achievement.achievementID] >= achievement.targetGoal)
                {
                    UnlockAchievement(achievement.achievementID);
                }
            }
        }
    }

    private void UnlockAchievement(string achievementID)
    {
        if (unlockedAchievements.Contains(achievementID)) return; // Already unlocked

        AchievementData achievement = allAchievements.Find(a => a.achievementID == achievementID);
        
        if (achievement != null)
        {
            unlockedAchievements.Add(achievementID);
            Debug.Log($"🏆 Achievement Unlocked: {achievement.title}");
            
            // Tell the UI to show the popup!
            OnAchievementUnlocked?.Invoke(achievement);
        }
    }
}