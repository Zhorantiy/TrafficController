using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameOver : MonoBehaviour
{
    public AudioManager audioManager;

    public static bool succes;
    public Text succes_text;

    public Text[] winners_text = new Text[5];
    public static int[,] winners_score = new int[5, 5];

    public int lvl;

    string filePath;

    public void Start()
    {
        filePath = Application.persistentDataPath + "/save.gamesave";

        LoadGame();

        lvl = Game.NumberOfLevel;
        if (succes)
        {
            SuccesText("Game over - You win!", new Color(0, 255, 0, 255));
            CheckLeaders((int) (Game.time));
        }
        else
        {
            SuccesText("Game over - You Lose!", new Color(255, 0, 0, 255));
            UpdateText();
        }
    }

    public void SaveGame(int[,] lb)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Create);

        Save save = new Save();

        save.leader_board = lb;

        bf.Serialize(fs, save);

        fs.Close();
    }

    public void LoadGame()
    {
        if (!File.Exists(filePath))
            return;


        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Open);

        Save save = (Save)bf.Deserialize(fs);
        fs.Close();

        winners_score = save.leader_board;
    }

    private void CheckLeaders(int score)
    {
        for (int i_score = 0; i_score < 5; i_score++)
        {
            if (winners_score[i_score, lvl] == 0)
            {
                winners_score[i_score, lvl] = score;
                break;
            }
            if (score < winners_score[i_score, lvl])
            {
                for (int j = 4; j > i_score; j--)
                {
                    winners_score[j, lvl] = winners_score[j - 1, lvl];
                }
                winners_score[i_score, lvl] = score;
                break;
            }
        }
        SaveGame(winners_score);
        UpdateText();
    }

    public void UpdateText()
    {
        for (int i = 0; i < 5; i++)
        {
            if (winners_score[i, lvl] == 0)
            {
                winners_text[i].text = "#" + (i + 1) + " -";
            }
            else
            {
                winners_text[i].text = "#" + (i + 1) + " = " + winners_score[i, lvl];
            }
            
        }
    }

    public void Restart()
    {
        audioManager.AudioPlay();

        Debug.Log(Game.NumberOfLevel);
        Loader.Load(Loader.scenes[Game.NumberOfLevel - 1]);
        /*switch (Game.NumberOfLevel)
        {
            case  1:
                Loader.Load(Loader.Scene.Level1);
                break;
            case 2:
                Loader.Load(Loader.Scene.Level2);
                break;
            case 3:
                Loader.Load(Loader.Scene.Level3);
                break;
            case 4:
                Loader.Load(Loader.Scene.Level4);
                break;
            case 5:
                Loader.Load(Loader.Scene.Level5);
                break;
            default:
                Loader.Load(Loader.Scene.MainMenu);
                break;
        }*/
    }

    public void ReturnToMenu()
    {
        audioManager.AudioPlay();

        Loader.Load(Loader.Scene.MainMenu);
    }

    public void SuccesText(string text, Color color)
    {
        succes_text.text = text;
        succes_text.color = color;
    }
}

[System.Serializable]

public class Save
{
    public int[,] leader_board = new int[5, 5];
}
