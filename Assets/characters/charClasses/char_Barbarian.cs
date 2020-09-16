using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class char_Barbarian : ABC_character
{
    #region Ability Cooldowns and Uses
    int ab_Enraged_Cooldown = 2;
    int ab_Enraged_Uses = 3;
    int ab_SummonGang_Cooldown = 3;
    int ab_SummonGang_Uses = 2;
    int ab_NaturalArmour_Cooldown = 1;
    int ab_Intimidate_Cooldown = 1;
    bool ab_IsEnraged;
    #endregion

    protected override void Start()
    {
        base.Start();
    }

    public override void actGetAllTargets()
    {
        base.actGetAllTargets();

        // Will happily target stronger characters so he can use Intimidate on them
        foreach (ABC_character item in myTargets)
        {
            if(item.myCurHealth > 5)
            {
                myChosenTargets.Add(item);
            }
        }
    }

    public override void actChooseAbility()
    {
        // Base function will cause combat to skip if no enemy is found
        base.actChooseAbility();

        if (myTargets.Count == 0)
        {
            return;
        }
        // If surrounded by enemies, summon gang
        if ((myTargets.Count > 3) && (ab_SummonGang_Cooldown == 3))
        {
            ab_SummonGang();
            return;
        }

        // If character is high health, 1/3 chance to trigger enraged bonus
        if ((myCurHealth >= myMaxHealth - myLevel) && (Random.Range(0, 2) == 2) && (ab_Enraged_Cooldown == 2))
        {
            ab_Enraged();
            return;
        }

        // Attempts to intimidate a healthy opponent
        if ((myChosenTargets[0].myCurHealth >= myMaxHealth * 2) && (ab_Intimidate_Cooldown == 1))
        {
            ab_Intimidate(myChosenTargets[0]);
            return;
        }

        // If low on health, use Natural Armour
        if (myCurHealth < 5)
        {
            ab_NaturalArmour();
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
        if (ab_Enraged_Cooldown != 2)
            ab_Enraged_Cooldown++;
        if (ab_SummonGang_Cooldown != 3)
            ab_SummonGang_Cooldown++;
        if (ab_NaturalArmour_Cooldown != 1)
            ab_NaturalArmour_Cooldown++;
        if (ab_Intimidate_Cooldown != 1)
            ab_Intimidate_Cooldown++;
        if ((ab_IsEnraged)&&(ab_Enraged_Cooldown == 2))
        {
            ab_IsEnraged = false;
        }
    }

    // Functions for each ability the character is capable of
    #region Abilities
    private void ab_baseAttack(ABC_character targetChar)
    {
        // Standard attack
        // Enraged will cause the if statement to trigger, doing double the normal damage
        int outDmg = gameEnums.DiceRoll(1, 5, 0);
        if (ab_IsEnraged)
        {
            targetChar.TakeDamage(outDmg * 2);
            Debug.Log(myName + " attacked " + targetChar.myName + " for " + outDmg);
        }
        else
        {
            targetChar.TakeDamage(outDmg);
            Debug.Log(myName + " attacked " + targetChar.myName + " for " + outDmg);
        }
        return;
    }
    
    private void ab_SummonGang()
    {
        // Summons gang
        for (int i = 0; i < 3; i++)
        {
            // Spawning lifted from the teamGenerator script
            GameObject newCharobj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newCharobj.transform.localScale = new Vector3(.5f, .5f, .5f);
            newCharobj.transform.SetParent(myTeam.transform);
            newCharobj.GetComponent<MeshRenderer>().material.color = myTeam.teamColour;
            newCharobj.name = ("Gang" + i);
            char_Fighter newCharScript = newCharobj.AddComponent<char_Fighter>();
            newCharScript.InitCharacter("Gang" + i, 1, gameEnums.charClasses.fighter, myRace, myTeam.gameObject);
            newCharScript.xLocation = xLocation + (Random.Range(-3, 3));
            newCharScript.zLocation = zLocation + (Random.Range(-3, 3));
            newCharScript.actMoveCharacterToPosition(xLocation, zLocation);
            summonedChars.Add(newCharScript);
        }
        ab_SummonGang_Cooldown = 0;
        ab_SummonGang_Uses--;
        Debug.Log(myName + " summoned allies!");
        return;
    }

    private void ab_Enraged()
    {
        // Switches a boolean to make the player do more damage
        ab_IsEnraged = true;
        Debug.Log(myName + " is enraged!");
        ab_Enraged_Cooldown = 0;
        ab_Enraged_Uses--;
        ab_baseAttack(myChosenTargets[0]);
        return;
    }

    private void ab_Intimidate(ABC_character targetChar)
    {
        int rollDie = gameEnums.DiceRoll(1, 6, 0);
        ab_Intimidate_Cooldown = 0;
        if (rollDie > targetChar.mySaveRoll)
        {
            targetChar.isMissingTurn = true;
            Debug.Log(myName = " successfully intimidated " + targetChar.myName);
        }
        else
        {
            Debug.Log(myName = " failed to intimidate " + targetChar.myName);
        }
        ab_Intimidate_Cooldown = 0;
        return;
    }

    private void ab_NaturalArmour()
    {
        Debug.Log(myName + " is stronger now!");
        ab_NaturalArmour_Cooldown = 0;
        defenseCounter = 2;
    }
    #endregion
}
