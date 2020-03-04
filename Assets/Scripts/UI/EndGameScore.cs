﻿/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 01/10/2019
 * Last Modified: 16/02/2020
 * Modified By: Antoni Gudejko, Dominik Waldowski, Alex Watson
 */
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class EndGameScore : MonoBehaviour
{
    [SerializeField]
    private Player[] players;
    [SerializeField]
    private GameObject[] thePlayerData;
    [SerializeField]
    private List<Player> sortedPlayers = new List<Player>();    //stores a sorted list of players by score
    [SerializeField]
    private Transform[] podiumLocations;
    private int usedPodiums;
    private float total;
    private Loading loading;
    [SerializeField]
    private GameObject[] menuButtons;
    [SerializeField]
    private GameObject[] winningPlayer;
    [SerializeField]
    private GameObject[] winnerIcons;
    [SerializeField]
    private Text mostDashes, mostStuns, mostPowerups;

    private void Start()
    {
        StartCoroutine(WinnerCheck());

        //enables end score menu/rematch buttons
        foreach (GameObject goMB in menuButtons)
        {
            goMB.SetActive(true);
        }
        //enables player score texts
        foreach (GameObject goWP in winningPlayer)
        {
            goWP.SetActive(true);
        }


        loading = GameObject.Find("LoadingManager").GetComponent<Loading>();
        sortedPlayers = players.OrderByDescending(o => o.playerScore).ToList();
        {

            for (int i = 0; i < players.Length; i++)
            {

                Debug.Log($"P{i}: {players[i].scorePercentage}");

            }

            loading = GameObject.Find("LoadingManager").GetComponent<Loading>();
            sortedPlayers = players.OrderByDescending(o => o.scorePercentage).ToList();

            for (int i = 0; i < sortedPlayers.Count; i++)
            {
                sortedPlayers[i].scorePercentage = 0;
                if (i == 0)
                {
                    sortedPlayers[i].hasWon = true;
                }
                else
                {
                    sortedPlayers[i].hasWon = false;
                }
            }
            foreach (Player p in players)
            {

                total += p.playerScore;

            }
            for (int i = 0; i < thePlayerData.Length; i++)
            {
                thePlayerData[i].gameObject.SetActive(false);
            }
            usedPodiums = 0;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].isActivated == true && players[i].isLocked == true)
                {
                    thePlayerData[usedPodiums].GetComponent<PodiumController>().AddPlayer(players[i]);
                    thePlayerData[usedPodiums].transform.position = podiumLocations[usedPodiums].transform.position;
                    thePlayerData[usedPodiums].gameObject.SetActive(true);
                    thePlayerData[usedPodiums].GetComponent<PodiumController>().SetTotal(total);
                    usedPodiums++;
                    if (SoundManager.Instance != null)
                    {
                        SoundManager.Instance.SetBGM("Win");
                    }
                }
            }
        }
        medalAssignment();
    }

    private void medalAssignment()
    {
        if (players.Length >= 3)
        {
            List<Player> medalDash = new List<Player>();
            List<Player> medalStun = new List<Player>();
            List<Player> medalPowerup = new List<Player>();
            medalDash.AddRange(players);

            medalDash = players.OrderByDescending(o => o.DashCount).ToList();
            medalPowerup.AddRange(players);
            medalPowerup.Remove(medalDash[0]);

            medalPowerup = players.OrderByDescending(o => o.PowerUpsCount).ToList();
            medalStun.AddRange(players);

            medalStun = players.OrderByDescending(o => o.StunCount).ToList();
            medalStun.Remove(medalDash[0]);
            medalStun.Remove(medalPowerup[0]);

            mostDashes.text = "Most Dashes: " + medalDash[0].name;
            mostStuns.text = "Most Stuns: " + medalStun[0].name;
            mostPowerups.text = "Most Powerups: " + medalPowerup[0].name;
        }
        else
        {
            List<Player> medalDash = new List<Player>();
            List<Player> medalStun = new List<Player>();
            List<Player> medalPowerup = new List<Player>();
            medalDash.AddRange(players);

            medalDash = players.OrderByDescending(o => o.DashCount).ToList();
            medalPowerup.AddRange(players);
            medalPowerup = players.OrderByDescending(o => o.PowerUpsCount).ToList();
            medalStun.AddRange(players);
            medalStun = players.OrderByDescending(o => o.StunCount).ToList();

            mostDashes.text = "Most Dashes: " + medalDash[0].name;
            mostStuns.text = "Most Stuns: " + medalStun[0].name;
            mostPowerups.text = "Most Powerups: " + medalPowerup[0].name;
        }
    }

    //returns to main menu
    public void MainMenuReturnBtn()
    {
        loading.InitializeLoading();

        //disables player score texts and menu buttons when main menu button is pressed
        foreach (GameObject goMB in menuButtons)
        {
            goMB.SetActive(false);
        }
        foreach (GameObject goWP in winningPlayer)
        {

            goWP.SetActive(false);
            // IB: I believe this code does not belong in here, please confirm?
            //if(players[i].isActivated == true && players[i].isLocked == true)
            //{
            //    thePlayerData[usedPodiums].GetComponent<PodiumController>().AddPlayer(players[i]);
            //    thePlayerData[usedPodiums].transform.position = podiumLocations[usedPodiums].transform.position;
            //    thePlayerData[usedPodiums].gameObject.SetActive(true);
            //    thePlayerData[usedPodiums].GetComponent<PodiumController>().SetTotal(total);
            //    usedPodiums++;
            //    SoundManager.Instance.SetBGMTempo(1);
            //    SoundManager.Instance.SetBGM("Win");
            //}

        }

        foreach (Player p in players)
        {

            p.hasWon = false;

        }

        SceneManager.LoadScene(0);
    }

    //restarts the game
    public void Rematch()
    {
        loading.InitializeLoading();

        //disables player score texts and menu buttons when rematch button is pressed
        foreach (GameObject goMB in menuButtons)
        {
            goMB.SetActive(false);
        }
        foreach (GameObject goWP in winningPlayer)
        {
            goWP.SetActive(false);
        }

        foreach(Player p in sortedPlayers)
        {

            p.hasWon = false;

        }

        //reload characters

        SceneManager.LoadScene(1);
    }

    private void DisplayWinner()
    {
        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            if (sortedPlayers[i].hasWon)
            {
                winnerIcons[i].SetActive(true);
            }
        }
    }

    private IEnumerator WinnerCheck()
    {
        yield return new WaitForSeconds(3f);
        DisplayWinner();

        yield return null;
    }
}