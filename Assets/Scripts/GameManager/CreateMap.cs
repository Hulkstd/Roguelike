using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{

    public static CreateMap Instance { get; private set; }

    private enum MapCode { Mordern = 0, Elite = 1, Shop = 2, Gold = 3, Boss = 4 } // 2~4는 하나씩만 1은 2개까지만
    private enum Direction { Stand = 0, Top = 1, Right = 2, Buttom = 3, Left = 4 }

    private bool[,] Gragh;
    private int GraghI, GraghJ;
    private List<MapCode> list = new List<MapCode>(); // 랜덤으로 쉽게 넣기위한 리스트
    private Vector3 LastMapPos;

    [SerializeField]
    private static int StageNum;
    [SerializeField]
    private Transform MapCreator;
    [SerializeField]
    private Queue<MapCode> Maps = new Queue<MapCode>();
    [SerializeField]
    private int Floor;
    [SerializeField]
    private int RoomCount; // 층 * 10 - 층

    private Transform[] MordernMaps = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/MordernMaps");
    private Transform[] EliteMaps = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/EliteMaps");
    private Transform[] ShopMaps = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/ShopMaps");
    private Transform[] GoldMaps = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/GoldMaps");
    private Transform[] BossMaps = Resources.LoadAll<Transform>(@"TileMaps/Stage" + StageNum + "/BossMaps");

    private void SetQueue()
    {
        RoomCount = Floor * 10 - Floor;

        Gragh = new bool[RoomCount * 2, RoomCount * 2];


        for (int i = 0; i < RoomCount - 4; ++i)
        {
            list.Add(MapCode.Mordern);
        }
        list.Add(MapCode.Shop);
        list.Add(MapCode.Gold);

        // input queue
        Maps.Enqueue(MapCode.Mordern);
        while (list.Count > 0)
        {
            int random = Random.Range(0, list.Count - 1);
            Maps.Enqueue(list[random]);
            list.RemoveAt(random);
        }
        Maps.Enqueue(MapCode.Boss);
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

    private Vector3 GetMapPos(Direction direction)
    {
        switch (direction)
        {
            case Direction.Stand:
                return LastMapPos;
            case Direction.Top:
                return LastMapPos + new Vector3(0, 5);
            case Direction.Right:
                return LastMapPos + new Vector3(8.9f, 0);
            case Direction.Buttom:
                return LastMapPos + new Vector3(0, -5);
            case Direction.Left:
                return LastMapPos + new Vector3(-8.9f, 0);
            default:
                Debug.Log("GetMapPosError");
                return LastMapPos;
        }
    }

    private bool SetGragh(Direction direction)
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
        return true;
    }

    private void SetFloor()
    {
        Direction direction;

        GraghI = RoomCount - 1;
        GraghJ = RoomCount - 1;

        LastMapPos = MapCreator.position;
        SetMaps(Maps.Dequeue(), GetDirection(0), false);
        Gragh[GraghI, GraghJ] = false;

        while (Maps.Count > 0)
        {
            direction = GetDirection(Random.Range(1, 4));
            if (SetGragh(direction))
                SetMaps(Maps.Dequeue(), direction, true);
            else
                continue;
        }
    }

    private void SetMaps(MapCode Code, Direction direction, bool InMonster)
    {
        Transform obj;

        switch (Code)
        {
            case MapCode.Mordern:
                if(InMonster) obj = Instantiate(MordernMaps[0]); // 0번째를 몬스터 없는 처음 시작맵으로 설정해주세요
                else obj = Instantiate(MordernMaps[Random.Range(0, MordernMaps.Length - 1)]);
                break;
            case MapCode.Elite:
                obj = Instantiate(EliteMaps[Random.Range(0, EliteMaps.Length - 1)]);
                break;
            case MapCode.Shop:
                InMonster = false;
                obj = Instantiate(ShopMaps[Random.Range(0, ShopMaps.Length - 1)]);
                break;
            case MapCode.Gold:
                InMonster = false;
                obj = Instantiate(GoldMaps[Random.Range(0, GoldMaps.Length - 1)]);
                break;
            case MapCode.Boss:
                obj = Instantiate(BossMaps[Random.Range(0, BossMaps.Length - 1)]);
                break;
            default:
                Debug.Log("SetMaps error");
                return;
        }
        if (Code != MapCode.Boss)
            obj.localPosition = GetMapPos(direction);
        else
            obj.localPosition = GetMapPos(direction) * 2;

        obj.SetParent(MapCreator);
    }

    private void Awake()
    {
        Instance = this;
    }
}
