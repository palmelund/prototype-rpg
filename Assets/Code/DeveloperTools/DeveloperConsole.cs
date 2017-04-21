using UnityEngine;
using UnityEngine.UI;

namespace Code.DeveloperTools
{
    public class DeveloperConsole : MonoBehaviour
    {

        public RectTransform DeveloperConsolePanel; // Set in editor
        public InputField DeveloperConsoleInputField;   // Set in editor
        public Text DeveloperConsoleText; // Set in editor
        public ScrollRect DeveloperConsoleScrollRect; // Set in editor

        private bool _visible;

        // Use this for initialization
        void Start()
        {
            DeveloperConsolePanel.gameObject.SetActive(false);
            DeveloperConsoleInputField.onEndEdit.AddListener(t =>
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    var s = t.Split(' ');
                    if (s[0].Equals("camera"))
                    {
                        int x, y;
                        var bx = int.TryParse(s[1], out x);
                        var by = int.TryParse(s[2], out y);
                        if (bx == false || by == false)
                        {
                            t = "Error: Cannot parse camera position!";
                        }
                        else
                        {
                            Camera.main.transform.position = new Vector3(x, y, -10);
                        }
                    }

                    DeveloperConsoleText.text += t + "\n";
                    DeveloperConsoleScrollRect.verticalScrollbar.value = 0f;

                    /*
                    DeveloperConsoleInputField.text = string.Empty;
                    DeveloperConsoleText.text += "\n" + t;
                    var rt = DeveloperConsoleText.GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + 12);
                    rt.transform.position = new Vector3(rt.transform.position.x, rt.transform.position.y - 6);
                    DeveloperConsoleInputField.Select();
                    DeveloperConsoleInputField.ActivateInputField();
                    */
                }
            });
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F12))
            {
                DeveloperConsolePanel.gameObject.SetActive(!_visible);
                _visible = !_visible;
                if (_visible)
                {
                    DeveloperConsoleInputField.Select();
                    DeveloperConsoleInputField.ActivateInputField();
                }
            }
        }
    }
}
