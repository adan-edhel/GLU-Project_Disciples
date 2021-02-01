using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public static SpawnArea Instance;
    [SerializeField] private Vector3 _Center;
    [SerializeField] private Vector3 _scale;
    [SerializeField] private float _checkRadius;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 RandomPosition
    {
        get
        {
            Vector3 TempVector = new Vector3();
            bool spawnSafe = false;
            while (!spawnSafe)
            {
                float TempX = Random.Range((_Center.x - (_scale.x / 2)), _Center.x + (_scale.x / 2));
                float TempY = Random.Range((_Center.y - (_scale.y / 2)), _Center.y + (_scale.y / 2));
                float TempZ = Random.Range((_Center.z - (_scale.z / 2)), _Center.z + (_scale.z / 2));
                TempVector.Set(TempX, TempY, TempZ);

                if (Physics2D.OverlapCircleAll(TempVector, _checkRadius).Length == 0)
                {
                    spawnSafe = true;
                }
            }
            return TempVector;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_Center, _scale);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_Center, _checkRadius);
    }
}
