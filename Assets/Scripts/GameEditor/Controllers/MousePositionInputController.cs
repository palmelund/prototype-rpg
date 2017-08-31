using UnityEngine;
using UnityEngine.EventSystems;
using World;

namespace GameEditor.Controllers
{
    public class MousePositionInputController : MonoBehaviour {
        public GameObject TileMarker;   // Set in editor

        private void Start()
        {
            var spriteRenderer = TileMarker.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerName = "Marker";
        }

        private void Update ()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                TileMarker.SetActive(true);
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                TileMarker.transform.position = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
            }
            else
            {
                TileMarker.SetActive(false);
            }
        }
    }
}