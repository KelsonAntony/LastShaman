using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class PrefabMove : MonoBehaviour
{
    //Using nullable values as a prefab might not need to use one of these values
    private int? _iDamage;
    private float? _fSpeed;
    private int? _iHealth;
    private float _fDuration;
    private bool _bTicks;
    private float? _fTickTimer;
    private float? _fTickTrack;
    private Transform _transform;
    private Rigidbody _rb;

    // Start is called before the first frame update
    private void Start()
    {
        //Get the transform of the object
        _transform = gameObject.transform;

        //Add rigidbody component
        gameObject.AddComponent<Rigidbody>();
        _rb = gameObject.GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        _rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Make sure that speed isn't a null value and move the object forward
        if (_fSpeed.HasValue)
        {
            _transform.Translate(Vector3.forward * _fSpeed.Value * Time.deltaTime);
        }

        //If duration is greater than zero
        if (_fDuration > 0f)
        {
            _fDuration -= Time.deltaTime;

            //If tick tracker isn't null increase by time delta
            if (_fTickTrack.HasValue)
            {
                _fTickTrack += Time.deltaTime;
            }

            //If duration is less than the smallest float
            if ( _fDuration < Mathf.Epsilon ) 
            {
                Destroy(gameObject);
            }
        }
    }

    //For objects that don't have character controller component
    private void OnTriggerEnter(Collider other)
    {
        //Make sure damage as a value
        if (_iDamage.HasValue)
        {
            //Try and find the component for the health 
            if (other.gameObject.TryGetComponent(out Healthbar healthbar))
            {
                //Enemy Take Damage
            }
            //Find the component if the object is a prefrab
            else if (other.gameObject.TryGetComponent(out PrefabMove a_object))
            {
                a_object.PrefabTakeDamage(1);
            }

            //If the skill doesn't have damage over time then destroy on collision
            if (!_bTicks)
            {
                //Destroy the prefab
                Destroy(gameObject);
            }
        }
    }

    //Player and Enemy characters will have character controllers not rigidbody / collider components
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_iDamage.HasValue)
        {
            //Try and find the component fort the health
            if (hit.gameObject.TryGetComponent(out Healthbar healthbar))
            {
                //Take Damage
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //If the tick tracker is higher than the timer
        if (_fTickTrack >= _fTickTimer)
        {
            _fTickTrack = 0f;
            //If the prefab has a damage value try and get the health component
            if (_iDamage.HasValue && other.gameObject.TryGetComponent(out Healthbar healthbar))
            {
                //Enemy Take Damage
            }
        }
    }

    //Functions to control how the prefabs values can be used
    public void SetPrefabDamage(int a_iDamage)
    {
        _iDamage = a_iDamage;
    }
    public void SetPrefabSpeed(float a_fSpeed)
    {
        _fSpeed = a_fSpeed;
    }
    public void SetPrefabHealth(int a_iHealth)
    {
        _iHealth = a_iHealth;
    }
    public void SetPrefabDuration(float a_fDuration, bool a_bTicks)
    {
        _fDuration = a_fDuration;
        _bTicks = a_bTicks;
        _fTickTimer = 1f;
        _fTickTrack = 0f;
    }
    public void PrefabTakeDamage(int a_iTakeDamage)
    {
        _iHealth -= a_iTakeDamage;

        if (_iHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
