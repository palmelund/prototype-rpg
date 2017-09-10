using System;
using System.Collections.Generic;
using GameEditor.MapEditor.MapModelEditors;
using Models.Components;
using Models.DataModels;
using Models.MapModels;
using UnityEngine;
using UnityEngine.UI;
using World;

namespace GameEditor.MapEditor
{
    public class MapBuilder : MonoBehaviour
    {
        public EditorLeftClickActionState EditorLeftClickActionState = EditorLeftClickActionState.Select;
        public GameObject SelectedGameObject { get; set; }
        public DataModel SelectedDataModel { get; set; }

        public InputField FileNameInputField;

        public readonly Dictionary<IWorldComponent, GameObject> OpenWindowMap = new Dictionary<IWorldComponent, GameObject>();

        public bool FocusOnInputField { get; set; } = false;

        private void Start()
        {
            CreateBuildableObjectList();
        }

        private void Update()
        {
            if (!FocusOnInputField)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (SelectedGameObject != null)
                    {
                        SelectedGameObject.GetComponent<IWorldComponent>().OpenEditorWindow();
                    }
                    else
                    {
                        Debug.Log("Nothing to edit!");
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SelectedGameObject = null;
                }
                else if (Input.GetKeyDown(KeyCode.Delete))
                {
                    // TODO: Delete the selected gameobject
                }
            }
        }

        public void CreateBuildableObjectList()
        {
            var scrollView = Instantiate(Resources.Load<GameObject>("Prefabs/LeftSideScrollView"), GameObject.Find("GlobalCanvas").transform);
            scrollView.name = "Buildable Objects Scroll View";
            var content = scrollView.transform.Find("Viewport").Find("Content").gameObject;
            
            var count = GameRegistry.ModelRegistry.Count;

            var height = count * 35;
            
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(175, height);
            var verticalPositionOffset = height; // (height - 35) / 2;

            foreach (var model in GameRegistry.ModelRegistry.Values)
            {
                EditorSelectorButtonBuilder(model.Identifier, verticalPositionOffset, content);
                verticalPositionOffset -= 35;
            }
        }

