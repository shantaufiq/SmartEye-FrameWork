using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Seville
{
    public class TriggerArea : MonoBehaviour
    {
        public UnityEvent OnPlayerEnter;
        public UnityEvent OnPlayerStay;
        public UnityEvent OnPlayerExit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerEnter?.Invoke();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerStay?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerExit?.Invoke();
            }
        }
    }
}