using SaveSystem.Data;

namespace SaveSystem.Interfaces
{
    public interface ISavable
    {
        void SaveData(ref GameData gameData);
    }
}
