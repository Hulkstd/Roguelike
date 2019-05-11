using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;

public class SkillDatabase : MonoBehaviour
{
    public static SkillDatabase Instance { get; private set; }

    [SerializeField]
    public List<SkillObject> Skills;

    void Awake()
    {
        Instance = this;
    }

    public void BuildDatabase()
    {
        Skills = JsonConvert.DeserializeObject<List<SkillObject>>(Resources.Load<TextAsset>("Json/Skill").ToString());
    }
}

[System.Serializable]
public class SkillObject
{
    public SkillObject(string SkillName, string SkillImagePath, string AnimatorPath, string SkillFuncName, float OriginalCooltime, int Damage)
    {
        this.SkillName = SkillName;
        this.SkillImage = Resources.Load<Sprite>(SkillImagePath);
        this.Animator = Resources.Load<Animator>(AnimatorPath);
        this.Skill = UseSkill;
        //this.Skill += ;// 나중에 고처야할 코드
        this.OriginalCooltime = OriginalCooltime;
        this.Damage = Damage;
    }

    public string SkillName;
    public Sprite SkillImage;
    public Animator Animator;
    public SkillManager.SkillFunc Skill;
    public float OriginalCooltime;
    public float Cooltime;
    public int Damage;

    public float CoolRatio()
    {
        if (Cooltime <= 0)
            return 0;

        float value = ((OriginalCooltime - Cooltime) / OriginalCooltime);

        return value;
    }

    private bool UseSkill()
    {
        if(Cooltime <= 0)
        {
            Cooltime = OriginalCooltime;
            return true;
        }
        return false;
    }
}

public enum SkillNumber : short
{
    Test = 0,
    Test2 = 1
}
