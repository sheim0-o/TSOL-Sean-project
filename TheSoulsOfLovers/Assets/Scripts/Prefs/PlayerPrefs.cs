using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPrefs : MonoBehaviour, IDataPersistence
{
    public string currentTime = "Present";
    public GameObject sceneInPresent;
    public GameObject sceneInPast;
    [HideInInspector] public List<BoxCollider2D> presentObjectsCollider;
    [HideInInspector] public List<BoxCollider2D> pastObjectsCollider;
    [HideInInspector] public GameData.Location[] locations;
    [HideInInspector] public int location;
    [HideInInspector] public int scene;
    [HideInInspector] public int pickedUpItems;

    [HideInInspector] public Transform folderWithItemsInPresent;
    [HideInInspector] public Transform folderWithItemsInPast;
    [HideInInspector] public Transform folderWithMobsInPresent;
    [HideInInspector] public Transform folderWithMobsInPast;

    private void Start()
    {
        if (sceneInPast == null || sceneInPresent == null)
        {
            Debug.LogError("Set Past And Present in PlayerPrefs!");
            return;
        }
        if (locations.Length == 0)
            locations = GameData.InitialLocations();

        /*
        // Folders with Mobs And Items
        Transform folderWithMAIInPresent = takeOrCreateChildrenObject(sceneInPresent.transform, "MAI");
        Transform folderWithMAIInPast = takeOrCreateChildrenObject(sceneInPast.transform, "MAI");

        folderWithItemsInPresent = takeOrCreateChildrenObject(folderWithMAIInPresent.transform, "Items");
        folderWithItemsInPast = takeOrCreateChildrenObject(folderWithMAIInPast.transform, "Items");

        folderWithMobsInPresent = takeOrCreateChildrenObject(folderWithMAIInPresent, "Mobs");
        folderWithMobsInPast = takeOrCreateChildrenObject(folderWithMAIInPast, "Mobs");
        */

        if (location == 0 && scene == 0)
            checkCurrentLocAndScene();

        // Get colliders of objects in past and present
        List<Transform> pastObjects = GetAllChildsWithCollider(sceneInPast.transform);
        List<Transform> presentObjects = GetAllChildsWithCollider(sceneInPresent.transform);
        foreach(Transform obj in pastObjects)
            if (obj.GetComponents<BoxCollider2D>().Length > 0)
                pastObjectsCollider.AddRange(obj.GetComponents<BoxCollider2D>());

        foreach (Transform obj in presentObjects)
            if (obj.GetComponents<BoxCollider2D>().Length > 0)
                presentObjectsCollider.AddRange(obj.GetComponents<BoxCollider2D>());
    }

    public void activateAmulet()
    {
        if (sceneInPresent && sceneInPast)
        {
            switch (currentTime)
            {
                case "Present":
                    currentTime = "Past";
                    sceneInPast.SetActive(true);
                    sceneInPresent.SetActive(false);
                    break;
                case "Past":
                    currentTime = "Present";
                    sceneInPast.SetActive(false);
                    sceneInPresent.SetActive(true);
                    break;
            }
        }
    }

    public void EnableDisableBackScene(bool enable)
    {
        if (sceneInPresent && sceneInPast)
        {
            GameObject sceneToChange = null;
            switch (currentTime)
            {
                case "Present":
                    sceneToChange = sceneInPast;
                    break;
                case "Past":
                    sceneToChange = sceneInPresent;
                    break;
            }
            if (enable)
            {
                sceneToChange.transform.localScale = new Vector3(0, 0, 0);
                sceneToChange.SetActive(true);
            }
            else
            {
                sceneToChange.SetActive(false);
                sceneToChange.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    List<Transform> GetAllChildsWithCollider(Transform parentT)
    {
        List<Transform> listOfChildrenT = new List<Transform>();
        foreach (Transform childrenT in parentT)
        {
            if (childrenT.name == "MAI")
                continue;
            if (childrenT.GetComponent<Collider2D>() != null)
                listOfChildrenT.Add(childrenT);
            if (childrenT.childCount > 0)
                listOfChildrenT.AddRange(GetAllChildsWithCollider(childrenT));
        }
        return listOfChildrenT;
    }

    public void LoadData(GameData gameData)
    {
        if (gameData.general.LastSelectedTime != currentTime)
            activateAmulet();
        if (gameData.locations.Length != 0)
            locations = gameData.locations;
        pickedUpItems = gameData.pickedUpItems;

        if (location == 0 && scene == 0)
            checkCurrentLocAndScene();

        if (sceneInPast == null || sceneInPresent == null)
        {
            Debug.LogError("Set Past And Present in PlayerPrefs!");
            return;
        }
        // Folders with Mobs And Items
        Transform folderWithMAIInPresent = takeOrCreateChildrenObject(sceneInPresent.transform, "MAI");
        Transform folderWithMAIInPast = takeOrCreateChildrenObject(sceneInPast.transform, "MAI");

        folderWithItemsInPresent = takeOrCreateChildrenObject(folderWithMAIInPresent.transform, "Items");
        folderWithItemsInPast = takeOrCreateChildrenObject(folderWithMAIInPast.transform, "Items");

        folderWithMobsInPresent = takeOrCreateChildrenObject(folderWithMAIInPresent, "Mobs");
        folderWithMobsInPast = takeOrCreateChildrenObject(folderWithMAIInPast, "Mobs");


        // Instantiate mobs in scene
        foreach (GameData.DefeatedEnemy defeatedEnemy in locations[location].DefeatedEnemies)
        {
            if (defeatedEnemy.location != location || defeatedEnemy.scene != scene)
                continue;

            // Take/create parent folder for item
            Transform parentForMob = null;
            switch (defeatedEnemy.selectedTime)
            {
                case "Present":
                    parentForMob = folderWithMobsInPresent;
                    break;
                case "Past":
                    parentForMob = folderWithMobsInPast;
                    break;
            }

            Transform oldMob = parentForMob.Find(defeatedEnemy.name);
            if (oldMob != null)
                DestroyImmediate(oldMob.gameObject);
        }

        // Instantiate items in scene
        foreach (GameData.PickedUpItem pickedUpItem in locations[location].PickedUpItems)
        {
            // Take parent folder
            Transform parentForItem = null;
            switch (pickedUpItem.selectedTime)
            {
                case "Present":
                    parentForItem = folderWithItemsInPresent;
                    break;
                case "Past":
                    parentForItem = folderWithItemsInPast;
                    break;
            }
            if (pickedUpItem.locationOfPickingUp == location && pickedUpItem.sceneOfPickingUp == scene)
            {
                // Destroy old item if exists
                Transform oldItem = parentForItem.Find(pickedUpItem.nameInScene);
                if (oldItem != null)
                    DestroyImmediate(oldItem.gameObject);
            }

            if (pickedUpItem.lastPos == new Vector3() || (pickedUpItem.locationOfDropping != location || pickedUpItem.sceneOfDropping != scene))
                continue;
            // Create item
            GameObject itemPrefab = Resources.Load<GameObject>("ItemsObjects/" + pickedUpItem.name);
            if (itemPrefab == null)
                continue;
            GameObject newItem = Instantiate(itemPrefab, pickedUpItem.lastPos, Quaternion.identity, parentForItem);
            newItem.name = pickedUpItem.nameInScene;
            if (newItem.GetComponent<ItemObject>())
            {
                newItem.GetComponent<ItemObject>().item.id = pickedUpItem.id;
                newItem.GetComponent<ItemObject>().item.nameInScene = pickedUpItem.nameInScene;
            }
        }
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.general.LastSelectedTime = this.currentTime;
        gameData.locations = locations;
        gameData.pickedUpItems = pickedUpItems;
    }



    private Transform takeOrCreateChildrenObject(Transform parent, string nameOfChildren)
    {
        Transform children = null;
        if (parent.Find(nameOfChildren))
            children = parent.Find(nameOfChildren);
        else
        {
            children = new GameObject() { name = nameOfChildren }.transform;
            children.parent = parent;
        }
        return children;
    }

    private void checkCurrentLocAndScene()
    {
        location = int.Parse(SceneManager.GetActiveScene().name.Split(new char[] { '-' }).First().Replace("Loc", "")) - 1;
        string[] scenesInThisLoc;
        scenesInThisLoc = getListOfScenes()
                     .Where(scene => scene.Contains("Loc" + (location + 1) + "-"))
                     .ToArray();
        for (int i = 0; i < scenesInThisLoc.Length; i++)
        {
            if (scenesInThisLoc[i].Equals(SceneManager.GetActiveScene().name))
            {
                scene = i;
                break;
            }
        }
    }

    public List<string> getListOfScenes()
    {
        var sceneNumber = SceneManager.sceneCountInBuildSettings;
        string[] arrayOfNames;
        arrayOfNames = new string[sceneNumber];
        for (int i = 0; i < sceneNumber; i++)
        {
            arrayOfNames[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
        }
        return arrayOfNames.ToList();
    }
}
