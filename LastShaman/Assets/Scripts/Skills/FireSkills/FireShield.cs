using System.Collections;
using UnityEngine;
using SkillTypes;


public class FireShield : Skills
{
    private int _iAbsorb;
    Coroutine fireShieldTimer;

    private void Start()
    {
        sName = "Fire Shield";
        _sDescription = "Conjure a fiery shield to defend yourself against some attacks, deal damage to attackers";
        _fCooldown = 10f;
        _fDamage = 10f;
        _fDamageMod = 1f;
        _fCastTime = 0f;
        _fSpeed = null;
        _eAttackType = AttackType.Defensive;
        _eDamageType = ElementType.Fire;
        _fDuration = 30f;
        _fLastUsedTime = -_fCooldown;

        _iAbsorb = 50;
    }

    //Override function of parent
    public override void ActivateSkill()
    {
        _fLastUsedTime = Time.time;
        _playerManager.GainAbsorb(_iAbsorb);
        fireShieldTimer = StartCoroutine(RemoveSkill(_fDuration));
        base.ActivateSkill();
    }

    //Remove the skill after a timer
    public IEnumerator RemoveSkill(float a_fWait)
    {
        yield return new WaitForSeconds(a_fWait);

    }

    //Stop the timer if the skills ends prematurly
    public void StopTimer()
    {
        StopCoroutine(fireShieldTimer);
    }
}
