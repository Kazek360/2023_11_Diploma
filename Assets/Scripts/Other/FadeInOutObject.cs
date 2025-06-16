using System.Collections;
using UnityEngine;

namespace Other
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FadeInOutObject: MonoBehaviour
    {
        [SerializeField] private float transitionSpeed;
        [SerializeField] private bool notVisibleOnStart;
        private SpriteRenderer _spriteRenderer;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            if (notVisibleOnStart)
            {
                var color = _spriteRenderer.color;
                color.a = 0;
                _spriteRenderer.color = color;
            }
        }
        
        public IEnumerator FadeIn()
        {
            while (_spriteRenderer.color.a < 1)
            {
                var color = _spriteRenderer.color;
                color.a += transitionSpeed * Time.deltaTime;
                _spriteRenderer.color = color;
                yield return null;
            }
        }
        
        public IEnumerator FadeOut()
        {
            while (_spriteRenderer.color.a > 0)
            {
                var color = _spriteRenderer.color;
                color.a -= transitionSpeed * Time.deltaTime;
                _spriteRenderer.color = color;
                yield return null;
            }
        }
    }
}