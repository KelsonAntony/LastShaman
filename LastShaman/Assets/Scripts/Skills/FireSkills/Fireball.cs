using UnityEngine;
using SkillTypes;


public class Fireball : Skills
{
    private CharacterController _characterController;

    private void Start()
    {
        sName = "Fireball";
        _sDescription = "Launch a ball of fire, burning foes";
        _fCooldown = 0.5f;
        _fDamage = 100f;
        _fDamageMod = 1f;
        _fCastTime = 0.5f;
        _fSpeed = 40f;
        _eAttackType = AttackType.Basic | AttackType.Ranged | AttackType.Direct;
        _eDamageType = ElementType.Fire;
        _fLastUsedTime = -_fCooldown;
    }

    //Override function of parent
    public override void ActivateSkill()
    {
        _characterController = _player.GetComponent<CharacterController>(); 
        //Set the last time used so IsOnCooldown can work
        _fLastUsedTime = Time.time;
        //Delay Fireball so it only fire after its cast time
        Invoke("CreatePrefab", _fCastTime);
        base.ActivateSkill();
    }

    private void CreatePrefab()
    {
        //Start the fireball slightly forward of the player position starting at half the height of the player
        //Inherit the rotation of the player so the fireball will keep moving directly forward of the player
        GameObject fireball = Instantiate(_prefabObject, 
            _player.transform.position + (_player.transform.forward) + (_player.transform.up * _characterController.height/2), _player.transform.rotation);

        //Give certain stats to the Prefab object so its own methods can handle how it acts
        PrefabMove fireballstats = fireball.GetComponent<PrefabMove>();
        fireballstats.SetPrefabDamage((int)_fDamage);
        fireballstats.SetPrefabSpeed(_fSpeed.Value);
        fireballstats.SetPrefabDuration(_fDuration, false);
    }
}
