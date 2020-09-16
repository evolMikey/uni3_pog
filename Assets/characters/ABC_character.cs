using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ABC_character : MonoBehaviour
{

    public gameEnums.charRaces myRace; // What race this character is
    public gameEnums.charClasses myClass; // What class this character is
    protected List<gameEnums.classAbility> myAbilities = new List<gameEnums.classAbility>(); // A List of all the character's classes
    public teamScript myTeam; // Reference to which team character is on, this also stores data like AI fighting style and the Wizard's ward
    public List<ABC_character> myTargets = new List<ABC_character>(); // List of all targets surrounding this dude
    public List<ABC_character> myChosenTargets = new List<ABC_character>();
    protected SphereCollider myTargetZone = new SphereCollider();
    protected NavMeshAgent myAgent = new NavMeshAgent();
    protected Rigidbody myRB = new Rigidbody();

    public string myName = "character";
    protected int myLevel = 1; // Affects amount of health character has as well as damage output
    public int myMaxHealth = 1; // Maximum health
    public int myCurHealth = 1; // Current health
    public int mySaveRoll = 3; // Used with a random roll to determine success of fear-inducing attacks
    public bool isMissingTurn = false;
    public int wardCounter = 0; // Wizard's ward, disappears when this reaches 0
    public int defenseCounter = 0; // Fighter/Monk/Barbarian defensive feat, disappears when this reaches 0
    public int saveCounter = 0; // Ranger's feat, disappears when it reaches 0
    protected List<ABC_character> summonedChars = new List<ABC_character>();

    // Used to identify location on the board
    public int xLocation = 0;
    public int zLocation = 0;

    protected virtual void Start()
    {
        myCurHealth = 200;

        myRB = gameObject.AddComponent<Rigidbody>();
        myAgent = gameObject.AddComponent<NavMeshAgent>();

        // Sets up the radius around the character that triggers targets
        myTargetZone = gameObject.AddComponent<SphereCollider>();
        myTargetZone.radius = 4;
        myTargetZone.isTrigger = true;

        // Adds this character to the gameManager list
        gameWorld_Manager.Instance.AddNewCharacter(this);
    }

    #region Trigger Zones
    private void OnTriggerEnter(Collider other)
    {
        // Character has entered zone
        if (other.gameObject.GetComponent<ABC_character>() != null)
        {
            // If not on my team, add it to target list
            if (other.gameObject.GetComponent<ABC_character>().myTeam.teamNumber == myTeam.teamNumber)
            {
                // Does nothing if same team
            }
            else
            {
                myTargets.Add(other.GetComponent<ABC_character>());
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ABC_character>() != null)
        {
            if (myTargets.Contains(other.GetComponent<ABC_character>()))
            {
                myTargets.Remove(other.GetComponent<ABC_character>());
            }
        }
    }
    #endregion

    // Fills out some variables, as well as the Abilties list depending on class
    public void InitCharacter(string newName, int newLevel, gameEnums.charClasses myNewClass, gameEnums.charRaces myNewRace, GameObject myNewTeam)
    {
        myAbilities.Add(new gameEnums.classAbility());
        myAbilities.Add(new gameEnums.classAbility());
        myAbilities.Add(new gameEnums.classAbility());
        myAbilities.Add(new gameEnums.classAbility());
        myAbilities.Add(new gameEnums.classAbility());
        myAbilities.Add(new gameEnums.classAbility());
        myAbilities.Add(new gameEnums.classAbility());

        // Basic stats
        myName = newName;
        myLevel = newLevel;
        myMaxHealth = 6 * myLevel;
        myCurHealth = myMaxHealth;

        // Race, Class, Team
        myClass = myNewClass;
        myRace = myNewRace;
        Debug.Log(myTeam);
        myTeam = myNewTeam.GetComponent<teamScript>();
        Debug.Log(myTeam);
        Debug.Log(gameObject);
        myTeam.characterAdd(gameObject);

        // Switch-case based on class
        // Each case sets the character's HP as well as their abilities
        switch (myClass)
        {
            case (gameEnums.charClasses.barbarian):
                {
                    myAbilities[0] = new gameEnums.classAbility("Swing Axe", "The Barbarian swings his weapon at the enemy", 0, 0); // 1d5 damage
                    myAbilities[1] = new gameEnums.classAbility("Enraged", "The Barbarian is filled with rage and hits harder ", 2, 3); // Doubles damage dealt
                    myAbilities[2] = new gameEnums.classAbility("Summon Gang", "The Barbarian calls for aid", 4, 2); // Spawns 3 weak fighter allies of same race
                    myAbilities[3] = new gameEnums.classAbility("Natural Armour", "The Barbarian's rockhard abs protects him from harm", 1, 0); // 1d3 damage next hit
                    myAbilities[4] = new gameEnums.classAbility("Intimidate", "The Barbarian attempts to scare the opponent", 0, 0); // Target rolls will-save or misses next turn
                    break;
                }

            case (gameEnums.charClasses.druid):
                {
                    myAbilities[0] = new gameEnums.classAbility("Swing Stick", "The Druid swings his stick", 0, 0); // 1d4 damage
                    myAbilities[1] = new gameEnums.classAbility("Forest's Wrath", "Druid does 3d3 damage against Orcs and Dwarves ", 2, 0); // 3d3 against certain races
                    myAbilities[2] = new gameEnums.classAbility("Natural Order", "Druid does 3d3 against Undead and Demons ", 2, 0); // 3d3 against certain races
                    myAbilities[3] = new gameEnums.classAbility("Plains' Reclamation", "Druid does 3d3 against Humans and Elves ", 2, 0); // 3d3 against certain races
                    break;
                }

            case (gameEnums.charClasses.fighter):
                {
                    myAbilities[0] = new gameEnums.classAbility("Swing Sword", "The Fighter swings his sword", 0, 0); // 2d3 damage
                    myAbilities[1] = new gameEnums.classAbility("Raised Shield", "The Fighter raises his shield in a defensive stance", 1, 0); // 1d3 on all received attacks next turn
                    myAbilities[2] = new gameEnums.classAbility("Flanking Move", "Enemy has to roll Save or take 2d5 damage", 2, 4); // 2d5 damage against enemies who fail save
                    break;
                }

            case (gameEnums.charClasses.monk):
                {
                    myAbilities[0] = new gameEnums.classAbility("Swing Fists", "The Monk throws a punch", 0, 0); // 1d4 damage
                    myAbilities[1] = new gameEnums.classAbility("Holy Fury", "The Monk does 4d2 damage against Demons", 2, 3); // Extra damage against demons
                    myAbilities[2] = new gameEnums.classAbility("Focused Defense", "The Monk's training allows him to negate damage and fear", 1, 0); // 1d3 damage removed until next turn
                    break;
                }

            case (gameEnums.charClasses.paladin):
                {
                    myAbilities[0] = new gameEnums.classAbility("Swing Mace", "The Paladin swings his mace", 0, 0); // 1d4 damage
                    myAbilities[1] = new gameEnums.classAbility("Smite", "The Paladin does extra damage against Undead/Demons", 2, 0); // 1d6 damage against bias, 1d3 against nonbias
                    myAbilities[2] = new gameEnums.classAbility("Turn Undead", "The Paladin terrifies all Undead, causing them to flee", 1, 0); // Undead have to roll save or miss a turn
                    myAbilities[3] = new gameEnums.classAbility("Blessed Healing", "The Paladin heals the living and harms the unholy", 1, 0); // 1d3 healing or 1d6 damage to Undead/Demon
                    break;
                }

            case (gameEnums.charClasses.ranger):
                {
                    myAbilities[0] = new gameEnums.classAbility("Fire Arrow", "The Ranger fires his bow", 0, 0); // 1d4 damage, range of 4
                    myAbilities[1] = new gameEnums.classAbility("Multi-shot", "The Ranger attacks multiple nearby targets at once", 1, 0); // 1d3 against up to 3 nearby targets
                    myAbilities[2] = new gameEnums.classAbility("Camouflage", "The Ranger increases his save rolls until next turn", 1, 0); // 1d3 added to all saves until next turn
                    break;
                }

            case (gameEnums.charClasses.wizard):
                {
                    myAbilities[0] = new gameEnums.classAbility("Magic Missile", "The Wizard fires a bolt of pure magic", 0, 0); // 1d4 damage
                    myAbilities[1] = new gameEnums.classAbility("Fireball", "The Wizard fires an explosive bolt of flames", 3, 2); // 2d4 damage, spread of 1, damages all
                    myAbilities[2] = new gameEnums.classAbility("Blessed Healing", "The Wizard heals the living and harms the unholy", 1, 0); // 1d3 healing or 1d6 damage to Undead/Demon
                    myAbilities[3] = new gameEnums.classAbility("Turn Undead", "The Wizard terrifies all Undead, causing them to flee", 1, 0); // Undead have to roll save or miss a turn
                    myAbilities[4] = new gameEnums.classAbility("Summon Undead", "The Wizard spawns two weak Undead to fight for him", 3, 2); // Summons 2 weak Undead not of class Wizard, unsummoned on death
                    myAbilities[5] = new gameEnums.classAbility("Magical Ward", "The Wizard creates a Ward that protects his team for 2 turns", 3, 3); // 2d2 damage removed when teammates are hit for two turns
                    myAbilities[6] = new gameEnums.classAbility("Unholy Ritual", "The Wizard summons a powerful demon to his aid", 5, 1); // Summons 1 Demon equal to highest level in enemy team, unsummoned on death
                    break;
                }
        }
    }

    // Called when character has to take damage
    public void TakeDamage(int dmgVal)
    {
        Debug.Log(myName + " is receiving " + dmgVal + " of damage!");
        // Temp storage of the value to be edited
        int modVal = dmgVal;

        // If character has a magical ward, remove 2d2 of damage
        if (wardCounter > 0)
        {
            modVal -= gameEnums.DiceRoll(2, 2, 0);
        }
        // If character has a defensive feat, remove 1d3 of damage
        if (defenseCounter > 0)
        {
            modVal -= gameEnums.DiceRoll(1, 3, 0);
        }
        // Will only trigger damage if it isn't all negated by buffs above
        if (modVal > 0)
        {
            // Remove the new modified damage from health
            myCurHealth -= modVal;

            // If it is a killing blow (i.e. health goes to 0 or negative), remove from team then delete
            if (myCurHealth <= 0)
            {
                KillCharacter();
            }
        }
    }
    // Called when character has to die (either due to summon-breaking or death
    public void KillCharacter()
    {
        Debug.Log(myName + " is dead!");
        // Removes this guy from team then kills
        myTeam.characterRemove(gameObject);
        gameWorld_Manager.Instance.RemoveCharacter(this);
        Destroy(gameObject, 1);
    }


    #region Turn Actions

    // Moves the character visibly on the board as well as mark its new position in code
    public void actMoveCharacterToPosition(int xPos, int zPos)
    {
        xLocation = xPos;
        zLocation = zPos;
        myAgent.SetDestination(new Vector3(xLocation, 0, zLocation));
    }

    public virtual void actGetAllTargets()
    {
        if (myChosenTargets.Count > 0)
        {
            // Clears list from previous round
            myChosenTargets.Clear();
        }

        if (myTargets.Count > 0)
        {
            // Loops through each character for targets of opportunity
            foreach (ABC_character item in myTargets)
            {
                // Vulnerable area always good targets
                if ((item.myCurHealth < 3) && (item.wardCounter == 0))
                {
                    myChosenTargets.Add(item);
                }
            }
            return;
        }
        else
        {
            return;
        }
    }

    public virtual void actChooseAbility()
    {
        /*
         * Will be called before child class special abilities
         * Current targets should be priorities, if none exist then it will attempt to add *all* surrounding enemies
         * If there are still no targets then it will attempt to move towards an enemy not within its target range
        */
        
        if (myChosenTargets.Count == 0)
        {
            if (myTargets.Count > 0)
            {
                // Targets everything nearby
                myChosenTargets = myTargets;
            }
            // Check if count is still zero, move character to new target if so
            else if (myChosenTargets.Count == 0)
            {
                if (xLocation > 0)
                {
                    xLocation -= 1;
                    actMoveCharacterToPosition(xLocation, zLocation);
                }
                else if (xLocation < 0)
                {
                    xLocation++;
                    actMoveCharacterToPosition(xLocation, zLocation);
                }
                else if (zLocation > 0)
                {
                    zLocation -= 1;
                    actMoveCharacterToPosition(xLocation, zLocation);
                }
                else if (zLocation < 0)
                {
                    zLocation++;
                    actMoveCharacterToPosition(xLocation, zLocation);
                }
            }
        }
        else // If there are targets, shuffle targets to randomise who is attacked (out of priorities)
        {
            myChosenTargets = gameWorld_Manager.FisherYatesShuffle(myChosenTargets);
            Debug.Log("Targeting " + myChosenTargets[0]);
            actChooseAbility();
            return;
        }
    }

    // Deals with all the Ability Cooldowns for each class
    public virtual void turnGetCooldowns()
    {
        // Basic counters for each class already existing
        if (wardCounter < 0)
            wardCounter--;
        if (defenseCounter < 0)
            defenseCounter--;
        if (saveCounter < 0)
            saveCounter--;
        isMissingTurn = false;
    }

    #endregion
}