using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    public static MinimapManager Instance { get; private set; }

    #region struct and class

    #endregion

    #region Variable

    #region static Field
    #endregion

    #region public Field
    #endregion

    #region private Field

    private CreateMap CreateMapInstance;
    private Transform[,] Graphs;
    private int Floor;

    #endregion

    #region Serializable Field

    [SerializeField]
    private GameObject MaskForm;
    [SerializeField]
    private Sprite MapSprite;
    [SerializeField]
    private Vector2 BaseMapSize;
    [SerializeField]
    private Vector3 OneByOneOffset;
    [SerializeField]
    private Vector3 TwoByTwoOffset;

    #endregion
    #endregion

    #region StartSetting

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CreateMapInstance = CreateMap.Instance;
        CreateMapInstance.MakeMap();
    }

    #endregion

    #region Check Map Graph

    private bool IsOneByOne(int x, int y)
    {
        if (Graphs[x, y] == Graphs[x, y + 1])
        {
            return false;
        }

        if (Graphs[x, y] == Graphs[x, y - 1])
        {
            return false;
        }

        if (Graphs[x, y] == Graphs[x + 1, y - 1])
        {
            return false;
        }

        if (Graphs[x, y] == Graphs[x - 1, y - 1])
        {
            return false;
        }

        return true;
    }

    private bool IsThereMap(int x, int y)
    {
        if (Graphs[x, y] != null) return true;
        return false;
    }

    #endregion

    #region Make Minimap

    public void MakeMiniMap()
    {
        Graphs = CreateMap.GetGragh();
        Floor = CreateMap.GetRoomFloor();

        int x = Floor * 2;
        int y = Floor * 2;

        if(IsThereMap(x, y))
        {
            Image image = new GameObject("MiniMap").AddComponent<Image>();
            image.sprite = MapSprite;
            image.transform.SetParent(MaskForm.transform);

            image.transform.localPosition = Vector3.zero;
            image.rectTransform.sizeDelta = BaseMapSize;

            Graphs[x, y] = null;
            MakeMiniMap(x, y, true, image.transform.position);
        }
        else
        {
            Debug.LogError("There is no Map in there");
            return;
        }
    }

    private void MakeMiniMap(int x, int y, bool PrevMapInfo, Vector3 PrevPosition)
    {
        if (IsThereMap(x + 1, y))
        {
            if(IsOneByOne(x + 1, y))
            {
                Graphs[x + 1, y] = null;
                Image image = MakeImage(PrevMapInfo, PrevPosition, new Vector3(OneByOneOffset.x, 0), false);
                MakeMiniMap(x + 1, y, false, image.transform.position);
            }
            else
            {
                if(IsThereMap(x + 1, y + 1))
                {
                    Graphs[x + 1, y] = Graphs[x + 2, y] = Graphs[x + 1, y + 1] = Graphs[x + 2, y + 1] = null;
                    Image image = MakeImage(PrevMapInfo, PrevPosition, new Vector3(TwoByTwoOffset.x, 0), true);
                    MakeMiniMap(x + 1, y, true, image.transform.position);
                    MakeMiniMap(x + 2, y, true, image.transform.position);
                    MakeMiniMap(x + 1, y + 1, true, image.transform.position);
                    MakeMiniMap(x + 2, y + 1, true, image.transform.position);
                }
                else if(IsThereMap(x + 1, y - 1))
                {
                    Graphs[x + 1, y] = Graphs[x + 2, y] = Graphs[x + 1, y - 1] = Graphs[x + 2, y - 1] = null;
                    Image image = MakeImage(PrevMapInfo, PrevPosition, new Vector3(TwoByTwoOffset.x, 0), true);
                    MakeMiniMap(x + 1, y, true, image.transform.position);
                    MakeMiniMap(x + 2, y, true, image.transform.position);
                    MakeMiniMap(x + 1, y - 1, true, image.transform.position);
                    MakeMiniMap(x + 2, y - 1, true, image.transform.position);
                }
            }
        }
        else if (IsThereMap(x, y + 1))
        {
            if (IsOneByOne(x, y + 1))
            {
                Graphs[x, y + 1] = null;
                Image image = MakeImage(PrevMapInfo, PrevPosition, new Vector3(0, OneByOneOffset.y), false);
                MakeMiniMap(x, y + 1, false, image.transform.position);
            }
            else
            {
                if (IsThereMap(x + 1, y + 1))
                {
                    Graphs[x, y + 1] = Graphs[x + 1, y + 1] = Graphs[x, y + 2] = Graphs[x + 1, y + 2]; 
                    Image image = MakeImage(PrevMapInfo, PrevPosition, new Vector3(OneByOneOffset.x,0), true);
                    MakeMiniMap(x, y + 1, true, image.transform.position);
                    MakeMiniMap(x, y + 2, true, image.transform.position);
                    MakeMiniMap(x + 1, y + 1, true, image.transform.position);
                    MakeMiniMap(x + 1, y + 2, true, image.transform.position);
                }
                else if (IsThereMap(x - 1, y + 1))
                {
                    Graphs[x, y + 1] = Graphs[x - 1, y + 1] = Graphs[x, y + 2] = Graphs[x - 1, y + 2];
                    Image image = MakeImage(PrevMapInfo, PrevPosition, new Vector3(-OneByOneOffset.x, 0), true);
                    MakeMiniMap(x, y + 1, true, image.transform.position);
                    MakeMiniMap(x, y + 2, true, image.transform.position);
                    MakeMiniMap(x - 1, y + 1, true, image.transform.position);
                    MakeMiniMap(x - 1, y + 2, true, image.transform.position);
                }
            }
        }
        else if (IsThereMap(x - 1, y))
        {
            if (IsOneByOne(x - 1, y))
            {
                Graphs[x - 1, y] = null;
                Image image = MakeImage(PrevMapInfo, PrevPosition, new Vector3(-OneByOneOffset.x, 0), false);
                MakeMiniMap(x - 1, y, false, image.transform.position);
            }
            else
            {
                if (IsThereMap(x - 1, y + 1))
                {
                    Graphs[x - 1, y] = Graphs[x - 2, y] = Graphs[x - 1, y + 1] = Graphs[x - 2, y + 1] = null;
                    Image image = MakeImage(PrevMapInfo, PrevPosition, new Vector3(-TwoByTwoOffset.x, 0), true);
                    MakeMiniMap(x - 1, y, true, image.transform.position);
                    MakeMiniMap(x - 2, y, true, image.transform.position);
                    MakeMiniMap(x - 1, y + 1, true, image.transform.position);
                    MakeMiniMap(x - 2, y + 1, true, image.transform.position);
                }
                else if (IsThereMap(x - 1, y - 1))
                {
                    Graphs[x - 1, y] = Graphs[x - 2, y] = Graphs[x - 1, y - 1] = Graphs[x - 2, y - 1] = null;
                    Image image = MakeImage(PrevMapInfo, PrevPosition, new Vector3(-TwoByTwoOffset.x, 0), true);
                    MakeMiniMap(x - 1, y, true, image.transform.position);
                    MakeMiniMap(x - 2, y, true, image.transform.position);
                    MakeMiniMap(x - 1, y - 1, true, image.transform.position);
                    MakeMiniMap(x - 2, y - 1, true, image.transform.position);
                }
            }
        }
        else if (IsThereMap(x, y - 1))
        {

            if (IsOneByOne(x, y - 1))
            {
                Graphs[x, y - 1] = null;
                Image image = MakeImage(PrevMapInfo, PrevPosition, new Vector3(0, -OneByOneOffset.y), false);
                MakeMiniMap(x, y - 1, false, image.transform.position);
            }
            else
            {
                if (IsThereMap(x + 1, y - 1))
                {
                    Graphs[x, y - 1] = Graphs[x + 1, y - 1] = Graphs[x, y - 2] = Graphs[x + 1, y - 2];
                    Image image = MakeImage(PrevMapInfo, PrevPosition, new Vector3(OneByOneOffset.x, 0), true);
                    MakeMiniMap(x, y - 1, true, image.transform.position);
                    MakeMiniMap(x, y - 2, true, image.transform.position);
                    MakeMiniMap(x + 1, y - 1, true, image.transform.position);
                    MakeMiniMap(x + 1, y - 2, true, image.transform.position);
                }
                else if (IsThereMap(x - 1, y - 1))
                {
                    Graphs[x, y - 1] = Graphs[x - 1, y - 1] = Graphs[x, y - 2] = Graphs[x - 1, y - 2];
                    Image image = MakeImage(PrevMapInfo, PrevPosition, , true);
                    MakeMiniMap(x, y - 1, true, image.transform.position);
                    MakeMiniMap(x, y - 2, true, image.transform.position);
                    MakeMiniMap(x - 1, y - 1, true, image.transform.position);
                    MakeMiniMap(x - 1, y - 2, true, image.transform.position);
                }
            }
        }
        else
        {
            Debug.LogError("There is no Map in there");
        }
    }

    private Image MakeImage(bool PrevMapInfo, Vector3 PrevPosition, Vector3 Offset, bool IsLarge)
    {
        Image image = new GameObject("MiniMap").AddComponent<Image>();
        image.sprite = MapSprite;
        image.transform.SetParent(MaskForm.transform);

        image.transform.position = PrevPosition;
        image.transform.position += Offset;
        image.rectTransform.sizeDelta = BaseMapSize * (IsLarge ? 2 : 1);

        return image;
    }

    private Vector3 GetOffset(bool PrevMapInfo, bool XPlus, bool YPlus)
    {
        // return new Vector3( (PrevMapInfo ? ( (XPlus ? TwoByTwoOffset.x : -TwoByTwoOffset.x) : ( OneByOneOffset.x)) , (PrevMapInfo ? TwoByTwoOffset.y : OneByOneOffset.y));

        return new Vector3();
    }

    #endregion
}
