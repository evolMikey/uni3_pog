using UnityEngine;

public class gameEnums {

    public enum gameDifficulty
    {
        beginner,
        easy,
        medium,
        hard,
        impossible
    };

    public enum charRaces
    {
        // Certain abilities behave differently for different races
        // Race also dictates which classes are available
        human, // Druid, Fighter,Monk, Paladin, Ranger
        dwarf, // Barbarian, Fighter, Monk, Paladin
        elf, // Druid, Monk, Ranger, Wizard
        orc, // Barbarian, Druid, Fighter, Wizard
        undead, // Barbarian, Fighter, Ranger, Wizard
        demon // Barbarian, Fighter, Paladin
    };

    public enum charClasses
    {
        // Determines which abilities are available to the character
        barbarian,
        druid,
        fighter,
        monk,
        paladin,
        ranger,
        wizard
    };

    public struct classAbility
    {
        public string sName; // Name of ability
        public string sDescription; // Description that appears on tooltip
        public int iMaxTurnCooldown; // Number of turns (after current) it takes to reuse
        public int iCurTurnCooldown; // Number of turns (after current) it takes to reuse
        public int iUses; // Number of times this can be used in combat before exhausted

        public classAbility(string newName, string newDesc, int newmaxCooldown, int newUses)
        {
            sName = newName;
            sDescription = newDesc;
            iMaxTurnCooldown = newmaxCooldown;
            iCurTurnCooldown = newmaxCooldown;
            iUses = newUses;
        }
    }

    public enum teamStyle
    {
        standard, // Melee characters charge, archers/wizards stay at range behind them
        turtle, // Characters rarely move, archers/wizards less likely to flee
        rush // Characters charge forward, including archers/wizards
    };

    // Used to calculate dice rolls easier
    // 2d4+1 means 2 dice with 4 sides each, plus 1 to end result
    public static int DiceRoll(int diceAmount, int diceSides, int modifier)
    {
        // We add to this number each time
        int endVal = 0;

        // For each dice, we roll its number of sides and add it to the total value
        for (int i = 0; i < diceAmount; i++)
        {
            endVal += Random.Range(1, diceSides);
        }

        // Adds modifier
        endVal += modifier;
        return endVal;
    }
}
