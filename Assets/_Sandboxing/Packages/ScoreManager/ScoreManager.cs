using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score;
    public TextMeshProUGUI textScore;

    private void Update()
    {
        textScore.text = score.ToString();
    }

    public void IncreaseScore(int val)
    {
        score += val;
    }

    public void DecreaseScore(int val)
    {
        score -= val;
    }

    public void ResetScore()
    {
        score = 0;
    }
}
