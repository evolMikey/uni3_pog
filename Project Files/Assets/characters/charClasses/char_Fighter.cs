using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_Fighter : ABC_character
{
    #region Ability Cooldowns and Uses
    int ab_RaiseShield_Cooldown = 1;
    int ab_FlankingMove_Cooldown = 2;
    int ab_FlankingMove_Uses = 4;
    #endregion

    protected override void Start()
    {
        base.Start();
    }

    public override void actGetAllTargets()
    {
        base.actGetAllTargets();
    }

    public override void actChooseAbility()
    {
        base.actChooseAbility();

        if (myTargets.Count == 0)
        {
            return;
        }

        // If low on health, use Raise Shield
        if ((myCurHealth < 5) && (ab_RaiseShield_Cooldown == 1))
        {
            ab_RaiseShield();
            return;
        }
        
        // If enemy is particularly strong, 1/4 chance use flanking
        if ((myChosenTargets[0].myCurHealth > 5) && (Random.Range(0, 3) == 4) && (ab_FlankingMove_Cooldown == 2))
        {
            ab_FlankingMove(myChosenTargets[0]);
            return;
        }

        // If all else fails, do the standard attack
        {
            ab_baseAttack(myChosenTargets[0]);
            return;
        }
    }

    // Increases the counter for each cooldown per turn
    public override void turnGetCooldowns()
    {
        base.turnGetCooldowns();
        if (ab_RaiseShield_Cooldown != 1)
            ab_RaiseShield_Cooldown++;
        if (ab_FlankingMove_Cooldown != 2)
            ab_FlankingMove_Cooldown++;
    }

    // Functions for each ability the character is capable of
    #region Abilities
    private void ab_baseAttack(ABC_character targetChar)
    {
        // Standard attack
        int outDmg = gameEnums.DiceRoll(2, 3, 0);
        targetChar.TakeDamage(outDmg);
        Debug.Log(myName + " attacked " + targetChar.myName + " for " + outDmg);
    }

    private void ab_RaiseShield()
    {
        Debug.Log(myName + " raises his shield!");
        ab_RaiseShield_Cooldown = 0;
        defenseCounter = 2;
    }

    private void ab_FlankingMove(ABC_character myTarget)
    {
        // If target rolls higher
        if (myTarget.mySaveRoll > (gameEnums.DiceRoll(1,6,0)))
        {
            // Flanking attack
            int outDmg = gameEnums.DiceRoll(2, 5, 0);
            myTarget.TakeDamage(outDmg);
            Debug.Log(myName + " flanked " + myTarget.myName + " for " + outDmg);
        }
        ab_FlankingMove_Cooldown = 0;
        ab_FlankingMove_Uses--;
    }
    #endregion
}
