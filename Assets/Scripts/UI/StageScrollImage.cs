using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GCManager;
public class StageScrollImage : MonoBehaviour
{
    #region enum & structure

    public enum ScrollDirection
    {
        Horizontal,
        Vertical
    }

    #endregion

    #region variable

    [Tooltip("아이템들 움직이는 방향")]
    public ScrollDirection Direction = ScrollDirection.Horizontal;

    [Tooltip("고를 수 있는 아이템의 갯수.")]
    [Range(0, 100)]
    public int ContentCount = 1;

    [Tooltip("지금 콘텐츠 위치")]
    [Range(0, 100)]
    public int CurrentContent;

    [Tooltip("컨텐츠 디자인 프리펩")]
    public GameObject ContentPrefab = null;

    [Tooltip("넘어가는 버튼의 크기")]
    public Vector2 ButtonSize = Vector2.one * 100;

    [Tooltip("중앙 콘탠츠 양 옆에 표시하는 콘텐츠 크기")]
    public Vector2 centerContentSize = Vector2.up * 160 + Vector2.right * 90;

    [Tooltip("중앙 콘탠츠 양 옆에 표시하는 콘텐츠 크기")]
    public Vector2 sideContentSize = Vector2.zero;

    [Tooltip("중앙 콘텐츠 와 옆의 콘텐츠와의 떨어진 거리")]
    public float Merge = 20.0f;

    [Tooltip("넘어갈 시 움직이는 속도")]
    public float ScrollSpeed = 2.0f;
    protected float ScrollSpeedPerFixedDeltaTime
    {
        get
        {
            return Time.fixedDeltaTime * ScrollSpeed;
        }
    }

    public List<GameObject> ContentsList;

    public Button.ButtonClickedEvent ButtonsEvents;

    public Button NextButton;

    public Button PreviousButton;

    public GameObject Contents;

    private Coroutine action;

    private Sprite[] DungeonImages;

    private string[] Titles;

    private string[] Descriptions;

    private string[] Infos;

    #endregion

    #region Initiate

    public virtual void InitiateContents()
    {
        if(ContentCount < ContentsList.Count)
        {
            for(int i = ContentsList.Count; i > ContentCount; i--)
            {
                StartCoroutine(Destroy(ContentsList[i - 1]));
                ContentsList.RemoveAt(i-1);
            }
        }
        else if(ContentsList.Count <= ContentCount)
        {
            for (int i = 0; i < ContentCount; i++)
            {
                if (ContentsList.Count <= i)
                {
                    if (ContentPrefab == null)
                    {
                        GameObject obj = new GameObject("content", typeof(Image), typeof(Button));
                        RectTransform rectTransform = obj.GetComponent<RectTransform>();
                        
                        rectTransform.SetParent(Contents.transform);
                        rectTransform.localScale = Vector3.one;
                        rectTransform.localPosition = CalculatePosition(i);
                        rectTransform.sizeDelta = CalculateSize(i);

                        obj.GetComponent<Button>().onClick = ButtonsEvents;

                        ContentsList.Add(obj);
                    }
                    else
                    {
                        GameObject obj = Instantiate(ContentPrefab, Contents.transform, false);
                        RectTransform rectTransform = obj.GetComponent<RectTransform>();

                        rectTransform.localScale = Vector3.one;
                        rectTransform.localPosition = CalculatePosition(i);
                        rectTransform.sizeDelta = CalculateSize(i);

                        obj.GetComponent<Button>().onClick = ButtonsEvents;

                        ContentsList.Add(obj);
                    }
                }
                else
                {
                    RectTransform rectTransform = ContentsList[i].GetComponent<RectTransform>();

                    rectTransform.localPosition = CalculatePosition(i);
                    rectTransform.sizeDelta = CalculateSize(i);

                    rectTransform.GetComponent<Button>().onClick = ButtonsEvents;
                }
            }
        }
    }

