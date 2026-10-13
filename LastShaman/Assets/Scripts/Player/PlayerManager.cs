using UnityEngine;
using SkillTypes;
using TMPro;

/*
 * ++++
 * 
 * Holding Player Stats and modifiers to do with health
 * 
 * ++++
 */

public class PlayerManager : MonoBehaviour
{
    //UI Objects
    [SerializeField] private Healthbar healthbar;
    [SerializeField] private StanceImage stanceImage;
    [SerializeField] private TextMeshProUGUI tmpText;

    //Unit event delegate, static so other scripts can subscribe to the event
    public delegate void OnPlayerDeath();
    public static event OnPlayerDeath onPlayerDeath;

    //Player
    private int _iHealthBase;
    private int _iHealthMax;
    private int _iHealthCurrent;
    private int _iAborbCurrent;
    private bool _bIsDead;

    //Modifiers
    public float iHealthMod;

    void Start()
    {
        //Default stats
        _iHealthBase = 100;
        _iHealthMax = _iHealthBase;
        _iHealthCurrent = _iHealthBase;
        iHealthMod = 1;
        _iAborbCurrent = 0;
        _bIsDead = false;

        //UI Bar
        healthbar.SetHealthMax(_iHealthMax);
    }

    //Subscribe to events when enabled and remove from events when disabled
    private void OnEnable()
    {
        PlayerManager.onPlayerDeath += PlayerDeath;
    }
    private void OnDisable()
    {
        PlayerManager.onPlayerDeath -= PlayerDeath;
    }

    // Update is called once per frame
    void Update()
    {
        if (_bIsDead)
        {
            return;
        }
    }

    //Reference for Damage and healing functions.  Two functions for clarity (negative damage is technically "healing" but harder for maintenance)
    public void TakeDamage(int iDamage)
    {
        //Asorb should take damage before health.  If Aborb is less than the damage amount
        //then the absorb takes some of the damage and the rest bleeds into health
        if (_iAborbCurrent > 0 && iDamage < _iAborbCurrent)
        {
            _iAborbCurrent -= iDamage;
            healthbar.SetAbsorb(_iAborbCurrent);
        }
        else if (_iAborbCurrent > 0 && iDamage > _iAborbCurrent)
        {
            iDamage -= _iAborbCurrent;
            healthbar.SetAbsorb(0);
            _iHealthCurrent -= iDamage;
            healthbar.SetHealth(_iHealthCurrent);
        }
        else
        {
            _iHealthCurrent -= iDamage;
            healthbar.SetHealth(_iHealthCurrent);
        }

        if (_iHealthCurrent <= 0)
        {
            //If not null Invoke delegate then empty events
            onPlayerDeath?.Invoke();
            onPlayerDeath = null;
        }
    }

    public void GainHealth(int iHeal)
    {
        _iHealthCurrent += iHeal;
        healthbar.SetHealth(_iHealthCurrent);
    }

    public void GainAbsorb (int iAbsorb)
    {
        _iAborbCurrent += iAbsorb;
        healthbar.SetAbsorb(iAbsorb);
    }

    private void PlayerDeath()
    {
        Debug.Log("Player Death event");
        tmpText.gameObject.SetActive(true);
        _bIsDead = true;
    }
}

    /*
    public static AttackType SetFlag(AttackType a, AttackType b)
    {
        return a | b;
    }

    public static AttackType UnsetFlag(AttackType a, AttackType b)
    {
        return a & (~b);
    }
    // Works with "None" as well

    public static bool HasFlag(AttackType a, AttackType b)
    {
        return (a & b) == b;
    }

    public static AttackType ToogleFlag(AttackType a, AttackType b)
    {
        return a ^ b;
    }
    */