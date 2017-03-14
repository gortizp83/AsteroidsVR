using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.ShootingGallery;
using TargetType = VRStandardAssets.ShootingGallery.ShootingTarget.TargetType;

namespace VRStandardAssets.Utils
{
    // This is a simple object pooling script that
    // allows for random variation in prefabs.
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_Prefabs;            // These are prefabs which are all variations of the same (for example various asteroids).
        [SerializeField] private int m_NumberInPool;                // The number of prefabs to be initially instanced for the pool.


        private Dictionary<TargetType, List<GameObject>> m_Pool = new Dictionary<TargetType, List<GameObject>>();  // The list of instantiated prefabs making up the pool.
        private Dictionary<TargetType, GameObject> m_PrefabDictionary = new Dictionary<TargetType, GameObject>();

        private void Awake ()
        {
            // Add as many random variations to the pool as initially determined.
            for (int i = 0; i < m_Prefabs.Length; i++)
            {
                var prefab = m_Prefabs[i];
                var type = prefab.GetComponent<ShootingTarget>().Type;
                m_PrefabDictionary.Add(prefab.GetComponent<ShootingTarget>().Type, prefab);
                for (int j = 0; j < m_NumberInPool; j++)
                {
                    AddToPool(type);
                }
            }
        }


        private void AddToPool (TargetType targetType)
        {
            // Instantiate the prefab.
            GameObject instance = Instantiate(m_PrefabDictionary[targetType]);

            // Make the instance a child of this pool and turn it off.
            instance.transform.parent = transform;
            instance.SetActive (false);

            // Add the instance to the pool for later use.
            List<GameObject> gameObjectList;
            if (m_Pool.TryGetValue(instance.GetComponent<ShootingTarget>().Type, out gameObjectList))
            {
                gameObjectList.Add(instance);
            }
            else
            {
                m_Pool.Add(instance.GetComponent<ShootingTarget>().Type, new List<GameObject> { instance });
            }
        }


        public GameObject GetGameObjectFromPool (TargetType targetType)
        {
            List<GameObject> gameObjects;
            m_Pool.TryGetValue(targetType, out gameObjects);

            // If there aren't any instances left in the pool, add one.
            if (gameObjects.Count == 0)
                AddToPool (targetType);
            
            // Get a reference to the first gameobject in the pool.
            GameObject ret = gameObjects[0];

            // Remove that gameobject from the pool list.
            gameObjects.RemoveAt(0);

            // Activate the instance.
            ret.SetActive (true);

            // Put it in the root of the hierarchy.
            ret.transform.parent = null;

            // Return the unpooled instance.
            return ret;
        }


        public void ReturnGameObjectToPool (GameObject go)
        {
            // Add the gameobject to the pool list.
            List<GameObject> gameObjects;
            if (m_Pool.TryGetValue(go.GetComponent<ShootingTarget>().Type, out gameObjects))
            {
                gameObjects.Add(go);
            }

            // Deactivate the gameobject and make it a child of the pool.
            go.SetActive (false);
            go.transform.parent = transform;
        }
    }
}