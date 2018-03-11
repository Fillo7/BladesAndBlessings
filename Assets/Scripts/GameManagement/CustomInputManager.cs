using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomInputManager : MonoBehaviour
{
    [SerializeField] private GameObject inputPanel;
    [SerializeField] private GameObject inputErrorPanel;

    [SerializeField] private Button upButton;
    [SerializeField] private Button upButtonAlt;
    [SerializeField] private Button downButton;
    [SerializeField] private Button downButtonAlt;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button leftButtonAlt;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button rightButtonAlt;
    [SerializeField] private Button basicAttackButton;
    [SerializeField] private Button basicAttackButtonAlt;
    [SerializeField] private Button specialAttack1Button;
    [SerializeField] private Button specialAttack1ButtonAlt;
    [SerializeField] private Button specialAttack2Button;
    [SerializeField] private Button specialAttack2ButtonAlt;
    [SerializeField] private Button weaponSwapButton;
    [SerializeField] private Button weaponSwapButtonAlt;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button cancelButtonAlt;
    [SerializeField] private Button screenshotButton;
    [SerializeField] private Button screenshotButtonAlt;

    private SortedDictionary<string, InputAction> inputActions = new SortedDictionary<string, InputAction>();
    private Button pushedButton;
    private KeyCode pushedKey;
    private bool pushedKeyValid = false;
    private bool waitingForKey = false;

    void Awake()
    {
        inputPanel.SetActive(false);
        inputErrorPanel.SetActive(false);
        
        InitializeInputActions();
    }

    public bool GetKeyDown(string name)
    {
        return Input.GetKeyDown(GetKeyForAction(name)) || Input.GetKeyDown(GetAlternateKeyForAction(name));
    }

    public float GetAxisRaw(string name)
    {
        if (name.Equals("Vertical"))
        {
            if (Input.GetKey(GetKeyForAction("InputUp")))
            {
                return 1.0f;
            }
            else if(Input.GetKey(GetKeyForAction("InputDown")))
            {
                return -1.0f;
            }
            else
            {
                return 0.0f;
            }
        }
        else if (name.Equals("Horizontal"))
        {
            if (Input.GetKey(GetKeyForAction("InputRight")))
            {
                return 1.0f;
            }
            else if (Input.GetKey(GetKeyForAction("InputLeft")))
            {
                return -1.0f;
            }
            else
            {
                return 0.0f;
            }
        }

        return 0.0f;
    }

    public bool IsWaitingForKey()
    {
        return waitingForKey;
    }

    public void SetPushedButton(Button button)
    {
        pushedButton = button;
    }

    public void SetKeyForAction(string actionName)
    {
        inputPanel.SetActive(true);
        waitingForKey = true;
        StartCoroutine(WaitForKeyDown(true, actionName, true));
    }

    public void SetAlternateKeyForAction(string actionName)
    {
        inputPanel.SetActive(true);
        waitingForKey = true;
        StartCoroutine(WaitForKeyDown(true, actionName, false));
    }

    public KeyCode GetKeyForAction(string actionName)
    {
        InputAction action;
        if (inputActions.TryGetValue(actionName, out action))
        {
            return action.GetKey();
        }
        return 0;
    }

    public KeyCode GetAlternateKeyForAction(string actionName)
    {
        InputAction action;
        if (inputActions.TryGetValue(actionName, out action))
        {
            return action.GetAlternateKey();
        }
        return 0;
    }

    public string GetActionForKey(KeyCode key)
    {
        foreach (KeyValuePair<string, InputAction> action in inputActions)
        {
            if (action.Value.HasKeyMapped(key))
            {
                return action.Value.GetActionName();
            }
        }
        return null;
    }

    public bool IsKeyMapped(KeyCode key)
    {
        foreach (KeyValuePair<string, InputAction> action in inputActions)
        {
            if (action.Value.HasKeyMapped(key))
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator WaitForKeyDown(bool wait, string actionName, bool mainButton)
    {
        while (wait)
        {
            if (Input.anyKeyDown)
            {
                KeyCode key = 0;
                foreach (KeyCode code in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(code))
                    {
                        key = code;
                    }
                }
                SetPushedKey(key, actionName);
                SetKeyForActionInternal(actionName, mainButton);
                inputPanel.SetActive(false);
                wait = false;
                waitingForKey = false;
            }
            yield return null;
        }
    }

    private void SetPushedKey(KeyCode key, string actionName)
    {
        if (IsKeyMapped(key) && !GetActionForKey(key).Equals(actionName))
        {
            pushedKeyValid = false;
            return;
        }

        pushedKey = key;
        pushedKeyValid = true;
    }

    private void SetKeyForActionInternal(string actionName, bool mainButton)
    {
        if (!pushedKeyValid)
        {
            inputErrorPanel.SetActive(true);
            return;
        }

        InputAction action;
        if (inputActions.TryGetValue(actionName, out action))
        {
            if (mainButton)
            {
                action.SetKey(pushedKey);
                PlayerPrefs.SetInt(action.GetActionName(), (int)pushedKey);
            }
            else
            {
                action.SetAlternateKey(pushedKey);
                PlayerPrefs.SetInt(action.GetActionName() + "Alt", (int)pushedKey);
            }
            pushedButton.GetComponentInChildren<Text>().text = pushedKey.ToString();
        }
    }

    private void InitializeInputActions()
    {
        if (!PlayerPrefs.HasKey("InputUp"))
        {
            PlayerPrefs.SetInt("InputUp", (int)KeyCode.W);
        }
        if (!PlayerPrefs.HasKey("InputUpAlt"))
        {
            PlayerPrefs.SetInt("InputUpAlt", (int)KeyCode.UpArrow);
        }

        if (!PlayerPrefs.HasKey("InputDown"))
        {
            PlayerPrefs.SetInt("InputDown", (int)KeyCode.S);
        }
        if (!PlayerPrefs.HasKey("InputDownAlt"))
        {
            PlayerPrefs.SetInt("InputDownAlt", (int)KeyCode.DownArrow);
        }

        if (!PlayerPrefs.HasKey("InputLeft"))
        {
            PlayerPrefs.SetInt("InputLeft", (int)KeyCode.A);
        }
        if (!PlayerPrefs.HasKey("InputLeftAlt"))
        {
            PlayerPrefs.SetInt("InputLeftAlt", (int)KeyCode.LeftArrow);
        }

        if (!PlayerPrefs.HasKey("InputRight"))
        {
            PlayerPrefs.SetInt("InputRight", (int)KeyCode.D);
        }
        if (!PlayerPrefs.HasKey("InputRightAlt"))
        {
            PlayerPrefs.SetInt("InputRightAlt", (int)KeyCode.RightArrow);
        }

        if (!PlayerPrefs.HasKey("InputBasicAttack"))
        {
            PlayerPrefs.SetInt("InputBasicAttack", (int)KeyCode.Mouse0);
        }
        if (!PlayerPrefs.HasKey("InputBasicAttackAlt"))
        {
            PlayerPrefs.SetInt("InputBasicAttackAlt", (int)KeyCode.H);
        }

        if (!PlayerPrefs.HasKey("InputSpecialAttack1"))
        {
            PlayerPrefs.SetInt("InputSpecialAttack1", (int)KeyCode.Mouse1);
        }
        if (!PlayerPrefs.HasKey("InputSpecialAttack1Alt"))
        {
            PlayerPrefs.SetInt("InputSpecialAttack1Alt", (int)KeyCode.J);
        }

        if (!PlayerPrefs.HasKey("InputSpecialAttack2"))
        {
            PlayerPrefs.SetInt("InputSpecialAttack2", (int)KeyCode.R);
        }
        if (!PlayerPrefs.HasKey("InputSpecialAttack2Alt"))
        {
            PlayerPrefs.SetInt("InputSpecialAttack2Alt", (int)KeyCode.K);
        }

        if (!PlayerPrefs.HasKey("InputWeaponSwap"))
        {
            PlayerPrefs.SetInt("InputWeaponSwap", (int)KeyCode.X);
        }
        if (!PlayerPrefs.HasKey("InputWeaponSwapAlt"))
        {
            PlayerPrefs.SetInt("InputWeaponSwapAlt", (int)KeyCode.X);
        }

        if (!PlayerPrefs.HasKey("InputCancel"))
        {
            PlayerPrefs.SetInt("InputCancel", (int)KeyCode.Escape);
        }
        if (!PlayerPrefs.HasKey("InputCancelAlt"))
        {
            PlayerPrefs.SetInt("InputCancelAlt", (int)KeyCode.Escape);
        }

        if (!PlayerPrefs.HasKey("InputScreenshot"))
        {
            PlayerPrefs.SetInt("InputScreenshot", (int)KeyCode.F10);
        }
        if (!PlayerPrefs.HasKey("InputScreenshotAlt"))
        {
            PlayerPrefs.SetInt("InputScreenshotAlt", (int)KeyCode.F10);
        }

        KeyCode inputUp = (KeyCode)PlayerPrefs.GetInt("InputUp");
        KeyCode inputUpAlt = (KeyCode)PlayerPrefs.GetInt("InputUpAlt");
        inputActions.Add("InputUp", new InputAction("InputUp", inputUp, inputUpAlt));
        upButton.GetComponentInChildren<Text>().text = inputUp.ToString();
        upButtonAlt.GetComponentInChildren<Text>().text = inputUpAlt.ToString();

        KeyCode inputDown = (KeyCode)PlayerPrefs.GetInt("InputDown");
        KeyCode inputDownAlt = (KeyCode)PlayerPrefs.GetInt("InputDownAlt");
        inputActions.Add("InputDown", new InputAction("InputDown", inputDown, inputDownAlt));
        downButton.GetComponentInChildren<Text>().text = inputDown.ToString();
        downButtonAlt.GetComponentInChildren<Text>().text = inputDownAlt.ToString();

        KeyCode inputLeft = (KeyCode)PlayerPrefs.GetInt("InputLeft");
        KeyCode inputLeftAlt = (KeyCode)PlayerPrefs.GetInt("InputLeftAlt");
        inputActions.Add("InputLeft", new InputAction("InputLeft", inputLeft, inputLeftAlt));
        leftButton.GetComponentInChildren<Text>().text = inputLeft.ToString();
        leftButtonAlt.GetComponentInChildren<Text>().text = inputLeftAlt.ToString();

        KeyCode inputRight = (KeyCode)PlayerPrefs.GetInt("InputRight");
        KeyCode inputRightAlt = (KeyCode)PlayerPrefs.GetInt("InputRightAlt");
        inputActions.Add("InputRight", new InputAction("InputRight", inputRight, inputRightAlt));
        rightButton.GetComponentInChildren<Text>().text = inputRight.ToString();
        rightButtonAlt.GetComponentInChildren<Text>().text = inputRightAlt.ToString();

        KeyCode inputBasicAttack = (KeyCode)PlayerPrefs.GetInt("InputBasicAttack");
        KeyCode inputBasicAttackAlt = (KeyCode)PlayerPrefs.GetInt("InputBasicAttackAlt");
        inputActions.Add("InputBasicAttack", new InputAction("InputBasicAttack", inputBasicAttack, inputBasicAttackAlt));
        basicAttackButton.GetComponentInChildren<Text>().text = inputBasicAttack.ToString();
        basicAttackButtonAlt.GetComponentInChildren<Text>().text = inputBasicAttackAlt.ToString();

        KeyCode inputSpecialAttack1 = (KeyCode)PlayerPrefs.GetInt("InputSpecialAttack1");
        KeyCode inputSpecialAttack1Alt = (KeyCode)PlayerPrefs.GetInt("InputSpecialAttack1Alt");
        inputActions.Add("InputSpecialAttack1", new InputAction("InputSpecialAttack1", inputSpecialAttack1, inputSpecialAttack1Alt));
        specialAttack1Button.GetComponentInChildren<Text>().text = inputSpecialAttack1.ToString();
        specialAttack1ButtonAlt.GetComponentInChildren<Text>().text = inputSpecialAttack1Alt.ToString();

        KeyCode inputSpecialAttack2 = (KeyCode)PlayerPrefs.GetInt("InputSpecialAttack2");
        KeyCode inputSpecialAttack2Alt = (KeyCode)PlayerPrefs.GetInt("InputSpecialAttack2Alt");
        inputActions.Add("InputSpecialAttack2", new InputAction("InputSpecialAttack2", inputSpecialAttack2, inputSpecialAttack2Alt));
        specialAttack2Button.GetComponentInChildren<Text>().text = inputSpecialAttack2.ToString();
        specialAttack2ButtonAlt.GetComponentInChildren<Text>().text = inputSpecialAttack2Alt.ToString();

        KeyCode inputWeaponSwap = (KeyCode)PlayerPrefs.GetInt("InputWeaponSwap");
        KeyCode inputWeaponSwapAlt = (KeyCode)PlayerPrefs.GetInt("InputWeaponSwapAlt");
        inputActions.Add("InputWeaponSwap", new InputAction("InputWeaponSwap", inputWeaponSwap, inputWeaponSwapAlt));
        weaponSwapButton.GetComponentInChildren<Text>().text = inputWeaponSwap.ToString();
        weaponSwapButtonAlt.GetComponentInChildren<Text>().text = inputWeaponSwapAlt.ToString();

        KeyCode inputCancel = (KeyCode)PlayerPrefs.GetInt("InputCancel");
        KeyCode inputCancelAlt = (KeyCode)PlayerPrefs.GetInt("InputCancelAlt");
        inputActions.Add("InputCancel", new InputAction("InputCancel", inputCancel, inputCancelAlt));
        cancelButton.GetComponentInChildren<Text>().text = inputCancel.ToString();
        cancelButtonAlt.GetComponentInChildren<Text>().text = inputCancelAlt.ToString();

        KeyCode inputScreenshot = (KeyCode)PlayerPrefs.GetInt("InputScreenshot");
        KeyCode inputScreenshotAlt = (KeyCode)PlayerPrefs.GetInt("InputScreenshotAlt");
        inputActions.Add("InputScreenshot", new InputAction("InputScreenshot", inputScreenshot, inputScreenshotAlt));
        screenshotButton.GetComponentInChildren<Text>().text = inputScreenshot.ToString();
        screenshotButtonAlt.GetComponentInChildren<Text>().text = inputScreenshotAlt.ToString();
    }
}
