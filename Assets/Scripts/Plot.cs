using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject _towerObject;
    private Wizard _wizard;
    private Color _startColor;

    private void Start()
    {
        _startColor = sr.color;
    }
    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseDown()
    {
        if(EventSystem.current.IsPointerOverGameObject()) return;

        if (UIManager.Main.IsHoverUI()) return;
        
        if (_towerObject != null)
        {
            _wizard.OpenUpgradeUI();
            return;
        }

        Tower buildTower = BuildManager.main.GetSelectedTower();

        if (!LevelManager.main.UseArcana(buildTower.cost))
        {
            return;
        }
        _towerObject = Instantiate(buildTower.prefab, transform.position, Quaternion.identity);
        _wizard = _towerObject.GetComponent<Wizard>();
    }

    private void OnMouseExit()
    {
        sr.color = _startColor;
    }
}
