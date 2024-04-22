using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public bool ifFinalCutscene = false;
    public DataPersistenceManager dataPersistenceManager;

    IEnumerator Load(GameObject �utscene)
    {
        float dur = (float)�utscene.GetComponent<PlayableDirector>().duration;
        �utscene.SetActive(true);
        yield return new WaitForSeconds(dur);
        �utscene.SetActive(false);
    }
    IEnumerator LoadFinal(float duration)
    {
        yield return new WaitForSeconds(duration);
        MainMenu mainMenu = new MainMenu();
        dataPersistenceManager.NewGame();
        dataPersistenceManager.SaveGame();
        mainMenu.BackToMainMenu();
    }
    public void OnEnable()
    {
        PlayableDirector playableDirector = GetComponent<PlayableDirector>();

        if(playableDirector && playableDirector.playOnAwake && ifFinalCutscene)
        {
            StartCoroutine(LoadFinal((float)playableDirector.duration));
        }
    }
}
