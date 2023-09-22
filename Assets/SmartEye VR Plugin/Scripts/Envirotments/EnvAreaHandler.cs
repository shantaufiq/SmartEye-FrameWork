using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartEye.Framework
{
    public class EnvAreaHandler : MonoBehaviour
    {
        public List<GameObject> areaObjsList;
        public Texture2D areaTexture;
        public bool isRestartOnExitArea = false;

        public void SetActiveObjsState(bool state)
        {
            foreach (var item in areaObjsList)
            {
                item.SetActive(state);
            }
        }
    }
}