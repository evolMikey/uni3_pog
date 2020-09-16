using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_Ranger : ABC_character
{
    #region Ability Cooldowns and Uses
    int ab_MultiShot_Cooldown = 1;
    int ab_Camouflage_Cooldown = 1;
    #endregion

    protected override void Start()
    {
        base.Start();
        myTargetZone.radius = 8;
    }

    public override void actGetAllTargets()
    {
        base.actGetAllTargets();
    }

    public override void actChooseAbility()
    {
        // Base function will cause combat to skip if no enemy is found
        base.actChooseAbility();

        if (myTargets.Count == 0)
        {
            return;
        }

        // If there are at least three enemies around and cooldown is not in effect
        if ((myChosenTargets.Count >= 3) && (ab_MultiShot_Cooldown == 1))
        {
            // Targets first three on list, then exits function
            ab_MultiShot(myChosenTargets[0], myChosenTargets[1], myChosenTargets[2]);
            return;
        }
        else
        {
            // If there aren't enough targets or power is cooling down, fire regular arrow
            ab_baseAttack(myChosenTargets[0]);
            return;
        }

    }

    // Increases the counter for each cooldown per turn
    public override void turnGetCooldowns()
    {
        base.turnGetCooldowns();
        if (ab_MultiShot_Cooldown != 1)
            ab_MultiShot_Cooldown++;
        if (ab_Camouflage_Cooldown != 1)
            ab_Camouflage_Cooldown++;
    }

    // Functions for each ability the character is capable of
    #region Abilities
    private void ab_baseAttack(ABC_character targetChar)
    {
        // Standard attack
        int outDmg = gameEnums.DiceRoll(1, 4, 0);
        targetChar.TakeDamage(outDmg);
        Debug.Log(myName + " attacked " + targetChar.myName + " for " + outDmg);
    }

    private void ab_MultiShot(ABC_character target1, ABC_character target2, ABC_character target3)
    {
        // Slightly weaker attack, does damage to multiple characters
        int outDmg1 = gameEnums.DiceRoll(1, 3, 0);
        int outDmg2 = gameEnums.DiceRoll(1, 3, 0);
        int outDmg3 = gameEnums.DiceRoll(1, 3, 0);
        if (target1 != null)
        {
            target1.TakeDamage(outDmg1);
            Debug.Log(myName + " attacked " + target1.myName + " for " + outDmg1);
        }
        if (target2 != null)
        {
            target2.TakeDamage(outDmg2);
            Debug.Log(myName + " attacked " + target2.myName + " for " + outDmg2);
        }
        if (target3 != null)
        {
            target3.TakeDamage(outDmg3);
            Debug.Log(myName + " attacked " + target3.myName + " for " + outDmg3);
        }
        ab_MultiShot_Cooldown = 0;
        return;
    }
    #endregion
}