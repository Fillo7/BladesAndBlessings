using System.Collections.Generic;
using UnityEngine;

public class LoadoutController : MonoBehaviour
{
    [SerializeField] private List<GameObject> weaponDescriptions = new List<GameObject>();
    [SerializeField] private GameObject errorPanel;

    private LinkedList<int> selectedWeapons = new LinkedList<int>();

    void Awake()
    {
        ActivateDescription(0);
    }

    public bool ToggleWeapon(int index)
    {
        if (selectedWeapons.Contains(index))
        {
            selectedWeapons.Remove(index);
            return false;
        }
        else
        {
            selectedWeapons.AddLast(index);
            return true;
        }
    }

    public void ActivateDescription(int index)
    {
        ResetDescriptions();
        weaponDescriptions[index].SetActive(true);
    }

    public void StartGame()
    {
        if (selectedWeapons.Count != 2)
        {
            errorPanel.SetActive(true);
            return;
        }

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().StartGame(selectedWeapons.First.Value, selectedWeapons.Last.Value);
    }

    private void ResetDescriptions()
    {
        foreach (GameObject item in weaponDescriptions)
        {
            item.SetActive(false);
        }
    }
}
