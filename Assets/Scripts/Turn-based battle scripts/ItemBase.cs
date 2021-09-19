using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Heal, Attack, Other}

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemBase : ScriptableObject
{
    public new string name;
    public string description;
    public ItemType type;
    public int power;

    public IEnumerator ActivateItem(BattleUnit target)
    {
        switch (type)
        {
            case ItemType.Heal:
                target.currHP = Mathf.Clamp(target.currHP + power, 0, target.maxHP);
                break;

            case ItemType.Attack:
                break;

            case ItemType.Other:
                break;
        }
        yield return null;
    }
}

[System.Serializable]
public class Item
{
    public ItemBase itemBase;
    public int quantity;

    public Item(ItemBase itemBase, int quantity)
    {
        this.itemBase = itemBase;
        this.quantity = quantity;
    }
}
