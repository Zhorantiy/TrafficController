using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public enum Scene
    {
        MainMenu,
        Level1,
        Level2,
        Level3,
        //Level4,
        //Level5,
        //Loading,
        GameOver,
    }

    public static List<Scene> scenes = new List<Scene>
    {
        Scene.Level1,
        Scene.Level2,
        Scene.Level3,
        //Scene.Level4,
        //Scene.Level5
    };

    /*public void Start()
    {
        Load(Scene.MainMenu);        
    }*/


    public static void Load(Scene scene)
    {
        //SceneManager.LoadScene(Scene.Loading.ToString());
        //Debug.Log("Loading - " + scene.ToString());
        SceneManager.LoadScene(scene.ToString());
    }
}
