using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class gameWorld_Manager : MonoBehaviour {

    #region Instance setting
    private static gameWorld_Manager _instance;
    public static gameWorld_Manager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    #region List referencing all characters and teams
    // List that stores all the characters in the game currently
    public List<ABC_character> allCombatants = new List<ABC_character>();
    public List<teamScript> allTeams = new List<teamScript>();

    public void AddNewCharacter(ABC_character newChar)
    {
        // Called when a character is generated
        allCombatants.Add(newChar);
    }
    public void RemoveCharacter(ABC_character deadChar)
    {
        // Called when a character is killed
        if (allCombatants.Contains(deadChar))
            allCombatants.Remove(deadChar);
    }

    public void AddNewTeam(teamScript newTeam)
    {
        // Called when a team is created
        allTeams.Add(newTeam);
    }
    public void RemoveTeam(teamScript deadTeam)
    {
        // Called when a team is empty
        if (allTeams.Contains(deadTeam))
            allTeams.Remove(deadTeam);
        // If there is only one team remaining, declare it victorious
        if (allTeams.Count == 1)
            DeclareVictory();

    }
    #endregion

    private bool bIsGameWon = false;
    private int curCharacter = 0;

    public void DeclareVictory()
    {
        bIsGameWon = true;
        // Would fill with a better victory screen but this should be good enough for prototyping purposes
        Debug.Log("Victory for Team " + allTeams[0].name);
    }

    #region Shuffle Characters
    public static List<ABC_character> FisherYatesShuffle (List<ABC_character>aList)
    {
        System.Random _random = new System.Random();

        ABC_character myChar;

        int n = aList.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + (int)(_random.NextDouble() * (n - i));
            myChar = aList[r];
            aList[r] = aList[i];
            aList[i] = myChar;
        }

        return aList;
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        StartCoroutine(delayedStart());
    }

    private IEnumerator delayedStart()
    {
        // Runs the shuffler a few times, it doesn't seem to do a good job on one run
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.4f);
            allCombatants = FisherYatesShuffle(allCombatants);
        }
        Debug.Log("List shuffled");

        StartCoroutine(gameTurnLoop());
    }

    private IEnumerator gameTurnLoop()
    {
        // Wait two seconds just for everything to settle
        yield return new WaitForSeconds(2.5f);
        // Game Loop, runs through each character and does actions
        do
        {
            // If statement checks if character is allowed a turn
            if (!allCombatants[curCharacter].isMissingTurn)
            {
                // Will loop through the various 
//                Debug.Log(allCombatants[curCharacter].myName + " is having their turn!");
                // The two main functions within characters that determines who they target and how they attack
                allCombatants[curCharacter].actGetAllTargets();
                allCombatants[curCharacter].actChooseAbility();
            }
            else // Failing the if statement means missing turn
            {
                Debug.Log(allCombatants[curCharacter].myName + " missed their turn!");
                allCombatants[curCharacter].turnGetCooldowns();
                allCombatants[curCharacter].isMissingTurn = false;
            }
            
            yield return new WaitForSeconds(.5f);
            // Gets next character
            curCharacter++;
            if (curCharacter >= allCombatants.Count)
            {
                curCharacter = 0;
            }
        }
        while (!bIsGameWon);
    }
}
