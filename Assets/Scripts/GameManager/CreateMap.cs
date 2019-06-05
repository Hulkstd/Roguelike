using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphType = System.Collections.Generic.List<System.Collections.Generic.List<CreateMap.ListParam>>;

public class CreateMap : MonoBehaviour
{
    public static CreateMap Instance { get; private set; }

    #region EnumsAndClass
    private enum MapCode { Modern = 0, Elite = 1, Shop = 2, HealSpot = 3, Boss = 4 } // 2~4는 하나씩만 1은 2개까지만 맵 구별 방법
    private enum Direction { Stand = 0, Top = 1, Right = 2, Buttom = 3, Left = 4 } // 맵 설치 방향

    public class ListParam : System.IEquatable<ListParam>, System.IComparable<ListParam>
    {
        public int i;
        public int j;
        public Transform trans;
        public ListParam() { i = j = 0; }
        public ListParam(int i, int j, Transform trans) { this.i = i; this.j = j; this.trans = trans; }

        #region operator
        public static bool operator >(ListParam Param1, ListParam Param2) => Param1.j > Param2.j;
        public static bool operator <(ListParam Param1, ListParam Param2) => Param1.j < Param2.j;
        public static bool operator >=(ListParam Param1, ListParam Param2) => Param1.j >= Param2.j;
        public static bool operator <=(ListParam Param1, ListParam Param2) => Param1.j <= Param2.j;

        public int CompareTo(ListParam other) => j.CompareTo(other.j);
        public bool Equals(ListParam other) => other == null ? false : other == this;
        #endregion

        public string MakeString() {
            return i.ToString() + " " + j.ToString() + " " + trans.position + " " + trans.rotation + " " + trans.localScale;
        }
    }
    
    #endregion

    #region MapCreateSurportValues
    private static GraphType Graph = new GraphType();
    private static int GraghI, GraghJ; // now position
    private static List<MapCode> MapCodes = new List<MapCode>(); // 랜덤으로 쉽게 넣기위한 리스트
    private static Transform LastMapTransForm;
    private static int RoomFloor;
    private static string Tag = "InMonsterMap";
    private static Queue<MapCode> Maps = new Queue<MapCode>();
    private static bool IsPrevMapLarge;
    // Top Right Buttom Left -------------------------------------------------------
    public readonly static Vector3 LargeVector = new Vector3(30, 0);
    public readonly static Vector3 BasicVector = new Vector3(21, 0);
    public readonly static Vector3 PrevLarge_LargeVector = new Vector3(39f, 0f);
    public readonly static Vector3 PrevLarge_BasicVector = new Vector3(30f, 0f);
    // ----------------------------------------------------------------------------
    public static readonly float MapDistance = 3f;
    public static bool IsRoading;
    public static int Seed;
    #endregion

