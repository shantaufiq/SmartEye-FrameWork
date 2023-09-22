using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartEye.Framework
{
    public class EnvNavigation : MonoBehaviour
    {
        public void OnClickChangeEnvirontment(int index)
        {
            EnvironmentManager.Instance.ChangeAreaByIndex(index);
        }
    }
}