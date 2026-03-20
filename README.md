# Unity Event-Driven Achievement System

A decoupled, scalable Achievement and Trophy system for Unity. Built using a Global Event Bus and **ScriptableObjects**, this framework allows you to track progress and unlock achievements without tangling your core gameplay code with UI or manager references.

---

## ✨ Features

* **Event-Bus Architecture:** Gameplay scripts simply broadcast events (e.g., "Player Jumped") into the void. The Manager listens and calculates the rest.
* **ScriptableObject Blueprints:** Define achievement titles, descriptions, icons, and target goals entirely in the Unity Editor.
* **Multi-Step Progression:** Easily support one-off achievements ("Find the hidden sword") or cumulative goals ("Kill 100 Goblins").
* **Zero Dependencies:** No hard-coded Singletons forced into your player or enemy scripts, making your codebase much cleaner and less prone to errors.
* **Save-System Ready:** Runtime progress is tracked in a simple C# "Dictionary" and "HashSet", making it incredibly easy to serialize into JSON for saving and loading.

---

## 🧠 Design Notes

In standard game development, a common mistake is putting a reference to the "AchievementManager" directly inside the "Player" script. This creates a tight coupling. If you delete the Achievement Manager, your Player script throws errors and breaks.

This system uses the **Observer Pattern**. We created a static "GameEvents" class. When a player collects a coin, the player script triggers "GameEvents.OnProgressMade". That's all it does. The "AchievementManager" passively listens to that event, checks its database of "AchievementData" ScriptableObjects, does the math, and fires off an unlock event to the UI if a goal is met.

---

## 📂 Included Scripts

* "GameEvents.cs" - A static event bus containing the C# Actions that act as the middleman between gameplay and the manager.
* "AchievementData.cs" - The ScriptableObject blueprint defining the ID, visual data, and numerical requirements for a single achievement.
* "AchievementManager.cs" - The persistent brain of the system that tracks progress dictionaries and validates unlocks.
* "EnemyExample.cs" - A simple example script demonstrating how an object triggers an achievement event without requiring any references.

---

## 🧩 How To Use

1. **Create an Achievement:** Right-click in your Project window: "Create > Achievement System > Achievement". Fill in the details. Set the "progressEventID" to something memorable (like "jump").
2. **Setup the Manager:** Attach the "AchievementManager.cs" script to a persistent GameObject in your scene. Add your new ScriptableObject to its "All Achievements" list.
3. **Trigger Progress in Game:** From any script (like your player controller), invoke the global event:

"""
// Adds 1 to the progress of any achievement listening for "jump"
GameEvents.OnProgressMade?.Invoke("jump", 1);
"""

4. **Instant Unlocks:** If you want an achievement to unlock immediately without tracking progress:

"""
GameEvents.OnInstantUnlock?.Invoke("found_secret_room");
"""

5. **UI Updates:** Subscribe your canvas notification script to the manager's static action:

"""
AchievementManager.OnAchievementUnlocked += ShowPopupPanel;
"""


---

## 🚀 Possible Extensions

* **Save/Load Integration:** Combine this with an "ISaveable" interface. Save the "unlockedAchievements" list and "achievementProgress" dictionary to a JSON file so players keep their trophies.
* **Hidden Achievements:** Add a "bool isHidden" to the "AchievementData" so your UI scripts know to mask the title/description in the player's trophy menu until it is unlocked.
* **Reward Grants:** Add an "xpReward" or "itemReward" field to the ScriptableObject, and have the Manager grant the player those rewards upon unlocking.

---

## 🛠 Unity Version

* Tested in Unity6+ (should work without any problems in newer versions)

---

## 📜 License

MIT