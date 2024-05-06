using SaveSystem.Data;

namespace SaveSystem
{
    public interface IDataPersistence
    {
        void LoadData(GameData gameData);
        void SaveData(ref GameData gameData);
    }

}
