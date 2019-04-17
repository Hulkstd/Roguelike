using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    private enum MapCode { Mordern = 0, Elite = 1, Shop = 2, Gold = 3, Boss = 4 } // 2~4는 하나씩만 1은 2개까지만
    private enum Direction { Stand = 0, Top = 1, Right = 2, Buttom = 3, Left = 4 }

    private List<MapCode> list = new List<MapCode>(); // 랜덤으로 쉽게 넣기위한 리스트
    private Transform LastMapTransform;
    
    [SerializeField]
    private Transform MapSize;
    [SerializeField]
    private Transform MapCreator;
    [SerializeField]
    private Queue<MapCode> Maps = new Queue<MapCode>();
    [SerializeField]
    private int Floor;
    [SerializeField]
    private readonly Vector2 RoomDistance;
    [SerializeField]
    private int RoomCount; // 층 * 10 - 층

    private Object[] MordernMaps = Resources.LoadAll(@"TileMaps/MordernMaps");
    private Object[] EliteMaps = Resources.LoadAll(@"TileMaps/EliteMaps");
    private Object[] ShopMaps = Resources.LoadAll(@"TileMaps/ShopMaps");
    private Object[] GoldMaps = Resources.LoadAll(@"TileMaps/GoldMaps");
    private Object[] BossMaps = Resources.LoadAll(@"TileMaps/BossMaps");

    private void SetQueue()
    {
        RoomCount = Floor * 10 - Floor;
        for (int i = 0; i < RoomCount - 4; ++i)
        {
            list.Add(MapCode.Mordern);
        }
        list.Add(MapCode.Shop);
        list.Add(MapCode.Gold);

        // input queuew
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
            default: return GetDirection(Random.Range(1, 4));
        }
    }
    private void SetFloor()
    {
        LastMapTransform = MapCreator;
        SetMaps(Maps.Dequeue(), GetDirection(0), false);
        while (Maps.Count > 0)
        {
            SetMaps(Maps.Dequeue(), GetDirection(Random.Range(1, 4)), true);
        }
    }

    private void SetMaps(MapCode Code, Direction direction, bool InMonster)
    {
        Transform NowTransform = LastMapTransform;
        Object obj;
        switch (Code)
        {
            case MapCode.Mordern:
                obj = Instantiate(MordernMaps[Random.Range(0, MordernMaps.Length - 1)], MapCreator);
                break;
            case MapCode.Elite:
                obj = Instantiate(MordernMaps[Random.Range(0, EliteMaps.Length - 1)], MapCreator);
                break;
            case MapCode.Shop:
                InMonster = false;
                obj = Instantiate(MordernMaps[Random.Range(0, ShopMaps.Length - 1)], MapCreator);
                break;
            case MapCode.Gold:
                InMonster = false;
                obj = Instantiate(MordernMaps[Random.Range(0, GoldMaps.Length - 1)], MapCreator);
                break;
            case MapCode.Boss:
                obj = Instantiate(MordernMaps[Random.Range(0, BossMaps.Length - 1)], MapCreator);
                break;
        }
    }

    private void Awake()
    {
        
    }
}
