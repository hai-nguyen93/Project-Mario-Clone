using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;
    public int atk;
    public int maxHP;
    public int currHP;
    public bool isDead;
    public bool isTurn;

    public BattleHandler bh;
    public BattleHUD bhud;

    private Vector2 forward = Vector2.left;
    private Vector2 originalPos;

    private void Start()
    {
        isDead = false;
        isTurn = false;
        originalPos = transform.position;
    }

    public Vector2 GetPosition()
    {
        return (Vector2)transform.position;
    }

    public void Attack(BattleUnit target)
    {
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
        yield return Move(originalPos + forward * 2f, 0.5f);
        Attack(bh.enemies[0]);
    }

    IEnumerator TurnEnd()
    {
        yield return Move(originalPos, 0.5f);
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
        Vector2 moveVector = (target.GetPosition() - (Vector2)transform.position);
        Vector2 movePos = (Vector2)transform.position + moveVector * 0.9f;
        yield return Move(movePos, 0.5f);
        target.Damage(atk);
        string log = $"{unitName} hit {target.unitName} for {atk} damage.";
        bhud.SetLog(log);
        yield return new WaitForSeconds(0.5f);
        yield return TurnEnd();
    }
}
