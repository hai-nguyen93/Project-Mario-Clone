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
    public KeyCode keyToPress = KeyCode.J;
    public bool active = false;
    public QTEState state = QTEState.Processing;

    private void Start()
    {
        active = false;
        timerText.enabled = false;
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

    public IEnumerator PlayQTE()
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
                active = false;
                timerText.enabled = false;
                yield break;
            }

            timerText.text = timer + "s";
            timer -= Time.deltaTime;
            yield return null;
        }

        timer = 0;
        state = QTEState.Fail;
        active = false;
        timerText.enabled = false;
    }
}
