using System.Collections;
using UnityEngine;
using SkillTypes;


public class FireSpeed : Skills
{
    private float _fSpeedBoost;
    private int _iPatches;
    private DebuffType _debuff;

    private void Start()
    {
        sName = "Fire Speed";
        _sDescription = "Sprint forward, periodically dropping fire in your wake";
        _fCooldown = 10f;
        _fDamage = 40f;
        _fDamageMod = 1f;
        _fCastTime = 0f;
        _fSpeed = null;
        _eAttackType = AttackType.Area | AttackType.Melee;
        _eDamageType = ElementType.Fire;
        _fDuration = 4f;
        _fLastUsedTime = -_fCooldown;

        _fSpeedBoost = 2f;
        _iPatches = (int)_fDuration;
    }

    public override void ActivateSkill()
    {
        _fLastUsedTime = Time.time;

        //Increase player movement speed and remove the movement speed after a timer
        _playerMovement.ModifyAddSpeed(_fSpeedBoost);
        StartCoroutine(RemoveSkill(_fDuration));

        //Drop 4 patches of fire, 1 instantly and 3 more spaced out
        StartCoroutine(DropFire(0));
        StartCoroutine(DropFire(1));
        StartCoroutine(DropFire(2));
        StartCoroutine(DropFire(3));
        base.ActivateSkill();
    }

    //Remove the speed boost gives to the player on a timer
    private IEnumerator RemoveSkill(float a_fWait)
    {
        yield return new WaitForSeconds(a_fWait);
        _playerMovement.ModiftyRemoveSpeed(_fSpeedBoost);
    }

    //Drop game objects on a timer
    private IEnumerator DropFire(float a_fWait)
    {
        GameObject groundFire;
        PrefabMove groundFireStats;

        yield return new WaitForSeconds(a_fWait);
        groundFire = Instantiate(_prefabObject, _player.transform.position, Quaternion.identity);
        groundFireStats = groundFire.GetComponent<PrefabMove>();
        groundFireStats.SetPrefabDamage((int)_fDamage);
        groundFireStats.SetPrefabDuration(_fDuration, true);
    }

    public override string DisplaySkillInfo()
    {
        return $"Skill: {sName}\nDescription: {_sDescription}\nCooldown: {_fCooldown} seconds\nDamage: {_fDamage} {_eDamageType} Damage per second";
    }
}
