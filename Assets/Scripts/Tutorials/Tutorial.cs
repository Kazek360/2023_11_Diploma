using System;
using Newtonsoft.Json;

namespace Tutorials
{
    public class Tutorial
    {
        [NonSerialized] public TutorialInfo info;
        [JsonProperty] public bool hasTriggered;

        public Tutorial(TutorialInfo tutorialInfo)
        {
            info = tutorialInfo;
            hasTriggered = false;
        }
    }
}