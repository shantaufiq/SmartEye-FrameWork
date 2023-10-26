using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Seville.Sandbox
{
    public class PupupMessage : MonoBehaviour
    {
        public TextMeshProUGUI textMessage;

        private void Start()
        {
            Invoke(nameof(DestroyPopup), 2.5f);
        }

        public void DestroyPopup()
        {
            Destroy(this.gameObject);
        }
    }
}