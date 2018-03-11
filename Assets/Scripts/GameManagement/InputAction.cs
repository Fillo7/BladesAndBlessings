using UnityEngine;

public class InputAction
{
    private string actionName;
    private KeyCode key;
    private KeyCode alternateKey;

    public InputAction(string actionName, KeyCode key)
    {
        this.actionName = actionName;
        this.key = key;
        alternateKey = key;
    }

    public InputAction(string actionName, KeyCode key, KeyCode alternateKey)
    {
        this.actionName = actionName;
        this.key = key;
        this.alternateKey = alternateKey;
    }

    public void SetKey(KeyCode key)
    {
        this.key = key;
    }

    public void SetAlternateKey(KeyCode alternateKey)
    {
        this.alternateKey = alternateKey;
    }

    public string GetActionName()
    {
        return actionName;
    }

    public KeyCode GetKey()
    {
        return key;
    }

    public KeyCode GetAlternateKey()
    {
        return alternateKey;
    }

    public bool HasKeyMapped(KeyCode key)
    {
        return key == this.key || key == alternateKey;
    }
}
