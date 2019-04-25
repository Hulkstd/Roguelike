using System;
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
    private static int GraghI, GraghJ;
    private static List<MapCode> MapCodes = new List<MapCode>(); // 랜덤으로 쉽게 넣기위한 리스트
    private static Transform LastMapTransForm;
    private static int RoomFloor;
    private static string Tag = "InMonsterMap";
    private static Queue<MapCode> Maps = new Queue<MapCode>();
    private static bool IsPrevLarge;
    // Top Right Buttom Left -------------------------------------------------------
    public static readonly Vector3[] PrevLargeAddLargeVectors = { new Vector3(0f, 20f), new Vector3(36f, 0f), new Vector3(0f, -20f), new Vector3(-36f, 0f) };
    public static readonly Vector3[] PrevLargeAddBasicVectors = { new Vector3(9, 15), new Vector3(27f, -5), new Vector3(-9, -15), new Vector3(-27f, 5) };
    public static readonly Vector3[] AddLargeVectorsType1 = { new Vector3(-9f, 15), new Vector3(27f, -5f), new Vector3(-9f, -15f), new Vector3(-27f, -5f) };
    public static readonly Vector3[] AddLargeVectorsType2 = { new Vector3(9f, 15), new Vector3(27f, 5f), new Vector3(9f, -15f), new Vector3(-27f, 5f) };
    public static readonly Vector3[] AddBasicVectors = { new Vector3(0, 10), new Vector3(18f, 0), new Vector3(0, -10), new Vector3(-18f, 0) };
    // ----------------------------------------------------------------------------
    public static readonly float MapDistance = 3f;
    public static bool IsRoading;
#endregion

#region InputValues
    [SerializeField]
    private static int StageNum;
    [SerializeField]
    private static Transform MapCreator;
#endregion

#region Resourse
    private static Transform[] ModernMapsBasic;
    private static Transform[] ModernMapsLarge;
    private static Transform[] ShopMaps;
    private static Transform[] HealSpotMaps;
    private static Transform[] BossMaps;
#endregion

