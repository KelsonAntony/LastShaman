using UnityEngine;
using SkillTypes;

public class Meteor : Skills
{
    //Vectors
    private Vector3 _prefabSpawnLocation;
    private Vector3 _targetLocation;

    private void Start()
    {
        sName = "Meteor";
        _sDescription = "Summon a Meteor to fall from the sky, devatating foes, stunning briefly";
        _fCooldown = 20f;
        _fDamage = 500f;
        _fDamageMod = 1f;
        _fCastTime = 1.5f;
        _fSpeed = 25f;
        _eAttackType = AttackType.Ranged | AttackType.Area;
        _eDamageType = ElementType.Volcanic;
        _fLastUsedTime = -_fCooldown;
    }

    public override void ActivateSkill()
    {
        //Last time the skill was used
        _fLastUsedTime = Time.time;

        //Use raycast to determine where to send the prefab object
        _prefabSpawnLocation = _player.transform.position + (Vector3.up * 10);
        _targetLocation = Vector3.zero;
        RaycastHit rayHit;
        Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Set the target location to where the ray collides with an object
        //minus spawn location to find direction
        if (Physics.Raycast(screenRay, out rayHit) )
        {
            _targetLocation = rayHit.point - _prefabSpawnLocation;
        }

        Invoke("CreatePrefab", _fCastTime);
        base.ActivateSkill();
    }

    private void CreatePrefab()
    {
        //Set up the meteor above the player location and rotate the object to look at the mouse position
        GameObject meteor;
        meteor = Instantiate(_prefabObject, _prefabSpawnLocation, Quaternion.LookRotation(_targetLocation, Vector3.up));

        //Set up meteor stats for moving and damage
        PrefabMove meteorStats = meteor.GetComponent<PrefabMove>();
        meteorStats.SetPrefabDamage((int)_fDamage);
        meteorStats.SetPrefabSpeed(_fSpeed.Value);
        meteorStats.SetPrefabDuration(_fDuration, false);
    }

}