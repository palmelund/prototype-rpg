using System;
using Characters.PathFinding;
using Characters.Player;
using Models.MapModels;
using UnityEngine;
using UnityEngine.EventSystems;
using World;

namespace GameEditor.MapEditor
{
    public class MapBuilder : MonoBehaviour
    {
        public EditorLeftClickActionState EditorLeftClickActionState = EditorLeftClickActionState.Move;
        public GameObject SelectedGameObject;

        private void Start()
        {
            
        }

        private void Update()
        {
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
        
        public void BuildDoor()
        {
            var world = FindObjectOfType<WorldComponent>();

            Vector3 sideRotation;
            var pos = RoundToBuildModePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), BuildPositionMode.Side, out sideRotation);

            if (world.WorldModelMap.ContainsKey(pos))
            {
                return;
            }

            var go = GameRegistry.DoorDataModelRegistry["door_stone_1"].InstantiateGame(pos, sideRotation);
            world.WorldModelMap.Add(pos, go);
        }

        public void BuildWall()
        {
            var world = FindObjectOfType<WorldComponent>();

            Vector3 sideRotation;
            var pos = RoundToBuildModePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), BuildPositionMode.Side, out sideRotation);

            if (world.WorldModelMap.ContainsKey(pos))
            {
                Debug.Log("Side already occupied!");
                return;
            }

            var go = GameRegistry.WallDataModelRegistry["wall_stone_1"].InstantiateGame(pos, sideRotation);
            world.WorldModelMap.Add(pos, go);
        }

        public void SetModeMove()
        {
            EditorLeftClickActionState = EditorLeftClickActionState.Move;
        }

        public void SetModeWall()
        {
            EditorLeftClickActionState = EditorLeftClickActionState.BuildWall;
        }

        public void SetModelDoor()
        {
            EditorLeftClickActionState = EditorLeftClickActionState.BuildDoor;
        }

        public void LoadMap()
        {
            var mapModel = MapModelRepresentation.DeserializeFromFile("map.xml");
            mapModel.CreateMapFromModel();
            var debugActorSpawner = new DebugActorSpawner();
        }

        public void SaveMap()
        {
            var mapModel = new MapModelRepresentation();
            mapModel.CreateModelFromMap();
            mapModel.Serialize("map.xml");
        }
    }
}