#region SetFunctions

    public static void SetFolder()
    {
        IsRoading = true;
        MapCreator = GameObject.FindGameObjectWithTag("MapCreator").transform;
        ModernMapsBasic = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/Modern/Basic");
        ModernMapsLarge = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/Modern/Large");
        ShopMaps = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/Shop");
        HealSpotMaps = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/HealSpot");
        BossMaps = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/Boss");
    }

    public static void SetQueue()
    {
        System.Random rand = new System.Random();
        UnityEngine.Random.InitState(rand.Next(int.MinValue, int.MaxValue));
        RoomFloor = StageNum * 9;

        Gragh = new Transform[RoomFloor * 4, RoomFloor * 4];

        for (int i = 0; i < RoomFloor * 4; ++i)
        {
            for (int j = 0; j < RoomFloor * 4; ++j)
            {
                Gragh[i, j] = null;
            }
        }

        for (int i = 0; i < RoomFloor - 4; ++i)
        {
            MapCodes.Add(MapCode.Modern);
        }
        MapCodes.Add(MapCode.Elite);
        MapCodes.Add(MapCode.Shop);
        MapCodes.Add(MapCode.HealSpot);

        Maps.Enqueue(MapCode.Modern);
    
        while (MapCodes.Count > 0)
        {
            int random = UnityEngine.Random.Range(0, MapCodes.Count - 1);
            Maps.Enqueue(MapCodes[random]);
            MapCodes.RemoveAt(random);
        }
        Maps.Enqueue(MapCode.Boss);
    }

    public static void SetFloor()
    {
        Direction direction;
        bool IsLarge;
        MapCode IsMapCode;

        GraghI = RoomFloor * 2 - 1;
        GraghJ = RoomFloor * 2 - 1;

        IsLarge = false;
        LastMapTransForm = MapCreator;
        IsMapCode = Maps.Dequeue();
        Gragh[GraghI, GraghJ] = SetMap(IsMapCode, GetDirection(0), false, IsLarge);
        IsPrevLarge = false;

        while (Maps.Count > 0)
        {
            direction = GetDirection(UnityEngine.Random.Range(1, 4));
            IsLarge = GetLarge();
            IsMapCode = Maps.Peek();

            if (IsMapCode == MapCode.HealSpot || IsMapCode == MapCode.Shop)
                IsLarge = false;
            else if (IsMapCode == MapCode.Elite || IsMapCode == MapCode.Boss)
                IsLarge = true;

            if (GetInstallMap(direction, IsLarge) > 0)
            {
                SetGragh(direction, IsLarge, SetMap(Maps.Dequeue(), direction, true, IsLarge));
            }
            else if (Gragh[GraghI + 1, GraghJ] == null && Gragh[GraghI, GraghJ + 1] == null
                && Gragh[GraghI, GraghJ - 1] == null && Gragh[GraghI - 1, GraghJ] == null)
            {
                IsPrevLarge = false;
                SetMapPos();
            }
            else
            {
                continue;
            }

            IsPrevLarge = IsLarge;
        }

        IsRoading = false;
    }

    private static Transform SetMap(MapCode Code, Direction direction, bool InMonster, bool IsLarge)
    {
        Transform obj;

        switch (Code)
        {
            case MapCode.Modern:
                if (!InMonster) obj = Instantiate(ModernMapsBasic[0]); // 0번째를 몬스터 없는 처음 시작맵으로 설정해주세요
                else obj = Instantiate(IsLarge ? ModernMapsLarge[UnityEngine.Random.Range(0, ModernMapsLarge.Length - 1)] : 
                    ModernMapsBasic[UnityEngine.Random.Range(1, ModernMapsBasic.Length - 1)]);
                break;
            case MapCode.Elite:
                obj = Instantiate(BossMaps[UnityEngine.Random.Range(0, BossMaps.Length - 1)]);
                break;
            case MapCode.Shop:
                InMonster = false;
                obj = Instantiate(ShopMaps[UnityEngine.Random.Range(0, ShopMaps.Length - 1)]);
                break;
            case MapCode.HealSpot:
                InMonster = false;
                obj = Instantiate(HealSpotMaps[UnityEngine.Random.Range(0, HealSpotMaps.Length - 1)]);
                break;
            case MapCode.Boss:
                obj = Instantiate(BossMaps[UnityEngine.Random.Range(0, BossMaps.Length - 1)]);
                break;
            default:
                Debug.Log("SetMaps error");
                return null;
        }
        if (Code != MapCode.Boss)
        {
            obj.localPosition = GetMapPos(direction, IsLarge);
        }
        else
        {
            obj.localPosition = GetMapPos(direction, true);
        }
        if (InMonster)
        {
            obj.tag = Tag;
        }

        obj.SetParent(MapCreator);
        return obj;
    }

    private static void SetMapPos()
    {
        for (int i = 0; i < RoomFloor * 4; ++i)
        {
            for (int j = 0; j < RoomFloor * 4; ++j)
            {
                if (!Gragh[i, j])
                {
                    GraghI = i;
                    GraghJ = j;
                    LastMapTransForm = Gragh[i, j];
                    return;
                }
            }
        }
    }

    private static void SetGragh(Direction direction, bool IsLarge, Transform trans)
    {
        if (IsLarge)
        {

            List<Transform> childs = new List<Transform>();

            for (int i = 0; i < trans.childCount; ++i)
            {
                Transform child = trans.GetChild(i);
                if (child.CompareTag("BasicTransform"))
                {
                    childs.Add(child);
                }
            }

            if (childs.Count < 4)
            {
                return;
            }

            switch (direction)
            {
                case Direction.Top:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[--GraghI, GraghJ] = childs[1];
                        Gragh[GraghI, --GraghJ] = childs[2];
                        Gragh[--GraghI, GraghJ] = childs[3];
                        Gragh[GraghI, ++GraghJ] = childs[0];

                        GraghI += UnityEngine.Random.Range(0, 1);
                        GraghJ -= UnityEngine.Random.Range(0, 1);
                    }
                    else if(GetInstallMap(direction, IsLarge) == 2)
                    {
                        Gragh[--GraghI, GraghJ] = childs[2];
                        Gragh[GraghI, ++GraghJ] = childs[1];
                        Gragh[--GraghI, GraghJ] = childs[0];
                        Gragh[GraghI, --GraghJ] = childs[3];

                        GraghI += UnityEngine.Random.Range(0, 1);
                        GraghJ += UnityEngine.Random.Range(0, 1);
                    }
                    // i - 2 j
                    break;
                case Direction.Right:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[GraghI, ++GraghJ] = childs[3];
                        Gragh[++GraghI, GraghJ] = childs[2];
                        Gragh[GraghI, ++GraghJ] = childs[1];
                        Gragh[--GraghI, GraghJ] = childs[0];

                        GraghI += UnityEngine.Random.Range(0, 1);
                        GraghJ -= UnityEngine.Random.Range(0, 1);
                    }
                    else if (GetInstallMap(direction, IsLarge) == 2)
                    {
                        Gragh[GraghI, ++GraghJ] = childs[2];
                        Gragh[--GraghI, GraghJ] = childs[3];
                        Gragh[GraghI, ++GraghJ] = childs[0];
                        Gragh[++GraghI, GraghJ] = childs[1];

                        GraghI += UnityEngine.Random.Range(0, 1);
                        GraghJ -= UnityEngine.Random.Range(0, 1);
                    }
                    // i j + 2
                    break;
                case Direction.Buttom:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[++GraghI, GraghJ] = childs[0];
                        Gragh[GraghI, --GraghJ] = childs[3];
                        Gragh[++GraghI, GraghJ] = childs[2];
                        Gragh[GraghI, ++GraghJ] = childs[1];

                        GraghI -= UnityEngine.Random.Range(0, 1);
                        GraghJ -= UnityEngine.Random.Range(0, 1);                        
                    }
                    else if (GetInstallMap(direction, IsLarge) == 2)
                    {
                        Gragh[++GraghI, GraghJ] = childs[3];
                        Gragh[GraghI, ++GraghJ] = childs[0];
                        Gragh[++GraghI, GraghJ] = childs[1];
                        Gragh[GraghI, --GraghJ] = childs[2];

                        GraghI -= UnityEngine.Random.Range(0, 1);
                        GraghJ += UnityEngine.Random.Range(0, 1);
                    }
                    // i + 2 j
                    break;
                case Direction.Left:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[GraghI, --GraghJ] = childs[0];
                        Gragh[++GraghI, GraghJ] = childs[1];
                        Gragh[GraghI, --GraghJ] = childs[2];
                        Gragh[--GraghI, GraghJ] = childs[3];

                        GraghI += UnityEngine.Random.Range(0, 1);
                        GraghJ += UnityEngine.Random.Range(0, 1);
                    }
                    else if (GetInstallMap(direction, IsLarge) == 2)
                    {
                        Gragh[GraghI, --GraghJ] = childs[1];
                        Gragh[--GraghI, GraghJ] = childs[0];
                        Gragh[GraghI, --GraghJ] = childs[3];
                        Gragh[++GraghI, GraghJ] = childs[2];

                        GraghI -= UnityEngine.Random.Range(0, 1);
                        GraghJ += UnityEngine.Random.Range(0, 1);
                    }
                    // i j - 2
                    break;
            }
            LastMapTransForm = Gragh[GraghI, GraghJ];
            return;
        }
        else
        {
            switch (direction)
            {
                case Direction.Stand:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[GraghI, GraghJ] = trans;
                        LastMapTransForm = Gragh[GraghI, GraghJ];
                    }
                    // i j
                    break;
                case Direction.Top:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[--GraghI, GraghJ] = trans;
                        LastMapTransForm = Gragh[GraghI, GraghJ];
                    }
                    // i - 1 j
                    break;
                case Direction.Right:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[GraghI, ++GraghJ] = trans;
                        LastMapTransForm = Gragh[GraghI, GraghJ];
                    }
                    // i j + 1
                    break;
                case Direction.Buttom:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[++GraghI, GraghJ] = trans;
                        LastMapTransForm = Gragh[GraghI, GraghJ];
                    }
                    // i + 1 j
                    break;
                case Direction.Left:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        Gragh[GraghI, --GraghJ] = trans;
                        LastMapTransForm = Gragh[GraghI, GraghJ];
                    }
                    // i j + 1
                    break;
            }
        }
    }

