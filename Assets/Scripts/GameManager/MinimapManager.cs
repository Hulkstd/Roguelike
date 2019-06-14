using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GraghType = System.Collections.Generic.List<System.Collections.Generic.List<CreateMap.ListParam>>;

[AddComponentMenu("GameManager/MinimapManager")]
public class MinimapManager : MonoBehaviour
{
    public static MinimapManager Instance { get; private set; }

    #region Enum and Class

    public enum Direction { Stand = 0, Top = 1, Right = 2, Buttom = 3, Left = 4 }

    #endregion

    #region Variable

    #region public Field
    #endregion

    #region private Field

    private CreateMap CreateMapInstance
    {
        get
        {
            return CreateMap.Instance;
        }
    }
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
    [SerializeField]
    private Animator OpenAnimator;

    #endregion

    #endregion

    #region StartSetting

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
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

    private void MakeMiniMap(int x, int y, bool PrevMapInfo, Vector3 PrevPosition)
    {
        if (IsThereMap(x + 1, y))
        {
            if(IsOneByOne(x + 1, y))
            {
                Graphs[x + 1, y] = null;
                Image image = MakeImage(PrevMapInfo, PrevPosition, GetOffset(PrevMapInfo, false, false, true, false), false);
                MakeMiniMap(x + 1, y, false, image.transform.position);
            }
            else
            {
                Transform tmp = Graphs[x + 1, y];

                if(IsThereMap(x + 1, y + 1))
                {
                    Graphs[x + 1, y] = Graphs[x + 2, y] = Graphs[x + 1, y + 1] = Graphs[x + 2, y + 1] = null;
                    Image image = MakeImage(PrevMapInfo, PrevPosition, GetOffset(PrevMapInfo, true, false, true, true), true);
                    MakeMiniMap(x + 1, y, true, GetBasicTransformPosition("3", tmp, image));
                    MakeMiniMap(x + 2, y, true, GetBasicTransformPosition("2", tmp, image));
                    MakeMiniMap(x + 1, y + 1, true, GetBasicTransformPosition("4", tmp, image));
                    MakeMiniMap(x + 2, y + 1, true, GetBasicTransformPosition("1", tmp, image));
                }
                else if(IsThereMap(x + 1, y - 1))
                {
                    Graphs[x + 1, y] = Graphs[x + 2, y] = Graphs[x + 1, y - 1] = Graphs[x + 2, y - 1] = null;
                    Image image = MakeImage(PrevMapInfo, PrevPosition, GetOffset(PrevMapInfo, true, false, true, false), true);
                    MakeMiniMap(x + 1, y, true, GetBasicTransformPosition("4", tmp, image));
                    MakeMiniMap(x + 2, y, true, GetBasicTransformPosition("1", tmp, image));
                    MakeMiniMap(x + 1, y - 1, true, GetBasicTransformPosition("3", tmp, image));
                    MakeMiniMap(x + 2, y - 1, true, GetBasicTransformPosition("2", tmp, image));
                }
            }
        }
        if (IsThereMap(x, y + 1))
        {
            if (IsOneByOne(x, y + 1))
            {
                Graphs[x, y + 1] = null;
                Image image = MakeImage(PrevMapInfo, PrevPosition, GetOffset(PrevMapInfo, false, true, true, true), false);
                MakeMiniMap(x, y + 1, false, image.transform.position);
            }
            else
            {
                Transform tmp = Graphs[x, y + 1];

                if (IsThereMap(x + 1, y + 1))
                {
                    Graphs[x, y + 1] = Graphs[x + 1, y + 1] = Graphs[x, y + 2] = Graphs[x + 1, y + 2] = null; 
                    Image image = MakeImage(PrevMapInfo, PrevPosition, GetOffset(PrevMapInfo, true, true, true, true), true);
                    MakeMiniMap(x, y + 1, true, GetBasicTransformPosition("3", tmp, image));
                    MakeMiniMap(x, y + 2, true, GetBasicTransformPosition("4", tmp, image));
                    MakeMiniMap(x + 1, y + 1, true, GetBasicTransformPosition("2", tmp, image));
                    MakeMiniMap(x + 1, y + 2, true, GetBasicTransformPosition("1", tmp, image));
                }
                else if (IsThereMap(x - 1, y + 1))
                {
                    Graphs[x, y + 1] = Graphs[x - 1, y + 1] = Graphs[x, y + 2] = Graphs[x - 1, y + 2] = null;
                    Image image = MakeImage(PrevMapInfo, PrevPosition, GetOffset(PrevMapInfo, true, true, false, true), true);
                    MakeMiniMap(x, y + 1, true, GetBasicTransformPosition("2", tmp, image));
                    MakeMiniMap(x, y + 2, true, GetBasicTransformPosition("1", tmp, image));
                    MakeMiniMap(x - 1, y + 1, true, GetBasicTransformPosition("3", tmp, image));
                    MakeMiniMap(x - 1, y + 2, true, GetBasicTransformPosition("4", tmp, image));
                }
            }
        }
        if (IsThereMap(x - 1, y))
        {
            if (IsOneByOne(x - 1, y))
            {
                Graphs[x - 1, y] = null;
                Image image = MakeImage(PrevMapInfo, PrevPosition, GetOffset(PrevMapInfo, false, false, false, true), false);
                MakeMiniMap(x - 1, y, false, image.transform.position);
            }
            else
            {
                Transform tmp = Graphs[x - 1, y];

                if (IsThereMap(x - 1, y + 1))
                {
                    Graphs[x - 1, y] = Graphs[x - 2, y] = Graphs[x - 1, y + 1] = Graphs[x - 2, y + 1] = null;
                    Image image = MakeImage(PrevMapInfo, PrevPosition, GetOffset(PrevMapInfo, true, false, false, true), true);
                    MakeMiniMap(x - 1, y, true, GetBasicTransformPosition("2", tmp, image));
                    MakeMiniMap(x - 2, y, true, GetBasicTransformPosition("3", tmp, image));
                    MakeMiniMap(x - 1, y + 1, true, GetBasicTransformPosition("1", tmp, image));
                    MakeMiniMap(x - 2, y + 1, true, GetBasicTransformPosition("4", tmp, image));
                }
                else if (IsThereMap(x - 1, y - 1))
                {
                    Graphs[x - 1, y] = Graphs[x - 2, y] = Graphs[x - 1, y - 1] = Graphs[x - 2, y - 1] = null;
                    Image image = MakeImage(PrevMapInfo, PrevPosition, GetOffset(PrevMapInfo, true, false, false, false), true);
                    MakeMiniMap(x - 1, y, true, GetBasicTransformPosition("1", tmp, image));
                    MakeMiniMap(x - 2, y, true, GetBasicTransformPosition("4", tmp, image));
                    MakeMiniMap(x - 1, y - 1, true, GetBasicTransformPosition("2", tmp, image));
                    MakeMiniMap(x - 2, y - 1, true, GetBasicTransformPosition("3", tmp, image));
                }
            }
        }
        if (IsThereMap(x, y - 1))
        {

            if (IsOneByOne(x, y - 1))
            {
                Graphs[x, y - 1] = null;
                Image image = MakeImage(PrevMapInfo, PrevPosition, GetOffset(PrevMapInfo, false, true, true, false), false);
                MakeMiniMap(x, y - 1, false, image.transform.position);
            }
            else
            {
                Transform tmp = Graphs[x, y - 1];

                if (IsThereMap(x + 1, y - 1))
                {
                    Graphs[x, y - 1] = Graphs[x + 1, y - 1] = Graphs[x, y - 2] = Graphs[x + 1, y - 2] = null;
                    Image image = MakeImage(PrevMapInfo, PrevPosition, GetOffset(PrevMapInfo, true, true, true, false), true);
                    MakeMiniMap(x, y - 1, true, GetBasicTransformPosition("4", tmp, image));
                    MakeMiniMap(x, y - 2, true, GetBasicTransformPosition("3", tmp, image));
                    MakeMiniMap(x + 1, y - 1, true, GetBasicTransformPosition("1", tmp, image));
                    MakeMiniMap(x + 1, y - 2, true, GetBasicTransformPosition("2", tmp, image));
                }
                else if (IsThereMap(x - 1, y - 1))
                {
                    Graphs[x, y - 1] = Graphs[x - 1, y - 1] = Graphs[x, y - 2] = Graphs[x - 1, y - 2] = null;
                    Image image = MakeImage(PrevMapInfo, PrevPosition, GetOffset(PrevMapInfo, true, true, false, false), true);
                    MakeMiniMap(x, y - 1, true, GetBasicTransformPosition("1", tmp, image));
                    MakeMiniMap(x, y - 2, true, GetBasicTransformPosition("2", tmp, image));
                    MakeMiniMap(x - 1, y - 1, true, GetBasicTransformPosition("4", tmp, image));
                    MakeMiniMap(x - 1, y - 2, true, GetBasicTransformPosition("3", tmp, image));
                }
            }
        }
    }

