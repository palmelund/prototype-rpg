using System;
using System.Collections.Generic;
using System.Linq;
using Models.Components;
using Models.MapModels;
using UnityEngine;
using UnityEngine.UI;

namespace GameEditor.MapEditor.MapModelEditors
{
    public class DoorMapModelEditor : MonoBehaviour
    {
        public Button CloseButton;
        public Text IdentifierText;
        public Text PositionText;
        public Toggle IsOpenToggle;
        public InputField ReferenceInputField;
        public Toggle CanUseDoor;
        public Toggle LoadOtherLevelOnUse;
        public InputField MapReference;
        public InputField SpawnReference;

        private DoorComponent _component;

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update ()
        {
            FindObjectOfType<MapBuilder>().FocusOnInputField = ReferenceInputField.isFocused;
        }

        public static GameObject CreateFromData(DoorComponent component)
        {
            var go = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Windows/Editor/DoorMapModelEditor"), GameObject.Find("GlobalCanvas").transform);
            go.GetComponent<DoorMapModelEditor>().ConfigureFromData(component);
            return go;
        }

        public void ConfigureFromData(DoorComponent component)
        {
            _component = component;

            Debug.Log(_component.CanUseDoor);
            Debug.Log(_component.LoadOtherLevelOnUse);
            Debug.Log(_component.IsOpen);

            CloseButton.onClick.AddListener(CloseWindow);
            IdentifierText.text = component.Identifier;
            PositionText.text = component.gameObject.transform.position.ToString();

            var references = string.Empty;
            for (var i = 0; i < component.References.Count; i++)
            {
                if (i == component.References.Count - 1)
                {
                    references += component.References[i];
                }
                else
                {
                    references += component.References[i] + "\n";
                }
            }

            ReferenceInputField.text = references;
            ReferenceInputField.onEndEdit.AddListener(ReferenceOnEndEdit);

            IsOpenToggle.isOn = component.IsOpen;
            IsOpenToggle.onValueChanged.AddListener(IsOpenOnToggle);

            CanUseDoor.isOn = component.CanUseDoor;
            CanUseDoor.onValueChanged.AddListener(CanUseDoorOnToggle);

            LoadOtherLevelOnUse.isOn = component.LoadOtherLevelOnUse;
            LoadOtherLevelOnUse.onValueChanged.AddListener(LoadOtherLevelOnToggle);

            MapReference.text = component.MapReference;
            MapReference.onEndEdit.AddListener(MapReferenceOnEndEdit);

            SpawnReference.text = component.SpawnPointReference;
            SpawnReference.onEndEdit.AddListener(SpawnReferenceOnEndEdit);
        }

        private void IsOpenOnToggle(bool value)
        {
            _component.IsOpen = value;
        }

        private void CanUseDoorOnToggle(bool value)
        {
            _component.CanUseDoor = value;
        }

        private void LoadOtherLevelOnToggle(bool value)
        {
            _component.LoadOtherLevelOnUse = value;
        }

        private void ReferenceOnEndEdit(string value)
        {
            var lines = value.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var references = new List<string>(lines);
            references = references.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            _component.References = references;
        }

        private void MapReferenceOnEndEdit(string value)
        {
            _component.MapReference = value;
        }

        private void SpawnReferenceOnEndEdit(string value)
        {
            _component.SpawnPointReference = value;
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