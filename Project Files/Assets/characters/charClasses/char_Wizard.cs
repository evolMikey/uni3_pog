using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_Wizard : ABC_character
{
    /*
    #region Ability Cooldowns and Uses
    int ab_Fireball_Cooldown = 3;
    int ab_Fireball_Uses = 2;
    int ab_BlessedHealing_Cooldown = 1;
    int ab_TurnUndead_Cooldown = 1;
    int ab_SummonUndead_Cooldown = 3;
    int ab_SummonUndead_Uses = 2;
    int ab_MagicalWard_Cooldown = 3;
    int ab_MagicalWard_Uses = 3;
    int ab_SummonDemon_Cooldown = 5;
    int ab_SummonDemon_Uses = 1;
    #endregion

    protected override void Start()
    {
        base.Start();
        myTargetZone.radius = 6;
    }

    public override void actGetAllTargets()
    {
        // Allows base-character to get some targets first, then go to class priorities
        base.actGetAllTargets();

        foreach (ABC_character item in myTargets)
        {
            // Undead are preferred targets
            if (item.myRace == gameEnums.charRaces.undead)
            {
                myChosenTargets.Add(item);
            }
        }
    }

    public override void actChooseAbility()
    {
        base.actChooseAbility();

        if (myChosenTargets[0].myRace == gameEnums.charRaces.demon)
    }

    // Increases the counter for each cooldown per turn
    public override void turnGetCooldowns()
    {
        base.turnGetCooldowns();
        if (ab_Fireball_Cooldown != 3)
            ab_Fireball_Cooldown++;
        if (ab_BlessedHealing_Cooldown != 1)
            ab_BlessedHealing_Cooldown++;
        if (ab_SummonUndead_Cooldown != 3)
            ab_SummonUndead_Cooldown++;
        if (ab_MagicalWard_Cooldown != 3)
            ab_MagicalWard_Cooldown++;
        if (ab_SummonDemon_Cooldown != 5)
            ab_SummonDemon_Cooldown++;
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
    
    private void ab_BlessedHeal(ABC_character targetChar)
    {
        // Blessed Heal will harm undead/demons but heal (negative damage) all else
        if ((targetChar.myRace == gameEnums.charRaces.undead) || (targetChar.myRace == gameEnums.charRaces.demon))
        {
            int outDmg = gameEnums.DiceRoll(1, 6, 0);
            targetChar.TakeDamage(outDmg);
            Debug.Log(myName + " Blessed Healed " + targetChar.myName + " for " + outDmg);
        }
        else
        {
            int outDmg = gameEnums.DiceRoll(1, 3, 0);
            targetChar.TakeDamage(-outDmg);
            Debug.Log(myName + " Blessed Healed " + targetChar.myName + " for " + outDmg);
        }
        ab_BlessedHealing_Cooldown = 0;
    }
    
    private void ab_TurnUndead(ABC_character targetChar)
    {
        // Roll to cause fear in the enemy, causing them to skip a turn
        int rollDie = gameEnums.DiceRoll(1, 6, 0);
        ab_TurnUndead_Cooldown = 0;
        if (rollDie > targetChar.mySaveRoll)
        {
            targetChar.isMissingTurn = true;
            Debug.Log(myName = " successfully intimidated " + targetChar.myName);
        }
        else
        {
            Debug.Log(myName = " failed to intimidate " + targetChar.myName);
        }
        return;
    }
    #endregion
    */
}