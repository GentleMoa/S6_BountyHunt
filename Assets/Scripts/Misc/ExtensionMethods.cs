using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BountyHunt
{
    namespace ExtensionMethods
    {
        public static class ExtensionMethods
        {
            public static void ToggleGameObjectArray(this GameObject[] gameobjects, bool boolean)
            {
                foreach (var item in gameobjects)
                {
                    item.SetActive(boolean);
                }
            }
        }
    }
}
