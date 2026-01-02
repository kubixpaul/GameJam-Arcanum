using NUnit.Framework.Constraints;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyUI;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject hints;
    
    private bool _hintsActive = true;

    private void Update()
    {
        currencyUI.text = LevelManager.main.arcana.ToString();
    }

    private void Start()
    {
        
    }
 
    private bool _isMenuOpen = true;

    public void ToggleMenu()
    {
        _isMenuOpen = !_isMenuOpen;
        anim.SetBool("MenuOpen", _isMenuOpen);
    }
    
    public void ToggleHints()
        {
            if (_hintsActive)
            {
                hints.SetActive(false);
                _hintsActive  = false;
            }
            else
            {
                hints.SetActive(true);
                _hintsActive = true;
            }
        }

}
