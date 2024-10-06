using System;

namespace Collectibles
{
    public static class CollectibleEvents
    {
        public static event Action<int> OnCollectiblePickup;
        public static void PickupCollectible(int id)
        {
            OnCollectiblePickup?.Invoke(id);
        }
    }
}