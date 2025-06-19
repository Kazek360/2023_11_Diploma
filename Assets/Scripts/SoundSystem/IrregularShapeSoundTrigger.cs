using UnityEngine;

public class IrregularShapeSoundTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AnimationCurve volumeCurve = 
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private Transform volumeCenter; // Punkt odniesienia dla głośności
    [SerializeField] private float maxVolumeDistance = 5f; // Maksymalna odległość dla pełnej głośności
    
    private Transform _player;
    private AudioSource _audioSource;
    private bool _playerInTrigger = false;
    private Collider _triggerCollider;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1f;
        _audioSource.loop = true;

        // Znajdź collider oznaczony jako trigger
        _triggerCollider = GetComponent<Collider>();
        if (_triggerCollider != null)
        {
            _triggerCollider.isTrigger = true;
        }

        // Jeśli nie określono centrum, użyj pozycji obiektu
        if (volumeCenter == null)
        {
            volumeCenter = transform;
        }
    }

    private void Update()
    {
        if (_playerInTrigger && _player != null)
        {
            // Oblicz głośność na podstawie odległości od centrum
            float distance = Vector3.Distance(volumeCenter.position, _player.position);
            float normalizedDistance = Mathf.Clamp01(distance / maxVolumeDistance);
            float volume = volumeCurve.Evaluate(1f - normalizedDistance);
            
            _audioSource.volume = volume;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInTrigger = true;
            PlayRandomSound();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInTrigger = false;
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }
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
        if (volumeCenter != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(volumeCenter.position, maxVolumeDistance);
        }
    }
}