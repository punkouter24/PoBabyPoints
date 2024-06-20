using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    private const string HighScoresKey = "HighScores";
    private const int MaxHighScores = 10;

    public List<int> highScores = new List<int>();

    void Start()
    {
        LoadHighScores();
    }

    public void SaveHighScore(int score)
    {
        highScores.Add(score);
        highScores.Sort((a, b) => b.CompareTo(a)); // Sort descending
        if (highScores.Count > MaxHighScores)
        {
            highScores.RemoveAt(highScores.Count - 1); // Remove lowest score if more than max
        }

        string highScoresString = string.Join(",", highScores);
        PlayerPrefs.SetString(HighScoresKey, highScoresString);
        PlayerPrefs.Save();
    }

    public void LoadHighScores()
    {
        highScores.Clear();
        string highScoresString = PlayerPrefs.GetString(HighScoresKey, string.Empty);
        if (!string.IsNullOrEmpty(highScoresString))
        {
            string[] scores = highScoresString.Split(',');
            foreach (string score in scores)
            {
                if (int.TryParse(score, out int parsedScore))
                {
                    highScores.Add(parsedScore);
                }
            }
        }
    }

    public List<int> GetHighScores()
    {
        return new List<int>(highScores);
    }
}
