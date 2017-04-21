using Code.World;
using UnityEngine;
using UnityEngine.UI;

namespace Code.DeveloperTools
{
    public class DebugInfoScreen : MonoBehaviour
    {
        private bool _active = false;
        public GameObject Canvas; // Set in editor

        public Text XText;                  // Set in editor
        public Text YText;                  // Set in editor
        public Text WorldsizeText;          // Set in editor
        public Text ScreenResolutionText;   // Set in editor

        // Use this for initialization
        void Start()
        {
            Canvas.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Canvas.SetActive(!_active);

                _active = !_active;
            }
            if (_active)
            {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var x = Mathf.RoundToInt(pos.x);
                var y = Mathf.RoundToInt(pos.y);

                XText.text = "x: " + pos.x + " - " + x;
                YText.text = "y: " + pos.y + " - " + y;

                ScreenResolutionText.text = "Screen res: " + Screen.width + " x " + Screen.height;

                WorldsizeText.text = "World size: " + Map.Instance.Width + " x " + Map.Instance.Height;
            }
        }
    }
}
