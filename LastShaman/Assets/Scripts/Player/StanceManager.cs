using UnityEngine;
using SkillTypes;

/*
 * +++++++
 * 
 * Holding data to do with player stance
 * 
 * +++++++
 */

public class StanceManager : MonoBehaviour
{
    //Unity event delgates, static so other scripts can referance
    public delegate void PlayerStanceChange(ElementType a_element, int a_iStanceNo);
    public static event PlayerStanceChange playerStanceChange;

    //Private Variables handling stance controls
    public float fStanceDelay;
    private float _fStanceTime;
    private ElementType _stance;
    private bool _bFirstStance;
    private bool _bSecondStance;

    // Start is called before the first frame update
    void Start()
    {
        //Stance
        fStanceDelay = 1f;
        _fStanceTime = 0f;
        _stance = ElementType.Fire | ElementType.DualSkill;
        _bFirstStance = false;
        _bSecondStance = false;

        //Invoke on start so buttons aren't empty
        playerStanceChange?.Invoke(_stance, 1);
        playerStanceChange?.Invoke(_stance, 2);
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
            StanceChange(ElementType.Fire);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StanceChange(ElementType.Frost);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StanceChange(ElementType.Earth);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StanceChange(ElementType.Air);
        }

    }

    //Change the stance of the player. Checks to make sure that the combination is viable.
    private ElementType StanceCheck(ElementType a_element, ElementType a_stance)
    {
        ElementType newStance;

        //Check stance against invalid combinations
        if ((a_stance | a_element) != (ElementType.Fire | ElementType.Air)
            && (a_stance | a_element) != (ElementType.Earth | ElementType.Frost))
        {
            newStance = a_stance |= a_element;

            //Determine if the first or second stance is being called
            if (!_bSecondStance)
            {
                playerStanceChange?.Invoke(newStance, 1);
                //Invoke twice so all 4 abilities get changed (less clunky)
                playerStanceChange?.Invoke(ElementType.None, 2);
            }
            else
            {
                playerStanceChange?.Invoke(newStance, 2);
            }

            return newStance;
        }
        //Return current stance if combination is invalid
        else
        {
            return a_stance;
        }
    }

    public void StanceChange(ElementType a_element)
    {
        //If you aren't in first or second stance - clear stance and set 1st stance flag 
        if (!_bFirstStance && !_bSecondStance)
        {
            _bFirstStance = true;
            _stance = ElementType.None;
            _stance = StanceCheck(a_element, _stance);
        }
        // If in first stance and not second stance, add 2nd stance flag
        else if (_bFirstStance && !_bSecondStance &&
            (_stance & a_element) == ElementType.None)
        {
            _bSecondStance = true;
            _stance = StanceCheck(a_element, _stance);
        }
        //If in first stance and not second (after check if you've selected the same) set DualSame flag
        else if (_bFirstStance && !_bSecondStance)
        {
            _bSecondStance = true;
            _stance |= ElementType.DualSkill;
            //Need to Invoke here as I don't call StanceCheck
            playerStanceChange?.Invoke(_stance, 2);
        }
        Debug.Log("Current Stance:" + _stance);
    }

    //Force a stance change with no checks.  Use Carefully.
    private ElementType ForcedStanceChange(ElementType a_Element, ElementType a_Stance)
    {
        ElementType newStance;

        newStance = a_Stance |= a_Element;
        playerStanceChange?.Invoke(newStance, 1);
        return newStance;
    }
}