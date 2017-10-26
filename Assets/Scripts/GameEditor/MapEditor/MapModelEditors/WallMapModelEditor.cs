using System;
using System.Collections.Generic;
using System.Linq;
using Models.Components;
using UnityEngine;
using UnityEngine.UI;

namespace GameEditor.MapEditor.MapModelEditors
{
    public class WallMapModelEditor : MonoBehaviour
    {
        public Button CloseButton;
        public Text IdentifierText;
        public Text PositionText;

        private WallComponent _component;

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update ()
        {

        }

        public static GameObject CreateFromData(WallComponent component)
        {
            var go = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Windows/Editor/WallMapModelEditor"), GameObject.Find("GlobalCanvas").transform);
            go.GetComponent<WallMapModelEditor>().ConfigureFromData(component);
            return go;
        }

        public void ConfigureFromData(WallComponent component)
        {
            _component = component;

            CloseButton.onClick.AddListener(CloseWindow);
            IdentifierText.text = component.WallModel.IdName;
            PositionText.text = component.gameObject.transform.position.ToString();
            
        }
        private void OnEndEdit(string value)
        {
            var lines = value.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var references = new List<string>(lines);
            references = references.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
        }

        public void CloseWindow()
        {
            FindObjectOfType<MapBuilder>().OpenWindowMap.Remove(_component);
            _component = null;
            Debug.Log("Closing window!");
            Destroy(gameObject);
        }
    }
}