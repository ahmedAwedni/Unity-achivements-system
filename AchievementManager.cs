// 3. AchievementManager.cs
using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour, ISaveable
{
    public static AchievementManager Instance { get; private set; }

    // Event for your UI to listen to
    public static event Action<AchievementData> OnAchievementUnlocked;

    [Header("Database")]
    public List<AchievementData> allAchievements;

    // Runtime Tracking
    private Dictionary<string, int> achievementProgress = new Dictionary<string, int>();
    private HashSet<string> unlockedAchievements = new HashSet<string>();

    // --- ISaveable Implementation ---
    public string SaveID => "Global_Achievements";

    [System.Serializable]
    private struct AchievementSaveData
    {
        public List<string> savedUnlockedAchievements;
        public List<string> progressKeys;
        public List<int> progressValues;
    }

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
        GameEvents.OnProgressMade += HandleProgress;
        GameEvents.OnInstantUnlock += UnlockAchievement;
    }

    private void OnDisable()
    {
        GameEvents.OnProgressMade -= HandleProgress;
        GameEvents.OnInstantUnlock -= UnlockAchievement;
    }

    private void HandleProgress(string eventID, int amount)
    {
        foreach (AchievementData achievement in allAchievements)
        {
            if (achievement.progressEventID == eventID && !unlockedAchievements.Contains(achievement.achievementID))
            {
                if (!achievementProgress.ContainsKey(achievement.achievementID))
                {
                    achievementProgress[achievement.achievementID] = 0;
                }

                achievementProgress[achievement.achievementID] += amount;

                if (achievementProgress[achievement.achievementID] >= achievement.targetGoal)
                {
                    UnlockAchievement(achievement.achievementID);
                }
            }
        }
    }

    private void UnlockAchievement(string achievementID)
    {
        if (unlockedAchievements.Contains(achievementID)) return;

        AchievementData achievement = allAchievements.Find(a => a.achievementID == achievementID);
        
        if (achievement != null)
        {
            unlockedAchievements.Add(achievementID);
            Debug.Log($"🏆 Achievement Unlocked: {achievement.title}");
            
            OnAchievementUnlocked?.Invoke(achievement);
        }
    }

    // --- Save & Load Logic ---
    public string SaveState()
    {
        // Pack Dictionaries and HashSets into Lists so JsonUtility can read them
        AchievementSaveData data = new AchievementSaveData
        {
            savedUnlockedAchievements = new List<string>(unlockedAchievements),
            progressKeys = new List<string>(achievementProgress.Keys),
            progressValues = new List<int>(achievementProgress.Values)
        };

        return JsonUtility.ToJson(data);
    }

    public void LoadState(string stateJson)
    {
        AchievementSaveData data = JsonUtility.FromJson<AchievementSaveData>(stateJson);

        // Restore the unlocked HashSet
        unlockedAchievements = new HashSet<string>(data.savedUnlockedAchievements);

        // Restore the progress Dictionary
        achievementProgress.Clear();
        for (int i = 0; i < data.progressKeys.Count; i++)
        {
            achievementProgress.Add(data.progressKeys[i], data.progressValues[i]);
        }

        Debug.Log("Achievement data loaded successfully.");
    }
}
