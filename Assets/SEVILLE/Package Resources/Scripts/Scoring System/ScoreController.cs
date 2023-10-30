using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Seville
{
    public class ScoreController : MonoBehaviour
    {
        public DataManager dataManager;
        public TextMeshProUGUI textScore;

        private void Update()
        {
            if (dataManager)
                textScore.text = dataManager.playerScore.ToString();
        }

        public void IncreaseScore(int val)
        {
            dataManager.IncreaseScore(val);
        }

        public void DecreaseScore(int val)
        {
            dataManager.playerScore -= val;
        }

        public void ResetScore()
        {
            dataManager.playerScore = 0;
        }
    }
}