using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public delegate bool SkillFunc();
    public SkillObject Skill1;
    public SkillObject Skill2;
    public SkillObject Skill3;
    public SkillObject Skill4;
    public SkillFunc   Attack;

    public void Start()
    {
        Instance = this;
    }

    public void UseSkill(int Num)
    {
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
}
