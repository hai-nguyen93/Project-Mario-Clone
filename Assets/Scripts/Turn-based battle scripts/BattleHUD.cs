using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI battleLog;
    public GameObject commandMenu;
    public GameObject skillMenu;
    public GameObject itemMenu;
    public SkillPanelUI skillPanel;
    public ItemPanelUI itemPanel;
    public FloatingText pfFloatingText;

    // Start is called before the first frame update
    void Start()
    {
        commandMenu.SetActive(false);
        skillMenu.SetActive(false);
        battleLog.text = "Battle begins!";
    }

    public void SetLog(string log)
    {
        battleLog.text = log;
    }
    
    public void OpenCommandMenu(Vector2 position)
    {
        commandMenu.transform.position = Camera.main.WorldToScreenPoint(position);
        commandMenu.SetActive(true);
    }

    public void CloseCommandMenu()
    {
        commandMenu.SetActive(false);
    }

    public void OpenSkillMenu(BattleUnit owner)
    {
        // set up skill menu
        skillPanel.SetupSkills(owner);

        skillMenu.SetActive(true);
        skillPanel.gameObject.SetActive(true);
    }

    public void CloseSkillMenu()
    {
        skillMenu.SetActive(false);
    }

    public void OpenItemMenu(BattleUnit owner)
    {
        // set up skill menu
        itemPanel.SetupItems(owner);

        itemMenu.SetActive(true);
        itemPanel.gameObject.SetActive(true);
    }

    public void CloseItemMenu()
    {
        itemMenu.SetActive(false);
    }

    public void TurnOffAllMenus()
    {
        skillMenu.SetActive(false);
        itemMenu.SetActive(false);
        commandMenu.SetActive(false);
    }

    public void ShowFloatingText(Vector2 position, int value, Color color)
    {
        FloatingText ft = Instantiate(pfFloatingText, position, Quaternion.identity);
        ft.Setup(value, color);
    }
}
