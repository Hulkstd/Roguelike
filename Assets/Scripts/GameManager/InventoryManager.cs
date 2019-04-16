using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public Transform Inventories;
    public Transform Equipments;
    public List<Transform> InventorySpaces;
    public List<Transform> EquipmentSpaces;

    private List<ItemMove> ItemImages = new List<ItemMove>();
    private List<Image> EquipmentImages = new List<Image>(4) { null, null, null, null };
    private ItemDatabase itemDatabase;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Text HP;
    [SerializeField]
    private Text MP;
    [SerializeField]
    private Text DF;
    [SerializeField]
    private Text AD;
    [SerializeField]
    private Text AS;
    [SerializeField]
    private Text PO;
    [SerializeField]
    private Text SP;
    [SerializeField]
    private Text CP;
    [SerializeField]
    private Text CD;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        itemDatabase = ItemDatabase.Instance;

        AddItem(itemDatabase.Equipments[0]);
        AddItem(itemDatabase.Equipments[1]);
        AddItem(itemDatabase.Equipments[1]);
        AddItem(itemDatabase.Equipments[2]);
        AddItem(itemDatabase.Equipments[2]);
        AddItem(itemDatabase.Equipments[2]);
    }

    public void OpenInventory()
    {
        animator.SetBool("Open Inventory", true);
    }

    public void CloseInventory()
    {
        animator.SetBool("Open Inventory", false);
    }

    public bool AddItem(EquipmentsObject item)
    {
        if(ItemImages.Count == InventorySpaces.Count)
        {
            return false;
        }

        GameObject gameObject = new GameObject("Item");

        gameObject.transform.SetParent(Inventories);
        RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
        rectTransform.localScale = Vector3.one * 1.4f;

        Image image = gameObject.AddComponent<Image>();
        image.sprite = item.EquipImage;

        ItemMove itemMove = gameObject.AddComponent<ItemMove>();
        itemMove.Activity += EquipmentApplication;
        itemMove.Image = image;
        itemMove.Item = item;

        gameObject.transform.position = InventorySpaces[ItemImages.Count].transform.position;

        ItemImages.Add(itemMove);

        return true;
    }

    public bool AddItem(Image item)
    {
        if (ItemImages.Count == InventorySpaces.Count)
        {
            return false;
        }

        ItemMove itemMove = item.GetComponent<ItemMove>();

        itemMove.enabled = true;
        itemMove.transform.position = InventorySpaces[ItemImages.Count].transform.position;
        itemMove.transform.localScale = Vector3.one * 1.4f;

        ItemImages.Add(itemMove);

        return true;
    }

    public Image RemoveItem(ItemMove item)
    {
        if (!ItemImages.Contains(item))
            return null;

        int index = ItemImages.FindIndex(obj => obj == item);
        Image image = ItemImages[index].Image;
        ItemImages.RemoveAt(index);

        for (int i = 0; i < ItemImages.Count; i++)
        {
            ItemImages[i].transform.position = InventorySpaces[i].position;
        }

        return image;
    }

    public bool EquipmentApplication(Transform ImageTransform, ItemMove image)
    {
        float MinDistance = float.MaxValue;
        Transform position = ImageTransform;
        int index = 0;

        for(int i = 0; i < EquipmentSpaces.Count; i++)
        {
            Transform trans = EquipmentSpaces[i];

            if(Vector3.Distance(trans.position, ImageTransform.position) < MinDistance)
            {
                MinDistance = Vector3.Distance(trans.position, ImageTransform.position);
                position = trans;
                index = i;
            }
        }

        if(MinDistance <= 50)
        {
            if (position.CompareTag(image.Item.EquipType))
            {
                if (EquipmentImages[index] == null)
                {
                    RemoveItem(image);
                    EquipmentImages[index] = image.Image;

                    image.transform.localScale = Vector3.one * 0.6f;
                    image.transform.position = position.position;
                }
                else
                {
                    Image tmp = EquipmentImages[index];
                    EquipmentImages[index] = image.Image;
                    RemoveItem(image);
                    AddItem(tmp);

                    image.transform.localScale = Vector3.one * 0.6f;
                    image.transform.position = position.position;
                }

                return true;
            }
            return false;
        }
        return false;
    }

    public void ApplyStatWindow()
    {
        StatManager statManager = StatManager.Instance;

        HP.text = $"HP : { statManager.HPStat * statManager.StatperHP + 30 /*기본*/ }";
        MP.text = $"MP : { statManager.MPStat * statManager.StatperMP + 22 /*기본*/ }";
        DF.text = $"DF : { statManager.HPStat * statManager.StatperHP + 0  /*기본*/ }";
        AD.text = $"AD : { (EquipmentImages[0] ? EquipmentImages[0].GetComponent<ItemMove>().Item.TryGetStat("Power") : 0) }";
        AS.text = $"AS : { (EquipmentImages[0] ? EquipmentImages[0].GetComponent<ItemMove>().Item.AttackSpeed : 1) }";
        PO.text = $"PO : { statManager.PowerStat * statManager.StatperPower + 3  /*기본*/ }";
        SP.text = $"SP : { statManager.SpeedStat * statManager.StatperSpeed + (EquipmentImages[1] ? EquipmentImages[1].GetComponent<ItemMove>().Item.Speed : 0)+ 1 /*기본*/ }";
        CP.text = $"CP : { statManager.CriticalPercent * statManager.StatperCriticalPercent + 0 /*기본*/ }";
        CD.text = $"CD : { statManager.CriticalDamage * statManager.StatperCriticalDamage + 1.5f  /*기본*/ }";
    }
}
