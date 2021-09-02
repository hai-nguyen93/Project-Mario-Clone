using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI battleLog;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLog(string log)
    {
        battleLog.text = log;
    }
}
