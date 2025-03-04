using SaveSystem.Data;

namespace SaveSystem.Interfaces
{
    public interface IFromFileLoadable
    {
        void LoadDataFromSaveFile(GameData gameData);
    }
}