using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class QuestController : MonoBehaviour
{
    public List<ItemList> toDoList;
    public bool isPlayOnStart;
    public QuestItem itemPrefabs;
    public Transform itemListParent;
    public GameObject questPanel;
    public PupupMessage popupMessage;
    public Transform canvas;

    [Serializable]
    public struct ItemList
    {
        public Sprite iconSprite;
        public string title;
        [TextArea]
        public string description;
        public bool isDone;
        public string doneMessage;
        public UnityEvent AfterFinishedFunction;
    }

    private void Start()
    {
        if (isPlayOnStart)
            PrintItems();
    }

    public void FinishItem(int index)
    {
        if (index > toDoList.Count - 1)
        {
            Debug.LogWarning($"number {index} is out of todolist count");
            return;
        }

        if (!toDoList[index].isDone)
        {
            ItemList temp = toDoList[index];
            temp.isDone = true;
            toDoList[index] = temp;

            temp.AfterFinishedFunction?.Invoke();
            ShowMessage(toDoList[index].doneMessage);
        }
        // PrintItems();
    }

    public void PrintItems()
    {
        StartCoroutine(PrintQuestItem());
    }

    IEnumerator PrintQuestItem()
    {
        questPanel.SetActive(true);

        for (int child = 0; child < itemListParent.childCount; child++)
        {
            Destroy(itemListParent.transform.GetChild(child).gameObject);
        }

        yield return new WaitUntil(() => itemListParent.childCount == 0);

        int i = 0;
        while (i < toDoList.Count + 1)
        {
            if (i == toDoList.Count)
            {
                // bug can't scroll in awake
                var rowTemp = Instantiate(itemPrefabs, itemListParent);
                // rowTemp.textUI.text = " ";
                StartCoroutine(DestroyTemp(rowTemp.gameObject));
            }
            else
            {
                var itemGO = Instantiate(itemPrefabs, itemListParent);
                itemGO.SetValueItem(toDoList[i].iconSprite, toDoList[i].title, toDoList[i].description, toDoList[i].isDone);
            }

            yield return new WaitForSeconds(0.01f);
            i++;
        }
    }

    IEnumerator DestroyTemp(GameObject go)
    {
        yield return new WaitUntil(() => go != null);
        Destroy(go);
        // Debug.Log($"destroy {go}");
    }

    public void ShowMessage(string msg)
    {
        var popup = Instantiate(popupMessage, canvas);
        popup.textMessage.text = msg;
    }
}
