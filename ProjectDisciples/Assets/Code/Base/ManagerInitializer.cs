using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInitializer : MonoBehaviour
{
    // Name and location of the Manager prefab
    static string ManagerPath = "Base/Managers";

    /// <summary>
    /// Loads the necessary prefabs before the scene is loaded
    /// </summary>
    [RuntimeInitializeOnLoadMethod()]
    public static void Initialize()
    {
        Instantiate(Resources.Load(ManagerPath));
    }
}
