using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace Seville
{
    public class CanvasRayChecker : TrackedDeviceGraphicRaycaster
    {

        [Header("SmartEye Framework")]
        public bool isTrigger = false;
        [HideInInspector] public bool isPlayerHoverCanvas;

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            base.Raycast(eventData, resultAppendList);

            CheckerRayCast(eventData);

            // Debug.Log(eventData.pointerEnter.name);
        }

        private void CheckerRayCast(PointerEventData eventData)
        {
            if (!isTrigger) return;

            // if (eventData.IsPointerMoving() && eventData.hovered.Find((x) => x.gameObject.CompareTag("VideoPlayerCanvas")))
            // {
            //     // Debug.Log($"{eventData.hovered[0].name}");
            //     isPlayerHoverCanvas = true;
            // }

            if (eventData.pointerEnter)
            {
                isPlayerHoverCanvas = true;
            }

            else isPlayerHoverCanvas = false;
        }
    }
}