using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public TextMeshProUGUI textScore;

    private void Update()
    {
        textScore.text = DataManager.Instance.playerScore.ToString();
    }

    public void IncreaseScore(int val)
    {
        DataManager.Instance.playerScore += val;
    }

    public void DecreaseScore(int val)
    {
        DataManager.Instance.playerScore -= val;
    }

    public void ResetScore()
    {
        DataManager.Instance.playerScore = 0;
    }
}
