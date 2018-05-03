using System;

[Serializable]
public class SaveData
{
    private bool tutorialSeen;
    private int unlockedLevelCount;
    private int unlockedWeaponCount;

    public SaveData() :
        this(false, 1, 2)
    {}

    public SaveData(bool tutorialSeen, int unlockedLevelCount, int unlockedWeaponCount)
    {
        this.tutorialSeen = tutorialSeen;
        this.unlockedLevelCount = unlockedLevelCount;
        this.unlockedWeaponCount = unlockedWeaponCount;
    }

    public bool GetTutorialSeen()
    {
        return tutorialSeen;
    }

    public int GetUnlockedLevelCount()
    {
        return unlockedLevelCount;
    }

    public int GetUnlockedWeaponCount()
    {
        return unlockedWeaponCount;
    }

    public void SetTutorialSeen(bool tutorialSeen)
    {
        this.tutorialSeen = tutorialSeen;
    }

    public void SetUnlockedLevelCount(int unlockedLevelCount)
    {
        this.unlockedLevelCount = unlockedLevelCount;
    }

    public void SetUnlockedWeaponCount(int unlockedWeaponCount)
    {
        this.unlockedWeaponCount = unlockedWeaponCount;
    }
}
