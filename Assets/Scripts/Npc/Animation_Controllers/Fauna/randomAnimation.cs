using UnityEngine;

public class randomAnimation : MonoBehaviour
{

    [SerializeField] private string triggerName;
    [SerializeField] public float minDelay;
    [SerializeField] public float maxDelay;

    private Animator _animator;

    private float _timer;
    private float _nextTrigger;


    public void OnEnable()
    {
        _animator = GetComponent<Animator>();
        SetNextTriggerTime();
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _nextTrigger)
        {
            _animator.SetTrigger(triggerName);

            _timer = 0f;
            SetNextTriggerTime();
        }
    }

    private void SetNextTriggerTime()
    {
        _nextTrigger = Random.Range(minDelay, maxDelay);
    }
}
