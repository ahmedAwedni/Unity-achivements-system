// 1. GameEvents.cs
using System;

/// A global event bus. Any script can trigger these, and any script can listen to them.
/// This completely decouples your game logic from your achievement system.

public static class GameEvents
{
    // Pass a specific string ID and the amount of progress made
    public static Action<string, int> OnProgressMade;
    
    // Pass an exact achievement ID to unlock it instantly
    public static Action<string> OnInstantUnlock;
}