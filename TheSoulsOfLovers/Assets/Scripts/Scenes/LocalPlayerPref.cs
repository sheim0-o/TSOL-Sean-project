using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LocalPlayerPref : MonoBehaviour
{
    /*
    public DataPersistenceManager dataPersistenceManager;
    public CutsceneManager cutsceneManager;

    private GameObject player;
    private bool isNextScene;
    private int idOfScene; 
    private Vector3 nextPlayerPos;
    [HideInInspector] public bool playStartCutscene;

    public void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (playStartCutscene)
                cutsceneManager.LoadCutscene(0);
            playStartCutscene = false;
        }
    }

    public void SaveToNextScene(int idOfScene, Vector3 nextPlayerPos, GameObject player)
    {
        this.idOfScene = idOfScene;
        this.nextPlayerPos = nextPlayerPos;
        this.player = player;

        isNextScene = true;
        dataPersistenceManager.SaveGame();
        isNextScene = false;
    }

    public void LoadData(GameData gameData)
    {
        playStartCutscene = gameData.playStartCutscene;
        GameObject[] list = GameObject.FindGameObjectsWithTag("PlayerUI");
        if (list.Length > 1)
        {
            player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = gameData.playerPos;
            }

            GameObject tempPlayerUi = list[0];
            GameObject playerUi = list[1];
            Destroy(playerUi);
            foreach (Transform child in tempPlayerUi.transform)
            {
                if (child.CompareTag("HealthBar"))
                {
                    child.GetComponent<HealthBar>().SetHealth(player.GetComponent<Player>().health);
                    player.GetComponent<Player>().healthBar = child.GetComponent<HealthBar>();
                }
                else if (child.childCount>0)
                {
                    if(child.GetChild(0).CompareTag("Inventory"))
                        player.GetComponent<Player>().inventory = child.GetChild(0).GetComponent<InventorySystem>();
                }
            }

            dataPersistenceManager.SaveGame();
        }
    }

    public void SaveData(ref GameData gameData)
    {

        if (isNextScene)
        {
            gameData.idLocation = idOfScene;
            gameData.playerPos = nextPlayerPos;
            gameData.isJustNextScene = true;
            gameData.playerHealth = player.GetComponent<Player>().health;
        }
        else
        {
            gameData.idLocation = SceneManager.GetActiveScene().buildIndex;
            //gameData.playerPos = player.transform.position;
            gameData.isJustNextScene = false;
        }
        if (this.playStartCutscene == null)
            return;
        gameData.playStartCutscene = this.playStartCutscene;
    }
    */
}
