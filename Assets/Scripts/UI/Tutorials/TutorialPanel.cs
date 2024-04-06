using UnityEngine;

namespace UI.Tutorials
{
    public class TutorialPanel : MonoBehaviour
    {
        private float _originalTimeScale;
        // Start is called before the first frame update
        void Start()
        {
            _originalTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }

        public void ClosePanel()
        {
            Time.timeScale = _originalTimeScale;
            Destroy(gameObject);
        }
    }
}
