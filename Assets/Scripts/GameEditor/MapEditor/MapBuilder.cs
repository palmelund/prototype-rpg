using System;
using Characters.PathFinding;
using Characters.Player;
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
            Vector3 sideRotation;
            var pos = RoundToBuildModePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), BuildPositionMode.Side, out sideRotation);

            if (Map.Instance.WorldModelMap.ContainsKey(pos))
            {
                return;
            }

            var go = Map.Instance.ModelCatalogue["door_stone_1"].InstantiateGame(pos, sideRotation);
            Map.Instance.WorldModelMap.Add(pos, go);
        }

        public void BuildWall()
        {
            Vector3 sideRotation;
            var pos = RoundToBuildModePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), BuildPositionMode.Side, out sideRotation);

            if (Map.Instance.WorldModelMap.ContainsKey(pos))
            {
                Debug.Log("Side already occupied!");
                return;
            }

            var go = Map.Instance.ModelCatalogue["wall_stone_1"].InstantiateGame(pos, sideRotation);
            Map.Instance.WorldModelMap.Add(pos, go);
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
            
        }

        public void SaveMap()
        {
            
        }
    }
}