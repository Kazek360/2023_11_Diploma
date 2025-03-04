using System;
using SaveSystem.Data;

namespace SaveSystem
{
    public static class SaveSystemEvents
    {
        public static event Action OnNewGame;
        public static void InvokeOnNewGame()
        {
            OnNewGame?.Invoke();
        }
        public static event Action<GameData> OnFromFileLoad;
        public static void InvokeOnFromFileLoad(GameData data)
        {
            OnFromFileLoad?.Invoke(data);
        }
        public static event Action<GameData> OnBetweenScenesLoad;
        public static void InvokeOnBetweenScenesLoad(GameData data)
        {
            OnBetweenScenesLoad?.Invoke(data);
        }
    }
}