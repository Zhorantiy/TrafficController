using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{   
    public enum acce_state
    {
        Acce,
        Norm,
        Break,
    }

    public acce_state a_s = acce_state.Norm;

    public float car_speed;

    public GameObject game_object;


    public float normal_speed;
    public float acce_speed;
    public float break_speed;

    public AudioManager audioManager;

    void Start()
    {
        //normal_speed = 2f;
        acce_speed = 2f;
        break_speed = 6f;
        car_speed = normal_speed;
    }

    void FixedUpdate()
    {
        if (a_s == acce_state.Acce)
        {
            car_speed += acce_speed * Time.fixedDeltaTime;
            if (car_speed >= normal_speed)
            {
                car_speed = normal_speed;
                a_s = acce_state.Norm;
            }
        }
        if (a_s == acce_state.Break)
        {
            //Debug.Log("Тормоз");
            car_speed -= break_speed * Time.fixedDeltaTime;
            if (car_speed <= 0)
            {
                car_speed = 0;
                a_s = acce_state.Norm;
            }
        }
        game_object.transform.position += game_object.transform.right * car_speed * Time.fixedDeltaTime;
        //car_speed = 0.02f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("въехал" + car_speed);
        a_s = acce_state.Break;
        //car_speed = 0f;
    }

    /*private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Car")
        {
            Debug.Log("я сейчас в зоне");
            a_s = acce_state.Break;
        }
    }*/

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("покинул зону" + car_speed);
        a_s = acce_state.Acce;
        //car_speed = normal_speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        a_s = acce_state.Break;
        StartCoroutine(Crash());

        

    }

    IEnumerator Crash()
    {
        audioManager.AudioPlay(); // включает звук

        yield return new WaitForSeconds(audioManager.GetLength());

        //Debug.Log("Столконвение!" + car_speed);
        
        Game.is_game = false;
        //car_speed = 0;
    }
}
