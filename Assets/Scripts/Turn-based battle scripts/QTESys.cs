using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum QTEState { Fail, Success, CritialSuccess, Processing}
public class QTESys : MonoBehaviour
{
    public float timeLimit = 3f;
    public float timer;
    public Text timerText;
    public Text comboText;
    public bool active = false;
    public QTEState state = QTEState.Processing;

    private void Start()
    {
        active = false;
        timerText.enabled = false;
        comboText.enabled = false;
        state = QTEState.Processing;
    }

    public void SetupQTE()
    {
        state = QTEState.Processing;
        timer = timeLimit;
        timerText.text = timer + "s";
        timerText.enabled = true;
        active = true;
    }

    public IEnumerator PlayQTE(KeyCode keyToPress)
    {
        SetupQTE();

        yield return new WaitForSeconds(0.05f);
        while (timer > 0)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(keyToPress))
                {
                    state = QTEState.Success;
                }
                else
                {
                    state = QTEState.Fail;
                }
                TurnOffUI();
                yield break;
            }

            timerText.text = timer + "s";
            timer -= Time.deltaTime;
            yield return null;
        }

        timer = 0;
        state = QTEState.Fail;
        TurnOffUI();
    }

    public IEnumerator PlayQTECombo(List<KeyCode> keys)
    {
        SetupQTE();
        int i = 0;
        comboText.text = "";
        foreach (KeyCode key in keys)
        {
            comboText.text += (key.ToString() + " ");
        }
        comboText.enabled = true;

        yield return new WaitForSeconds(0.05f);
        while (timer > 0)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(keys[i]))
                {
                    ++i;
                    if (i >= keys.Count)
                    {
                        state = QTEState.Success;
                        TurnOffUI();
                        yield break;
                    }
                }
                else
                {
                    state = QTEState.Fail;
                    TurnOffUI();
                    yield break;
                }
            }

            timerText.text = timer + "s";
            timer -= Time.deltaTime;
            yield return null;
        }

        timer = 0;
        state = QTEState.Fail;
        TurnOffUI();
    }

    public void TurnOffUI()
    {
        active = false;
        timerText.enabled = false;
        comboText.enabled = false;
    }
}

