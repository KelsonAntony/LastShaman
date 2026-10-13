using UnityEngine;
using UnityEngine.UI;
using SkillTypes;
using TMPro;

public class ActionbarManager : MonoBehaviour
{
    //UI Buttons
    [SerializeField] private Button[] _buttons;

    //UI Slider
    [SerializeField] private Slider _castBar;

    //Referance Scripts for cooldown tracking
    private ButtonManager[] _buttonManager = new ButtonManager[4];

    //Holding player values
    private ElementType _playerStance1;
    private ElementType _playerStance2;

    //To access the array
    private int _iElement;
    private int _iSkillSlot;
    private readonly int _iSkillLength = 4;

    //Script referance
    [SerializeField] private SkillsManager _skillManager;

    //Holding skill referances
    private Skills[] _skills = new Skills[4];

    //Check to see if the player is already casting
    private bool _bPlayerIsCasting;
    private bool _bFirstLoad = true;

    //Subscribe / Unsubscribe from events
    private void OnEnable()
    {
        StanceManager.playerStanceChange += ChangeButtonSkills;
        
    }
    private void OnDisable()
    {
        StanceManager.playerStanceChange -= ChangeButtonSkills;
    }

    private void Start()
    {
        //Set up each button with a onClick Listener (using Lambda) and a referance to the button manager component
        for (int i = 0; i < _iSkillLength; i++)
        {
            //variable to hold index so the lambda can resolve correctly even if the loop concludes
            int iCurrentIndex = i;
            _buttons[i].onClick.AddListener(() => _skills[iCurrentIndex].ActivateSkill());
        }
        _bPlayerIsCasting = false;
    }

    private void Update()
    {
        //Check input and if skill is off cooldown, if on cooldown update cooldown script
        if(Input.GetKeyDown(KeyCode.Q))
        {
            CheckAndUseSkill(0);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            CheckAndUseSkill(1);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            CheckAndUseSkill(2);
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            CheckAndUseSkill(3);
        }

        for(int i = 0; i < _iSkillLength; i++)
        {
            if (_skills[i].IsOnCooldown())
            {
                _buttonManager[i].UpdateCooldown(_skills[i].GetCooldown(), _skills[i].GetCooldownTimeRemaining());
            }
        }

        //If the cast bar as a max value, and the cast bar value is the max value, disable the cast bar
        //Otherwise if the cast bar has a max value, increase value by delta time
        if (_castBar.maxValue > 0 && _castBar.value == _castBar.maxValue)
        {
            _bPlayerIsCasting = false;
            _castBar.maxValue = 0;
            _castBar.value = 0;
            _castBar.gameObject.SetActive(false);
        }
        else if (_castBar.maxValue > 0)
        {
            _bPlayerIsCasting = true;
            _castBar.value += Time.deltaTime;
        }
    }

    //Set up which skills the buttons should be activating
    private void ChangeButtonSkills(ElementType a_element, int a_iStanceNo)
    {
        //First stance controls first three buttons
        if (a_iStanceNo == 1)
        {
            _playerStance1 = a_element;
            _playerStance2 = ElementType.None;
        }
        //Second stance controls the 4th button
        else if (a_iStanceNo == 2)
        {
            _playerStance2 = a_element;
        }

        //Switch to get the primary element type
        switch (_playerStance1)
        {
            case ElementType.Fire : _iElement = _skillManager.iFire; break;

            case ElementType.Frost : _iElement = _skillManager.iFrost; break;

            case ElementType.Earth : _iElement = _skillManager.iEarth; break;

            case ElementType.Air : _iElement = _skillManager.iAir; break;

            default: break;
        }

        //Nested ifs for conditional for secondary element type (4th skill)
        if (_playerStance2 == ElementType.DualSkill || _playerStance2 == ElementType.None)
        {
            _iSkillSlot = _skillManager.iSkillDualSame;
        }
        else if (_playerStance2 == ElementType.Volcanic || _playerStance2 == ElementType.Lighting)
        {
            _iSkillSlot = _skillManager.iSkillDualType1;
        }
        else if (_playerStance2 == ElementType.Water || _playerStance2 == ElementType.Tempest)
        {
            _iSkillSlot = _skillManager.iSkillDualType2;
        }

        for (int i = 0; i < _iSkillLength; i++)
        {
            string sInfo;

            if (i < _iSkillLength - 1)
            {
                _skills[i] = _skillManager.aSkills[_iElement, i];
                sInfo = _skills[i].DisplaySkillInfo();
            }
            else
            {
                _skills[i] = _skillManager.aSkills[_iElement, _iSkillSlot];
                sInfo = _skills[i].DisplaySkillInfo();
            }

            if (!_bFirstLoad)
            {
                _buttonManager[i].UpdateCooldown(_skills[i].GetCooldown(), _skills[i].GetCooldownTimeRemaining());
                _buttonManager[i].SkillInfo(sInfo);
            }
            //Setting up button manager here on first load as this is called before start on occasion from player script
            else
            {
                _buttonManager[i] = _buttons[i].GetComponent<ButtonManager>();
                _buttonManager[i].SkillInfo(sInfo);
            }

            ChangeButtonText(_buttons[i], _skills[i]);
        }
        _bFirstLoad = false;
    }

    private void ChangeButtonText(Button a_button, Skills a_skill)
    {
        //Get the text componet and set it to the skill name
        a_button.GetComponentInChildren<TextMeshProUGUI>().text = a_skill.sName;
    }

    private void StartCastBar(float a_fCastTime)
    {
        //If there's no cast time, don't start the cast bar
        if (a_fCastTime < Mathf.Epsilon)
        {
            return;
        }

        //If gameobject tied to the cast bar is disabled, enable it
        if (_castBar.gameObject.activeSelf == false)
        {
            _castBar.gameObject.SetActive(true);
            _castBar.maxValue = a_fCastTime;
            _castBar.value = 0;
        }
    }

    private void CheckAndUseSkill(int a_iskill)
    {
        if (!_skills[a_iskill].IsOnCooldown() && !_bPlayerIsCasting)
        {
            StartCastBar(_skills[a_iskill].GetCastTime());
            _buttons[a_iskill].onClick?.Invoke();
        }
    }
}

