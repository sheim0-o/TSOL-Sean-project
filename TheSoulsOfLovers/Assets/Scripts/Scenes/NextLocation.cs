using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using System.IO;

public class NextLocation : MonoBehaviour, IDataPersistence
{
    public enum SideOfMapBorderNearTransition // your custom enumeration
    {
        Left,
        Right,
        Up,
        Bottom
    };
    public SideOfMapBorderNearTransition sideOfMapBorder = SideOfMapBorderNearTransition.Left;
    public string transitionToZone = "1-1-1";

    private string idSavePoint;
    private GameObject player;
    private GameObject playerPrefs;
    private bool saveData = false;
    private DataPersistenceManager dataPersistenceManager;
    private string[] scenesInThisLoc;
    private bool transitionEnabled = true;

    public void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerPrefs = GameObject.FindWithTag("PlayerPrefs");
        dataPersistenceManager = GameObject.FindWithTag("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        idSavePoint = name;
        scenesInThisLoc = playerPrefs.GetComponent<PlayerPrefs>().getListOfScenes()
             .Where(scene => scene.Contains("Loc" + idSavePoint.Split(new char[] { '-' })[0] + "-"))
             .ToArray();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (!transitionEnabled)
                return;
            idSavePoint = transitionToZone;

            int nextLocation = int.Parse(idSavePoint.Split(new char[] { '-' })[0]) - 1;
            if(nextLocation!=playerPrefs.GetComponent<PlayerPrefs>().location)
                scenesInThisLoc = playerPrefs.GetComponent<PlayerPrefs>().getListOfScenes()
                     .Where(scene => scene.Contains("Loc" + (nextLocation + 1) + "-"))
                     .ToArray();

            int nextScene = int.Parse(idSavePoint.Split(new char[] { '-' })[1]) - 1;
            if (nextScene < scenesInThisLoc.Length && nextScene >= 0)
            {
                saveData = true;
                dataPersistenceManager.SaveGame();
                SceneManager.LoadScene(scenesInThisLoc[nextScene]);
            }
            else
            {
                idSavePoint = name;
                Debug.LogError("Error of loading next scene!");
            }
        }
    }

    public void LoadData(GameData gameData)
    {
        if(player==null)
            player = GameObject.FindWithTag("Player");
        if (name == gameData.general.SavePosition)
        {
            float padding = 2;
            Vector3 position = transform.position;
            switch(sideOfMapBorder)
            {
                case SideOfMapBorderNearTransition.Left:
                    position.x = position.x + padding;
                    break;
                case SideOfMapBorderNearTransition.Right:
                    position.x = position.x - padding;
                    break;
                case SideOfMapBorderNearTransition.Up:
                    position.y = position.y - padding;
                    break;
                case SideOfMapBorderNearTransition.Bottom:
                    position.y = position.y + padding;
                    break;
            }
            player.transform.position = position;
        }
    }

    public void SaveData(ref GameData gameData)
    {
        if(saveData)
            gameData.general.SavePosition = this.idSavePoint;
        saveData = false;
    }
}
