using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class generateTeams : EditorWindow
{

    #region Variables to generate
    private int teamNumber = 0;
    private Color teamColour = new Color(0, 0, 0);
    private gameEnums.teamStyle teamFormation = gameEnums.teamStyle.standard;
    #endregion

    #region Get/Set for Team Size
    List<string> teamUnit_Name = new List<string>();
    List<int> teamUnit_Level = new List<int>();
    List<gameEnums.charRaces> teamUnit_Race = new List<gameEnums.charRaces>();
    List<gameEnums.charClasses> teamUnit_Class = new List<gameEnums.charClasses>();
    int _teamSize = 0;
    public int teamSize
    {
        get { return _teamSize; }
        set
        {
            // If the new value does not equal the original...
            if (value != _teamSize)
            {
                // If the value is 1 or more, sets value and resets array
                if (value >= 1)
                {
                    _teamSize = value;

                    // Clears List, sets its Capacity to new value, then fills with nulls
                    teamUnit_Name.Clear();
                    teamUnit_Level.Clear();
                    teamUnit_Race.Clear();
                    teamUnit_Class.Clear();

                    teamUnit_Name.Capacity = value;
                    teamUnit_Level.Capacity = value;
                    teamUnit_Race.Capacity = value;
                    teamUnit_Class.Capacity = value;

                    for (int i = 0; i < _teamSize; i++)
                    {
                        teamUnit_Name.Add("temp");
                        teamUnit_Level.Add(1);
                        teamUnit_Race.Add(gameEnums.charRaces.human);
                        teamUnit_Class.Add(gameEnums.charClasses.fighter);
                    }
                }
            }
        }
    }
    #endregion

    private string sGenerationMessage = "Create Team";
    private bool bGenerateSuccess = false;

    [MenuItem("Window/Team Creator")]
    public static void ShowWindow()
    {
        GetWindow<generateTeams>("Team Creator");
    }

    private void OnGUI()
    {
        #region Enter Input Fields
        GUILayout.Label("Team Details", EditorStyles.boldLabel);

        teamNumber = int.Parse(EditorGUILayout.TextField("Team Number:", teamNumber.ToString()));
        var tt_number = GUILayoutUtility.GetLastRect();
        GUI.Label(tt_number, new GUIContent("", "Used to Identify which team is being made"));

        teamColour = EditorGUILayout.ColorField("Team Colour:", teamColour);
        var tt_colour = GUILayoutUtility.GetLastRect();
        GUI.Label(tt_colour, new GUIContent("", "Changes colour of the team"));

        teamFormation = (gameEnums.teamStyle)EditorGUILayout.EnumPopup("Team Formation", teamFormation);
        var tt_formation = GUILayoutUtility.GetLastRect();
        GUI.Label(tt_formation, new GUIContent("", "Whether the team rushes into combat, turtles, etc"));

        teamSize = EditorGUILayout.IntField("Team Size:", teamSize);
        var tt_Size = GUILayoutUtility.GetLastRect();
        GUI.Label(tt_Size, new GUIContent("", "Number of members in the team"));

        for (int i = 0; i < teamUnit_Name.Capacity; i++)
        {
            teamUnit_Name[i] = EditorGUILayout.TextField("Character Name:", teamUnit_Name[i]);
            teamUnit_Level[i] = EditorGUILayout.IntField("Character Level:", teamUnit_Level[i]);
            teamUnit_Race[i] = (gameEnums.charRaces)EditorGUILayout.EnumPopup("Character Race:", teamUnit_Race[i]);
            teamUnit_Class[i] = (gameEnums.charClasses)EditorGUILayout.EnumPopup("Character Race:", teamUnit_Class[i]);
        }
        #endregion

        #region Error Checking
        if (GUILayout.Button(sGenerationMessage))
        {
            for (int i = 0; i < teamSize; i++)
            {
                if ((teamUnit_Name[i] == null) || (teamUnit_Name[i] == ""))
                {
                    sGenerationMessage = "Name is null or empty";
                    bGenerateSuccess = false;
                }
            }
            for (int i = 0; i < teamSize; i++)
            {
                if (teamUnit_Level[i] <= 0)
                {
                    sGenerationMessage = "Level is 0 or negative";
                    bGenerateSuccess = false;
                }
            }
            if (teamSize != teamUnit_Name.Count)
            {
                sGenerationMessage = "Name isn't equal to Team Size";
                bGenerateSuccess = false;
            }
            else if (teamSize != teamUnit_Race.Count)
            {
                sGenerationMessage = "Race isn't equal to Team Size";
                bGenerateSuccess = false;
            }
            else if (teamSize != teamUnit_Level.Count)
            {
                sGenerationMessage = "Level isn't equal to Team Size";
                bGenerateSuccess = false;
            }
            else if (teamSize != teamUnit_Class.Count)
            {
                sGenerationMessage = "Class isn't equal to Team Size";
                bGenerateSuccess = false;
            }
            else if (teamSize <= 0)
            {
                sGenerationMessage = "Team Size is 0 or negative";
                bGenerateSuccess = false;
            }
            else if (teamUnit_Class.Contains(gameEnums.charClasses.wizard))
            {
                sGenerationMessage = "Wizard not supported in game!";
                bGenerateSuccess = false;
            }
            else
            {
                sGenerationMessage = "Success!";
                bGenerateSuccess = true;
                GenerateTeam();
            }
        }
        #endregion
    }

    private void GenerateTeam()
    {
        if (!bGenerateSuccess)
        {
            Debug.Log("Error on Team Generation");
        }
        else
        {
            Debug.Log("No errors, enjoy!");

            // Initialises the team object
            GameObject newTeamObj = new GameObject("team" + teamNumber);
            teamScript newScript = newTeamObj.AddComponent<teamScript>();
            newScript.initTeam(teamColour, teamFormation, teamNumber);

            // Initialises each character object
            for (int i = 0; i < teamSize; i++)
            {
                GameObject newCharObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                newCharObj.transform.localScale = new Vector3(.5f, .5f, .5f);
                newCharObj.transform.SetParent(newTeamObj.transform);
                newCharObj.GetComponent<MeshRenderer>().material.color = teamColour;
                newCharObj.name = (teamUnit_Name[i]);

                // Switch-case to determine which script is added
                ABC_character newCharScript;
                switch(teamUnit_Class[i])
                {
                    case gameEnums.charClasses.barbarian:
                        newCharScript = newCharObj.AddComponent<char_Barbarian>();
                        break;

                    case gameEnums.charClasses.druid:
                        newCharScript = newCharObj.AddComponent<char_Druid>();
                        break;

                    case gameEnums.charClasses.fighter:
                        newCharScript = newCharObj.AddComponent<char_Fighter>();
                        break;

                    case gameEnums.charClasses.monk:
                        newCharScript = newCharObj.AddComponent<char_Monk>();
                        break;

                    case gameEnums.charClasses.paladin:
                        newCharScript = newCharObj.AddComponent<char_Paladin>();
                        break;

                    case gameEnums.charClasses.ranger:
                        newCharScript = newCharObj.AddComponent<char_Ranger>();
                        break;

                    case gameEnums.charClasses.wizard:
                        newCharScript = newCharObj.AddComponent<char_Wizard>();
                        break;

                        // Backup default to fighter
                    default:
                        newCharScript = newCharObj.AddComponent<char_Fighter>();
                        break;
                }
                newCharScript.InitCharacter(teamUnit_Name[i], teamUnit_Level[i], teamUnit_Class[i], teamUnit_Race[i], newTeamObj);
            }
        }
    }
}
