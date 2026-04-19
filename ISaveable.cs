public interface ISaveable
{
    // A unique identifier for the object
    string SaveID { get; }
    
    // Serializes the specific script's data into a JSON string
    string SaveState();
    
    // Deserializes the JSON string back into the script's variables
    void LoadState(string stateJson);
}
