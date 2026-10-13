using UnityEngine;
using SkillTypes;
using UnityEngine.UI;

public class SkillsManager : MonoBehaviour
{
    //2D Array to hold skill types.  Number of skills is known makes it easer to access skills directly
    public Skills[,] aSkills = new Skills[4,6];
    public Image[,] skillImage = new Image[4,6];

    //Hold and assign prefabs
    [SerializeField] private GameObject[] _skillPrefab;

    //Private Variables handling damage modifiers
    private float[] _afElementDamageMod = new float[4];

    //Holding element reference indexs
    public readonly int iFire = 0;
    public readonly int iFrost = 1;
    public readonly int iEarth = 2;
    public readonly int iAir = 3;

    //Holding Skills slot reference indexs
    public readonly int iSkill1 = 0;
    public readonly int iSkill2 = 1;
    public readonly int iSkill3 = 2;
    public readonly int iSkillDualSame = 3;
    public readonly int iSkillDualType1 = 4;
    public readonly int iSkillDualType2 = 5;

    private void Awake()
    {
        //Fire Skills ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        aSkills[iFire, iSkill1] = gameObject.AddComponent<Fireball>();
        aSkills[iFire, iSkill2] = gameObject.AddComponent<FireShield>();
        aSkills[iFire, iSkill3] = gameObject.AddComponent<FireSpeed>();
        aSkills[iFire, iSkillDualSame] = gameObject.AddComponent<Pyro>();
        aSkills[iFire, iSkillDualType1] = gameObject.AddComponent<Meteor>();
        // aSkills[_iFire, _iSkillDualType2] = new SkillFireFrost

        //Frost Skills +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // aSkills[_iAir, _iSkillDualType1] = gameObject.AddComponent<LightningBolt>();
        // aSkills[_iFire, _iSkillDualType2] = new SkillFireFrost

        //Earth Skills +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        aSkills[iEarth, iSkill2] = gameObject.AddComponent<EarthWall>();
        aSkills[iEarth, iSkill1] = aSkills[iEarth, iSkill2];
        aSkills[iEarth, iSkill3] = aSkills[iEarth, iSkill2];
        aSkills[iEarth, iSkillDualSame] = aSkills[iEarth, iSkill2];
        aSkills[iEarth, iSkillDualType1] = aSkills[iFire, iSkillDualType1];
        //aSkills[_iEarth, _iSkillDualType2] = new SkillEarthAir

        //Air Skills +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // aSkills[_iAir, _iSkillDualType1] = gameObject.AddComponent<LightningBolt>();
        //aSkills[_iEarth, _iSkillDualType2] = new SkillEarthAir
    }

    private void Start()
    {
        //Adding prefabs to skills that need them
        aSkills[iFire, iSkill1].SetPrefab(_skillPrefab[(int)PrefabType.FIREBALL]);
        aSkills[iFire, iSkill3].SetPrefab(_skillPrefab[(int)PrefabType.FIRESPEED]);
        aSkills[iFire, iSkillDualSame].SetPrefab(_skillPrefab[(int)PrefabType.PYRO]);
        aSkills[iFire, iSkillDualType1].SetPrefab(_skillPrefab[(int)PrefabType.METEOR]);
        aSkills[iEarth, iSkill2].SetPrefab(_skillPrefab[(int)PrefabType.EARTHWALL]);

        //Fire , Frost, Earth, Air damage modifiers
        for (int i = 0;i < _afElementDamageMod.Length;i++)
        {
            _afElementDamageMod[i] = 1;
        }

    }

    //Add overall damage
    public void AddDamage(float a_fMod)
    {
        for (int i = 0;i < _afElementDamageMod.Length;i++)
        {
            _afElementDamageMod[i] *= a_fMod;
        }
    }
    //Add damage to a specific element
    public void AddDamageOfType(int a_fMod, ElementType a_eElement)
    {

        if ((a_eElement & ElementType.Fire) == ElementType.Fire)
        {
            _afElementDamageMod[iFire] *= a_fMod;
        }

        if ((a_eElement & ElementType.Frost) == ElementType.Frost)
        {
            _afElementDamageMod[iFrost] *= a_fMod;
        }

        if ((a_eElement & ElementType.Earth) == ElementType.Earth)
        {
            _afElementDamageMod[iEarth] *= a_fMod;
        }

        if ((a_eElement & ElementType.Air) == ElementType.Air)
        {
            _afElementDamageMod[iAir] *= a_fMod;
        }
    }

    //Remove overall damage
    public void RemoveDamage(float a_fMod)
    {
        for (int i = 0;i < _afElementDamageMod.Length;i++)
        {
            _afElementDamageMod[i] /= a_fMod;
        }
    }
    //Remove damage to a specific element
    public void RemoveDamageOfType(float a_fMod, ElementType a_eElement)
    {
        if ((a_eElement & ElementType.Fire) == ElementType.Fire)
        {
            _afElementDamageMod[iFire] /= a_fMod;
        }

        if ((a_eElement & ElementType.Frost) == ElementType.Frost)
        {
            _afElementDamageMod[iFrost] /= a_fMod;
        }

        if ((a_eElement & ElementType.Earth) == ElementType.Earth)
        {
            _afElementDamageMod[iEarth] /= a_fMod;
        }

        if ((a_eElement & ElementType.Air) == ElementType.Air)
        {
            _afElementDamageMod[iAir] /= a_fMod;
        }
    }

    //Get damage modifer of a specfic type
    //If type is dual type get damage modifier of both but halfed
    public float GetDamageModOfType(ElementType a_eElement)
    {
        float fMod = 0f;

        if ((a_eElement & ElementType.Volcanic) == ElementType.Volcanic ||
            (a_eElement & ElementType.Water) == ElementType.Water)
        {
            fMod += _afElementDamageMod[iFire] / 2;
        }
        else if ((a_eElement & ElementType.Fire) == ElementType.Fire)
        {
            fMod += _afElementDamageMod[iFire];
        }

        if ((a_eElement & ElementType.Water) == ElementType.Water ||
            (a_eElement & ElementType.Lighting) == ElementType.Lighting)
        {
            fMod += _afElementDamageMod[iFrost] / 2;
        }
        else if ((a_eElement & ElementType.Frost) == ElementType.Frost)
        {
            fMod += _afElementDamageMod[iFrost];
        }

        if ((a_eElement & ElementType.Volcanic) == ElementType.Volcanic ||
           (a_eElement & ElementType.Tempest) == ElementType.Tempest)
        {
            fMod += _afElementDamageMod[iEarth] / 2;
        }
        else if ((a_eElement & ElementType.Earth) == ElementType.Earth)
        {
            fMod += _afElementDamageMod[iEarth];
        }

        if ((a_eElement & ElementType.Tempest) == ElementType.Tempest ||
           (a_eElement & ElementType.Lighting) == ElementType.Lighting)
        {
            fMod += _afElementDamageMod[iAir] / 2;
        }
        else if ((a_eElement & ElementType.Air) == ElementType.Air)
        {
            fMod += _afElementDamageMod[iAir];
        }

        if (fMod > 1)
        {
            return fMod;
        }
        else
        {
            return 1;
        }
    }
}
