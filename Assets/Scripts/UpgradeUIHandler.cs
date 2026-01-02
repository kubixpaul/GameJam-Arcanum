using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUIHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouseOver = false;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        UIManager.Main.SetHoveringState(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        UIManager.Main.SetHoveringState(false);
        gameObject.SetActive(false);
    }
}
