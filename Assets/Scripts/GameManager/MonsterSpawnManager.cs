using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    public static MonsterSpawnManager Instance { get; private set; }

    private static Transform[,] Gragh;
    private static string MonsterPath = @"Monsters/";
    private static string objName = "Monster";

    public static void SetMonster()
    {
        Gragh = CreateMap.GetGragh();

        Transform Monsters;
        List<Transform> Childs = new List<Transform>();
        Transform Child;
        Transform Resource;
        Transform Monster;

        for (int i = 0; i < Mathf.Sqrt(Gragh.Length); ++i)
        {
            for (int j  = 0; j < Mathf.Sqrt(Gragh.Length); ++j)
            {
                if (Gragh[i, j] != null) {
                    Debug.Log(Gragh[i, j].name);
                    if (Gragh[i, j].CompareTag(CreateMap.GetTag()))
                    {
                        int k = 0;

                        Monsters = GetMonsters(Gragh[i, j]);

                        if (Monsters != null)
                        {
                            for (k = 0; k < Monsters.childCount; ++k)
                            {
                                Child = Monsters.GetChild(0);
                                Childs.Add(Child);
                            }

                            /*while (Childs.Count != 0)
                            {
                                Child = Childs[0];

                                Resource = Resources.Load<Transform>(MonsterPath + Child.tag);

                                if (Resource != null)
                                {
                                    Monster = Instantiate(Resource);

                                    Monster.position = Child.localPosition;
                                    Monster.localScale = Child.lossyScale;
                                    Monster.SetParent(Monsters);

                                    Destroy(Monster.gameObject);

                                    Childs.RemoveAt(0);
                                }
                            }*/

                            k = 0;

                            while (k < Childs.Count || Childs.Count != 0)
                            {
                                Child = Childs[k];

                                Resource = Resources.Load<Transform>(MonsterPath + Child.tag);

                                if (Resource != null)
                                {
                                    Monster = Instantiate(Resource);

                                    Monster.position = Child.localPosition;
                                    Monster.localScale = Child.lossyScale;
                                    Monster.SetParent(Monsters);

                                    Destroy(Child.gameObject);

                                    Childs.RemoveAt(0);
                                }
                                else
                                {
                                    k++;
                                }
                            }
                        }
                    }
                }
            }
        }
    } 

    private static Transform GetMonsters(Transform trans)
    {
        Transform Monsters = null;

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
