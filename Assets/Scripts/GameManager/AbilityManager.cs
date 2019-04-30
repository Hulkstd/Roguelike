using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance { get; private set; }

    #region Class

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
    public class CToFrom : Dictionary<GameObject, List<GameObject>>
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
        public List<string> SelectedNodes;
    }

    #endregion

    public CFromTo FromTo;
    public CToFrom ToFrom;
    public CToFrom FromToList;
    public string FilePath;

    private SelectedNode SelectedNodes;
    [SerializeField]
    private Transform Buttons;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        FilePath = Application.dataPath + FilePath;

        if(System.IO.File.Exists(FilePath))
        {
            SelectedNodes = BinaryDeserialize(FilePath);
        }
        else
        {
            SelectedNodes = new SelectedNode();
            SelectedNodes.SelectedNodes = new List<string>();
        }

        foreach(KeyValuePair<NodeToNode, UILineRenderer> keyValue in FromTo)
        {
            List<GameObject> a = new List<GameObject>();

            if(!ToFrom.TryGetValue(keyValue.Key.To, out a))
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
            if(keyValue.Value != null) keyValue.Value.color = Color.gray;
        }

        SelectedNodes.SelectedNodes.ForEach(str =>
        {
            KeyValuePair<NodeToNode, UILineRenderer> keyValue = FromTo.GetKeyValuePairUsingString(str);
            if (keyValue.Key != null && keyValue.Value != null)
            {
                keyValue.Key.From.GetComponent<Button>().image.color = Color.white;
                keyValue.Key.To.GetComponent<Button>().image.color = Color.white;
                keyValue.Value.color = Color.white;
            }
            else if(keyValue.Key != null && keyValue.Value == null)
            {
                keyValue.Key.To.GetComponent<Button>().image.color = Color.white;
            }
        });

        SelectedNodes.SelectedNodes.ForEach((str) =>
        {
            NodeToNode node = FromTo.GetKeyValuePairUsingString(str).Key;

            List<GameObject> list = FromToList.GetValue(node.From);

            list.ForEach((item) =>
            {
                if (item.name != str)
                {
                    item.GetComponent<Button>().interactable = false;
                    item.GetComponent<Button>().onClick.RemoveAllListeners();
                }
            });

            list = FromToList.GetValue(node.To);

            list.ForEach((item) =>
            {
                if (item.name != str)
                {
                    item.GetComponent<Button>().interactable = false;
                    item.GetComponent<Button>().onClick.RemoveAllListeners();
                }
            });
        });

        for (int i = 0; i < Buttons.childCount; i++)
        {
            Button button = Buttons.GetChild(i).GetComponent<Button>();

            button.onClick.AddListener(() => { SelectNode(button.gameObject); });
        }
    }

    public void SaveSelectedNode()
    {
        BinarySerialize(SelectedNodes, FilePath);
    }

    public void SelectNode(GameObject to)
    {
        List<GameObject> list = ToFrom.GetValue(to);
        bool flag = false;
        GameObject fromitem = null;

        if(ToFrom.GetValue(to) == null)
        {
            flag = true;
        }

        SelectedNodes.SelectedNodes.ForEach((item) =>
        {
            list.ForEach((from) =>
            {
                if(item == from.name)
                {
                    flag = true;
                    fromitem = from;
                }
            });
        });

        if(flag)
        {
            SelectedNodes.SelectedNodes.Add(to.name);
            to.gameObject.GetComponent<Button>().image.color = Color.white;
            to.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            FromTo.GetValue(fromitem, to).color = Color.white;

            list = FromToList.GetValue(fromitem);

            list.ForEach((item) =>
            {
                if (item != to)
                {
                    item.GetComponent<Button>().interactable = false;
                    item.GetComponent<Button>().onClick.RemoveAllListeners();
                }
            });
        }

        BinarySerialize(SelectedNodes, FilePath);
    }

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
