using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI battleLog;
    public GameObject commandMenu;

    // Start is called before the first frame update
    void Start()
    {
        commandMenu.SetActive(false);
        battleLog.text = "Battle begins!";
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
