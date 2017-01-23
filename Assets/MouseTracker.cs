using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class MouseTracker : MonoBehaviour {
        public Text XText;              // Set in editor
        public Text YText;              // Set in editor

        public GameObject TileMarker;   // Set in editor

        void Start()
        {
            var spriteRenderer = TileMarker.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerName = "Marker";
        }

        void Update ()
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var x = Mathf.RoundToInt(pos.x);
            var y = Mathf.RoundToInt(pos.y);

            var p = Input.mousePosition;
            if (Map.Instance.GetTileAt(x, y) != null)
            {
                TileMarker.GetComponent<Renderer>().enabled = true;
                TileMarker.transform.position = new Vector3(x, y);
            }
            else
            {
                TileMarker.GetComponent<Renderer>().enabled = false;
            }

            XText.text = "x: " + pos.x + " - " + x;
            YText.text = "y: " + pos.y + " - " + y;
        }
    }
}
