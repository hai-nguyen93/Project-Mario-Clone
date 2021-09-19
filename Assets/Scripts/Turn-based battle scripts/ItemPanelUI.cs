using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPanelUI : MonoBehaviour
{
    public GameObject itemItemPrefab;
    public RectTransform scrollArea;
    private RectTransform panel;
    public BattleUnit owner;

    public List<GameObject> menuItems;
    private int currIndex;
    private Vector3[] currItemCorners = new Vector3[4];
    private Vector3[] scrollAreaCorners = new Vector3[4];
    private float extraHeight;

    void Awake()
    {
        panel = GetComponent<RectTransform>();
        scrollArea.GetWorldCorners(scrollAreaCorners);
        extraHeight = GetComponent<VerticalLayoutGroup>().spacing;
        //SetupSkills(owner);
    }

    private void OnEnable()
    {
        //Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currIndex < menuItems.Count - 1) // not at last item yet
            {
                SwitchIndicator(menuItems[currIndex], false);
                ++currIndex;
                SwitchIndicator(menuItems[currIndex], true);

                // scroll menu down if needed
                var itemRect = menuItems[currIndex].GetComponent<RectTransform>();
                itemRect.GetWorldCorners(currItemCorners);
                if (currItemCorners[0].y < scrollAreaCorners[0].y)
                {
                    panel.localPosition = new Vector3(panel.localPosition.x, panel.localPosition.y + itemRect.rect.height + extraHeight, panel.localPosition.z);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currIndex > 0) // not at first item yet
            {
                SwitchIndicator(menuItems[currIndex], false);
                --currIndex;
                SwitchIndicator(menuItems[currIndex], true);

                // scroll menu up if needed
                var itemRect = menuItems[currIndex].GetComponent<RectTransform>();
                itemRect.GetWorldCorners(currItemCorners);
                if (currItemCorners[1].y > scrollAreaCorners[1].y)
                {
                    panel.localPosition = new Vector3(panel.localPosition.x, panel.localPosition.y - itemRect.rect.height - extraHeight, panel.localPosition.z);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.J)) // confirm skill
        {
            owner.currSkillIndex = currIndex;
            owner.ChangeState(UnitState.ChoosingTarget);
            gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.K)) // cancel
        {
            owner.ChangeState(UnitState.ChoosingAction);
            gameObject.SetActive(false);
        }
    }

    public void ClearAllItems()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        menuItems.Clear();
    }

    public void SetupItems(BattleUnit owner)
    {
        ClearAllItems();

        this.owner = owner;
        foreach (var item in owner.party.inventory)
        {
            var itemItem = Instantiate(itemItemPrefab, transform);
            itemItem.GetComponent<TextMeshProUGUI>().text = item.itemBase.name + "\t" + item.quantity;
            menuItems.Add(itemItem);
            SwitchIndicator(itemItem, false);
        }

        currIndex = 0;
        SwitchIndicator(menuItems[currIndex], true);

        scrollArea.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
    }

    public void SwitchIndicator(GameObject skillItem, bool on)
    {
        skillItem.transform.GetChild(0).gameObject.SetActive(on);
    }
}
