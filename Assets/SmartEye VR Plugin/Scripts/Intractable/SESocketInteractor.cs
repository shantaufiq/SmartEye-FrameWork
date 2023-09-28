using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace SmartEye.Framework
{
    public class SESocketInteractor : XRSocketInteractor
    {
        [Header("Framework Settings")]
        public Transform parentArea;

        [Tooltip("make sure you have set objName on XRGrabIntractableTwoAttach")]
        public string targetTag = string.Empty;

        [System.Obsolete]
        protected override void OnSelectEntered(XRBaseInteractable interactable)
        {
            // Debug.Log($"object entered with name {interactable.name}");

            interactable.transform.SetParent(parentArea);

            base.OnSelectEntered(interactable);
        }

        [System.Obsolete]
        public override bool CanHover(XRBaseInteractable interactable)
        {
            return base.CanHover(interactable) && MatchUsingTag(interactable);
        }

        [System.Obsolete]
        public override bool CanSelect(XRBaseInteractable interactable)
        {
            return base.CanSelect(interactable) && MatchUsingTag(interactable);
        }

        private bool MatchUsingTag(XRBaseInteractable interactable)
        {
            var obj = interactable.GetComponent<XRGrabIntractableTwoAttach>();

            return obj.objName == targetTag;
            // return interactable.CompareTag(targetTag);
        }
    }
}