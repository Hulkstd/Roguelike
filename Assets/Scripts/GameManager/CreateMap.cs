using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{

    public static CreateMap Instance { get; private set; }

#region Enums
    private enum MapCode { Modern = 0, Elite = 1, Shop = 2, HealSpot = 3, Boss = 4 } // 2~4는 하나씩만 1은 2개까지만 맵 구별 방법
    private enum Direction { Stand = 0, Top = 1, Right = 2, Buttom = 3, Left = 4 } // 맵 설치 방향
#endregion

#region MapCreateSurportValues
    private static Transform[,] Gragh;
    private int GraghI, GraghJ;
    private List<MapCode> list = new List<MapCode>(); // 랜덤으로 쉽게 넣기위한 리스트
    private Vector3 LastMapPos;
    private static int RoomFloor;
    private static string Tag = "InMonsterMap";
    private Queue<MapCode> Maps = new Queue<MapCode>();
    private bool IsPrevLarge;
#endregion

#region InputValues
    [SerializeField]
    private static int StageNum;
#endregion

#region Resourse
    private Transform[] ModernMapsBasic;
    private Transform[] ModernMapsLarge;
    private Transform[] ShopMaps;
    private Transform[] HealSpotMaps;
    private Transform[] BossMaps;
#endregion

#region SetFunctions
    private void SetQueue()
    {
        RoomFloor = StageNum * 9;

        Gragh = new Transform[RoomFloor * 4, RoomFloor * 4];

        for (int i = 0; i < RoomFloor * 4; ++i)
        {
            for (int j = 0; j < RoomFloor * 4; ++j)
            {
                Gragh[i, j] = transform;
            }
        }

        for (int i = 0; i < RoomFloor - 4; ++i)
        {
            list.Add(MapCode.Modern);
        }
        list.Add(MapCode.Elite);
        list.Add(MapCode.Shop);
        list.Add(MapCode.HealSpot);

        Maps.Enqueue(MapCode.Modern);
    
        while (list.Count > 0)
        {
            int temp = (int)Time.time * list.Count;
            Random.InitState(temp);
            int random = Random.Range(0, list.Count - 1);
            Maps.Enqueue(list[random]);
            list.RemoveAt(random);
        }
        Maps.Enqueue(MapCode.Boss);
    }

    private void SetFloor()
    {
        Direction direction;
        bool IsLarge;

        GraghI = RoomFloor * 2 - 1;
        GraghJ = RoomFloor * 2 - 1;

        IsLarge = false;
        LastMapPos = transform.position;
        Gragh[GraghI, GraghJ] = SetMap(Maps.Dequeue(), GetDirection(0), false, IsLarge);

        while (Maps.Count > 0)
        {
            direction = GetDirection(Random.Range(1, 4));
            IsPrevLarge = IsLarge;
            IsLarge = GetLarge();

            if ((Gragh[GraghI + 1, GraghJ] && Gragh[GraghI, GraghJ + 1] && Gragh[GraghI, GraghJ - 1] && Gragh[GraghI - 1, GraghJ]) || IsPrevLarge)
            {
                SetMapPos();
            }
            if (GetInstallMap(direction, IsLarge) > 0)
            {
                SetGragh(direction, IsLarge, SetMap(Maps.Dequeue(), direction, true, IsLarge));
            }
            
        }
    }

    private Transform SetMap(MapCode Code, Direction direction, bool InMonster, bool IsLarge)
    {
        Transform obj;


        switch (Code)
        {
            case MapCode.Modern:
                if (!InMonster) obj = Instantiate(ModernMapsBasic[0]); // 0번째를 몬스터 없는 처음 시작맵으로 설정해주세요
                else obj = Instantiate(IsLarge ? ModernMapsLarge[Random.Range(0, ModernMapsLarge.Length - 1)] : ModernMapsBasic[Random.Range(0, ModernMapsBasic.Length - 1)]);
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
                return transform;
        }
        if (Code != MapCode.Boss)
            obj.localPosition = GetMapPos(direction, IsLarge);
        else
            obj.localPosition = GetMapPos(direction, true);
        obj.tag = Tag;
        obj.SetParent(transform);
        return obj;
    }

    private void SetMapPos()
    {
        Transform trans = transform;

        for (int i = 0; i < RoomFloor * 4; ++i)
        {
            for (int j = 0; j < RoomFloor * 4; ++j)
            {
                if (!Gragh[i, j])
                {
                    GraghI = i;
                    GraghJ = j;
                    LastMapPos = new Vector3(trans.localPosition.y - 10 * RoomFloor * 2 - 1 - GraghI, trans.localPosition.x - 18 * RoomFloor * 2 - 1 - GraghJ);
                    return;
                }
            }
        }
    }

    private void SetGragh(Direction direction, bool IsLarge, Transform trans)
    {
        if (IsLarge)
        {
            switch (direction)
            {
                case Direction.Top:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[--GraghI, GraghJ] = trans;
                        Gragh[GraghI, --GraghJ] = trans;
                        Gragh[--GraghI, GraghJ] = trans;
                        Gragh[GraghI, ++GraghJ] = trans;
                    }
                    else if(GetInstallMap(direction, IsLarge) == 2)
                    {
                        Gragh[--GraghI, GraghJ] = trans;
                        Gragh[GraghI, ++GraghJ] = trans;
                        Gragh[--GraghI, GraghJ] = trans;
                        Gragh[GraghI, --GraghJ] = trans;
                    }
                    break;
                case Direction.Right:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[GraghI, ++GraghJ] = trans;
                        Gragh[++GraghI, GraghJ] = trans;
                        Gragh[GraghI, ++GraghJ] = trans;
                        Gragh[--GraghI, GraghJ] = trans;
                    }
                    else if (GetInstallMap(direction, IsLarge) == 2)
                    {
                        Gragh[GraghI, ++GraghJ] = trans;
                        Gragh[--GraghI, GraghJ] = trans;
                        Gragh[GraghI, ++GraghJ] = trans;
                        Gragh[--GraghI, GraghJ] = trans;
                    }
                    break;
                case Direction.Buttom:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[++GraghI, GraghJ] = trans;
                        Gragh[GraghI, --GraghJ] = trans;
                        Gragh[++GraghI, GraghJ] = trans;
                        Gragh[GraghI, ++GraghJ] = trans;
                    }
                    else if (GetInstallMap(direction, IsLarge) == 2)
                    {
                        Gragh[++GraghI, GraghJ] = trans;
                        Gragh[GraghI, ++GraghJ] = trans;
                        Gragh[++GraghI, GraghJ] = trans;
                        Gragh[GraghI, --GraghJ] = trans;
                    }
                    break;
                case Direction.Left:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[GraghI, --GraghJ] = trans;
                        Gragh[++GraghI, GraghJ] = trans;
                        Gragh[GraghI, --GraghJ] = trans;
                        Gragh[--GraghI, GraghJ] = trans;
                    }
                    else if (GetInstallMap(direction, IsLarge) == 2)
                    {
                        Gragh[GraghI, --GraghJ] = trans;
                        Gragh[--GraghI, GraghJ] = trans;
                        Gragh[GraghI, --GraghJ] = trans;
                        Gragh[++GraghI, GraghJ] = trans;
                    }
                    break;
            }
        }
        else
        {
            switch (direction)
            {
                case Direction.Stand:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[GraghI, GraghJ] = trans;
                    }
                    break;
                case Direction.Top:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[--GraghI, GraghJ] = trans;
                    }
                    break;
                case Direction.Right:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[GraghI, ++GraghJ] = trans;
                    }
                    break;
                case Direction.Buttom:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[++GraghI, GraghJ] = trans;
                    }
                    break;
                case Direction.Left:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[GraghI, --GraghJ] = trans;
                    }
                    break;
            }
        }
    }
    #endregion

