using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UnitState { Acting, Waiting, ChoosingAction, ChoosingTarget, ChoosingSkill, ChoosingItem}
public enum CommandType { Attack, Skill, Item}

public class BattleUnit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;
    public int atk;
    public int maxHP;
    public int currHP;
    public bool isDead;
    public List<SkillBase> skills;
    public PlayerParty party; // for ally unit
    public UnitState state;

    public BattleHandler bh;
    public BattleHUD bhud;
    public QTESys qteSys;
    public GameObject indicator;

    private Vector2 forward = Vector2.left;
    private Vector2 originalPos;

    private CommandType chosenCommand;
    public int currSkillIndex = 0;
    public int currTargetIndex = 0;
    private List<BattleUnit> currTargetParty;

    private void Start()
    {
        indicator.SetActive(false);
        isDead = false;
        state = UnitState.Waiting;
        originalPos = transform.position;
        currHP = maxHP;
        currTargetParty = bh.enemies;
    }

    private void Update()
    {
        switch (state)
        {
            case UnitState.ChoosingAction:
                if (Input.GetKeyDown(KeyCode.J)) // attack
                {
                    // select target then attack
                    ChangeState(UnitState.ChoosingTarget);
                    chosenCommand = CommandType.Attack;                 
                    bhud.SetLog("Choose a target!");
                }

                if (Input.GetKeyDown(KeyCode.I)) // skill
                {
                    chosenCommand = CommandType.Skill;
                    ChangeState(UnitState.ChoosingSkill);
                }

                if (Input.GetKeyDown(KeyCode.L)) // item
                {
                    chosenCommand = CommandType.Item;
                    ChangeState(UnitState.ChoosingItem);
                }

                if (Input.GetKeyDown(KeyCode.K)) // skip turn
                {
                    ChangeState(UnitState.Waiting);
                    bhud.SetLog("Skipped Turn!");
                    StartCoroutine(TurnEnd());
                }
                break;

            case UnitState.ChoosingTarget:
                ChoosingTarget();
                break;

            case UnitState.ChoosingSkill: // let SkillPanelUI handle this
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
            ChangeState(UnitState.ChoosingAction);
            bhud.SetLog("Choose an action!");
            return;
        }

        // switching target
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            bh.TurnOffAllIndicators();
            do
            {
                if (currTargetIndex == 0) currTargetIndex = currTargetParty.Count;
                currTargetIndex = (currTargetIndex - 1) % currTargetParty.Count;
            } while (currTargetParty[currTargetIndex].isDead);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            bh.TurnOffAllIndicators();
            do
            {
                currTargetIndex = (currTargetIndex + 1) % currTargetParty.Count;
            } while (currTargetParty[currTargetIndex].isDead);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) // switch to target player party
        {
            bh.TurnOffAllIndicators();
            SetDefaultTarget(bh.players);
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))// switch to target enemy party
        {
            bh.TurnOffAllIndicators();
            SetDefaultTarget(bh.enemies);
        }

        // indicator on target
        if (!currTargetParty[currTargetIndex].indicator.activeSelf)
        {
            currTargetParty[currTargetIndex].indicator.SetActive(true);
        }

        switch (chosenCommand)
        {
            case CommandType.Attack:
                if (Input.GetKeyDown(KeyCode.J))
                {
                    bh.TurnOffAllIndicators();
                    Attack(currTargetParty[currTargetIndex]);
                }
                break;

            case CommandType.Skill:
                if (Input.GetKeyDown(KeyCode.J))
                {
                    bh.TurnOffAllIndicators();
                    StartCoroutine(UseSkill(currTargetParty[currTargetIndex]));
                }
                break;

            case CommandType.Item:
                if (Input.GetKeyDown(KeyCode.J))
                {
                    bh.TurnOffAllIndicators();
                    StartCoroutine(UseItem(currTargetParty[currTargetIndex]));
                }
                break;
        }      
    }
    
    private void SetDefaultTarget(List<BattleUnit> party)
    {
        currTargetParty = party;
        currTargetIndex = 0;
        while (party[currTargetIndex].isDead)
        {
            currTargetIndex = (currTargetIndex + 1) % party.Count;
        }
    }

    public void Attack(BattleUnit target)
    {
        ChangeState(UnitState.Acting);
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

    public void ChangeState(UnitState newState)
    {
        state = newState;
        bhud.TurnOffAllMenus();
        bh.TurnOffAllIndicators();

        switch (state)
        {
            case UnitState.ChoosingAction:
                bhud.OpenCommandMenu((Vector2)transform.position + new Vector2(-2f, 0));
                break;

            case UnitState.ChoosingSkill:
                bhud.OpenSkillMenu(this);
                break;

            case UnitState.ChoosingItem:
                bhud.OpenItemMenu(this);
                break;

            case UnitState.ChoosingTarget:
                SetDefaultTarget(bh.enemies);
                break;
        }
    }

    IEnumerator PlayTurn()
    {
        // move forward
        yield return Move(originalPos + forward * 1f, 0.25f);
        ChangeState(UnitState.ChoosingAction);
        bhud.OpenCommandMenu((Vector2)transform.position + new Vector2(-2f, 0));
        bhud.SetLog("Choose an action!");
    }

    IEnumerator TurnEnd()
    {
        // move back
        yield return Move(originalPos, 0.25f);
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
        string log = $"{unitName} hit {target.unitName} for {atk} damage.";
        bhud.SetLog(log);
        bh.PlayParticleEffect("AttackParticle", target.GetPosition());
        yield return new WaitForSeconds(0.5f);
        target.Damage(atk);
        ChangeState(UnitState.Waiting);
        yield return TurnEnd();
    }

    IEnumerator UseSkill(BattleUnit target)
    {
        ChangeState(UnitState.Acting);

        float dmg = skills[currSkillIndex].power;
        if (skills[currSkillIndex].qte)
        {
            yield return qteSys.PlayQTE(skills[currSkillIndex].qteKey);
            if (qteSys.state != QTEState.Success)
            {
                bhud.SetLog("Fail QTE!!!");
                dmg = dmg / 2f;
            }
            else
                bhud.SetLog("Success QTE!!");

            yield return new WaitForSeconds(0.5f);
        }
        else if (skills[currSkillIndex].qteCombo)
        {
            yield return qteSys.PlayQTECombo(skills[currSkillIndex].qteKeys);
            if (qteSys.state != QTEState.Success)
            {
                bhud.SetLog("Fail QTE!!!");
                dmg = dmg / 2f;
            }
            else
                bhud.SetLog("Success QTE!!");

            yield return new WaitForSeconds(0.5f);
        }

        yield return skills[currSkillIndex].ActivateSkill(target);
        bhud.SetLog($"{name} used skill {skills[currSkillIndex].name} on {target.name} for {(int) dmg} damage.");
        target.Damage((int) dmg);
        yield return new WaitForSeconds(0.25f);
        ChangeState(UnitState.Waiting);
        yield return TurnEnd();
    }

    IEnumerator UseItem(BattleUnit target)
    {
        ChangeState(UnitState.Acting);

        yield return party.inventory[currSkillIndex].itemBase.ActivateItem(target);
        bhud.SetLog($"{name} used {party.inventory[currSkillIndex].itemBase.name} on {target.name}, healed {(int)party.inventory[currSkillIndex].itemBase.power}hp.");
        party.inventory[currSkillIndex].quantity--;
        yield return new WaitForSeconds(0.35f);
        ChangeState(UnitState.Waiting);
        yield return TurnEnd();
    }
}
