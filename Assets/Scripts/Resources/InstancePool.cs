using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources {
    public class InstancePool : MonoBehaviour, IEnumerable
    {
        private readonly Stack<GameObject> _availableObjects = new Stack<GameObject>();
        
        public void AddToPool(GameObject instance)
        {
            _availableObjects.Push(instance);
            instance.SetActive(false);
        }

        public GameObject GetFromPool() {
            foreach (var instance in _availableObjects) {
                if (!instance.activeSelf) {
                    instance.SetActive(true);
                    return instance;
                }
            }
            return null;
        }

        public IEnumerator GetEnumerator() {
            return _availableObjects.GetEnumerator();
        }
    }
}