using UnityEngine;

public class HintSystem : MonoBehaviour
{
    public GameObject[] popups; 

    private int currentPopup = 0;

    private void Start()
    {
  
        foreach (var popup in popups)
        {
            popup.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            ShowPopup(currentPopup); 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            foreach (var popup in popups)
            {
                popup.SetActive(false); 
            }
        }
    }

    private void Update()
    {
        if (popups[currentPopup].activeSelf) 
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                currentPopup++;
                if (currentPopup >= popups.Length)
                {
                    currentPopup = 0; 
                }
                ShowPopup(currentPopup);
            }
        }
    }

    void ShowPopup(int index)
    {
        foreach (var popup in popups)
        {
            popup.SetActive(false); 
        }
        popups[index].SetActive(true); 
    }
}