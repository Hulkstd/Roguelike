using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GCManager;

public class DungeonInfoCanvas : MonoBehaviour
{
    public static DungeonInfoCanvas Instance;

    public CanvasGroup CanvasAlpha;
    public CanvasGroup TitleAlpha;
    public GraphicRaycaster GraphicRaycaster;

    public CanvasGroup DungeonListAlpha;

    public Image DungeonImage;
    public Text Title;
    public Text Description;
    public Text Info;

    public GameObject Content;

    public float MoveSpeed = 5;
    public float MoveSpeedPerFixedDeltaTime
    {
        get
        {
            return MoveSpeed * Time.fixedDeltaTime;
        }
    }

    private Coroutine action;

    public void MoveToDungeonList()
    {
        if(action == null)
            action = StartCoroutine(Back());
    }

    public void MoveToDungeonInfo(Sprite DungeonImage, string Title, string Description, string Info)
    {
        if (action == null)
            action = StartCoroutine(Move(DungeonImage,Title, Description, Info));
    }

    public void IntoDungeon()
    {
        SceneChanger.Instance.SceneChangeWithLoadingScene(Title.text, Title.text);
    }

    private IEnumerator Back()
    {
        for (int i = 0; i < 60 / MoveSpeed; i++)
        {
            TitleAlpha.alpha = Mathf.Lerp(1, 0, i * MoveSpeedPerFixedDeltaTime);
            DungeonListAlpha.alpha = Mathf.Lerp(0, 1, i * MoveSpeedPerFixedDeltaTime);
            Content.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 90), Quaternion.Euler(0, 0, 0), i * MoveSpeedPerFixedDeltaTime);
            Content.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(Vector2.up * 1920 + Vector2.right * 1080, Vector2.up * 525 + Vector2.right * 350, i * MoveSpeedPerFixedDeltaTime);

            yield return CoroWaitForFixedUpdate;
        }

        CanvasAlpha.alpha = 0;
        GraphicRaycaster.enabled = false;

        action = null;
    }

    private IEnumerator Move(Sprite DungeonImage, string Title, string Description, string Info)
    {
        CanvasAlpha.alpha = 1;
        GraphicRaycaster.enabled = true;
        this.DungeonImage.sprite = DungeonImage;
        this.Title.text = Title;
        this.Description.text = Description;
        this.Info.text = Info;
        for (int i = 0; i < 60 / MoveSpeed; i++)
        {
            TitleAlpha.alpha = Mathf.Lerp(0, 1, i * MoveSpeedPerFixedDeltaTime);
            DungeonListAlpha.alpha = Mathf.Lerp(1, 0, i * MoveSpeedPerFixedDeltaTime);
            Content.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 90), i * MoveSpeedPerFixedDeltaTime);
            Content.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(Vector2.up * 525 + Vector2.right * 350, Vector2.up * 1920 + Vector2.right * 1080, i * MoveSpeedPerFixedDeltaTime);

            yield return CoroWaitForFixedUpdate;
        }

        action = null;
    }

    private void Awake()
    {
        Instance = this;
    }
}
