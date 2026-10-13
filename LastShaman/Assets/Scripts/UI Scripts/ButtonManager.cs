using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{ 
    [SerializeField] private Slider cooldownSlider;
    private string sSkillText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.ShowTooltip(sSkillText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.HideTooltip();
    }

    public void UpdateCooldown(float a_fCooldownMax, float a_fCooldownCurrent)
    {
        if (a_fCooldownCurrent > 0)
        {
            cooldownSlider.value = a_fCooldownCurrent / a_fCooldownMax;
        }
        else
        {
            a_fCooldownCurrent = 0;
            cooldownSlider.value = a_fCooldownCurrent;
        }
    }

    public void SkillInfo(string a_sSkillInfo)
    {
        sSkillText = a_sSkillInfo;
    }
}
