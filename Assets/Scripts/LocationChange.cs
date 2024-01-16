using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationChange : MonoBehaviour
{
    
    [SerializeField] private string sceneToLoad;
    private bool _canInteract;

    private void Start()
    {
        _canInteract = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _canInteract)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _canInteract = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _canInteract = false;
    }
}
