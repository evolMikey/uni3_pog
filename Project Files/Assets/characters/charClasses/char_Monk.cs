using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_Monk : ABC_character
{
    #region Ability Cooldowns and Uses
    int ab_HolyFury_Cooldown = 2;
    int ab_HolyFury_Uses = 3;
    int ab_FocusedDefense_Cooldown = 1;
    #endregion

    protected override void Start()
    {
        base.Start();
    }

    public override void actGetAllTargets()
    {
        base.actGetAllTargets();

        foreach (ABC_character item in myTargets)
        {
            // Will target Demons if the Monk is able to use Holy Fury
            if ((item.myRace == gameEnums.charRaces.demon) && (ab_HolyFury_Cooldown == 2))
            {
                myChosenTargets.Add(item);
            }
        }
    }

    public override void actChooseAbility()
    {
        base.actChooseAbility();

        if (myTargets.Count == 0)
        {
            return;
        }

        // Uses Holy Fury if target is a Demon
        if ((myChosenTargets[0].myRace == gameEnums.charRaces.demon) && (ab_HolyFury_Cooldown == 2))
        {
            ab_HolyFury(myChosenTargets[0]);
            return;
        }

        // If low on health, use Focused Defense
        if ((myCurHealth < 5) && (ab_FocusedDefense_Cooldown == 1))
        {
            ab_FocusedDefense();
            return;
        }

        // If target is not a Demon and health is okay
        {
            ab_baseAttack(myChosenTargets[0]);
            return;
        }
    }

    // Increases the counter for each cooldown per turn
    public override void turnGetCooldowns()
    {
        base.turnGetCooldowns();
        if (ab_HolyFury_Cooldown != 2)
            ab_HolyFury_Cooldown++;
        if (ab_FocusedDefense_Cooldown != 1)
            ab_FocusedDefense_Cooldown++;
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

    private void ab_HolyFury(ABC_character targetChar)
    {
        // Holy Fury attack only affects demons
        if (targetChar.myRace == gameEnums.charRaces.demon)
        {
            int outDmg = gameEnums.DiceRoll(4, 2, 0);
            targetChar.TakeDamage(outDmg);
            Debug.Log(myName + " used Holy Fury against " + targetChar.myName + " for " + outDmg);
        }
        ab_HolyFury_Cooldown = 0;
        ab_HolyFury_Uses--;
    }

    private void ab_FocusedDefense()
    {
        Debug.Log(myName + " focused on his defense!");
        ab_FocusedDefense_Cooldown = 0;
        defenseCounter = 2;
    }
    #endregion
}
