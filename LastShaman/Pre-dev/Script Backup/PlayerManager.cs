using UnityEngine;
using SkillTypes;

/*
 * ++++
 * Holding Player Stats and modifiers to do with stance and health
 * 
 * Speed moved to PlayerMovement
 * Damage moved to SkillsManager
 * ++++
 */

public class PlayerManager : MonoBehaviour
{
    //UI Objects
    [SerializeField] private Healthbar healthbar;
    [SerializeField] private StanceImage stanceImage;

    //Player
    private int _iHealthBase;
    private int _iHealthMax;
    private int _iHealthCurrent;
    private int _iAborbCurrent;
    //private bool _bIsDead;

    //Modifiers
    public float iHealthMod;

    //Private Variables handling stance controls
    public float fStanceDelay;
    private float _fStanceTime;
    private ElementType _stance;
    private bool _bFirstStance;
    private bool _bSecondStance;

    void Start()
    {
        //Default stats
        _iHealthBase = 100;
        _iHealthMax = _iHealthBase;
        _iHealthCurrent = _iHealthBase;
        iHealthMod = 1;
        _iAborbCurrent = 0;

        //UI Bar
        healthbar.SetHealthMax(_iHealthMax);

        //Stance
        fStanceDelay = 1f;
        _fStanceTime = 0f;
        _stance = ElementType.None;
        _bFirstStance = false;
        _bSecondStance = false;            
    }
    
    // Update is called once per frame
    void Update()
    {
        //If in first stance and stace timer is higher than delay, add time
        //If in first stance and stance timer is higher than delay reset trackers and stance timer.
        if (_bFirstStance && _fStanceTime < fStanceDelay)
        {
            _fStanceTime += Time.deltaTime;
        }
        else if (_bFirstStance && _fStanceTime > fStanceDelay)
        {
            _bFirstStance = false;
            _bSecondStance = false;
            _fStanceTime = 0f;
            Debug.Log("Stance reset");
        }
        
        //Controlling the stance mechanic of the player.  Change the stance dependant on key pressed with checks
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //If stance timer as reset, you are in your first element, reset stance variable
            //Set stance to fire
            if (!_bFirstStance && !_bSecondStance)
            {
                _bFirstStance = true;
                _stance = ElementType.None;
                _stance = StanceChange(ElementType.Fire, _stance);
                Debug.Log("Current Stance:" + _stance);
            }
            // If in your first element but not your second element, and the first element isn't fire (0001 & 0001 != 0000)
            // add fire to your current element
            else if(_bFirstStance && !_bSecondStance && 
                (_stance & ElementType.Fire) == ElementType.None)
            {
                _bSecondStance = true;
                _stance = StanceChange(ElementType.Fire, _stance);
                Debug.Log("Current Stance:" + _stance);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //If stance timer as reset, you are in your first element, reset stance variable
            //Set stance to frost
            if (!_bFirstStance && !_bSecondStance)
            {
                _bFirstStance = true;
                _stance = ElementType.None;
                _stance = StanceChange(ElementType.Frost, _stance);
                Debug.Log("Current Stance:" + _stance);
            }
            // If in your first element but not your second element, and the first element isn't frost (0010 & 0010 != 0000)
            // add fire to your current element
            else if (_bFirstStance && !_bSecondStance &&
                (_stance & ElementType.Frost) == ElementType.None)
            {
                _bSecondStance = true;
                _stance = StanceChange(ElementType.Frost, _stance);
                Debug.Log("Current Stance:" + _stance);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //If stance timer as reset, you are in your first element, reset stance variable
            //Set stance to earth
            if (!_bFirstStance && !_bSecondStance)
            {
                _bFirstStance = true;
                _stance = ElementType.None;
                _stance = StanceChange(ElementType.Earth, _stance);
                Debug.Log("Current Stance:" + _stance);
            }
            // If in your first element but not your second element, and the first element isn't earth (0100 & 0100 != 0000)
            // add earth to your current element
            else if (_bFirstStance && !_bSecondStance &&
                (_stance & ElementType.Earth) == ElementType.None)
            {
                _bSecondStance = true;
                _stance = StanceChange(ElementType.Earth, _stance);
                Debug.Log("Current Stance:" + _stance);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //If stance timer as reset, you are in your first element, reset stance variable
            //Set stance to air
            if (!_bFirstStance && !_bSecondStance)
            {
                _bFirstStance = true;
                _stance = ElementType.None;
                _stance = StanceChange(ElementType.Air, _stance);
                Debug.Log("Current Stance:" + _stance);
            }
            // If in your first element but not your second element, and the first element isn't earth (1000 & 1000 != 0000)
            // add air to your current element
            else if (_bFirstStance && !_bSecondStance &&
                (_stance & ElementType.Air) == ElementType.None)
            {
                _bSecondStance = true;
                _stance = StanceChange(ElementType.Air, _stance);
                Debug.Log("Current Stance:" + _stance);
            }
        }

        //Test statement for damage function
        if(Input.GetMouseButtonDown(0))
        {
            TakeDamage(20);
        }

    }

    //Reference for Damage and healing functions.  Two functions for clarity (negative damage is technically "healing" but harder for maintenance
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
            Debug.Log("You are dead");
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
    public ElementType GetStance()
    {
        return _stance;
    }
    //Change the stance of the player. Checks to make sure that the combination is viable.
    private ElementType StanceChange(ElementType a_element, ElementType a_stance)
    {
        ElementType newStance;

        if ((a_stance | a_element) != (ElementType.Fire | ElementType.Air)
            && (a_stance | a_element) != (ElementType.Earth | ElementType.Frost))
        {
            newStance = a_stance |= a_element;
            stanceImage.NewStance(newStance);
            return newStance;
        }
        else
        {
            return a_stance;
        }
    }
    //Force a stance change with no checks.  Use Carefully.
    private ElementType ForcedStanceChange(ElementType a_Element, ElementType a_Stance)
    {
        ElementType newStance;

        newStance = a_Stance |= a_Element;
        stanceImage.NewStance(newStance);
        return newStance;
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

