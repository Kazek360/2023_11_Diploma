using System;

namespace Combat
{
    public static class CombatEvents
    {
        public static event Action OnTurnEnd;
        public static void InvokeOnTurnEnd()
        {
            OnTurnEnd?.Invoke();
        }
    }
}