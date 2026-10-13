using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillTypes;

public class StanceImage : MonoBehaviour
{
    [SerializeField] private Sprite[] stanceSprites = new Sprite[9];
    [SerializeField] private Image imageSource;

    //Holding element reference indexs
    private readonly int _iFire = 0;
    private readonly int _iFrost = 1;
    private readonly int _iEarth = 2;
    private readonly int _iAir = 3;
    private readonly int _iVolanic = 4;
    private readonly int _iWater = 5;
    private readonly int _iLighting = 6;
    private readonly int _iTempest = 7;
    private readonly int _iElemental = 8;

    //Subscribe to events / Unsubscribe to events
    private void OnEnable()
    {
        StanceManager.playerStanceChange += NewStance;
    }
    private void OnDisable()
    {
        StanceManager.playerStanceChange -= NewStance;
    }

    private void Start()
    {
        //Make the color clear is there's no sprite
        if (imageSource.sprite == null)
        {
            imageSource.color = Color.clear;
        }
    }

    public void NewStance(ElementType a_element, int a_iStance)
    {
        int index = 0;
        
        //Switch case as I don't need the control of nested IFs
        switch(a_element)
        {
            case ElementType.Fire: index = _iFire; break;

            case ElementType.Frost: index = _iFrost; break;

            case ElementType.Earth: index = _iEarth; break;

            case ElementType.Air: index = _iAir; break;

            case ElementType.Volcanic: index = _iVolanic; break;

            case ElementType.Water: index = _iWater; break;

            case ElementType.Lighting: index = _iLighting; break;

            case ElementType.Tempest: index = _iTempest; break;

            case ElementType.Elemental: index = _iElemental; break;

            //default case just breaks switch
            default: break;
        }

        //If the color is clear
        if (imageSource.color == Color.clear)
        {
            //Make the color white so the sprite is fully visable
            imageSource.color = Color.white;
        }

        //Image becomes one of the images in the array
        imageSource.sprite = stanceSprites[index];
    }
}
