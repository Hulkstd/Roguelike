using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{

    public static CreateMap Instance { get; private set; }

#region Enums
    private enum MapCode { Mordern = 0, Elite = 1, Shop = 2, HealSpot = 3, Boss = 4 } // 2~4는 하나씩만 1은 2개까지만
    private enum Direction { Stand = 0, Top = 1, Right = 2, Buttom = 3, Left = 4 }
#endregion

#region MapCreateSurportValues
    private bool[,] Gragh;
    private int GraghI, GraghJ;
    private List<MapCode> list = new List<MapCode>(); // 랜덤으로 쉽게 넣기위한 리스트
    private Vector3 LastMapPos;
    private int RoomCount;
#endregion

#region InputValues
    [SerializeField]
    private static int StageNum;
    [SerializeField]
    private Queue<MapCode> Maps = new Queue<MapCode>();
#endregion

#region Resourse
    private Transform[] MordernMapsBasic = Resources.LoadAll<Transform>(@"TileMaps/Basic/Stage" + StageNum);
    private Transform[] MordernMapsLage = Resources.LoadAll<Transform>(@"TileMaps/Lage/Stage" + StageNum);
    private Transform[] ShopMaps = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/Shop");
    private Transform[] HealSpotMaps = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/HealSpot");
    private Transform[] BossMaps = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/Boss");
#endregion

    private void SetQueue()
    {
        RoomCount = StageNum * 9;

        Gragh = new bool[RoomCount * 4, RoomCount * 4];

        for (int i = 0; i < RoomCount - 4; ++i)
        {
            list.Add(MapCode.Mordern);
        }
        list.Add(MapCode.Elite);
        list.Add(MapCode.Shop);
        list.Add(MapCode.HealSpot);

        Maps.Enqueue(MapCode.Mordern);
        while (list.Count > 0)
        {
            int random = Random.Range(0, list.Count - 1);
            Maps.Enqueue(list[random]);
            list.RemoveAt(random);
        }
        Maps.Enqueue(MapCode.Boss);
    }

    private void SetFloor()
    {
        Direction direction;
        bool IsLage;

        GraghI = RoomCount * 2 - 1;
        GraghJ = RoomCount * 2 - 1;

        IsLage = false;
        LastMapPos = transform.position;
        SetMaps(Maps.Dequeue(), GetDirection(0), false, IsLage);
        Gragh[GraghI, GraghJ] = false;

        while (Maps.Count > 0)
        {
            direction = GetDirection(Random.Range(1, 4));
            IsLage = GetLage();
            if (SetGragh(direction, IsLage))
                SetMaps(Maps.Dequeue(), direction, true, IsLage);
            else
            {
                if (Gragh[GraghI + 1, GraghJ] && Gragh[GraghI, GraghJ + 1] && Gragh[GraghI, GraghJ - 1] && Gragh[GraghI - 1, GraghJ])
                {
                    SetMapPos();
                }
                continue;
            }
        }
    }

    private void SetMaps(MapCode Code, Direction direction, bool InMonster, bool IsLage)
    {
        Transform obj;


        switch (Code)
        {
            case MapCode.Mordern:
                if (InMonster) obj = Instantiate(MordernMapsBasic[0]); // 0번째를 몬스터 없는 처음 시작맵으로 설정해주세요
                else obj = Instantiate(IsLage ? MordernMapsLage[Random.Range(0, MordernMapsLage.Length - 1)] : MordernMapsBasic[Random.Range(0, MordernMapsBasic.Length - 1)]);
                break;
            case MapCode.Elite:
                obj = Instantiate(BossMaps[Random.Range(0, BossMaps.Length - 1)]);
                break;
            case MapCode.Shop:
                InMonster = false;
                obj = Instantiate(ShopMaps[Random.Range(0, ShopMaps.Length - 1)]);
                break;
            case MapCode.HealSpot:
                InMonster = false;
                obj = Instantiate(HealSpotMaps[Random.Range(0, HealSpotMaps.Length - 1)]);
                break;
            case MapCode.Boss:
                obj = Instantiate(BossMaps[Random.Range(0, BossMaps.Length - 1)]);
                break;
            default:
                Debug.Log("SetMaps error");
                return;
        }
        if (Code != MapCode.Boss)
            obj.localPosition = GetMapPos(direction, IsLage);
        else
            obj.localPosition = GetMapPos(direction, true);

        obj.SetParent(transform);
    }

    private void SetMapPos()
    {
        for (int i = 0; i < RoomCount * 4; ++i)
        {
            for (int j = 0; j < RoomCount * 4; ++j)
            {
                if (!Gragh[i, j])
                {
                    GraghI = i;
                    GraghJ = j;
                    return;
                }
            }
        }
    }

    private bool SetGragh(Direction direction, bool IsLage)
    {
        if (IsLage)
        {
            switch (direction)
            {
                case Direction.Top:
                    if (Gragh[GraghI - 1, GraghJ] && Gragh[GraghI - 1, GraghJ - 1] && Gragh[GraghI - 2, GraghJ - 1] && Gragh[GraghI - 2, GraghJ]) return false;
                    else
                    {
                        Gragh[--GraghI, GraghJ] = false;
                        Gragh[GraghI, --GraghJ] = false;
                        Gragh[--GraghI, GraghJ] = false;
                        Gragh[GraghI, ++GraghJ] = false;
                    }
                    break;
                case Direction.Right:
                    if (Gragh[GraghI, GraghJ + 1] && Gragh[GraghI, GraghJ + 2] && Gragh[GraghI + 1, GraghJ + 2] && Gragh[GraghI + 1, GraghJ + 1]) return false;
                    else
                    {
                        Gragh[GraghI, ++GraghJ] = false;
                        Gragh[GraghI, ++GraghJ] = false;
                        Gragh[++GraghI, GraghJ] = false;
                        Gragh[GraghI, --GraghJ] = false;
                    }
                    break;
                case Direction.Buttom:
                    if (Gragh[GraghI + 1, GraghJ] && Gragh[GraghI + 1, GraghJ - 1] && Gragh[GraghI + 2, GraghJ - 1] && Gragh[GraghI + 2, GraghJ]) return false;
                    else
                    {
                        Gragh[++GraghI, GraghJ] = false;
                        Gragh[GraghI, --GraghJ] = false;
                        Gragh[++GraghI, GraghJ] = false;
                        Gragh[GraghI, ++GraghJ] = false;
                    }
                    break;
                case Direction.Left:
                    if (Gragh[GraghI + 1, GraghJ] && Gragh[GraghI + 1, GraghJ - 1] && Gragh[GraghI + 2, GraghJ - 1] && Gragh[GraghI + 2, GraghJ]) return false;
                    else
                    {
                        Gragh[++GraghI, GraghJ] = false;
                        Gragh[GraghI, --GraghJ] = false;
                        Gragh[++GraghI, GraghJ] = false;
                        Gragh[GraghI, ++GraghJ] = false;
                    }
                    break;
            }
        }
        else
        {
            switch (direction)
            {
                case Direction.Stand:
                    if (Gragh[GraghI, GraghJ]) return false;
                    else Gragh[GraghI, GraghJ] = false;
                    break;
                case Direction.Top:
                    if (Gragh[GraghI - 1, GraghJ]) return false;
                    else Gragh[--GraghI, GraghJ] = false;
                    break;
                case Direction.Right:
                    if (Gragh[GraghI, GraghJ + 1]) return false;
                    else Gragh[GraghI, ++GraghJ] = false;
                    break;
                case Direction.Buttom:
                    if (Gragh[GraghI + 1, GraghJ]) return false;
                    else Gragh[++GraghI, GraghJ] = false;
                    break;
                case Direction.Left:
                    if (Gragh[GraghI, GraghJ - 1]) return false;
                    else Gragh[GraghI, --GraghJ] = false;
                    break;
            }
        }
        return true;
    }

    private Direction GetDirection(int num)
    {
        switch (num)
        {
            case 0: return Direction.Stand;
            case 1: return Direction.Top;
            case 2: return Direction.Right;
            case 3: return Direction.Buttom;
            case 4: return Direction.Left;
            default:
                Debug.Log("GetDirectionError");
                return Direction.Stand;
        }
    }

    private Vector3 GetAddPos(Direction direction, bool IsLage)
    {
        switch (direction)
        {
            case Direction.Top:
                return IsLage ? new Vector3(-8.9f, 15) : new Vector3(0, 10);
            case Direction.Right:
                return IsLage ? new Vector3(26.7f, -5) : new Vector3(17.8f, 0);
            case Direction.Buttom:
                return IsLage ? new Vector3(-8.9f, -15) : new Vector3(0, -10);
            case Direction.Left:
                return IsLage ? new Vector3(-26.7f, -5) : new Vector3(-17.8f, 0);
            default:
                return new Vector3(0, 0);
        }
    }

    
    private Vector3 GetMapPos(Direction direction, bool IsLage)
    {
        Vector3 ReturnVector = LastMapPos;        

        switch (direction)
        {
            case Direction.Stand:
                break;
            case Direction.Top:
                 ReturnVector = LastMapPos + GetAddPos(direction, IsLage);
                break;
            case Direction.Right:
                ReturnVector = LastMapPos + GetAddPos(direction, IsLage);
                break;
            case Direction.Buttom:
                ReturnVector = LastMapPos + GetAddPos(direction, IsLage);
                break;
            case Direction.Left:
                ReturnVector = LastMapPos + GetAddPos(direction, IsLage);
                break;
            default:
                Debug.Log("GetMapPosError");
                return ReturnVector;
        }

        return ReturnVector;
    }

    
    private bool GetLage()
    {
        switch (Random.Range(1, 5))
        {
            case 1:
                return true;
            default:
                return false;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

}
