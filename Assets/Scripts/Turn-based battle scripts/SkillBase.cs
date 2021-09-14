using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class SkillBase : ScriptableObject
{
    public new string name;
    public string description;
    public ParticleSystem particlePrefab;
    public bool qte;

    public int power;

    public IEnumerator ActivateSkill(BattleUnit target)
    {
        float waitTime = 0.25f;
        if (particlePrefab)
        {
            Instantiate(particlePrefab, target.GetPosition(), Quaternion.identity);
            yield return new WaitForSeconds(particlePrefab.main.startLifetime.constant + waitTime);
        }
        yield return new WaitForSeconds(waitTime);
    }
}
