using System;
using System.Collections;
using UnityEngine;

namespace Other
{
    public class Nestor : MonoBehaviour
    {
        public static event Action<string, bool> OnNestorStateChange;

        public static void ChangeNestorState(string nestorID, bool isAppearing)
        {
            OnNestorStateChange?.Invoke(nestorID, isAppearing);
        }
        
        [SerializeField] private string id;
        [SerializeField] private float disappearSpeed;
        [SerializeField] private bool notVisibleOnStart;
        private SpriteRenderer _spriteRenderer;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        private void OnEnable()
        {
            OnNestorStateChange += CheckID;
        }

        private void OnDisable()
        {
            OnNestorStateChange -= CheckID;
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

        private void CheckID(string nestorID, bool isAppearing)
        {
            if (nestorID.Equals(id))
            {
                if (isAppearing)
                {
                    StartCoroutine(AppearNestor());
                }
                else
                {
                    StartCoroutine(DisappearNestor());
                }
            }
        }

        private IEnumerator AppearNestor()
        {
            while (_spriteRenderer.color.a < 1)
            {
                var color = _spriteRenderer.color;
                color.a += disappearSpeed * Time.deltaTime;
                _spriteRenderer.color = color;
                yield return null;
            }
        }
        
        private IEnumerator DisappearNestor()
        {
            while (_spriteRenderer.color.a > 0)
            {
                var color = _spriteRenderer.color;
                color.a -= disappearSpeed * Time.deltaTime;
                _spriteRenderer.color = color;
                yield return null;
            }
        }
    }
}