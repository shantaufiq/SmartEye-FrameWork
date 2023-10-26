using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Tproject.Manager
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance;
        public List<QuestItem> questItemList;

        [Serializable]
        public struct QuestItem
        {
            public Sprite iconSprite;
            public string title;
            [TextArea]
            public string description;
            public bool isDone;
            public string doneMessage;
            public int score;
        }

        public int playerScore;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        public List<QuestItem> GetQuestData()
        {
            List<QuestItem> temp = new List<QuestItem>();

            foreach (var item in questItemList)
            {
                temp.Add(item);
            }

            return temp;
        }

        public void UpdateQuizItemDone(DataManager.QuestItem _item, int _itemId)
        {
            questItemList[_itemId] = _item;
        }
    }
}