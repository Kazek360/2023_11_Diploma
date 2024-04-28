using UnityEngine;

public class HintSystem : MonoBehaviour
{
    public GameObject[] popups; // Przypisz tutaj obiekty UI podpowiedzi w inspektorze

    private int currentPopup = 0;

    private void Start()
    {
        // Wyłącz wszystkie popupy na początku
        foreach (var popup in popups)
        {
            popup.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Upewnij się, że to gracz wszedł w collider
        {
            ShowPopup(currentPopup); // Pokaż pierwszy popup
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Upewnij się, że to gracz opuszcza collider
        {
            foreach (var popup in popups)
            {
                popup.SetActive(false); // Wyłącz wszystkie popupy
            }
        }
    }

    private void Update()
    {
        if (popups[currentPopup].activeSelf) // Sprawdź, czy jakikolwiek popup jest aktywny
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                currentPopup++;
                if (currentPopup >= popups.Length)
                {
                    currentPopup = 0; // Zresetuj na pierwszy popup, jeśli to konieczne
                }
                ShowPopup(currentPopup);
            }
        }
    }

    void ShowPopup(int index)
    {
        foreach (var popup in popups)
        {
            popup.SetActive(false); // Wyłącz wszystkie popupy
        }
        popups[index].SetActive(true); // Aktywuj tylko wybrany popup
    }
}