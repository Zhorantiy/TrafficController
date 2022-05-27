using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static int NumberOfLevel;

    public GameObject pref;

    public DataCars D_cars;
    public Level level;

    public List<Car> cars = new List<Car>();

    public static bool is_game = true;
    public static float time;

    public static int score;
    public Text text_score;
    public Text text_time;

    Color red = new Color(255, 0, 0, 100);
    Color green = new Color(0, 255, 0, 100);

    public AudioManager audioManager;

    void Start()
    {
        Debug.Log("Start" + NumberOfLevel);
        is_game = true;
        time = level.timer;
        score = 0;

        StartCoroutine(CreateNewCar());
        StartCoroutine(Timer());
    }

    void Update()
    {
        text_score.text = "Score: " + score + "/" + level.count_cars;
        if (score == level.count_cars)
        {
            EndGame(true); // true - победа
        }
    }

    public void ChangeTrafficColor(int id)
    {
        level.is_green[id] = !level.is_green[id];
        bool IsGreen = level.is_green[id];
        SpriteRenderer[] sprites = level.crossroad[id].GetComponentsInChildren<SpriteRenderer>();
        BoxCollider2D[] col = level.crossroad[id].GetComponentsInChildren<BoxCollider2D>();

        if (IsGreen)
        {
            sprites[0].color = green;
            sprites[2].color = green;
        } else {
            sprites[0].color = red;
            sprites[2].color = red;
        }

        foreach (BoxCollider2D colid in col)
        {
            colid.enabled = !IsGreen;
        }

        StartCoroutine(MakeSound(id));
        Debug.Log("change");
    }

    public void EndGame(bool succes)
    {
        is_game = false;
        Debug.Log("Game Over!");
        time = level.timer - time;
        GameOver.succes = succes;
        Loader.Load(Loader.Scene.GameOver);
        
    }

    IEnumerator Timer()
    {
        while (time >= 1 && is_game)
        {
            text_time.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time -= 1f;
            
        }
        EndGame(false);
        
    }

    IEnumerator CreateNewCar()
    {
        for (int i = 0; i < level.count_cars; i++)
        {
            int start_number = Random.Range(0, level.start_pos.Count);

            Vector3 start_position = level.start_pos[start_number];
            Vector3 start_rotate = level.start_rotate[start_number];
            //Vector3 start_position = level.start_pos[1];
            //Vector3 start_rotate = level.start_rotate[1];

            GameObject go_car = Instantiate(pref, start_position, Quaternion.identity) as GameObject;
            go_car.transform.Rotate(start_rotate);

            Car new_car = go_car.GetComponent<Car>();
            new_car.game_object = go_car;

            int NumberOfCar = Random.Range(0, 4);

            new_car.game_object.GetComponent<SpriteRenderer>().sprite = D_cars.car[NumberOfCar];
            new_car.normal_speed = level.car_norm_speed[start_number];
            cars.Add(new_car);

            float time = Random.Range(1f, 2f);

            yield return new WaitForSeconds(time);
        }
    }

    IEnumerator MakeSound(int id)
    {
        audioManager.AudioPlay(); // включает звук

        yield return new WaitForSeconds(audioManager.GetLength());

        Debug.Log("caru play");
    }
}

[System.Serializable]

public class Level
{
    public List<GameObject> crossroad;
    public List<bool> is_green;
    public List<Vector3> start_pos;
    public List<Vector3> start_rotate;
    public int count_cars;
    public float timer;
    public List<float> car_norm_speed;
}