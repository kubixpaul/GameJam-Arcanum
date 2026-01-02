using UnityEngine;

public class UIManager : MonoBehaviour
{
     public static UIManager Main;
     
     private bool _isHoveringUI = false;

     private void Awake()
     {
          Main = this;
     }

     public void SetHoveringState(bool state)
     {
          _isHoveringUI = state;
     }

     public bool IsHoverUI()
     {
          return _isHoveringUI;
     }
}
