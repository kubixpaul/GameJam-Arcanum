using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class Wizard : MonoBehaviour
{
    [SerializeField] private Transform wizardRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject magicPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private TextMeshProUGUI currencyUI;
    [SerializeField] private TextMeshProUGUI levelUI;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private bool isIce = false;
    [SerializeField] private float freezeTime = 1f;
    [SerializeField] private bool isLightning = false;
    [SerializeField] private bool isFire = false;
    [SerializeField] private int wizardDamage = 1;
    [SerializeField] private bool isWater = false;
    [SerializeField] private int waterHealing = 1;
    
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float magicPerSec = 1f;
    public int upgradeCost = 100;

    private int _level = 1;

    private float _mpsBase;
    private float _targetingRangeBase;
    private float _freezeTimeBase;

    private void Start()
    {
        _mpsBase = magicPerSec;
        _targetingRangeBase = targetingRange;
        _freezeTimeBase = freezeTime;
        
        upgradeButton.onClick.AddListener(Upgrade);
        currencyUI.text = upgradeCost.ToString();
        levelUI.text = _level.ToString();
    }

    private Transform _target;
    private float _timeUntilFire;

    
    public void Update()
    {
        if (isWater)
        {
            _timeUntilFire += Time.deltaTime;
            if (_timeUntilFire >= 1f / magicPerSec)
            {
                WaterHeal();
                _timeUntilFire = 0f;
            }
            
        }
        else
        {
            if (_target == null)
            {
                FindTarget();
                return;
            }

            RotateTowardsTarget();
            if (!CheckTargetIsInRange())
            {
                _target = null;
            }
            else
            {
                _timeUntilFire += Time.deltaTime;
                if (_timeUntilFire >= 1f / magicPerSec)
                {
                    if (!isIce)
                    {
                        Shoot();
                    }
                    else
                    {
                        FreezeEnemies();
                    }
                    _timeUntilFire = 0f;
                }
            }
        }
       
    }

    private void Shoot()
    {
        GameObject spellObj = Instantiate(magicPrefab, firingPoint.position, Quaternion.identity);
        Spell spellScript = spellObj.GetComponent<Spell>();
        spellScript.SetTarget(_target, wizardDamage);
    }
    
    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange,
            (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            _target = hits[0].transform;
        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(_target.position.y - transform.position.y, _target.position.x - transform.position.x) 
                      * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        wizardRotationPoint.rotation = Quaternion.RotateTowards(wizardRotationPoint.rotation, targetRotation, 
            rotationSpeed * Time.deltaTime);

    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(_target.position, transform.position) <= targetingRange;
    }

    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange,
            (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(0.5f);

                StartCoroutine(ResetEnemySpeed((em)));
            }
        }
    }

    private void WaterHeal()
    {
        if (LevelManager.main.playerHitPoints < 20)
        {
            LevelManager.main.PlayerHeal(waterHealing);
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);
        
        em.ResetSpeed();
    }

    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void Upgrade()
    {
        if (!LevelManager.main.UseArcana(CalculateCost()))
        {
            return;
        }

        _level++;
        
        levelUI.text = _level.ToString();
        currencyUI.text = CalculateCost().ToString();
        if (isIce)
        {
            freezeTime = CalculateFreeze();
        }
        else if (isLightning)
        {
            targetingRange = CalculateRange();
        }
        else if (isFire)
        {
            magicPerSec = CalculateMps(0.3f);
        }
        else if (isWater)
        {
            waterHealing++;
        }
        else
        {
            magicPerSec = CalculateMps(0.2f);
        }
    }

    private int CalculateCost()
    {
        return Mathf.RoundToInt(upgradeCost * Mathf.Pow(_level, 0.8f));
    }
    
    private float CalculateMps(float value)
    {
        return _mpsBase * Mathf.Pow(_level, value);
    }
    
    private float CalculateFreeze()
    {
        return _freezeTimeBase * Mathf.Pow(_level, 0.3f);
    }
    
    private float CalculateRange()
    {
        return _targetingRangeBase * Mathf.Pow(_level, 0.3f);
    }
    
    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
    }
}
