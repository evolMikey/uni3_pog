using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_Druid : ABC_character
{
    #region Ability Cooldowns and Uses
    int ab_ForestWrath_Cooldown = 2;
    int ab_NaturalOrder_Cooldown = 2;
    int ab_PlainReclamation_Cooldown = 2;
    #endregion

    protected override void Start()
    {
        base.Start();
    }

    public override void actGetAllTargets()
    {
        base.actGetAllTargets();

        // Druid has bonuses against everything, the limiting factor here is the cooldown
        foreach (ABC_character item in myTargets)
        {
            // Will target Dwarves and/or Orcs if the Druid has Forest's Wrath available
            if (((item.myRace == gameEnums.charRaces.orc) || (item.myRace == gameEnums.charRaces.dwarf)) && (ab_ForestWrath_Cooldown == 2))
            {
                myChosenTargets.Add(item);
            }

            // Will target Undead and/or Demons if the Druid has Natural Order available
            else if (((item.myRace == gameEnums.charRaces.undead) || (item.myRace == gameEnums.charRaces.demon)) && (ab_NaturalOrder_Cooldown == 2))
            {
                myChosenTargets.Add(item);
            }

            // Will target Humans and/or Elves if the Druid has Plain's Reclamation available
            else if (((item.myRace == gameEnums.charRaces.human) || (item.myRace == gameEnums.charRaces.elf)) && (ab_PlainReclamation_Cooldown == 2))
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

        // Attempts each race bias first, if they all fail then runs standard attack
        if ((myChosenTargets[0].myRace == gameEnums.charRaces.dwarf) || (myChosenTargets[0].myRace == gameEnums.charRaces.orc))
        {
            if (ab_ForestWrath_Cooldown == 2)
            {
                ab_BiasAttack(myChosenTargets[0], 0);
                return;
            }
        }
        if ((myChosenTargets[0].myRace == gameEnums.charRaces.undead) || (myChosenTargets[0].myRace == gameEnums.charRaces.demon))
        {
            if (ab_NaturalOrder_Cooldown == 2)
            {
                ab_BiasAttack(myChosenTargets[0], 1);
                return;
            }
        }
        if ((myChosenTargets[0].myRace == gameEnums.charRaces.elf) || (myChosenTargets[0].myRace == gameEnums.charRaces.human))
        {
            if (ab_PlainReclamation_Cooldown == 2)
            {
                ab_BiasAttack(myChosenTargets[0], 2);
                return;
            }
        }
        else
        {
            ab_baseAttack(myChosenTargets[0]);
            return;
        }
    }

    // Increases the counter for each cooldown per turn
    public override void turnGetCooldowns()
    {
        base.turnGetCooldowns();
        if (ab_ForestWrath_Cooldown != 2)
            ab_ForestWrath_Cooldown++;
        if (ab_NaturalOrder_Cooldown != 2)
            ab_NaturalOrder_Cooldown++;
        if (ab_PlainReclamation_Cooldown != 2)
            ab_PlainReclamation_Cooldown++;
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

    private void ab_BiasAttack(ABC_character targetChar, int attackType)
    {
        // Bias attack
        int outDmg = gameEnums.DiceRoll(3, 3, 0);
        targetChar.TakeDamage(outDmg);

        // Since all three do the same thing, if statements here are purely aesthetic
        if (attackType == 0)
        {
            Debug.Log(myName + " used Forest's Wrath against " + targetChar.myName + " for " + outDmg);
            ab_ForestWrath_Cooldown = 0;
        }
        if (attackType == 1)
        {
            Debug.Log(myName + " used Natural Order against " + targetChar.myName + " for " + outDmg);
            ab_NaturalOrder_Cooldown = 0;
        }
        if (attackType == 2)
        {
            Debug.Log(myName + " used Plains' Reclamation against " + targetChar.myName + " for " + outDmg);
            ab_PlainReclamation_Cooldown = 0;
        }
        return;
    }
    #endregion
}