    #region InputValues
    private static int StageNum;
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
        MapCreator = GameObject.FindWithTag("MapCreator").transform;
        ModernMapsBasic = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/Modern/Basic");
        ModernMapsLarge = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/Modern/Large");
        ShopMaps = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/Shop");
        HealSpotMaps = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/HealSpot");
        BossMaps = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/Boss");
    }

    public static void SetQueue()
    {
        Graph.Clear();
        System.Random rand = new System.Random();
        Seed = rand.Next(int.MinValue, int.MaxValue);
        Random.InitState(Seed);
        RoomFloor = StageNum * 9;
        
        for (int i = 0; i < (RoomFloor + 1) * 4 + 1; ++i)
        {
            Graph.Add(new List<ListParam>());
        }

        for (int i = 0; i < RoomFloor - 4; ++i)
        {
            MapCodes.Add(MapCode.Modern);
        }
        MapCodes.Add(MapCode.Elite);
        MapCodes.Add(MapCode.Shop);
        MapCodes.Add(MapCode.HealSpot);

        //------------------------------------------------------

        Maps.Enqueue(MapCode.Modern);

        int k = MapCodes.Count;
        while (MapCodes.Count > 0 || k != 0)
        {
            int random = Random.Range(0, MapCodes.Count);
            Maps.Enqueue(MapCodes[random]);
            MapCodes.RemoveAt(random);
            k--;
        }

        Maps.Enqueue(MapCode.Boss);
    }

    public static void SetFloor()
    {
        Direction direction;
        bool IsLarge;
        MapCode IsMapCode;

        GraghI = (Graph.Count - 1) / 2;
        GraghJ = (Graph.Count - 1) / 2;

        IsLarge = false;
        LastMapTransForm = MapCreator;
        IsMapCode = Maps.Dequeue();

        Graph[GraghI].Add(new ListParam(GraghI, GraghJ, SetMap(IsMapCode, GetDirection(0), false, IsLarge)));
        IsPrevMapLarge = false;

        while (Maps.Count > 0)
        {
            direction = GetDirection(Random.Range(1, 5));
            IsLarge = GetLarge();
            IsMapCode = Maps.Peek();

            if (IsMapCode == MapCode.HealSpot || IsMapCode == MapCode.Shop)
                IsLarge = false;
            else if (IsMapCode == MapCode.Elite || IsMapCode == MapCode.Boss)
                IsLarge = true;

            if (GetInstallMap(direction, IsLarge) > 0)
            {
                SetGragh(direction, IsLarge, SetMap(Maps.Dequeue(), direction, IsMapCode != MapCode.HealSpot && IsMapCode != MapCode.Shop, IsLarge));
            }
            else if (GetListParam(GraghI + 1, GraghJ) != null && GetListParam(GraghI + 1, GraghJ) != null
                && GetListParam(GraghI + 1, GraghJ) != null && GetListParam(GraghI + 1, GraghJ) != null)
            {
                SetMapPos();
                continue;
            }
            else
            {
                continue;
            }

            IsPrevMapLarge = IsLarge;
        }
    }

    private static Transform SetMap(MapCode Code, Direction direction, bool InMonster, bool IsLarge)
    {
        Transform obj;

        switch (Code)
        {
            case MapCode.Modern:
                if (!InMonster) obj = Instantiate(ModernMapsBasic[0]); // 0번째를 몬스터 없는 처음 시작맵으로 설정해주세요
                else obj = Instantiate(IsLarge ? ModernMapsLarge[Random.Range(0, ModernMapsLarge.Length)] :
                    ModernMapsBasic[Random.Range(1, ModernMapsBasic.Length)]);
                break;
            case MapCode.Elite:
                obj = Instantiate(BossMaps[Random.Range(0, BossMaps.Length)]);
                break;
            case MapCode.Shop:
                obj = Instantiate(ShopMaps[Random.Range(0, ShopMaps.Length)]);
                break;
            case MapCode.HealSpot:
                obj = Instantiate(HealSpotMaps[Random.Range(0, HealSpotMaps.Length)]);
                break;
            case MapCode.Boss:
                obj = Instantiate(BossMaps[Random.Range(0, BossMaps.Length)]);
                break;
            default:
                Debug.Log("SetMaps error");
                return null;
        }

        obj.localPosition = GetMapPos(direction, IsLarge);

        if (InMonster)
        {
            obj.tag = Tag;
        }

        Debug.Log(IsLarge ? "Large" : "Basic");

        obj.SetParent(MapCreator);
        return obj;
    }

    private static void SetMapPos()
    {
        for (int i = 0; i < (RoomFloor + 1) * 4 + 2; ++i)
        {
            for (int j = 0; j < (RoomFloor + 1) * 4 + 2; ++j)
            {
                if (GetListParam(i, j) != null)
                {
                    GraghI = i;
                    GraghJ = j;
                    return;
                }
            }
        }
    }

    private static void SetGragh(Direction direction, bool IsLarge, Transform trans)
    {
        if (IsLarge)
        {
            switch (direction)
            {
                case Direction.Top:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        SetListParam(--GraghI, GraghJ, trans);
                        SetListParam(GraghI, --GraghJ, trans);
                        SetListParam(--GraghI, GraghJ, trans);
                        SetListParam(GraghI, ++GraghJ, trans);
                        GraghI += Random.Range(0, 2);
                        GraghJ -= Random.Range(0, 2);
                    }
                    else if (GetInstallMap(direction, IsLarge) == 2)
                    {
                        SetListParam(--GraghI, GraghJ, trans);
                        SetListParam(GraghI, ++GraghJ, trans);
                        SetListParam(--GraghI, GraghJ, trans);
                        SetListParam(GraghI, --GraghJ, trans);
                        GraghI += Random.Range(0, 2);
                        GraghJ += Random.Range(0, 2);
                    }
                    // i - 2 j
                    break;
                case Direction.Right:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        SetListParam(GraghI, ++GraghJ, trans);
                        SetListParam(++GraghI, GraghJ, trans);
                        SetListParam(GraghI, ++GraghJ, trans);
                        SetListParam(--GraghI, GraghJ, trans);
                        GraghI += Random.Range(0, 2);
                        GraghJ -= Random.Range(0, 2);
                    }
                    else if (GetInstallMap(direction, IsLarge) == 2)
                    {
                        SetListParam(GraghI, ++GraghJ, trans);
                        SetListParam(--GraghI, GraghJ, trans);
                        SetListParam(GraghI, ++GraghJ, trans);
                        SetListParam(++GraghI, GraghJ, trans);
                        GraghI += Random.Range(0, 2);
                        GraghJ -= Random.Range(0, 2);
                    }
                    break;
                case Direction.Buttom:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        SetListParam(++GraghI, GraghJ, trans);
                        SetListParam(GraghI, --GraghJ, trans);
                        SetListParam(++GraghI, GraghJ, trans);
                        SetListParam(GraghI, ++GraghJ, trans);
                        GraghI -= Random.Range(0, 2);
                        GraghJ -= Random.Range(0, 2);
                    }
                    else if (GetInstallMap(direction, IsLarge) == 2)
                    {
                        SetListParam(++GraghI, GraghJ, trans);
                        SetListParam(GraghI, ++GraghJ, trans);
                        SetListParam(++GraghI, GraghJ, trans);
                        SetListParam(GraghI, --GraghJ, trans);
                        GraghI -= Random.Range(0, 2);
                        GraghJ += Random.Range(0, 2);
                    }
                    break;
                case Direction.Left:
                    if (GetInstallMap(direction, IsLarge) == 1)
                    {
                        SetListParam(GraghI, --GraghJ, trans);
                        SetListParam(++GraghI, GraghJ, trans);
                        SetListParam(GraghI, --GraghJ, trans);
                        SetListParam(--GraghI, GraghJ, trans);
                        GraghI += Random.Range(0, 2);
                        GraghJ += Random.Range(0, 2);
                    }
                    else if (GetInstallMap(direction, IsLarge) == 2)
                    {
                        SetListParam(GraghI, --GraghJ, trans);
                        SetListParam(--GraghI, GraghJ, trans);
                        SetListParam(GraghI, --GraghJ, trans);
                        SetListParam(++GraghI, GraghJ, trans);
                        GraghI -= Random.Range(0, 2);
                        GraghJ += Random.Range(0, 2);
                    }
                    break;
            }
        }
        else
        {
            switch (direction)
            {
                case Direction.Stand:
                    if (GetInstallMap(direction, IsLarge) == 1) { SetListParam(GraghI, GraghJ, trans); }
                    break;
                case Direction.Top:
                    if (GetInstallMap(direction, IsLarge) == 1) { SetListParam(--GraghI, GraghJ, trans); }
                    break;
                case Direction.Right:
                    if (GetInstallMap(direction, IsLarge) == 1) { SetListParam(GraghI, ++GraghJ, trans); }
                    break;
                case Direction.Buttom:
                    if (GetInstallMap(direction, IsLarge) == 1) { SetListParam(++GraghI, GraghJ, trans); }
                    break;
                case Direction.Left:
                    if (GetInstallMap(direction, IsLarge) == 1) { SetListParam(GraghI, --GraghJ, trans); }
                    break;
            }
        }

        LastMapTransForm = GetListParam(GraghI, GraghJ).trans;
    }

    private static void SetListParam(int i, int j, Transform trans)
    {
        Debug.Log("i=" + i + "\n" + "j=" + j + "\n");
        Graph[i].Add(new ListParam(i, j , trans));
        Graph[i].Sort();
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
                    if (GetListParam(GraghI - 1, GraghJ) == null &&
                        GetListParam(GraghI - 1, GraghJ - 1) == null &&
                        GetListParam(GraghI - 2, GraghJ - 1) == null &&
                        GetListParam(GraghI - 2, GraghJ) == null)
                    {
                        return 1;
                    }
                    else if (GetListParam(GraghI - 1, GraghJ) == null &&
                             GetListParam(GraghI - 1, GraghJ + 1) == null &&
                             GetListParam(GraghI - 2, GraghJ + 1) == null &&
                             GetListParam(GraghI - 2, GraghJ) == null)
                    {
                        return 2;
                    }
                    break;
                case Direction.Right:
                    if (GetListParam(GraghI, GraghJ + 1) == null &&
                        GetListParam(GraghI + 1, GraghJ + 1) == null &&
                        GetListParam(GraghI + 1, GraghJ + 2) == null &&
                        GetListParam(GraghI, GraghJ + 2) == null)
                    {
                        return 1;
                    }
                    else if (GetListParam(GraghI, GraghJ + 1) == null &&
                             GetListParam(GraghI - 1, GraghJ + 1) == null &&
                             GetListParam(GraghI - 1, GraghJ + 2) == null &&
                             GetListParam(GraghI, GraghJ + 2) == null)
                    {
                        return 2;
                    }
                    break;
                case Direction.Buttom:
                    if (GetListParam(GraghI + 1, GraghJ) == null &&
                        GetListParam(GraghI + 1, GraghJ - 1) == null &&
                        GetListParam(GraghI + 2, GraghJ - 1) == null &&
                        GetListParam(GraghI + 2, GraghJ) == null)
                    {
                        return 1;
                    }
                    else if (GetListParam(GraghI + 1, GraghJ) == null &&
                             GetListParam(GraghI + 1, GraghJ + 1) == null &&
                             GetListParam(GraghI + 2, GraghJ + 1) == null &&
                             GetListParam(GraghI + 2, GraghJ) == null)
                    {
                        return 2;
                    }
                    break;
                case Direction.Left:
                    if (GetListParam(GraghI, GraghJ - 1) == null &&
                        GetListParam(GraghI + 1, GraghJ - 1) == null &&
                        GetListParam(GraghI + 1, GraghJ - 2) == null &&
                        GetListParam(GraghI, GraghJ - 2) == null)
                    {
                        return 1;
                    }
                    else if (GetListParam(GraghI, GraghJ - 1) == null &&
                             GetListParam(GraghI - 1, GraghJ - 1) == null &&
                             GetListParam(GraghI - 1, GraghJ - 2) == null &&
                             GetListParam(GraghI, GraghJ - 2) == null)
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
                    if (GetListParam(GraghI, GraghJ) == null) { return 1; }
                    break;
                case Direction.Top:
                    if (GetListParam(GraghI - 1, GraghJ) == null) { return 1; }
                    break;
                case Direction.Right:
                    if (GetListParam(GraghI, GraghJ + 1) == null) { return 1; }
                    break;
                case Direction.Buttom:
                    if (GetListParam(GraghI + 1, GraghJ) == null) { return 1; }
                    break;
                case Direction.Left:
                    if (GetListParam(GraghI, GraghJ - 1) == null) { return 1; }
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
                Debug.LogError("GetDirectionError");
                return Direction.Stand;
        }
    }

    private static Vector3 GetAddPos(bool IsLarge)
    {
        Debug.Log(IsPrevMapLarge ? (IsLarge ? PrevLarge_LargeVector : PrevLarge_BasicVector) : (IsLarge ? LargeVector : BasicVector));
        return IsPrevMapLarge ? (IsLarge ? PrevLarge_LargeVector : PrevLarge_BasicVector) : (IsLarge ? LargeVector : BasicVector);
    }

    private static Vector3 GetMapPos(Direction direction, bool IsLarge)
    {
        Vector3 ReturnVector = LastMapTransForm.position;

        switch (direction)
        {
            case Direction.Stand:
                break;
            case Direction.Top:
                ReturnVector += GetAddPos(IsLarge);
                break;
            case Direction.Right:
                ReturnVector += GetAddPos(IsLarge);
                break;
            case Direction.Buttom:
                ReturnVector += GetAddPos(IsLarge);
                break;
            case Direction.Left:
                ReturnVector += GetAddPos(IsLarge);
                break;
        }

        return ReturnVector;
    }

    private static bool GetLarge()
    {
        switch (Random.Range(0, 10))
        {
            case 0:
                return true;
            default:
                return false;
        }
    }

    private static ListParam GetListParam(int i, int j)
    {
        if (i >= Graph.Count || i < 0)
            return null;

        for (int k = 0; k < Graph[i].Count; ++k)
        {
            if (Graph[i][k].j == j)
            {
                return Graph[i][k];
            }
        }

        return null;
    }

    public static GraphType GetGragh()
    {
        return Graph;
    }

    public static int GetRoomFloor()
    {
        return RoomFloor;
    }

    public static string GetTag()
    {
        return Tag;
    }

    public static Transform GetStartMapTransform()
    {
        return Graph[RoomFloor * 2][RoomFloor * 2].trans;
    }

    public static void GetStartPosition(out int x, out int y)
    {
        x = (Graph.Count - 1) / 2;
        y = (Graph.Count - 1) / 2;
    }

    public static void PrintGragh()
    {
        int Count;

        int[,] CopyGragh = new int[(RoomFloor + 1) * 4 + 1, (RoomFloor + 1) * 4 + 1];


        for (int i = 0; i < (RoomFloor + 1) * 4 + 1; ++i)
        {
            Count = 0;
            for (int j = 0; j < (RoomFloor + 1) * 4 + 1; ++j)
            {

                if (Graph[i].Count > 0 && Count < Graph[i].Count)
                {
                    if (Graph[i][Count].j == j)
                    {
                        CopyGragh[i, j] = 1;
                        Count++;
                    }
                    else
                    {
                        CopyGragh[i, j] = 0;
                    }
                }
                else
                {
                    CopyGragh[i, j] = 0;
                }
            }
        }

        string str = "";

        for (int i = 0; i < (RoomFloor + 1) * 4 + 1; ++i)
        {
            for (int j = 0; j < (RoomFloor + 1) * 4 + 1; ++j)
            {
                str += CopyGragh[i, j];
            }
            str += "\n";
        }

        Debug.Log(str);

    }

    public static Transform[,] GetTransformGragh()
    {
        int Count;
        Transform[,] CopyGraph = new Transform[(RoomFloor + 1) * 4 + 1, (RoomFloor + 1) * 4 + 1];

        for (int i = 0; i < (RoomFloor + 1) * 4 + 1; ++i)
        {
            Count = 0;

            for (int j = 0; j < (RoomFloor + 1) * 4 + 1; ++j)
            {
                if (Graph[i].Count > 0 && Count < Graph[i].Count)
                {
                    if (Graph[i][Count].j == j)
                    {
                        CopyGraph[i, j] = Graph[i][Count].trans;
                        Count++;
                    }
                    else
                    {
                        CopyGraph[i, j] = null;
                    }
                }
                else
                {
                    CopyGraph[i, j] = null;
                }
            }
        }

        return CopyGraph;
    }

    #endregion

    public void MakeMap()
    {
        SetFolder();
        SetQueue();
        SetFloor();
        MinimapManager.Instance.MakeMiniMap();
        MonsterSpawnManager.SetMonster();
        //PrintGragh();
        IsRoading = false;
    }

    private void Awake()
    {
        StageNum = 1;
        Instance = this;
    }
}
