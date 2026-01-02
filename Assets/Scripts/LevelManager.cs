using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    
    public int playerHitPoints = 20;
    [SerializeField] private TextMeshProUGUI healthUI;
    [SerializeField] private GameObject insufficientArcanaUI;
    [SerializeField] private GameObject deathScreen;
    public Transform startPoint;
    public Transform[] path;
    
    public int arcana;
    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        healthUI.text  = playerHitPoints.ToString();
        deathScreen.SetActive(false);
    }
    
    public void IncreaseArcana(int amount)
    {
        arcana += amount;
    }

    public bool UseArcana(int amount)
    {
        if (amount <= arcana)
        {
            arcana -= amount;
            return true;
        }
        else
        {
            insufficientArcanaUI.SetActive(true);
            StartCoroutine(HideInsufficientArcanaUI());
            return false;
        }
    }
    
    private IEnumerator HideInsufficientArcanaUI()
    {
        yield return new WaitForSeconds(1f);
        insufficientArcanaUI.SetActive(false);
    }
    
    public void PlayerTakeDamage(int damage)
    {
        playerHitPoints -= damage;
        healthUI.text = playerHitPoints.ToString();
        if (playerHitPoints <= 0)
        {
            deathScreen.SetActive(true);
            MenuUIManager.IsDead = true;
            Time.timeScale = 0f;
        }
    }
    
    public void PlayerHeal(int heal)
    {
        playerHitPoints += heal;
        healthUI.text = playerHitPoints.ToString();
    }
}