#region GetFuntions

    private int GetInstallMap(Direction direction, bool IsLarge)
    {
        if (IsLarge)
        {
            switch (direction)
            {
                case Direction.Top:
                    if (Gragh[GraghI - 1, GraghJ].localPosition == transform.localPosition &&
                        Gragh[GraghI - 1, GraghJ - 1].localPosition == transform.localPosition &&
                        Gragh[GraghI - 2, GraghJ - 1].localPosition == transform.localPosition &&
                        Gragh[GraghI - 2, GraghJ].localPosition == transform.localPosition)
                    {
                        return 1;
                    }
                    else if (Gragh[GraghI - 1, GraghJ].localPosition == transform.localPosition &&
                            Gragh[GraghI - 1, GraghJ + 1].localPosition == transform.localPosition &&
                            Gragh[GraghI - 2, GraghJ + 1].localPosition == transform.localPosition &&
                            Gragh[GraghI - 2, GraghJ].localPosition == transform.localPosition)
                    {
                        return 2;
                    }
                    break;
                case Direction.Right:
                    if (Gragh[GraghI, GraghJ + 1].localPosition == transform.localPosition &&
                        Gragh[GraghI, GraghJ + 2].localPosition == transform.localPosition &&
                        Gragh[GraghI + 1, GraghJ + 2].localPosition == transform.localPosition &&
                        Gragh[GraghI + 1, GraghJ + 1].localPosition == transform.localPosition)
                    {
                        return 1;
                    }
                    else if (Gragh[GraghI, GraghJ + 1].localPosition == transform.localPosition &&
                        Gragh[GraghI, GraghJ + 2].localPosition == transform.localPosition &&
                        Gragh[GraghI - 1, GraghJ + 2].localPosition == transform.localPosition &&
                        Gragh[GraghI - 1, GraghJ + 1].localPosition == transform.localPosition)
                    {
                        return 2;
                    }
                    break;
                case Direction.Buttom:
                    if (Gragh[GraghI + 1, GraghJ].localPosition == transform.localPosition &&
                        Gragh[GraghI + 1, GraghJ - 1].localPosition == transform.localPosition &&
                        Gragh[GraghI + 2, GraghJ - 1].localPosition == transform.localPosition &&
                        Gragh[GraghI + 2, GraghJ].localPosition == transform.localPosition)
                    {
                        return 1;
                    }
                    else if (Gragh[GraghI + 1, GraghJ].localPosition == transform.localPosition &&
                        Gragh[GraghI + 1, GraghJ + 1].localPosition == transform.localPosition &&
                        Gragh[GraghI + 2, GraghJ + 1].localPosition == transform.localPosition &&
                        Gragh[GraghI + 2, GraghJ].localPosition == transform.localPosition)
                    {
                        return 2;
                    }
                    break;
                case Direction.Left:
                    if (Gragh[GraghI, GraghJ - 1].localPosition == transform.localPosition &&
                        Gragh[GraghI, GraghJ - 2].localPosition == transform.localPosition &&
                        Gragh[GraghI - 1, GraghJ - 2].localPosition == transform.localPosition &&
                        Gragh[GraghI - 1, GraghJ - 1].localPosition == transform.localPosition)
                    {
                        return 1;
                    }
                    else if (Gragh[GraghI, GraghJ - 1].localPosition == transform.localPosition &&
                        Gragh[GraghI, GraghJ - 2].localPosition == transform.localPosition &&
                        Gragh[GraghI + 1, GraghJ - 2].localPosition == transform.localPosition &&
                        Gragh[GraghI + 1, GraghJ - 1].localPosition == transform.localPosition)
                    {
                        return 2;
                    }
                    break;
            }
        }
        else
        {
            switch (direction)
            {
                case Direction.Stand:
                    if (Gragh[GraghI, GraghJ] == transform) return 1;
                    break;
                case Direction.Top:
                    if (Gragh[GraghI - 1, GraghJ] == transform) return 1;
                    break;
                case Direction.Right:
                    if (Gragh[GraghI, GraghJ + 1] == transform) return 1;
                    break;
                case Direction.Buttom:
                    if (Gragh[GraghI + 1, GraghJ] == transform) return 1;
                    break;
                case Direction.Left:
                    if (Gragh[GraghI, GraghJ - 1] == transform) return 1;
                    break;
            }
        }
        return 0;
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

    private Vector3 GetAddPos(Direction direction, bool IsLarge)
    {
        switch (direction)
        {
            case Direction.Top:
                return IsLarge ? GetInstallMap(direction, IsLarge) == 1 ? new Vector3(-9f, 15) : new Vector3(9f, 15) : new Vector3(0, 10);
            case Direction.Right:
                return IsLarge ? GetInstallMap(direction, IsLarge) == 1 ? new Vector3(27f, -5f) : new Vector3(27f, 5f) : new Vector3(18f, 0);
            case Direction.Buttom:
                return IsLarge ? GetInstallMap(direction, IsLarge) == 1 ? new Vector3(-9f, -15f) : new Vector3(9f, -10f) : new Vector3(0, -10);
            case Direction.Left:
                return IsLarge ? GetInstallMap(direction, IsLarge) == 1 ? new Vector3(-27f, -5f) : new Vector3(-27f, 5f) : new Vector3(-18f, 0);
            default:
                return new Vector3(0, 0);
        }
    }
    
    private Vector3 GetMapPos(Direction direction, bool IsLarge)
    {
        Vector3 ReturnVector = LastMapPos;        

        switch (direction)
        {
            case Direction.Stand:
                break;
            case Direction.Top:
                 ReturnVector = LastMapPos + GetAddPos(direction, IsLarge);
                break;
            case Direction.Right:
                ReturnVector = LastMapPos + GetAddPos(direction, IsLarge);
                break;
            case Direction.Buttom:
                ReturnVector = LastMapPos + GetAddPos(direction, IsLarge);
                break;
            case Direction.Left:
                ReturnVector = LastMapPos + GetAddPos(direction, IsLarge);
                break;
            default:
                Debug.Log("GetMapPosError");
                return ReturnVector;
        }
        LastMapPos = ReturnVector;
        return ReturnVector;
    }

    private bool GetLarge()
    {
        switch (Random.Range(1, 10))
        {
            case 1:
                return true;
            default:
                return false;
        }
    }

    public static Transform[,] GetGragh()
    {
        return Gragh;
    }

    public static int GetRoomFloor()
    {
        return RoomFloor;
    }

    public static string GetTag()
    {
        return Tag;
    }

    #endregion

    private void Awake()
    {
        Instance = this;
        StageNum = 1;
        ModernMapsBasic = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/Modern/Basic");
        ModernMapsLarge = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/Modern/Large");
        ShopMaps = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/Shop");
        HealSpotMaps = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/HealSpot");
        BossMaps = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/Boss");
        SetQueue();
        SetFloor();
    }

}
