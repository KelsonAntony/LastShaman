using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SkillTypes;

public class Skills : MonoBehaviour
{
    //Skill info
    public string sName { get; protected set; }  
    protected string _sDescription;
    protected float _fCooldown;
    protected float _fDamage;
    protected float _fDamageMod;
    protected float _fCastTime;
    protected float _fDuration;
    protected float? _fSpeed;
    protected AttackType _eAttackType;
    protected ElementType _eDamageType;
    protected GameObject _prefabObject;

    protected float _fLastUsedTime;

    //Script Referances
    protected GameObject _player;
    protected PlayerManager _playerManager;
    protected PlayerMovement _playerMovement;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerManager = _player.GetComponent<PlayerManager>();
        _playerMovement = _player.GetComponent<PlayerMovement>();
        _fDuration = 10f;
    }

    //Get the skill damage type
    public ElementType GetSkillDamageType()
    {
        return _eDamageType;
    }
    //Get the skill attack type
    public AttackType GetSkillAttackType()
    {
        return _eAttackType;
    }

    // Get the remaining cooldown time
    public float GetCooldownTimeRemaining()
    {
        return Mathf.Max(0, (_fLastUsedTime + _fCooldown) - Time.time);
    }
    public float GetCooldown()
    {
        return _fCooldown;
    }
    //Get the cast time of a spell
    public float GetCastTime()
    {
        return _fCastTime;
    }

    //Virtual Methods to override in child class
    //Call event in virtual method, can be used in child object with base.ActivateSkill
    public virtual void ActivateSkill() { _playerMovement.PlayerUsedSkill(); }
    public void SetPrefab(GameObject a_prefab)
    {
        _prefabObject = a_prefab;
    }

    // Check if the skill is on cooldown
    public bool IsOnCooldown()
    {
        return Time.time < _fLastUsedTime + _fCooldown;
    }
    // Display skill information
    public virtual string DisplaySkillInfo()
    {
        string sCastTime;

        if(_fCastTime > 0)
        {
            sCastTime = _fCastTime.ToString();
        }
        else
        {
            sCastTime = "Instant";
        }

        return $"Skill: {sName}\nDescription: {_sDescription}\nCast Time: {sCastTime} \nCooldown: {_fCooldown} seconds\nDamage: {_fDamage} {_eDamageType} Damage";
    }
}
