using System;
using UnityEngine;
using UnityEngine.UI;

namespace MainScene {
    [ExecuteAlways]
    public class GridController : MonoBehaviour {
        private GridLayoutGroup group;
        
        private void Start() {
            group = GetComponent<GridLayoutGroup>();
        
            int count = group.gameObject.transform.childCount;
            Vector2 size = group.cellSize * count + group.spacing * count;
            group.padding = new RectOffset((int)-size.x / 2, 0, 0, 0);
        }
        
        private void Update() {
            int count = group.gameObject.transform.childCount;
            Vector2 size = group.cellSize * count + group.spacing * count;
            group.padding = new RectOffset((int)(-size.x / 2), 0, (int)(-size.y / 2), 0);
        }
    }
}