    private Image MakeImage(bool PrevMapInfo, Vector3 PrevPosition, Vector3 Offset, bool IsLarge)
    {
        if (IsLarge) Debug.Log("Large" + PrevPosition + " " + Offset + PrevMapInfo);
        else Debug.Log("Small" + PrevPosition + " " + Offset + PrevMapInfo);

        Image image = new GameObject("MiniMap").AddComponent<Image>();
        image.sprite = MapSprite;
        image.transform.SetParent(MaskForm.transform);
        image.transform.localScale = Vector3.one;
        image.type = Image.Type.Simple;
        image.raycastTarget = false;

        image.transform.position = PrevPosition;
        image.transform.position += Offset;
        image.rectTransform.sizeDelta = BaseMapSize * (IsLarge ? 2 : 1);

        return image;
    }

    private Vector3 GetOffset(bool PrevMapInfo, bool IsLarge, bool IsUpDown, bool XPlus, bool YPlus)
    {
        Debug.Log($"{PrevMapInfo} {IsLarge} {IsUpDown} {XPlus} {YPlus}");
        if (IsLarge)
        {
            if(IsUpDown)
            {
                return new Vector3((XPlus ? OneByOneOffset.x : -OneByOneOffset.x), 
                                   (PrevMapInfo ? (YPlus ? OneByOneOffset.y : -OneByOneOffset.y) : (YPlus ? OneByOneOffset.y : -OneByOneOffset.y)) + (YPlus ? TwoByTwoOffset.y : -TwoByTwoOffset.y)
                                  );
            }
            else
            {
                return new Vector3((PrevMapInfo ? (XPlus ? OneByOneOffset.x : -OneByOneOffset.x) : (XPlus ? OneByOneOffset.x : -OneByOneOffset.x)) + (XPlus ? TwoByTwoOffset.x : -TwoByTwoOffset.x),
                                   (YPlus ? OneByOneOffset.y : -OneByOneOffset.y));
            }
        }
        else
        {
            if(IsUpDown)
            {
                return new Vector3(0, (YPlus ? OneByOneOffset.y : -OneByOneOffset.y) * 2);
            }
            else
            {
                return new Vector3((XPlus ? OneByOneOffset.x : -OneByOneOffset.x) * 2, 0);
            }
        }
    }

