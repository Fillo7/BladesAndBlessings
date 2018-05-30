using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [SerializeField] bool active = true;
    [SerializeField] private Button newButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private List<Button> levelButtons = new List<Button>();
    [SerializeField] private Texture lockedWeaponTexture;
    [SerializeField] private List<GameObject> weaponImages = new List<GameObject>();

    private GameManager gameManager;
    private SaveData data = new SaveData();

    void Awake()
    {
        if (!active)
        {
            return;
        }

        gameManager = GetComponent<GameManager>();
        Load();

        if (newButton != null)
        {
            newButton.onClick.AddListener(LoadFirstLevel);
        }

        if (data.GetTutorialSeen() && continueButton != null)
        {
            continueButton.interactable = true;
            continueButton.onClick.AddListener(LoadCurrentLevel);
        }

        for (int i = data.GetUnlockedLevelCount(); i < levelButtons.Count; i++)
        {
            levelButtons[i].interactable = false;
            levelButtons[i].GetComponentInChildren<Text>().text = "???";
        }

        for (int i = data.GetUnlockedWeaponCount(); i < weaponImages.Count; i++)
        {
            weaponImages[i].GetComponent<WeaponSelector>().enabled = false;
            weaponImages[i].GetComponent<RawImage>().texture = lockedWeaponTexture;
        }
    }

    public SaveData GetSaveData()
    {
        return data;
    }

    public void SetSaveData(SaveData data)
    {
        this.data = data;
    }

    public void UpdateUnlockedLevelCount(int unlockedLevelCount)
    {
        data.SetUnlockedLevelCount(unlockedLevelCount);
        Save();
    }

    public void UpdateUnlockedWeaponCount(int unlockedWeaponCount)
    {
        data.SetUnlockedWeaponCount(unlockedWeaponCount);
        Save();
    }

    public void UpdateTutorialSeen(bool tutorialSeen)
    {
        data.SetTutorialSeen(tutorialSeen);
        Save();
    }

    public void Save()
    {
        if (!active)
        {
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/BaB.save");

        formatter.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (!active)
        {
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();

        if (!File.Exists(Application.persistentDataPath + "/BaB.save"))
        {
            FileStream file = File.Create(Application.persistentDataPath + "/BaB.save");
            SaveData defaultData = new SaveData();
            formatter.Serialize(file, defaultData);
            file.Close();
        }

        FileStream saveFile = File.Open(Application.persistentDataPath + "/BaB.save", FileMode.Open);
        data = (SaveData)formatter.Deserialize(saveFile);
        saveFile.Close();
    }

    public void ResetSave()
    {
        if (!active)
        {
            return;
        }

        if (File.Exists(Application.persistentDataPath + "/BaB.save"))
        {
            File.Delete(Application.persistentDataPath + "/BaB.save");
        }

        FileStream file = File.Create(Application.persistentDataPath + "/BaB.save");
        SaveData defaultData = new SaveData();

        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(file, defaultData);
        file.Close();

        data = defaultData;
    }

    public void ResetSaveAndLoadFirstLevel()
    {
        ResetSave();
        gameManager.LoadLevelWithIndex(1);
    }

    private void LoadFirstLevel()
    {
        gameManager.BeginNewGame(data.GetUnlockedLevelCount());
    }

    private void LoadCurrentLevel()
    {
        gameManager.LoadLevelWithIndex(data.GetUnlockedLevelCount());
    }
}
