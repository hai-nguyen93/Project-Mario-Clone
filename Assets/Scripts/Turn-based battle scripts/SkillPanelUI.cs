using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillPanelUI : MonoBehaviour
{
    public GameObject skillItemPrefab;

    public BattleUnit testing;

    // Start is called before the first frame update
    void Start()
    {
        ClearAllSkills();
        SetupSkills(testing);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearAllSkills()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }     
    }

    public void SetupSkills(BattleUnit owner)
    {
        foreach (var skill in owner.skills)
        {
            var skillItem = Instantiate(skillItemPrefab, transform);
            skillItem.GetComponent<TextMeshProUGUI>().text = skill.name;
            SwitchIndicator(skillItem, false);
        }
    }

    public void SwitchIndicator(GameObject skillItem, bool on)
    {
        skillItem.transform.GetChild(0).gameObject.SetActive(on);
    }
}
