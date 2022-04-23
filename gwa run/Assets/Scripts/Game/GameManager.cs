using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class GameManager : MonoBehaviour {
    #region Variables

    public static GameManager Instance;
    public bool isOnMainMenu = false;
    public UIManager uiManager;
    [Header("Player")]
    public GameObject playerObject;
    PlayerMovement playerMovement;
    public GameObject playerGFX;
    public ParticleSystem explosionParticles;
    
    
    [Header("Obstacles")]
    public List<GameObject> obstaclePrefabs = new List<GameObject>();

    public Transform obstaclesObject;
    public float obstacleDistance = 5f;
    float initObstacleDistance = 0f;
    public int obstaclesPerGeneration = 20;
    Vector3 initObstaclePos;
    bool hasSpawnedObstacle;
    GameObject lastObstacle;
    List<GameObject> randomObsts = new List<GameObject>();
    public int passedObsts = 0;

    // public ObjectPool objPool = ObjectPool.Instance;

    [Header("Sections")]
    public List<GameObject> sectionPrefabs = new List<GameObject>();
    public List<Material> sectionBGs = new List<Material>();
    public Transform sectionsObject;
    Vector3 lastSectionPos;
    int sectionsSpawned;
    int lastBG;
    
    [Header("Game State")]
    public bool gameEnded = false;
    public bool paused = false;
    public float restartDelay = 2f;
    
    [Header("Saving")]
    [HideInInspector]
    public PlayerData data;
    [HideInInspector]
    public static string savePath = "gwarun";

    float timeToSave;
    [SerializeField] bool resetSave; 
    
    float autosaveTime = 15f;
    public int score;
    public int scoreAmt = 100;
    public int coins;

    static readonly Random getRandom = new Random();

    #endregion
    
    #region Main Functions

    void Awake() {
        if (Instance == null) Instance = this;

        initObstacleDistance = obstacleDistance;

        if (!isOnMainMenu) {
            playerMovement = playerObject.GetComponent<PlayerMovement>();
            initObstaclePos = new Vector3(-2f, 0.5f, 25f);
            lastSectionPos = new Vector3(-6.83f, 2.5f, 0f);

            uiManager.UpdateRetryScreen(false);
        }
    }

    public void LoadData(string path) {
        try { // bruh
            data = SaveSystem.LoadData<PlayerData>(savePath);
        }
        catch (Exception e) {
            try {
                data = SaveSystem.LoadData<PlayerData>(savePath, false);
            }
            catch (Exception e1) {
                data = new PlayerData(); 
                SaveSystem.SaveData(data, savePath);
            }
        }
    }
    
    void Start() {
        // var data = new PlayerData();
        if (SaveSystem.SaveExists(savePath) && !resetSave) {
            LoadData(savePath);
        }
        // // if (data != new PlayerData()) {
        //     Debug.Log($"[{ string.Join(", ", data.unlockedSkins) }]");
        //     Debug.Log($"equipped: {data.equippedSkin} ({gwaSkins.List.Find(s => s.id == data.equippedSkin).skinName})");
        // // }

        if (!isOnMainMenu) {
            gwaSkin equippedSkin = gwaSkins.List.Find(s => s.id == data.equippedSkin);
            Debug.Log(equippedSkin.skinName);
            playerGFX.GetComponent<MeshFilter>().mesh = equippedSkin.mesh;
            playerGFX.GetComponent<MeshRenderer>().materials = equippedSkin.mats;//hope this works
            
            SpawnSection(0);

            lastSectionPos = new Vector3(
                lastSectionPos.x,
                lastSectionPos.y,
                lastSectionPos.z + 250
            );
            SpawnRandomSection();

            // populate randomObsts array this is slower but i can't figure out the other way
            // (which is,
            //  * get total weights
            //  * get random int (1 - total weights, inc.)
            //  * define current sum
            //  * loop through sorted list of obstacles
            //  * currentSum += obst.weight
            //  * if randint < currentSum: obstacle = findKey(obstPfs, obst)
            //  * spawn(obstacle)
            // )
            
            foreach (GameObject obst in obstaclePrefabs) {
                int weight = obst.GetComponent<ObstacleGroup>().weight; 
                // Debug.Log($"{weight}, {obst.name}");
                for (int j = 0; j < weight; j++) {
                    randomObsts.Add(obst);
                }
            }

            // Debug.Log(obstaclePrefabs.Count);
            
            GenerateObstacles();
        }
    }
    

    public void RestartGame() {
        Debug.Log("reloading scene");
        Time.timeScale = 1f;
        // Invoke(nameof(ReloadCurrentScene), restartDelay);
        ReloadCurrentScene();
    }
    
    void ReloadCurrentScene() {
        TMP_Text text = uiManager.retryButton.transform.GetComponentInChildren<TMP_Text>();
        text.text = "reloading level...";
        StartCoroutine(nameof(ReloadSceneAsync));
    }

    IEnumerator ReloadSceneAsync() {
        SaveSystem.SaveData(data, savePath);
        AsyncOperation loading = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

        while (!loading.isDone) {
            yield return null;
        }
        
        Debug.Log("reloading scene done!");
    }
    
    void Update() {
        if (isOnMainMenu) return;
        if (gameEnded) return;
        
        timeToSave += Time.deltaTime * (1 / (Time.timeScale == 0 ? 1 : Time.timeScale));
        if (timeToSave >= autosaveTime) {
            SaveSystem.SaveData(data, savePath);
            timeToSave = 0;
        }

        if (passedObsts >= 20) obstacleDistance = initObstacleDistance + (Time.time/ 60);
    }

    public void EndGame() {
        // explosion!
        ParticleSystem particles = Instantiate(explosionParticles, playerGFX.transform);
        particles.Play();
        
        // hide player
        playerGFX.GetComponent<MeshRenderer>().enabled = false;
        playerMovement.movementIsEnabled = false;

        // show retry screen
        Invoke(nameof(ShowRetryScreen), particles.main.duration / particles.main.simulationSpeed);
    }

    void ShowRetryScreen() {
        uiManager.ShowRetryScreen();
    }

    void OnApplicationQuit() {
        LoadData(savePath);
        PlayerData loadedData = data;
        if (data.coins == 0 && loadedData.coins < data.coins) {
            data.coins = loadedData.coins;
        }
        SaveSystem.SaveData(data, savePath);
    }
    
    #endregion

    #region Obstacles
    public void GenerateObstacles() {
        for (int i = 0; i < obstaclesPerGeneration; i++) SpawnRandomObstacle();
    }

    int lastRandomObstacle = 0;
    public void SpawnRandomObstacle() {
        int obstacle = lastRandomObstacle;

        while (obstacle == lastRandomObstacle) {
            int randInt = getRandom.Next(0, randomObsts.Count);
            obstacle = obstaclePrefabs.FindIndex(obst => obst == randomObsts[randInt]);
        }

        lastRandomObstacle = obstacle;

        SpawnObstacle(obstacle);
    }
    public void SpawnObstacle(int obstacle) {
        if (!hasSpawnedObstacle) lastObstacle = SpawnObstacle(0, initObstaclePos);
        hasSpawnedObstacle = true;
        Vector3 lastObstaclePos = lastObstacle.transform.position;
        
        if (obstaclePrefabs[obstacle].name == "Coins") {
            double randomDouble = getRandom.NextDouble(); // 0 - 1 noninclusive (exclusive?)
            randomDouble *= 3;
            randomDouble -= 1.5;
            float randomX = (float) randomDouble; // now -1.5 - 1.5 noninclusive
            lastObstaclePos = new Vector3(
                randomX,
                lastObstaclePos.y,
                lastObstaclePos.z + obstacleDistance
            );
            
            GameObject obstacleGO = SpawnObstacle(obstacle, lastObstaclePos);
            lastObstacle = obstacleGO;
        }
        else {
            lastObstaclePos = new Vector3(
                // lastObstaclePos.x,
                -2f,
                lastObstaclePos.y,
                lastObstaclePos.z + obstacleDistance
            );
            
            GameObject obstacleGO = SpawnObstacle(obstacle, lastObstaclePos);
            lastObstacle = obstacleGO;
        }
    }
    
    public GameObject SpawnObstacle(int obstacle, Vector3 spawnPos) {
        GameObject newObstacle = Instantiate(
            obstaclePrefabs[obstacle], 
            spawnPos, 
            Quaternion.identity
        );
        
        newObstacle.transform.parent = obstaclesObject;
        hasSpawnedObstacle = true;
        return newObstacle;
    }
    #endregion

    #region Sections

    public void SpawnSection(int section) {
        GameObject sectionGO = SpawnSection(section, lastSectionPos);
        
        lastSectionPos = new Vector3(
            lastSectionPos.x, 
            lastSectionPos.y,
            lastSectionPos.z + ((sectionsSpawned == 2) ? 11.09922f : 0f)
        );
    }
    
    public GameObject SpawnSection(int section, Vector3 spawnPos) {
        GameObject newSection = Instantiate(
            sectionPrefabs[section], 
            spawnPos, 
            Quaternion.identity
        );
        
        newSection.transform.parent = sectionsObject;
        sectionsSpawned++;
        return newSection;
    }

    int lastRandomSection = 0;

    public void SpawnRandomSection() {
        int section = lastRandomSection;
        while (section == 0 || section == lastRandomSection) { 
            section = getRandom.Next(0, sectionPrefabs.Count);
        }
        lastRandomSection = section;
        SpawnSection(section);
    }

    #endregion

}
