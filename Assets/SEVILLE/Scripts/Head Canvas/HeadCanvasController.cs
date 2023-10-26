using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
using TMPro;

public class HeadCanvasController : MonoBehaviour
{
    public Transform playerHead;
    public float spawnDistance = 2;
    public float maxDistance = 5f;

    [Header("Quest Components")]
    public InputActionProperty secondaryBtnAction;
    public QuestController questController;
    public float distanceBetweenObjects;

    [Header("Popup Components")]
    public GameObject UI_popupPanel;
    public TextMeshProUGUI UI_message;
    public bool popupState = false;

    private void Update()
    {
        CheckingQuestCanvas();

        if (popupState)
            PopupPos();
    }

    private void CheckingQuestCanvas()
    {
        if (secondaryBtnAction.action.WasPressedThisFrame())
        {
            // Debug.Log($"is {secondaryBtnAction.action.name} klick");

            if (questController.questCanvas.activeSelf == false)
                questController.PrintItems();
            else questController.CloseQuestCanvas();

            questController.questCanvas.transform.position = playerHead.position + new Vector3(playerHead.forward.x, 0, playerHead.forward.z).normalized * spawnDistance;
        }

        if (questController.questCanvas.activeSelf == true)
        {
            questController.questCanvas.transform.LookAt(new Vector3(playerHead.position.x, questController.questCanvas.transform.position.y, playerHead.position.z));
            questController.questCanvas.transform.forward *= -1;
        }

        if (playerHead != null)
        {
            distanceBetweenObjects = Vector3.Distance(playerHead.position, questController.questCanvas.transform.position);

            if (distanceBetweenObjects < maxDistance)
                Debug.DrawLine(playerHead.position, questController.questCanvas.transform.position, Color.green);
        }
        else Debug.LogWarning("HeadCanvas has not been assigned");


        if (distanceBetweenObjects > maxDistance && questController.questCanvas.activeSelf == true)
            questController.questCanvas.SetActive(false);
    }

    public void ShowPopupMessage(string msg)
    {
        UI_popupPanel.SetActive(true);
        UI_message.text = msg;

        if (playerHead != null)
            UI_popupPanel.transform.position = playerHead.position + new Vector3(playerHead.forward.x, 0, playerHead.forward.z).normalized * spawnDistance;
        else Debug.LogWarning("HeadCanvas has not been assigned");

        popupState = true;
        Invoke("DisactivePopup", 5f);
    }

    private void PopupPos()
    {
        UI_popupPanel.transform.LookAt(new Vector3(playerHead.position.x, UI_popupPanel.transform.position.y, playerHead.position.z));
        UI_popupPanel.transform.forward *= -1;
    }

    private void DisactivePopup()
    {
        popupState = false;
        UI_popupPanel.SetActive(false);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (playerHead != null)
        {
            GUI.color = Color.black;
            Handles.Label(transform.position - (playerHead.position -
             questController.questCanvas.transform.position) / 2, distanceBetweenObjects.ToString());
        }
    }
#endif
}


