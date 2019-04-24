using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    public static MonsterSpawnManager Instance { get; private set; }

    private static Transform[,] Gragh;
    private static string MonsterPath = @"Monsters/";
    private static string objName = "Monster";

    private static void SetMonster()
    {
        Gragh = CreateMap.GetGragh();
        Transform Monsters;
        Transform Child;

        for (int i = 0; i < CreateMap.GetRoomFloor() * 4; ++i)
        {
            for (int j  = 0; j < CreateMap.GetRoomFloor() * 4; ++i)
            {
                if (Gragh[i, j]) {
                    if (Gragh[i, j].CompareTag(CreateMap.GetTag()))
                    {
                        Monsters = GetMonsters(Gragh[i, j]);

                        for (int k = 0; k < Monsters.childCount; ++k)
                        {
                            Child = Monsters.GetChild(k);
                            if (Child) {
                                Transform Monster = Instantiate(Resources.Load<Transform>(MonsterPath + Child.tag));
                                Monster.localPosition = Child.localPosition;
                                Monster.localScale = Child.lossyScale;
                            }
                        }
                    }
                }
            }
        }
    }

    private static Transform GetMonsters(Transform trans)
    {
        Transform Monsters = trans;
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
