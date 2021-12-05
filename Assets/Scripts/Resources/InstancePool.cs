using System.Collections.Generic;
using UnityEngine;

namespace Resources {
    public class InstancePool : MonoBehaviour
    {
        private readonly Stack<GameObject> _availableObjects = new Stack<GameObject>();
        
        
        public void AddToPool(GameObject instance)
        {
            _availableObjects.Push(instance);
            instance.SetActive(false);
        }

        public GameObject GetFromPool() {
            var inst = _availableObjects.Pop();
            inst.SetActive(true);
            return inst;
        }
    }
}