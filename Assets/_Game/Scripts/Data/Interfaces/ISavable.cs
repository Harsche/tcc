public interface ISavable{
    void SaveState();
    void LoadState();
    void SubscribeToSave();
}