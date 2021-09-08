using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Start, Battle, Win, Lose}

public class BattleHandler : MonoBehaviour
{
    public BattleState state = BattleState.Start;
    public BattleHUD battleHUD;

    public List<BattleUnit> players;
    public List<BattleUnit> enemies;
    public List<BattleUnit> battlers;

    public int currentUnitTurn = 0;

    private void Start()
    {
        state = BattleState.Start;
        StartCoroutine(SetupBattle());
    }

    public void NextTurn()
    {
        CheckBattleCondition();
        if (state == BattleState.Win)
        {
            battleHUD.SetLog("Player Won!!");
        }
        else if (state == BattleState.Lose)
        {
            battleHUD.SetLog("Player Lost!!");
        }
        else
        {
            currentUnitTurn = (currentUnitTurn + 1) % battlers.Count;
            if (!battlers[currentUnitTurn].isDead)
            {
                if (players.Contains(battlers[currentUnitTurn])) // player's unit's turn
                {
                    battlers[currentUnitTurn].TurnStart();
                }
                else
                {
                    int targetIndex = Random.Range(0, 3);
                    while (players[targetIndex].isDead) { targetIndex = (targetIndex + 1) % players.Count; }
                    battlers[currentUnitTurn].Attack(players[targetIndex]);
                }
            }
            else
                NextTurn();
        }
    }

    public void CheckBattleCondition()
    {
        if (CheckPartyAllDead(players))
        {
            state = BattleState.Lose;
            return;
        }

        if (CheckPartyAllDead(enemies))
        {
            state = BattleState.Win;
            return;
        }
    }

    public bool CheckPartyAllDead(List<BattleUnit> party)
    {
        // assume all members are deadl if at least 1 is alive, return false
        foreach (BattleUnit b in party)
        {
            if (!b.isDead)
            {
                return false;
            }
        }
        return true;
    }

    public void TurnOffAllIndicators()
    {
        foreach (BattleUnit b in battlers)
        {
            b.indicator.SetActive(false);
        }
    }

    IEnumerator SetupBattle()
    {
        battlers = new List<BattleUnit>(players.Count + enemies.Count);
        foreach (BattleUnit b in players)
        {
            b.SetForwardVector(true);
            battlers.Add(b);
        }
        foreach (BattleUnit b in enemies)
        {
            b.SetForwardVector(false);
            battlers.Add(b);
        }
        currentUnitTurn = 0;

        yield return new WaitForSeconds(0.7f);
        state = BattleState.Battle;
        NextTurn();
    }
}
