// 2. AchievementData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewAchievement", menuName = "Achievement System/Achievement")]
public class AchievementData : ScriptableObject
{
    [Header("Details")]
    public string achievementID; // e.g., "kill_10_slimes"
    public string title;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Requirements")]
    [Tooltip("The string ID the manager will listen for to track progress.")]
    public string progressEventID; 
    [Tooltip("How many times the event must occur to unlock this achievement.")]
    public int targetGoal = 1;
}