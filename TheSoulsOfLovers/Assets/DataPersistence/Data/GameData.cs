using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    [SerializeField] public General general;
    [SerializeField] public Inventory inventory;
    [SerializeField] public Location[] locations;
    [SerializeField] public int pickedUpItems;

    [JsonIgnore] private static JsonSerializerSettings settings;

    public GameData()
    {
        this.general = new General();
        this.inventory = new Inventory();
        this.locations = InitialLocations();
        this.pickedUpItems = 0;

        settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    }

    [Serializable] public class General
    {
        [SerializeField] public int HP = 5;
        [SerializeField] public string SavePosition = "1-1-1";
        [SerializeField] public string LastSelectedTime = "Present";
    }
    [Serializable] public class Inventory
    {
        [SerializeField] public Item[] items = new Item[7];
    }
    [Serializable] public class Item
    {
        [SerializeField] public int id = 0;
        [SerializeField] public string name = "";
        [SerializeField] public string nameInScene = "";
        [SerializeField] public int count = 0;
    }
    [Serializable] public class Location
    {
        [SerializeField] public List<DefeatedEnemy> DefeatedEnemies = new List<DefeatedEnemy>();
        [SerializeField] public List<PickedUpItem> PickedUpItems = new List<PickedUpItem>();
    }
    public static Location[] InitialLocations()
    {
        return new Location[2]
        {
            new Loc1(), 
            new Loc2()
        };
    }
    [Serializable] public class Loc1:Location
    {
        [SerializeField] [JsonProperty] public bool TheInitialCutsceneAppeared = false;
        [SerializeField] [JsonProperty] public bool TalkedToTheMiner = false;
        [SerializeField] [JsonProperty] public bool TalkedToTheMinerWithPickaxe = false;
        [SerializeField] [JsonProperty] public bool TalkedToTheAlex = false;
        [SerializeField] [JsonProperty] public bool PickedSword = false;
        [SerializeField] [JsonProperty] public bool PickedAmulet = false;
        [SerializeField] [JsonProperty] public bool PickedPickaxe = false;
    }
    [Serializable]
    public class Loc2 : Location
    {
    }
    [Serializable] public class DefeatedEnemy
    {
        [SerializeField] public int location;
        [SerializeField] public int scene;
        [SerializeField] public string name;
        [SerializeField] public string selectedTime;
    }
    [Serializable] public class PickedUpItem
    {
        [SerializeField] public int id;
        [SerializeField] public int locationOfPickingUp;
        [SerializeField] public int sceneOfPickingUp;
        [SerializeField] public int locationOfDropping;
        [SerializeField] public int sceneOfDropping;
        [SerializeField] public string name;
        [SerializeField] public string nameInScene;
        [SerializeField] public string selectedTime;
        [SerializeField] public Vector3 lastPos;
    }
    public static string GetJsonFromGameData(GameData gameData)
    {
        return JsonConvert.SerializeObject(gameData, Formatting.Indented, settings);
    }
    public static GameData GetFromJsonGameData(string json)
    {
        return JsonConvert.DeserializeObject<GameData>(json, settings);
    }
}
