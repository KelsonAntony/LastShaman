using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _absorbBar;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _fill;

    //Subscribe to events when enabled and remove from events when disabled
    private void OnEnable()
    {
        PlayerManager.onPlayerDeath += DisableUI;
    }
    private void OnDisable()
    {
        PlayerManager.onPlayerDeath -= DisableUI;
    }

    //Set health and change fill color to match the point on the gradient depending on the slider value
    //Normalised vaule goes between 0 and 1 (0 - 100%)
    public void SetHealth(int health)
    {
        _healthBar.value = health;

        _fill.color = _gradient.Evaluate(_healthBar.normalizedValue);
    }

    public void SetAbsorb(int iAborb)
    {
        _absorbBar.value = iAborb;
    }

    //Set max health and set the gradient color to 100% (Far right)
    public void SetHealthMax(int health)
    {
        _healthBar.maxValue = health;
        _healthBar.value = health;
        _absorbBar.maxValue = health;

        _fill.color = _gradient.Evaluate(1);
    }

    private void DisableUI()
    {
        gameObject.SetActive(false);
    }
}
