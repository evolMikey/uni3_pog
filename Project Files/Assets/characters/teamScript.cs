using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teamScript : MonoBehaviour {

    public List<GameObject> charList = new List<GameObject>();
    public Color teamColour;
    public int teamNumber = 0;
    gameEnums.teamStyle teamFormation;

    private void Start()
    {
        gameWorld_Manager.Instance.AddNewTeam(this);

        int xPosTeam = (int)Random.Range(-25, 25);
        int zPosTeam = (int)Random.Range(-25, 25);
        // Sets location for each character around this location
        foreach (GameObject item in charList)
        {
            int xPosChar = xPosTeam + (int)Random.Range(-10, 10);
            int zPosChar = zPosTeam + (int)Random.Range(-10, 10);
            item.GetComponent<ABC_character>().xLocation = xPosChar;
            item.GetComponent<ABC_character>().zLocation = zPosChar;
            item.transform.position = new Vector3(xPosChar, 0, zPosChar);
        }
    }

    public void initTeam(Color newColor, gameEnums.teamStyle newStyle, int newTeamNumber)
    {
        teamColour = newColor;
        teamFormation = newStyle;
        teamNumber = newTeamNumber;
    }

    public void characterRemove(GameObject pChar)
    {
        // Will search through team for killed char, then deletes them
        for (int i = 0; i < charList.Count; i++)
        {
            if (charList[i] == pChar)
            {
                charList.Remove(pChar);
            }
        }

        if (charList.Count == 0)
        {
            gameWorld_Manager.Instance.RemoveTeam(this);
        }
    }
    public void characterAdd(GameObject pChar)
    {
        Debug.Log(pChar);
        // Adds new character to end of team list
        charList.Add(pChar);
    }
}
