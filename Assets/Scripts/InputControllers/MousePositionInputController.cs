using UnityEngine;
using UnityEngine.EventSystems;
using World;

namespace InputControllers
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
                var x = Mathf.RoundToInt(pos.x);
                var y = Mathf.RoundToInt(pos.y);

                if (Map.Instance.GetTileAt(x, y) != null)
                {
                    TileMarker.GetComponent<Renderer>().enabled = true;
                    TileMarker.transform.position = new Vector3(x, y);
                }
                else
                {
                    TileMarker.GetComponent<Renderer>().enabled = false;
                }
            }
            else
            {
                TileMarker.SetActive(false);
            }
        }
    }
}