        public void EditorSelectorButtonBuilder(string identifier, int verticalPositionOffset, GameObject content)
        {
            var go = Instantiate(Resources.Load<GameObject>("Prefabs/SampleButton"));
            go.transform.SetParent(content.transform);
            go.transform.localScale = Vector3.one;

            var button = go.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                GameRegistry.ModelRegistry[identifier].OnEditorBuilderSelect();
            });
            button.GetComponentInChildren<Text>().text = identifier;

            var rt = go.GetComponent<RectTransform>();
            rt.transform.position = new Vector3(rt.transform.position.x, rt.transform.position.y - verticalPositionOffset);
        }

        public Vector3 RoundToBuildModePosition(Vector3 position, BuildPositionMode buildPositionMode, out Vector3 sideRotation)
        {
            switch (buildPositionMode)
            {
                case BuildPositionMode.Center:
                    sideRotation = Vector3.zero;
                    return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
                case BuildPositionMode.Corner:
                    sideRotation = Vector3.zero;
                    return new Vector3(Mathf.Round(position.x + 0.5f) - 0.5f, Mathf.Round(position.y + 0.5f) - 0.5f);
                case BuildPositionMode.Side:
                    var xDecimal = position.x - (int)position.x;
                    var yDecimal = position.y - (int)position.y;

                    if (position.x < 0)
                    {
                        xDecimal = 1 - Mathf.Abs(xDecimal);
                    }

                    if (position.y < 0)
                    {
                        yDecimal = 1 - Mathf.Abs(yDecimal);
                    }

                    if (xDecimal >= 0.5f && yDecimal >= 0.5f)
                    {
                        if (xDecimal > yDecimal)
                        {
                            sideRotation = Vector3.zero;
                            return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y) - 0.5f);  // Down
                        }
                        else
                        {
                            sideRotation = new Vector3(0f, 0f, 90f);
                            return new Vector3(Mathf.RoundToInt(position.x) - 0.5f, Mathf.RoundToInt(position.y));  // Left
                        }
                    }
                    else if (yDecimal >= 0.5f)
                    {
                        yDecimal -= 0.5f;
                        xDecimal = 0.5f - xDecimal;

                        if (xDecimal > yDecimal)
                        {
                            sideRotation = Vector3.zero;
                            return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y) - 0.5f);  // Down
                        }
                        else
                        {
                            sideRotation = new Vector3(0f, 0f, 90f);
                            return new Vector3(Mathf.RoundToInt(position.x) + 0.5f, Mathf.RoundToInt(position.y));  // Right
                        }
                    }
                    else if (xDecimal >= 0.5f)
                    {
                        xDecimal -= 0.5f;
                        yDecimal = 0.5f - yDecimal;

                        if (xDecimal > yDecimal)
                        {
                            sideRotation = Vector3.zero;
                            return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y) + 0.5f);  // Up
                        }
                        else
                        {
                            sideRotation = new Vector3(0f, 0f, 90f);
                            return new Vector3(Mathf.RoundToInt(position.x) - 0.5f, Mathf.RoundToInt(position.y));  // Left
                        }
                    }
                    else
                    {
                        if (xDecimal > yDecimal)
                        {
                            sideRotation = new Vector3(0f, 0f, 90f);
                            return new Vector3(Mathf.RoundToInt(position.x) + 0.5f, Mathf.RoundToInt(position.y));  // Right
                        }
                        else
                        {
                            sideRotation = Vector3.zero;
                            return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y) + 0.5f);  // Up
                        }
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildPositionMode), buildPositionMode, null);
            }
        }

        public void BuildSelectedObject()
        {
            var world = FindObjectOfType<MapComponent>();

            Vector3 sideRotation;
            BuildPositionMode mode;

            // TODO: Better way to do this?
            if (SelectedDataModel is FloorDataModel)
            {
                mode = BuildPositionMode.Center;
            }
            else if (SelectedDataModel is WallDataModel || SelectedDataModel is DoorDataModel)
            {
                mode = BuildPositionMode.Side;
            }
            else
            {
                Debug.LogError($"Cannot set mode for: {SelectedDataModel.Identifier}");
                return;
            }

            var pos = RoundToBuildModePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), mode, out sideRotation);
            if (world.ModelsMap.ContainsKey(pos))
            {
                Debug.Log("Tile already occupied!");
                return;
                // TODO: Think about what should happen if trying to override existing object
            }
            
            var go = GameRegistry.ModelRegistry[SelectedDataModel.Identifier].Instantiate(pos, sideRotation);
            world.ModelsMap.Add(pos, go);
        }

        public void SetModeMove()
        {
            EditorLeftClickActionState = EditorLeftClickActionState.Move;
        }

        public void SetModeBuild()
        {
            EditorLeftClickActionState = EditorLeftClickActionState.Build;
        }

        public void SetModeSelect()
        {
            EditorLeftClickActionState = EditorLeftClickActionState.Select;
        }

        public void LoadMap()
        {
            FindObjectOfType<MapComponent>().ClearMap();
            var mapName = FileNameInputField.text == string.Empty ? "Data/Maps/map.xml" : "Data/Maps/" + FileNameInputField.text + ".xml";
            LoadMap(mapName);
        }

        public void SaveMap()
        {
            var mapName = FileNameInputField.text == string.Empty ? "Data/Maps/map.xml" : "Data/Maps/" + FileNameInputField.text + ".xml";
            SaveMap(mapName);
        }

        public void SaveMap(string mapName)
        {
            var mapModel = new MapModelConverter();
            mapModel.CreateModelFromMap();
            mapModel.Serialize(mapName);

        }

        public void LoadMap(string mapName)
        {
            var mapModel = MapModelConverter.DeserializeFromFile(mapName);
            mapModel.CreateMapFromModel();
        }
    }
}