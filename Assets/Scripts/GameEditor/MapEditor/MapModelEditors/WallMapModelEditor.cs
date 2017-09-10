﻿using System;
using System.Collections.Generic;
using System.Linq;
using Models.Components;
using Models.MapModels;
using UnityEngine;
using UnityEngine.UI;

namespace GameEditor.MapEditor.MapModelEditors
{
    public class WallMapModelEditor : MonoBehaviour
    {
        public Button CloseButton;
        public Text IdentifierText;
        public Text PositionText;
        public InputField ReferenceInputField;

        private WallComponent _component;

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update ()
        {
            FindObjectOfType<MapBuilder>().FocusOnInputField = ReferenceInputField.isFocused;
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
            ReferenceInputField.onEndEdit.AddListener(OnEndEdit);
        }
        private void OnEndEdit(string value)
        {
            var lines = value.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var references = new List<string>(lines);
            references = references.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            _component.References = references;
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