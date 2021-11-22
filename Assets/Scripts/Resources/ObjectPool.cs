using System.Collections.Generic;
using UnityEngine;

namespace Resources {
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int poolGrowSize;

        private readonly Stack<GameObject> _availableObjects = new Stack<GameObject>();

        private void Awake()
        {
            GrowPool();
        }

        private void GrowPool()
        {
            for (int i = 0; i < poolGrowSize; i++)
            {
                GameObject instanceToAdd = Instantiate(prefab, transform, true);
                AddToPool(instanceToAdd);
            }
        }

        public void AddToPool(GameObject instance)
        {
            _availableObjects.Push(instance);
            instance.SetActive(false);
        }

        public GameObject GetFromPool()
        {
            if(_availableObjects.Count == 0)
                GrowPool();


            GameObject inst = _availableObjects.Pop();
            inst.SetActive(true);
            return inst;
        }
    }
}