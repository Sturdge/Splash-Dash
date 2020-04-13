﻿/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 29/09/2019
 * Last Modified: 15/10/2019
 * Modified By: Antoni Gudejko, Dominik Waldowski
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ManageGame : MonoBehaviour
{
    private DeliveryPointsManager deliveryManager;
    [SerializeField]
    private Material[] coloursForPlatforms;
    private Loading loading;
    public delegate void GameWin();
    public static event GameWin OnGameWin;
    public static ManageGame instance;                            //instance of game manager
    [SerializeField]
    private Transform[] playerSpawnPositions;                   //stores all spawn positions for players
    [SerializeField]
    private Player[] players;                                   //stores all player data
    [Header("Player model")]
    [SerializeField]
    private GameObject emptyPlayer;
    [Header("Clock")]
    [SerializeField]
    private TextMesh timeRemaining;                                 //text that displays remaining time
    [SerializeField]
    private float reverseTime = 0;                             //actual timer 
    [SerializeField]
    private List<GameObject> mapEdgesForGodPower = new List<GameObject>();
    private bool isTimingDown;
    [SerializeField]
    private GameObject godPowerUp;
    private List<GameObject> playerObjects = new List<GameObject>();
    private LevelManager layoutManager;
    public GridManager gridManager { get; private set; }
    public DrawColor drawColor { get; private set; }
    [SerializeField]
    private ObjectPooling objectPooling;
    [SerializeField]
    private Transform clockHand, pointA, pointB;
    [SerializeField]
    private float journeyLength;
    public bool IsTimingDown { get => isTimingDown; set => isTimingDown = value; }
    public Transform[] PlayerSpawnPositions { get => playerSpawnPositions; set => playerSpawnPositions = value; }
    public Player[] Players { get => players; set => players = value; }
    public List<GameObject> PlayerObjects { get => playerObjects; set => playerObjects = value; }
    public List<GameObject> MapEdgesForGodPower { get => mapEdgesForGodPower; set => mapEdgesForGodPower = value; }
    public GameObject GodPowerUp { get => godPowerUp; set => godPowerUp = value; }
    public SpecialButton SpecialButton { get; private set; }
    private float timeLimit = 60;
    public CameraShaker camShake { get; private set; }
    public List<PlayerController> allPlayerControllers { get; private set; }
    [SerializeField]
    private int scoreLevelID;

    //Creates instance of game manager
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }

        clockHand.eulerAngles = v3Rot;

        
        camShake = Camera.main.GetComponent<CameraShaker>();
        allPlayerControllers = new List<PlayerController>();
        SpecialButton = GameObject.FindGameObjectWithTag("Special").GetComponent<SpecialButton>();
    }
    //handles display and counting of round timer
    [SerializeField]
    Vector3 v3Rot;
    [SerializeField]
    Vector3 v3Dest;
    [SerializeField]
    float speed = 3.8f; //2.5f

    private void Update()
    {
        if (isTimingDown == true)
        {
            v3Rot = Vector3.MoveTowards(v3Rot, v3Dest, speed * Time.deltaTime);
            v3Rot.y = 90;
            v3Rot.z = 0;
            //Debug.Log(reverseTime);
            reverseTime += Time.deltaTime;
            clockHand.transform.eulerAngles = v3Rot;
          //  var lookDir = pointB.position - clockHand.position;
           // lookDir.y = 90; 
          //  lookDir.z = 0;
            //clockHand.rotation = Quaternion.LookRotation(lookDir);
           // Quaternion rot = Quaternion.LookRotation(lookDir);
           // clockHand.rotation = Quaternion.Slerp(clockHand.rotation, rot, 1 * Time.deltaTime);
            //  string minutes = ((int)reverseTime / 60).ToString("00");
            //string seconds = ((int)reverseTime % 60).ToString("00");
            // timeRemaining.text = string.Format("{00:00}:{01:00}", minutes, seconds);
            if (reverseTime >= timeLimit)
            {
                reverseTime = timeLimit;
                if(OnGameWin != null)
                    OnGameWin();
                // loading.SetID(2);
                // loading.InitializeLoading();
                gridManager.CalculateFinalScore();
                SceneManager.LoadScene(scoreLevelID);
            }
            if (reverseTime % gridManager.TimeToCheck < 1 && reverseTime > 1) //Modulus operator to check if the value of reverseTime goes into TimeToCheck with a remainder that is less than 1, i.e. 60.23416 % 30 = 0.23416, 70.81674 % 30 = 10.81674 etc. -James
                gridManager.UpdateUI();
            if (reverseTime >= SpecialButton.ActivationTime && !SpecialButton.IsActive && !SpecialButton.HasBeenUsed)
                SpecialButton.ActivateButton();
            if (reverseTime % 1 < 1)
                SpecialButton.UpdateVisuals(reverseTime);
        }
    }


    private void Start()
    {
        loading = GameObject.Find("LoadingManager").GetComponent<Loading>();
        layoutManager = GetComponent<LevelManager>();
        gridManager = GetComponent<GridManager>();
        drawColor = GetComponent<DrawColor>();
        layoutManager.LayoutGeneration();
        for (int i = 0; i < layoutManager.SpawnPoints.Length; i++)
        {
            PlayerSpawnPositions[i] = layoutManager.SpawnPoints[i].transform;
        }
        ///terrain
        //drawColor._Terrain = layoutManager.PaintableObjects;

        godPowerUp.SetActive(false);
        //grabs time from main menu scene and checks if its in bounds if its not it sets it to 60 (temporary measure for when we test it straight from game scene)
        // reverseTime = PlayerPrefs.GetFloat("RoundDuration");
        //  if(reverseTime < 29 || reverseTime > 91)
        //  {
        //    reverseTime = 60;
        //  }
        reverseTime = 0;
        isTimingDown = false;
        // timeRemaining.gameObject.SetActive(false);
        PlacePlayers();

        PopulatePlayerControllerList();

        SpecialButton.Initialisation();

        gridManager.Initialisation(PlayerObjects.Count);
        gridManager.PopulateGridList();
    }

    private void PlacePlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isActivated == true)
            {
                if (players[i].isLocked == true)
                {
                    GameObject newPlayer = Instantiate(emptyPlayer) as GameObject;
                    newPlayer.name = "Player: " + (i + 1);
                    players[i].hasWon = false;
                    newPlayer.SetActive(false);
                    newPlayer.transform.position = playerSpawnPositions[i].transform.position;
                    newPlayer.transform.rotation = PlayerSpawnPositions[i].transform.rotation;
                    newPlayer.SetActive(true);
                    newPlayer.GetComponent<PlayerBase>().SetSkin(players[i].skinId);
                    players[i].playerScore = 0;
                    players[i].DashCount = 0;
                    players[i].StunCount = 0;
                    players[i].PowerUpsCount = 0;
                    players[i].InvulnerabilityCount = 0;
                    newPlayer.GetComponent<PlayerBase>().SetExpressionManager(players[i].skinId);
                    newPlayer.GetComponent<PlayerController>().Player = players[i];
                    PlayerBase playerBase = newPlayer.GetComponent<PlayerBase>();
                    playerBase.Player = players[i];
                    players[i].Speed = players[i].DefaultSpeed;
                    playerObjects.Add(newPlayer);
                }
            }
        }
    }

    private void PopulatePlayerControllerList()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < temp.Length; i++)
        {
            allPlayerControllers.Add(temp[i].GetComponent<PlayerController>());
        }
    }
    //updates scores of the player
    /*public void UpdateScore(int playerNum, int scoreReceived)
    {
        players[playerNum - 1].playerScore += scoreReceived;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isActivated == true)
            {
                if (players[i].isLocked == true)
                {
                    playerBoards[i].Find("RightSide").Find("ScoreTxt").GetComponent<Text>().text = "Score: " + players[i].playerScore.ToString("0");
                }
            }
        }
    }*/

    //updates respawn timers of the player
    /*public void UpdateRespawnTimer(int playerNum, int respawnTime)
    {
        if(respawnTime > 0)
        {
            playerBoards[playerNum - 1].Find("Foreground").Find("RespawnTime").gameObject.SetActive(true);
        }
        playerBoards[playerNum - 1].Find("Foreground").Find("RespawnTime").GetComponent<Text>().text = "Respawn: " + respawnTime;
        if(respawnTime <= 0)
        {
            playerBoards[playerNum - 1].Find("Foreground").Find("RespawnTime").gameObject.SetActive(false);
        }
    }*/

    //Activates round timer
    public void StartTimer()
    {
      //  timeRemaining.gameObject.SetActive(true);
        isTimingDown = true;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayGameTheme();
        }
    }
}
