using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class MouseTracker : MonoBehaviour {
        // Update is called once per frame
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
            int x = Mathf.RoundToInt(pos.x);
            int y = Mathf.RoundToInt(pos.y);

            var p = Input.mousePosition;
            if (map.Instance.GetTileAt(x, y) != null)
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
