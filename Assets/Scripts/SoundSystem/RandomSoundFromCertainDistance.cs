using UnityEngine;

public class RandomSoundFromCertainDistance : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float maxSoundDistance = 30f;
    [SerializeField] private float minSoundDistance = 5f;
    [SerializeField] private AnimationCurve volumeCurve = 
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    
    private Transform _player;
    private AudioSource _audioSource;
    private bool _wasInRange = false;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1f; // 3D sound
        _audioSource.loop = true; // WAŻNE: zapętla dźwięk
    }

    private void Update()
    {
        if (_player == null || audioClips.Length == 0) return;

        float distance = Vector3.Distance(transform.position, _player.position);
        bool isInRange = distance <= maxSoundDistance;
        
        if (isInRange)
        {
            // Oblicz głośność
            float normalizedDistance = Mathf.Clamp01(distance / maxSoundDistance);
            float volume = volumeCurve.Evaluate(1f - normalizedDistance);
            
            // Zacznij odtwarzać TYLKO jeśli wcześniej nie było w zasięgu
            if (!_wasInRange)
            {
                PlayRandomSound();
            }
            
            // Zawsze aktualizuj głośność
            _audioSource.volume = volume;
        }
        else
        {
            // Zatrzymaj dźwięk jeśli gracz wyszedł z zasięgu
            if (_wasInRange && _audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }
        
        _wasInRange = isInRange;
    }

    private void PlayRandomSound()
    {
        if (audioClips.Length == 0) return;
        
        AudioClip clipToPlay = audioClips[Random.Range(0, audioClips.Length)];
        _audioSource.clip = clipToPlay;
        _audioSource.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxSoundDistance);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minSoundDistance);
    }
}