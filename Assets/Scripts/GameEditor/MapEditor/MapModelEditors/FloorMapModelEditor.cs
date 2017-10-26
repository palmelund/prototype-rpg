using System;
using System.Collections.Generic;
using System.Linq;
using Models.Components;
using UnityEngine;
using UnityEngine.UI;

namespace GameEditor.MapEditor.MapModelEditors
{
    public class FloorMapModelEditor : MonoBehaviour
    {
        public Button CloseButton;
        public Text IdentifierText;
        public Text PositionText;
        public Toggle CanEnterToggle;

        private FloorComponent _component;

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update ()
        {

        }

        public static GameObject CreateFromData(FloorComponent component)
        {
            var go = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Windows/Editor/FloorMapModelEditor"), GameObject.Find("GlobalCanvas").transform);
            go.GetComponent<FloorMapModelEditor>().ConfigureFromData(component);
            return go;
        }

        public void ConfigureFromData(FloorComponent component)
        {
            _component = component;

            CloseButton.onClick.AddListener(CloseWindow);
            IdentifierText.text = component.FloorModel.IdName;
            PositionText.text = component.gameObject.transform.position.ToString();
            CanEnterToggle.isOn = component.CanEnter;
            CanEnterToggle.onValueChanged.AddListener(OnToggle);
        }

        private void OnToggle(bool value)
        {
            _component.CanEnter = value;
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