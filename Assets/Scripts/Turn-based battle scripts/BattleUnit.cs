using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UnitState { Acting, Waiting, Selecting, ChoosingTarget}
public enum CommandType { Attack, Skill}

public class BattleUnit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;
    public int atk;
    public int maxHP;
    public int currHP;
    public bool isDead;
    public UnitState state;

    public BattleHandler bh;
    public BattleHUD bhud;
    public GameObject indicator;

    private Vector2 forward = Vector2.left;
    private Vector2 originalPos;

    private CommandType chosenCommand;
    public int currTargetIndex = 0;

    private void Start()
    {
        indicator.SetActive(false);
        isDead = false;
        state = UnitState.Waiting;
        originalPos = transform.position;
        currHP = maxHP;
    }

    private void Update()
    {
        switch (state)
        {
            case UnitState.Selecting:
                if (Input.GetKeyDown(KeyCode.J)) // attack
                {
                    // select target then attack
                    state = UnitState.ChoosingTarget;
                    chosenCommand = CommandType.Attack;
                    SetDefaultTarget();
                    bhud.CloseCommandMenu();
                    bhud.SetLog("Choose a target!");
                }
                if (Input.GetKeyDown(KeyCode.K)) // skip turn
                {
                    bhud.CloseCommandMenu();
                    bhud.SetLog("Skipped Turn!");
                    StartCoroutine(TurnEnd());
                }
                break;

            case UnitState.ChoosingTarget:
                ChoosingTarget();
                break;

            case UnitState.Waiting:
                break;

            case UnitState.Acting:
                break;
        }
    }

    public Vector2 GetPosition()
    {
        return (Vector2)transform.position;
    }

    private void ChoosingTarget()
    {
        // return to selecting command if cancel
        if (Input.GetKeyDown(KeyCode.K))
        {
            bh.TurnOffAllIndicators();
            state = UnitState.Selecting;
            bhud.OpenCommandMenu((Vector2)transform.position + new Vector2(-2f, 0));
            bhud.SetLog("Choose an action!");
            return;
        }

        // switching target
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            bh.TurnOffAllIndicators();
            do
            {
                if (currTargetIndex == 0) currTargetIndex = bh.enemies.Count;
                currTargetIndex = (currTargetIndex - 1) % bh.enemies.Count;
            } while (bh.enemies[currTargetIndex].isDead);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            bh.TurnOffAllIndicators();
            do
            {
                currTargetIndex = (currTargetIndex + 1) % bh.enemies.Count;
            } while (bh.enemies[currTargetIndex].isDead);
        }

        // indicator on target
        if (!bh.enemies[currTargetIndex].indicator.activeSelf)
        {
            bh.enemies[currTargetIndex].indicator.SetActive(true);
        }

        switch (chosenCommand)
        {
            case CommandType.Attack:
                if (Input.GetKeyDown(KeyCode.J))
                {
                    bh.TurnOffAllIndicators();
                    Attack(bh.enemies[currTargetIndex]);
                }
                break;

            case CommandType.Skill:
                break;
        }      
    }

    private void SetDefaultTarget()
    {
        currTargetIndex = 0;
        while (bh.enemies[currTargetIndex].isDead)
        {
            currTargetIndex = (currTargetIndex + 1) % bh.enemies.Count;
        }
    }

    public void Attack(BattleUnit target)
    {
        state = UnitState.Acting;
        StartCoroutine(UnitAttack(target));
    }

    public void SetForwardVector(bool isPlayer)
    {
        forward = isPlayer ? Vector2.left : Vector2.right;
    }

    public void TurnStart()
    {
        StartCoroutine(PlayTurn());
    }

    public void Damage(int value)
    {
        currHP = Mathf.Clamp(currHP - value, 0, 9999);
        if (currHP <= 0)
        {
            isDead = true;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    IEnumerator PlayTurn()
    {
        // move forward
        yield return Move(originalPos + forward * 1f, 0.25f);
        state = UnitState.Selecting;
        bhud.OpenCommandMenu((Vector2)transform.position + new Vector2(-2f, 0));
        bhud.SetLog("Choose an action!");
    }

    IEnumerator TurnEnd()
    {
        // move back
        yield return Move(originalPos, 0.25f);
        state = UnitState.Waiting;
        bh.NextTurn();
    }

    IEnumerator Move(Vector2 dest, float duration)
    {
        float t = duration;
        Vector2 start = transform.position;
        while (t >=0)
        {
            transform.position = Vector2.Lerp(start, dest, (duration - t) / duration);
            t -= Time.deltaTime;
            yield return null;
        }
        transform.position = dest;
    }

    IEnumerator UnitAttack(BattleUnit target)
    {
        // move to target
        Vector2 moveVector = (target.GetPosition() - (Vector2)transform.position);
        Vector2 movePos = (Vector2)transform.position + moveVector * 0.9f;
        yield return Move(movePos, 0.5f);

        // attack
        target.Damage(atk);
        string log = $"{unitName} hit {target.unitName} for {atk} damage.";
        bhud.SetLog(log);
        yield return new WaitForSeconds(0.5f);
        yield return TurnEnd();
    }
}