    public virtual void InitiateButton()
    {
        if (NextButton == null)
        {
            if (transform.childCount - ContentsList.Count >= 1)
            {
                if (transform.GetChild(1).name == "NextButton")
                {
                    NextButton = transform.GetChild(1).GetComponent<Button>();
                }
            }
            else
            {
                GameObject gameObject = new GameObject("NextButton", typeof(Button), typeof(Image));
                RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
                NextButton = gameObject.GetComponent<Button>();

                rectTransform.SetParent(transform);
                rectTransform.localScale = Vector3.one;
                rectTransform.sizeDelta = ButtonSize;
                rectTransform.localPosition = Vector2.right * (GetComponent<RectTransform>().sizeDelta.x * 0.5f + ButtonSize.x + Merge);
            }
        }
        if (PreviousButton == null)
        {
            if (transform.childCount - ContentsList.Count >= 2)
            {
                if (transform.GetChild(2).name == "PreviousButton")
                {
                    PreviousButton = transform.GetChild(2).GetComponent<Button>();
                }
            }
            else
            {
                GameObject gameObject = new GameObject("PreviousButton", typeof(Button), typeof(Image));
                RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
                PreviousButton = gameObject.GetComponent<Button>();

                rectTransform.SetParent(transform);
                rectTransform.localScale = Vector3.one;
                rectTransform.sizeDelta = ButtonSize;
                rectTransform.localPosition = Vector2.left * (GetComponent<RectTransform>().sizeDelta.x * 0.5f + ButtonSize.x + Merge);
            }
        }
    }

    #endregion

    #region Calculate Position and Size

    #region int index

    protected Vector2 CalculatePosition(int index)
    {
        switch(Direction)
        {
            case ScrollDirection.Horizontal:
                {
                    return HorizontalCalculatePosition(index);
                }

            case ScrollDirection.Vertical:
                {
                    return VerticalCalculatePosition(index);
                }
        }
        return HorizontalCalculatePosition(index);
    }

    protected Vector2 CalculateSize(int index)
    {
        switch (Direction)
        {
            case ScrollDirection.Horizontal:
                {
                    return HorizontalCalculateSize(index);
                }

            case ScrollDirection.Vertical:
                {
                    return VerticalCalculateSize(index);
                }
        }
        return HorizontalCalculateSize(index);
    }

    protected Vector2 VerticalCalculatePosition(int index)
    {
        Vector2 returnPos = GetComponent<RectTransform>().position;
        int relativeIndex = index - CurrentContent;
        if (relativeIndex == 0)
        {
            return returnPos;
        }

        Vector2 currentSize = centerContentSize;
        Vector2 nextSize = Vector2.zero;
        Vector2 relativeSize = centerContentSize - sideContentSize;
        float mergeY = Merge;

        for (int i = 0; i < Mathf.Abs(relativeIndex); i++)
        {
            nextSize = currentSize - relativeSize;
            returnPos += (relativeIndex < 0 ? -1 : 1) * Vector2.up * (currentSize.y * 0.5f + nextSize.y * 0.5f + mergeY);

            currentSize = nextSize;
            mergeY *= 0.2f;
        }
        return returnPos;
    }

    protected Vector2 VerticalCalculateSize(int index)
    {
        Vector2 returnSize = centerContentSize;
        int relativeIndex = index - CurrentContent;
        if (relativeIndex == 0)
        {
            return returnSize;
        }

        Vector2 relativeSize = centerContentSize - sideContentSize;

        for (int i = 0; i < Mathf.Abs(relativeIndex); i++)
        {
            returnSize -= relativeSize;
        }

        return returnSize;
    }

    protected Vector2 HorizontalCalculatePosition(int index)
    {
        Vector2 returnPos = GetComponent<RectTransform>().position;
        int relativeIndex = index - CurrentContent;
        if(relativeIndex == 0)
        {
            return returnPos;
        }

        Vector2 currentSize = centerContentSize;
        Vector2 nextSize = Vector2.zero;
        Vector2 relativeSize = centerContentSize - sideContentSize;
        float mergeX = Merge;

        for(int i = 0; i < Mathf.Abs(relativeIndex); i++)
        {
            nextSize = currentSize - relativeSize;
            returnPos += (relativeIndex < 0 ? -1 : 1) * Vector2.right * (currentSize.x * 0.5f + nextSize.x * 0.5f + mergeX);

            currentSize = nextSize;
            mergeX *= 0.2f;
        }
        return returnPos;
    }

