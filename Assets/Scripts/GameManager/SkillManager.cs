using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public static int UseSkillNumber;
    public delegate bool SkillFunc();
    [HideInInspector]
    public SkillObject Skill1;
    public SkillNumber Skill1Number;
    public Image       Skill1Image;
    [HideInInspector]
    public SkillObject Skill2;
    public SkillNumber Skill2Number;
    public Image       Skill2Image;
    [HideInInspector]
    public SkillObject Skill3;
    public SkillNumber Skill3Number;
    public Image       Skill3Image;
    [HideInInspector]
    public SkillObject Skill4;
    public SkillNumber Skill4Number;
    public Image       Skill4Image;

    public SkillFunc   Attack;

    private SkillDatabase SkillDatabaseInstance
    {
        get
        {
            return SkillDatabase.Instance;
        }
    }
    private InventoryManager InventoryManagerInstance
    {
        get
        {
            return InventoryManager.Instance;
        }
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Skill1 = SkillDatabaseInstance.Skills[(int)Skill1Number];
        Skill2 = SkillDatabaseInstance.Skills[(int)Skill2Number];
        Skill3 = SkillDatabaseInstance.Skills[(int)Skill3Number];
        Skill4 = SkillDatabaseInstance.Skills[(int)Skill4Number];

        Skill1Image.sprite = Skill1.SkillImage;
        Skill2Image.sprite = Skill2.SkillImage;
        Skill3Image.sprite = Skill3.SkillImage;
        Skill4Image.sprite = Skill4.SkillImage;
    }

    public void UseSkill(int Num)
    {
        UseSkillNumber = Num - 1;
        switch(Num)
        {
            case 1:
                {
                    Skill1.Skill();
                }
                break;
            case 2:
                {
                    Skill2.Skill();
                }
                break;
            case 3:
                {
                    Skill3.Skill();
                }
                break;
            case 4:
                {
                    Skill4.Skill();
                }
                break;
            default:
                {

                }
                break;
        }
    }

    public void UseAttack()
    {
        Attack();
    }

    public void OpenInventory()
    {
        InventoryManagerInstance.OpenInventory();
    }
}
