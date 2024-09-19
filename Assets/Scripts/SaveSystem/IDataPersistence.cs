using SaveSystem.Data;

namespace SaveSystem
{
    public interface IDataPersistence
    {
        void InitializeOnNewGame();
        void LoadDataFromSaveFile(GameData gameData);
        void LoadDataBetweenScenes(GameData gameData);
        void SaveData(ref GameData gameData);
    }

}