    protected Vector2 HorizontalCalculateSize(int index)
    {
        Vector2 returnSize = centerContentSize; 
        int relativeIndex = index - CurrentContent;
        if (relativeIndex == 0)
        {
            return returnSize;
        }

        Vector2 relativeSize = centerContentSize - sideContentSize;

        for (int i = 0; i < Mathf.Abs(relativeIndex); i++)
        {
            returnSize -= relativeSize;
        }

        return returnSize;
    }

    #endregion

    #endregion

    #region ButtonEvent

    public void NextContent()
    {
        if (action == null && CurrentContent + 1 < ContentCount) 
            action = StartCoroutine(MoveContents(CurrentContent + 1));
    }

    public void PreviousContent()
    {
        if (action == null && CurrentContent - 1 >= 0) 
            action = StartCoroutine(MoveContents(CurrentContent - 1));
    }

    public void ShowDungeonInfo()
    {
        DungeonInfoCanvas.Instance.MoveToDungeonInfo(DungeonImages[CurrentContent], Titles[CurrentContent], Descriptions[CurrentContent], Infos[CurrentContent]);
    }

    #endregion

    protected IEnumerator MoveContents(int to)
    {
        List<Vector2> fromPosition = new List<Vector2>();
        List<Vector2> fromSizedelta = new List<Vector2>();

        for (int index = 0; index < ContentsList.Count; index++)
        {
            fromPosition.Add(HorizontalCalculatePosition(index));
            fromSizedelta.Add(HorizontalCalculateSize(index));
        }

        CurrentContent = to;

        List<Vector2> toPosition = new List<Vector2>();
        List<Vector2> toSizedelta = new List<Vector2>();

        for(int index = 0; index < ContentsList.Count; index++)
        {
            toPosition.Add(HorizontalCalculatePosition(index));
            toSizedelta.Add(HorizontalCalculateSize(index));
        }

        for (int i = 0; i < 60 / ScrollSpeed; i++)
        {
            for(int index = 0; index < ContentsList.Count; index++)
            {
                ContentsList[index].GetComponent<RectTransform>().localPosition = Vector3.Lerp(fromPosition[index], toPosition[index], i * ScrollSpeedPerFixedDeltaTime);
                ContentsList[index].GetComponent<RectTransform>().sizeDelta = Vector3.Lerp(fromSizedelta[index], toSizedelta[index], i * ScrollSpeedPerFixedDeltaTime);
            }
            yield return CoroWaitForFixedUpdate;
        }

        action = null;
        ButtonActiveSet();
    }

    private IEnumerator Destroy(GameObject obj)
    {
        yield return CoroWaitForEndFrame;

        DestroyImmediate(obj);
    }

    public void ButtonActiveSet()
    {
        for(int i = 0; i < ContentsList.Count; i++)
        {
            ContentsList[i].GetComponent<Button>().interactable = (CurrentContent == i);
        }
    }

    public void SetValues(Sprite[] DungeonImages,string[] Titles, string[] Descriptions, string[] Infos)
    {
        this.DungeonImages = DungeonImages;
        this.Titles = Titles;
        this.Descriptions = Descriptions;
        this.Infos = Infos;

        ContentCount = Titles == null ? 0 : Titles.Length;
        OnValidate();
    }

    private void OnValidate()
    {
        if (Contents == null)
        {
            if (0 < transform.childCount)
            {
                if (transform.GetChild(0).name == "Contents")
                {
                    Contents = transform.GetChild(0).gameObject;
                }
            }
            else
            {

                Contents = new GameObject("Contents", typeof(Mask), typeof(Image));
                Contents.transform.SetParent(transform);
                Contents.GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;
                Contents.GetComponent<RectTransform>().localPosition = Vector3.zero;
                Contents.GetComponent<RectTransform>().localScale = Vector2.one;
                Contents.GetComponent<Image>().color = new Color(1, 1, 1, 0.01f);
            }
        }
        if (ContentsList == null)
        {
            ContentsList = new List<GameObject>();
        }
        CurrentContent = Mathf.Clamp(CurrentContent, 0, ContentCount - 1);

        InitiateContents();
        InitiateButton();
        ButtonActiveSet();
    }
}
