using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public AudioManager audioManager;

    public void Chooselvl(int id)
    {
        StartCoroutine(ChooseLevel(id));
    }

    IEnumerator ChooseLevel(int id)
    {
        audioManager.AudioPlay(); // включает звук
        
        yield return new WaitForSeconds(audioManager.GetLength());

        Game.NumberOfLevel = id;
        //Debug.Log("id: " + id);
        Loader.Load(Loader.scenes[id - 1]); // загрузка сцены
    }
}
