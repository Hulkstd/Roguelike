using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance { get; private set; }

    #region Enum and Class

    [System.Serializable]
    public class NodeToNode
    {
        public GameObject From;
        public GameObject To;
    }

    [System.Serializable]
    public class CFromTo : SerializableDictionaryBase<NodeToNode, UILineRenderer>
    {
        public UILineRenderer GetValue(GameObject From, GameObject To)
        {
            foreach(KeyValuePair<NodeToNode, UILineRenderer> keyValue in this)
            {
                if (keyValue.Key.From == From && keyValue.Key.To == To)
                    return keyValue.Value;
            }

            return null;
        }

        public KeyValuePair<NodeToNode, UILineRenderer> GetKeyValuePairUsingString(string str)
        {
            foreach (KeyValuePair<NodeToNode, UILineRenderer> keyValue in this)
            {
                if (keyValue.Key.To.name == str)
                    return keyValue;
            }

            return new KeyValuePair<NodeToNode, UILineRenderer>(null, null);
        }
    }
    [System.Serializable]
    public class MatchGameObjectAndList : Dictionary<GameObject, List<GameObject>>
    {
        public List<GameObject> GetValue(GameObject To)
        {
            foreach (KeyValuePair<GameObject, List<GameObject>> keyValue in this)
            {
                if (keyValue.Key == To)
                    return keyValue.Value;
            }

            return null;
        }
    }

    [System.Serializable]
    public class SelectedNode
    {
        public List<string> NodeNameList;
    }

    #endregion
     
    #region Variable

    public CFromTo Page1FromTo;
    public CFromTo Page2FromTo;
    [HideInInspector]
    public MatchGameObjectAndList ToFrom;
    [HideInInspector]
    public MatchGameObjectAndList FromToList;
    public string FilePath;
    public int CurrentPageNum;

    private SelectedNode[] SelectedNodes;
    [SerializeField]
    private Transform[] AbilityButtonGroup;
    [SerializeField]
    private Transform[] PageTransforms;

    #endregion

    #region Start Setting

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        FilePath = Application.dataPath + FilePath;
        SelectedNodes = new SelectedNode[1];
        ToFrom = new MatchGameObjectAndList();
        FromToList = new MatchGameObjectAndList();

        LoadPage(1);
    }

    #endregion

    #region Functions

    #region Save and Load Node

    public void SaveSelectedNode()
    {
        BinarySerialize(SelectedNodes[CurrentPageNum], FilePath + CurrentPageNum.ToString() + ".dll");
    }

    public SelectedNode LoadSelectedNode()
    {
        return BinaryDeserialize(FilePath + CurrentPageNum.ToString() + ".dll");
    }

    #endregion

    #region Load Page

    private void LoadPage(int PageNum)
    {
        CurrentPageNum = PageNum;

        CFromTo FromTo = GetFromTo(PageNum);
        ToFrom.Clear();
        FromToList.Clear();

        if (System.IO.File.Exists(FilePath + CurrentPageNum.ToString() + ".dll"))
        {
            if (SelectedNodes.Length <= CurrentPageNum)
            {
                Array.Resize(ref SelectedNodes, CurrentPageNum + 1);
            }

            SelectedNodes[PageNum] = LoadSelectedNode();
        }
        else
        {
            if(SelectedNodes.Length <= CurrentPageNum)
            {
                Array.Resize(ref SelectedNodes, CurrentPageNum + 1);
            }

            SelectedNodes[PageNum] = new SelectedNode();
            SelectedNodes[PageNum].NodeNameList = new List<string>();
        }

        for (int i = 0; i < AbilityButtonGroup[CurrentPageNum - 1].childCount; i++)
        {
            //Button button = AbilityButtonGroup[CurrentPageNum - 1].GetChild(i).GetComponent<Button>();
            PressedButton pressedButton = AbilityButtonGroup[CurrentPageNum - 1].GetChild(i).gameObject.AddComponent<PressedButton>();

            pressedButton.RequiredHoldTime = 1;
            pressedButton.OnClick = new UnityEngine.Events.UnityEvent();
            pressedButton.OnClick.AddListener(() => { SelectNode(pressedButton.gameObject); });
            pressedButton.OnLongClick = new UnityEngine.Events.UnityEvent();
            pressedButton.OnLongClick.AddListener(() => { ShowAbilityInfo(pressedButton.transform.GetChild(0).gameObject); });
        }

        foreach (KeyValuePair<NodeToNode, UILineRenderer> keyValue in FromTo)
        {
            List<GameObject> a = new List<GameObject>();

            if (!ToFrom.TryGetValue(keyValue.Key.To, out a))
            {
                ToFrom.Add(keyValue.Key.To, new List<GameObject>() { keyValue.Key.From });
            }
            else
            {
                ToFrom[keyValue.Key.To].Add(keyValue.Key.From);
            }

            if (!FromToList.TryGetValue(keyValue.Key.From, out a))
            {
                FromToList.Add(keyValue.Key.From, new List<GameObject>() { keyValue.Key.To });
            }
            else
            {
                FromToList[keyValue.Key.From].Add(keyValue.Key.To);
            }

            keyValue.Key.To.GetComponent<Button>().image.color = Color.gray;
            keyValue.Key.From.GetComponent<Button>().image.color = Color.gray;
            if (keyValue.Value != null) keyValue.Value.color = Color.gray;
        }

        SelectedNodes[PageNum].NodeNameList.ForEach(str =>
        {
            KeyValuePair<NodeToNode, UILineRenderer> keyValue = FromTo.GetKeyValuePairUsingString(str);
            if (keyValue.Key != null && keyValue.Value != null)
            {
                keyValue.Key.From.GetComponent<Button>().image.color = Color.white;
                keyValue.Key.To.GetComponent<Button>().image.color = Color.white;
                keyValue.Value.color = Color.white;
            }
            else if (keyValue.Key != null && keyValue.Value == null)
            {
                keyValue.Key.To.GetComponent<Button>().image.color = Color.white;
            }
        });

        SelectedNodes[PageNum].NodeNameList.ForEach((str) =>
        {
            NodeToNode node = FromTo.GetKeyValuePairUsingString(str).Key;

            if (node == null)
                goto GetOutofHere;

            List<GameObject> list = FromToList.GetValue(node.From);

            if (list == null)
                goto GetOutofHere;

            list.ForEach((item) =>
            {
                if (item.name != str)
                {
                    item.GetComponent<Button>().interactable = false;
                    item.GetComponent<PressedButton>().OnClick.RemoveAllListeners();
                    item.GetComponent<PressedButton>().OnLongClick.RemoveAllListeners();
                }
            });

            GetOutofHere:;
        });
    }

    private CFromTo GetFromTo(int pageNum)
    {
        switch(pageNum)
        {
            case 1:
                return Page1FromTo;

            case 2:
                return Page2FromTo;
        }

        return null;
    }

    #endregion

    #endregion

    #region Button Function

    public void ShowAbilityInfo(GameObject Info)
    {
        Info.SetActive(true);
    }

    public void SelectNode(GameObject to)
    {
        List<GameObject> list = ToFrom.GetValue(to);
        bool flag = false;
        GameObject fromitem = null;

        if (ToFrom.GetValue(to) == null)
        {
            flag = true;
        }

        SelectedNodes[CurrentPageNum].NodeNameList.ForEach((item) =>
        {
            if (list != null)
            {
                list.ForEach((from) =>
                {
                    if (item == from.name)
                    {
                        flag = true;
                        fromitem = from;
                    }
                });
            }
        });

        if(flag)
        {
            SelectedNodes[CurrentPageNum].NodeNameList.Add(to.name);
            //if(fromitem != null && SelectedNodes[CurrentPageNum].NodeNameList.Contains(fromitem.name)) SelectedNodes[CurrentPageNum].NodeNameList.Remove(fromitem.name);
            to.gameObject.GetComponent<Button>().image.color = Color.white;
            to.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            if(GetFromTo(CurrentPageNum).GetValue(fromitem, to) != null)
                GetFromTo(CurrentPageNum).GetValue(fromitem, to).color = Color.white;

            list = FromToList.GetValue(fromitem);

            if (list != null)
            {

                list.ForEach((item) =>
                {
                    if (item != to)
                    {
                        item.GetComponent<Button>().interactable = false;
                        item.GetComponent<Button>().onClick.RemoveAllListeners();
                    }
                });
            }
        }

        to.transform.GetChild(0).gameObject.SetActive(false);
        SaveSelectedNode();
    }

    public void ChangePage(int PageNum)
    {
        PageTransforms[CurrentPageNum - 1].gameObject.SetActive(false);
        PageTransforms[PageNum - 1].gameObject.SetActive(true);

        LoadPage(PageNum);
    }

    #endregion

    #region BinaryFormmater

    private void BinarySerialize(SelectedNode node, string Path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(Path, FileMode.Create);
        binaryFormatter.Serialize(fileStream, node);
        fileStream.Close();
    }

    private SelectedNode BinaryDeserialize(string Path)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(Path, FileMode.Open);

        SelectedNode node = (SelectedNode)binaryFormatter.Deserialize(fileStream);
        fileStream.Close();

        return node;
    }

    #endregion
}