    private Vector3 GetBasicTransformPosition(string Num, Transform Parent, Image image)
    {
        switch (Num)
        {
            case "1":
                {
                    return new Vector3((image.transform.position.x * 2 + image.transform.right.x * TwoByTwoOffset.x) / 2,
                                       (image.transform.position.y * 2 + image.transform.up.y * TwoByTwoOffset.y) / 2);
                }

            case "2":
                {
                    return new Vector3((image.transform.position.x * 2 + image.transform.right.x * TwoByTwoOffset.x) / 2,
                                       (image.transform.position.y * 2 - image.transform.up.y * TwoByTwoOffset.y) / 2);
                }

            case "3":
                {
                    return new Vector3((image.transform.position.x * 2 - image.transform.right.x * TwoByTwoOffset.x) / 2,
                                       (image.transform.position.y * 2 - image.transform.up.y * TwoByTwoOffset.y) / 2);
                }

            case "4":
                {
                    return new Vector3((image.transform.position.x * 2 - image.transform.right.x * TwoByTwoOffset.x) / 2,
                                       (image.transform.position.y * 2 + image.transform.up.y * TwoByTwoOffset.y) / 2);
                }
        }
        Debug.LogError("There is not BasicMap Transform");

        return Vector3.zero;
    }

    #endregion

    #region Button Function
    
    public void OpenMinimap()
    {
        OpenAnimator.SetBool("OpenMinimap", !OpenAnimator.GetBool("OpenMinimap"));
    }

    #endregion

    #region Public Function

    public void MakeMiniMap()
    {
        Graphs = CreateMap.GetTransformGragh();
        Floor = CreateMap.GetRoomFloor();

        int x, y;

        CreateMap.GetStartPosition(out x, out y);

        if (IsThereMap(x, y))
        {
            Image image = MakeImage(false, MaskForm.transform.position, Vector3.zero, false);

            Graphs[x, y] = null;
            MakeMiniMap(x, y, false, image.transform.position);
        }
        else
        {
            Debug.LogError("There is no Map in there");
            return;
        }

        Graphs = CreateMap.GetTransformGragh();
    }

    public void UpdateCenter(int x, int y, Direction direction, bool PrevMapInfo)
    {
        switch(direction)
        {
            case Direction.Left:
                {
                    MaskForm.transform.position += GetOffset(PrevMapInfo, !IsOneByOne(x, y), false, false, Graphs[x - 2, y + 1] != null);
                }
                break;

            case Direction.Top:
                {
                    MaskForm.transform.position += GetOffset(PrevMapInfo, !IsOneByOne(x, y), true, Graphs[x + 1, y + 2] != null, true);
                }
                break;

            case Direction.Right:
                {
                    MaskForm.transform.position += GetOffset(PrevMapInfo, !IsOneByOne(x, y), false, true, Graphs[x + 2, y + 1] != null);
                }
                break;

            case Direction.Buttom:
                {
                    MaskForm.transform.position += GetOffset(PrevMapInfo, !IsOneByOne(x, y), true, Graphs[x + 1, y - 2] != null, false);
                }
                break;
        }
    }

    #endregion
}
