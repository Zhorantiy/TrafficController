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
    public List<List<int>> winners_score = new List<List<int>>();

    public int lvl;
    public int rank;

    string filePath;

    public void Start()
    {
        filePath = Application.persistentDataPath + "/save.gamesave";
        //File.Delete(filePath);

        for (int i = 0; i < 3; i++)
        {
            winners_score.Add(new List<int>());
        }

        LoadGame();

        lvl = Game.NumberOfLevel - 1;
        rank = 0;

        if (succes)
        {
            CheckLeaders((int)(Game.time));
            SuccesText("You won!", new Color(0, 255, 0, 255), rank.ToString());
        }
        else
        {
            SuccesText("Game over - You Lost!", new Color(255, 0, 0, 255));
        }
    }

    public void SaveGame(List<List<int>> lb)
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
        winners_score[lvl].Add(score);
        int i = winners_score[lvl].Count - 1;
        //Debug.Log(winners_score[lvl].Count - 1);
        while (i > 0)
        {
            if (winners_score[lvl][i] < winners_score[lvl][i - 1])
            {
                int temp = winners_score[lvl][i];
                winners_score[lvl][i] = winners_score[lvl][i - 1];
                winners_score[lvl][i - 1] = temp;
            } else
            {
                break;
            }
            i--;
        }
        rank = i + 1;
        SaveGame(winners_score);
        UpdateText();
    }

    public void UpdateText()
    {
        for (int i = 0; i < Math.Min(winners_score[lvl].Count, 5); i++)
        {
            winners_text[i].text = "#" + (i + 1) + " = " + winners_score[lvl][i] + " seconds";
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

    public void SuccesText(string text, Color color, string rank = "")
    {
        if (rank != "")
        {
            succes_text.text = text + " Your rank - " + rank;
        } else
        {
            succes_text.text = text + rank;
        }
        
        succes_text.color = color;
    }
}

[System.Serializable]

public class Save
{
    public List<List<int>> leader_board = new List<List<int>>();
}
