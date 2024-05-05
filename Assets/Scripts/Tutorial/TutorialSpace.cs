using UnityEngine;

public class TutorialSpace : MonoBehaviour
{
    public GameObject[] popups; // Przypisz tutaj obiekty UI podpowiedzi w inspektorze

    private int currentPopup = 0;
    private bool allPopupsSeen = false; // Dodano flagę do śledzenia, czy wszystkie popupy zostały obejrzane

    private void Start()
    {
        // Wyłącz wszystkie popupy na początku
        foreach (var popup in popups)
        {
            popup.SetActive(false);
        }
        // Pokaż pierwszy popup od razu po wejściu na scenę
        ShowPopup(currentPopup);
    }

    private void Update()
    {
        if (!allPopupsSeen && Input.GetKeyDown(KeyCode.Space))
        {
            currentPopup++;
            if (currentPopup >= popups.Length)
            {
                allPopupsSeen = true; // Ustaw flagę na true, gdy wszystkie popupy zostały obejrzane
                DisableAllPopups(); // Wywołaj funkcję wyłączającą wszystkie popupy
            }
            else
            {
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

    void DisableAllPopups()
    {
        foreach (var popup in popups)
        {
            popup.SetActive(false); // Wyłącz wszystkie popupy
        }
    }
}
