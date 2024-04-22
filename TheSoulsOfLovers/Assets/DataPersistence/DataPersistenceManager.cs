using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private FileDataHandler fileDataHandler;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceList;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Events Manager in the scene.");
        }
        instance = this;

        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceList = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData(); ;
    }

    public void LoadGame()
    {
        this.gameData = fileDataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No data was found. Processing new game");
            NewGame();
        }

        foreach(IDataPersistence dataPersistence in this.dataPersistenceList)
        {
            dataPersistence.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistence in this.dataPersistenceList)
        {
            dataPersistence.SaveData(ref gameData);
        }

        fileDataHandler.Save(gameData);
    }

    private List<IDataPersistence> FindAllDataPersistanceObjects ()
    {
        IEnumerable<IDataPersistence> dataPersistences = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence> (dataPersistences);
    }
}
