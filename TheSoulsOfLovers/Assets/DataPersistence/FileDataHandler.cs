using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Playables;
using Newtonsoft.Json;
using UnityEditor;

public class FileDataHandler
{
    private string dirPath = "";
    private string fileName = "";
    private readonly string encCodeWord = "Shein";

    public FileDataHandler(string dirPath, string fileName)
    {
        this.dirPath = dirPath;
        this.fileName = fileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dirPath, fileName);
        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                dataToLoad = EncryptDecrypt(dataToLoad);

                loadedData = GameData.GetFromJsonGameData(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error." + ". dir = " + fullPath + " error - " + e);
            }
        }
        
        return loadedData;
    }


    public void Save(GameData gameData)
    {
        string fullPath = Path.Combine(dirPath, fileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = GameData.GetJsonFromGameData(gameData);

            dataToStore = EncryptDecrypt(dataToStore);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error." + ". dir = " + fullPath + " error - " + e);
        }
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for(int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encCodeWord[i % encCodeWord.Length]);
        }
        return modifiedData;
    }
}
