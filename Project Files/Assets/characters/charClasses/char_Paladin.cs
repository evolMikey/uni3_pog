using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_Paladin : ABC_character
{
    #region Ability Cooldowns and Uses
    int ab_Smite_Cooldown = 2;
    int ab_TurnUndead_Cooldown = 1;
    int ab_BlessedHealing_Cooldown = 1;
    #endregion

    protected override void Start()
    {
        base.Start();
    }

    public override void actGetAllTargets()
    {
        // Allows base-character to get some targets first, then go to class priorities
        base.actGetAllTargets();

        foreach (ABC_character item in myTargets)
        {
            // Undead are preferred targets if Paladin can use Smite
            if ((item.myRace == gameEnums.charRaces.undead) && (ab_Smite_Cooldown == 2))
            {
                myChosenTargets.Add(item);
            }

            // Undead are preferred targets if Paladin can use Heal
            else if ((item.myRace == gameEnums.charRaces.undead) && (ab_BlessedHealing_Cooldown == 1))
            {
                myChosenTargets.Add(item);
            }

            // Demons are preferred targets if Paladin can use Smite
            else if ((item.myRace == gameEnums.charRaces.demon) && (ab_Smite_Cooldown == 2))
            {
                myChosenTargets.Add(item);
            }

            // Demons are preferred targets if Paladin can use Heal
            else if ((item.myRace == gameEnums.charRaces.demon) && (ab_BlessedHealing_Cooldown == 1))
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

        // Targeting for the Smite and Healing abilities
        if ((myChosenTargets[0].myRace == gameEnums.charRaces.undead) || (myChosenTargets[0].myRace == gameEnums.charRaces.demon))
        {
            if (ab_Smite_Cooldown == 2)
            {
                ab_Smite(myChosenTargets[0]);
            }
            else if ((ab_BlessedHealing_Cooldown == 1) && (Random.Range(0, 4) == 4)) // Added a random factor here because of how quick the cooldown is
            {
                ab_BlessedHeal(myChosenTargets[0]);
            }
            return;
        }
        
        // If target is Undead and random factor is met, use Turn Undead
        if ((myChosenTargets[0].myRace == gameEnums.charRaces.undead) && (ab_TurnUndead_Cooldown == 1) && (Random.Range(0, 3) == 4))
        {
            ab_TurnUndead(myChosenTargets[0]);
        }
    }

    // Increases the counter for each cooldown per turn
    public override void turnGetCooldowns()
    {
        base.turnGetCooldowns();
        if (ab_Smite_Cooldown != 2)
            ab_Smite_Cooldown++;
        if (ab_TurnUndead_Cooldown != 1)
            ab_TurnUndead_Cooldown++;
        if (ab_BlessedHealing_Cooldown != 1)
            ab_BlessedHealing_Cooldown++;
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
        return;
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
        ab_TurnUndead_Cooldown = 0;
        return;
    }

    private void ab_Smite(ABC_character targetChar)
    {
        // Does extra damage against bias, less damage against nonbias
        if ((targetChar.myRace == gameEnums.charRaces.undead) || (targetChar.myRace == gameEnums.charRaces.demon))
        {
            // Smite damage
            int outDmg = gameEnums.DiceRoll(1, 6, 0);
            targetChar.TakeDamage(outDmg);
            Debug.Log(myName + " smited " + targetChar.myName + " for " + outDmg);
        }
        else
        {
            // Smite fail damage
            int outDmg = gameEnums.DiceRoll(1, 3, 0);
            targetChar.TakeDamage(outDmg);
            Debug.Log(myName + " smited " + targetChar.myName + " for " + outDmg);
        }
        ab_Smite_Cooldown = 0;
        return;
    }
    #endregion
}
