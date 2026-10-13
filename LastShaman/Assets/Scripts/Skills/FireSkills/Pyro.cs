using UnityEngine;
using SkillTypes;

public class Pyro : Skills
{
    private CharacterController _characterController;
    private void Start()
    {
        sName = "Pyro";
        _sDescription = "Launch a slow firey projectile, buring foes";
        _fCooldown = 15f;
        _fDamage = 1500f;
        _fDamageMod = 1f;
        _fCastTime = 2.5f;
        _fSpeed = 25f;
        _eAttackType = AttackType.Ranged | AttackType.Direct;
        _eDamageType = ElementType.Fire;
        _fLastUsedTime = -_fCooldown;
    }

    //Override function of parent
    public override void ActivateSkill()
    {
        _characterController = _player.GetComponent<CharacterController>();
        //Set the last time used so IsOnCooldown can work
        _fLastUsedTime = Time.time;
        //Delay Pyro so it only fire after its cast time
        Invoke("CreatePrefab", _fCastTime);
        base.ActivateSkill();
    }

    private void CreatePrefab()
    {
        //Start the fireball slightly forward of the player position starting at half the height of the player
        //Inherit the rotation of the player so the fireball will keep moving directly forward of the player
        GameObject pyroball = Instantiate(_prefabObject,
            _player.transform.position + (_player.transform.forward) + (_player.transform.up * _characterController.height / 2), _player.transform.rotation);

        //Give certain stats to the Prefab object so its own methods can handle how it acts
        PrefabMove pyrostats = pyroball.GetComponent<PrefabMove>();
        pyrostats.SetPrefabDamage((int)_fDamage);
        pyrostats.SetPrefabSpeed(_fSpeed.Value);
        pyrostats.SetPrefabDuration(_fDuration, false);
    }
}
