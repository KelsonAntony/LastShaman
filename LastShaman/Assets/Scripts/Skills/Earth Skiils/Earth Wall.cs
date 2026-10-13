using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillTypes;

public class EarthWall : Skills
{
    //Vectors
    private Vector3 _prefabSpawnLocation;
    private Vector3 _targetLocation;

    //Health
    int iWallHealth;

    // Start is called before the first frame update
    void Start()
    {
        sName = "Earth Wall";
        _sDescription = "Summon a wall of earth";
        _fCooldown = 15f;
        _fCastTime = 0f;
        _fSpeed = null;
        _eAttackType = AttackType.Defensive;
        _eDamageType = ElementType.Earth;
        _fLastUsedTime = -_fCooldown;
        
        iWallHealth = 3;
    }

    public override void ActivateSkill()
    {
        _fLastUsedTime = Time.time;

        //Use raycast to determine where to send the prefab object
        _prefabSpawnLocation = _player.transform.position + (Vector3.up * 10);
        _targetLocation = Vector3.zero;
        RaycastHit rayHit;
        Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Set the target location to where the ray collides with an object
        if (Physics.Raycast(screenRay, out rayHit))
        {
            _targetLocation = rayHit.point;
        }

        CreatePrefab();
        base.ActivateSkill();
    }

    private void CreatePrefab()
    {
        //Set the object at the mouse position then rotate the object to loot at player
        GameObject earthwall = Instantiate(_prefabObject, _targetLocation, Quaternion.identity);
        earthwall.transform.LookAt(_player.transform);

        //Give certain stats to the Prefab object so its own methods can handle how it acts
        PrefabMove earthwallstats = earthwall.GetComponent<PrefabMove>();
        earthwallstats.SetPrefabHealth(iWallHealth);
        earthwallstats.SetPrefabDuration(_fDuration, false);
    }
}