#endregion

#region GetFuntions

    private static int GetInstallMap(Direction direction, bool IsLarge)
    {
        if (IsLarge)
        {
            switch (direction)
            {
                case Direction.Top:
                    if (Gragh[GraghI - 1, GraghJ] == null &&
                        Gragh[GraghI - 1, GraghJ - 1] == null &&
                        Gragh[GraghI - 2, GraghJ - 1] == null &&
                        Gragh[GraghI - 2, GraghJ] == null)
                    {
                        return 1;
                    }
                    else if (Gragh[GraghI - 1, GraghJ] == null &&
                             Gragh[GraghI - 1, GraghJ + 1] == null &&
                             Gragh[GraghI - 2, GraghJ + 1] == null &&
                             Gragh[GraghI - 2, GraghJ] == null)
                    {
                        return 2;
                    }
                    break;
                case Direction.Right:
                    if (Gragh[GraghI, GraghJ + 1] == null &&
                        Gragh[GraghI, GraghJ + 2] == null &&
                        Gragh[GraghI + 1, GraghJ + 2] == null &&
                        Gragh[GraghI + 1, GraghJ + 1] == null)
                    {
                        return 1;
                    }
                    else if (Gragh[GraghI, GraghJ + 1] == null &&
                             Gragh[GraghI, GraghJ + 2] == null &&
                             Gragh[GraghI - 1, GraghJ + 2] == null &&
                             Gragh[GraghI - 1, GraghJ + 1] == null)
                    {
                        return 2;
                    }
                    break;
                case Direction.Buttom:
                    if (Gragh[GraghI + 1, GraghJ] == null &&
                        Gragh[GraghI + 1, GraghJ - 1] == null &&
                        Gragh[GraghI + 2, GraghJ - 1] == null &&
                        Gragh[GraghI + 2, GraghJ] == null)
                    {
                        return 1;
                    }
                    else if (Gragh[GraghI + 1, GraghJ] == null &&
                             Gragh[GraghI + 1, GraghJ + 1] == null &&
                             Gragh[GraghI + 2, GraghJ + 1] == null &&
                             Gragh[GraghI + 2, GraghJ] == null)
                    {
                        return 2;
                    }
                    break;
                case Direction.Left:
                    if (Gragh[GraghI, GraghJ - 1] == null &&
                        Gragh[GraghI, GraghJ - 2] == null &&
                        Gragh[GraghI - 1, GraghJ - 2] == null &&
                        Gragh[GraghI - 1, GraghJ - 1] == null)
                    {
                        return 1;
                    }
                    else if (Gragh[GraghI, GraghJ - 1] == null &&
                             Gragh[GraghI, GraghJ - 2] == null &&
                             Gragh[GraghI + 1, GraghJ - 2] == null &&
                             Gragh[GraghI + 1, GraghJ - 1] == null)
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
                    if (Gragh[GraghI, GraghJ] == null)
                    {
                        return 1;
                    }
                    break;
                case Direction.Top:
                    if (Gragh[GraghI - 1, GraghJ] == null)
                    {
                        return 1;
                    }
                    break;
                case Direction.Right:
                    if (Gragh[GraghI, GraghJ + 1] == null)
                    {
                        return 1;
                    } 
                    break;
                case Direction.Buttom:
                    if (Gragh[GraghI + 1, GraghJ] == null)
                    {
                        return 1;
                    }
                    break;
                case Direction.Left:
                    if (Gragh[GraghI, GraghJ - 1] == null)
                    {
                        return 1;
                    }
                    break;
            }
        }
        return 0;
    }

    private static Direction GetDirection(int num)
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

    private static Vector3 GetAddPos(Direction direction, bool IsLarge)
    {
        /*if (IsPrevLarge)
        {
            switch (direction)
            {
                case Direction.Top:
                    return IsLarge ? PrevLargeAddLargeVectors[0] + new Vector3(0, MapDistance) : PrevLargeAddBasicVectors[0] + new Vector3(0, MapDistance);
                case Direction.Right:
                    return IsLarge ? PrevLargeAddLargeVectors[1] + new Vector3(MapDistance, 0) : PrevLargeAddBasicVectors[1] + new Vector3(MapDistance, 0);
                case Direction.Buttom:
                    return IsLarge ? PrevLargeAddLargeVectors[2] + new Vector3(0, -MapDistance) : PrevLargeAddBasicVectors[2] + new Vector3(0, -MapDistance);
                case Direction.Left:
                    return IsLarge ? PrevLargeAddLargeVectors[3] + new Vector3(-MapDistance, 0) : PrevLargeAddBasicVectors[3] + new Vector3(-MapDistance, 0);
                default:
                    return new Vector3(0, 0);
            }
        }
        else
        {*/
        switch (direction)
        {
            case Direction.Top:
                return IsLarge ? GetInstallMap(direction, IsLarge) == 1 ? AddLargeVectorsType1[0] + new Vector3(0, MapDistance) :
                    AddLargeVectorsType2[0] + new Vector3(0, MapDistance) : AddBasicVectors[0] + new Vector3(0, MapDistance);
            case Direction.Right:
                return IsLarge ? GetInstallMap(direction, IsLarge) == 1 ? AddLargeVectorsType1[1] + new Vector3(MapDistance, 0) :
                    AddLargeVectorsType2[1] + new Vector3(MapDistance, 0) : AddBasicVectors[1] + new Vector3(MapDistance, 0);
            case Direction.Buttom:
                return IsLarge ? GetInstallMap(direction, IsLarge) == 1 ? AddLargeVectorsType1[2] + new Vector3(0, -MapDistance) :
                    AddLargeVectorsType2[2] + new Vector3(0, -MapDistance) : AddBasicVectors[2] + new Vector3(0, -MapDistance);
            case Direction.Left:
                return IsLarge ? GetInstallMap(direction, IsLarge) == 1 ? AddLargeVectorsType1[3] + new Vector3(-MapDistance, 0) :
                    AddLargeVectorsType2[3] + new Vector3(-MapDistance, 0) : AddBasicVectors[3] + new Vector3(-MapDistance, 0);
            default:
                return new Vector3(0, 0);
        }
        //        }
    }

    private static Vector3 GetMapPos(Direction direction, bool IsLarge)
    {
        Vector3 ReturnVector = LastMapTransForm.position;

        switch (direction)
        {
            case Direction.Stand:
                break;
            case Direction.Top:
                ReturnVector += GetAddPos(direction, IsLarge);
                break;
            case Direction.Right:
                ReturnVector += GetAddPos(direction, IsLarge);
                break;
            case Direction.Buttom:
                ReturnVector += GetAddPos(direction, IsLarge);
                break;
            case Direction.Left:
                ReturnVector += GetAddPos(direction, IsLarge);
                break;
            default:
                Debug.Log("GetMapPosError");
                return ReturnVector;
        }

        return ReturnVector;
    }

    private static bool GetLarge()
    {
        switch (UnityEngine.Random.Range(1, 10))
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
        SetFolder();
        SetQueue();
        SetFloor();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            for (int i = 0; i < MapCreator.childCount; ++i)
            {
                Destroy(MapCreator.GetChild(i).gameObject);
            }

            SetFolder();
            SetQueue();
            SetFloor();
        }
    }

}
