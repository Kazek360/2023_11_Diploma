using System;

namespace Collectibles
{
    public static class CollectibleEvents
    {
        public static event Action<string> OnCollectiblePickup;
        public static void PickupCollectible(string id)
        {
            OnCollectiblePickup?.Invoke(id);
        }
    }
}