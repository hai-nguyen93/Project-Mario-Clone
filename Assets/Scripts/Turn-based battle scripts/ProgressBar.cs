using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public int max;
    public int current;
    public Image bar;


    public void SetupBar(int maxValue, int currentValue)
    {
        max = maxValue;
        GetComponent<Image>().enabled = true;
        bar.enabled = true;
        UpdateBar(currentValue);
    }

    public void UpdateBar(int currentValue)
    {
        current = currentValue;
        FillBar();
    }

    public void FillBar()
    {
        float amount = (float)current / (float)max;
        bar.GetComponent<RectTransform>().localScale = new Vector3(amount, 1, 1);

        if (amount <= 0) {
            GetComponent<Image>().enabled = false;
            bar.enabled = false;
        }
    }
}
