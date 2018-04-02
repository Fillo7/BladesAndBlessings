using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSelector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int weaponIndex;

    private LoadoutController loadoutController;
    private Image highlightImage;
    bool weaponSelected = false;

    void Awake()
    {
        loadoutController = gameObject.GetComponentInParent<LoadoutController>();
        highlightImage = GetComponentInChildren<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool result = loadoutController.ToggleWeapon(weaponIndex);
        weaponSelected = result;

        if (result)
        {
            highlightImage.color = new Color(highlightImage.color.r, highlightImage.color.g, highlightImage.color.b, 0.3f);
        }
        else
        {
            highlightImage.color = new Color(highlightImage.color.r, highlightImage.color.g, highlightImage.color.b, 0.0f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        loadoutController.ActivateDescription(weaponIndex);
        if (!weaponSelected)
        {
            highlightImage.color = new Color(highlightImage.color.r, highlightImage.color.g, highlightImage.color.b, 0.1f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!weaponSelected)
        {
            highlightImage.color = new Color(highlightImage.color.r, highlightImage.color.g, highlightImage.color.b, 0.0f);
        }
    }
}
