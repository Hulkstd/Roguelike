using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraghType = System.Collections.Generic.List<System.Collections.Generic.List<CreateMap.ListParam>>;

public class MonsterSpawnManager : MonoBehaviour
{
    public static MonsterSpawnManager Instance { get; private set; }

    private static GraghType Gragh;
    private static string MonsterPath = @"Monsters/";
    private static string objName = "Monster";

    public static void SetMonster()
    {
        Gragh = CreateMap.GetGragh();

        Transform Monsters;
        List<Transform> Childs = new List<Transform>();
        Transform Resource;
        Transform Monster;

        for (int i = 0; i < Gragh.Count; ++i)
        {
            for (int j = 0; j < Gragh[i].Count; ++j)
            {
                if (Gragh[i][j] != null)
                {
                    Monsters = GetMonsters(Gragh[i][j].trans);

                    if (Monsters != null)
                    {
                        for (int k = 0; k < Monsters.childCount; ++k)
                        {
                            Childs.Add(Monsters.GetChild(k));
                        }

                        while (Childs.Count > 0)
                        {
                            Resource = Resources.Load<Transform>(MonsterPath + Childs[0].tag);

                            if (Resource != null)
                            {
                                Monster = Instantiate(Resource);
                                Monster.SetParent(Monsters);
                                Monster.position = Childs[0].position;
                                Monster.localScale = Childs[0].lossyScale;
                                Monster.rotation = Childs[0].localRotation;
                            }

                            Destroy(Childs[0].gameObject);

                            Childs.RemoveAt(0);

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
