using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraghType = System.Collections.Generic.List<System.Collections.Generic.List<CreateMap.ListParam>>;

[AddComponentMenu("GameManager/MonsterSpawnManager")]
public class MonsterSpawnManager : MonoBehaviour
{
    public static MonsterSpawnManager Instance { get; private set; }

    private static GraghType Gragh;
    private static string MonsterPath = @"Monsters/";
    private static string objName = "Monster";

    public static void SetMonster()
    {
        Transform Monsters;
        Transform Monster;
        Transform IsNotNull;
        List<GameObject> list = new List<GameObject>();

        Gragh = CreateMap.GetGragh();

        foreach (List<CreateMap.ListParam> List in Gragh)
        {
            foreach (CreateMap.ListParam ListParam in List)
            {
                if (ListParam != null)
                {
                    Monsters = GetMonsters(ListParam.trans);

                    if (Monsters != null)
                    {
                        for (int i = 0; i < Monsters.childCount; ++i) { list.Add(Monsters.GetChild(i).gameObject); }

                        while (list.Count > 0)
                        {
                            if (list[0] != null) {
                                IsNotNull = Resources.Load<Transform>(MonsterPath + list[0].tag);
                                if (IsNotNull != null)
                                {
                                    Monster = Instantiate(IsNotNull);
                                    Monster.SetParent(ListParam.trans);
                                    Monster.position = list[0].transform.position;
                                    Monster.rotation = list[0].transform.rotation;
                                    Monster.localScale = list[0].transform.lossyScale;
                                }

                                Destroy(list[0]);
                            }

                            list.RemoveAt(0);
                        }
                    }
                }
            }
        }

    }



    private static Transform GetMonsters(Transform trans)
    {
        Transform Monsters = null;

        if (trans == null)
            return null;

        for (int i = 0; i < trans.childCount; ++i)
        { 
            if (trans.GetChild(i).name == objName)
            {
                Monsters = trans.GetChild(i);
                break;
            }
        }

        return Monsters;
    }

    private void Awake()
    {
        Instance = this;
    }

}
