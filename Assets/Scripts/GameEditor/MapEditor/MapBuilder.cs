using System;
using Characters.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using World;

namespace GameEditor.MapEditor
{
    public class MapBuilder : MonoBehaviour
    {
        private Vector3 _sideRotation = Vector3.zero;
        public MoveMode MoveMode = MoveMode.Move;

        private void Start()
        {
            
        }

        private void Update()
        {

        }

        public Vector3 RoundToBuildModePosition(Vector3 position, BuildMode buildMode)
        {
            switch (buildMode)
            {
                case BuildMode.Center:
                    return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
                case BuildMode.Corner:
                    return new Vector3(Mathf.Round(position.x + 0.5f) - 0.5f, Mathf.Round(position.y + 0.5f) - 0.5f);
                case BuildMode.Side:
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
                            _sideRotation = Vector3.zero;
                            return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y) - 0.5f);  // Down
                        }
                        else
                        {
                            _sideRotation = new Vector3(0f, 0f, 90f);
                            return new Vector3(Mathf.RoundToInt(position.x) - 0.5f, Mathf.RoundToInt(position.y));  // Left
                        }
                    }
                    else if (yDecimal >= 0.5f)
                    {
                        yDecimal -= 0.5f;
                        xDecimal = 0.5f - xDecimal;

                        if (xDecimal > yDecimal)
                        {
                            _sideRotation = Vector3.zero;
                            return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y) - 0.5f);  // Down
                        }
                        else
                        {
                            _sideRotation = new Vector3(0f, 0f, 90f);
                            return new Vector3(Mathf.RoundToInt(position.x) + 0.5f, Mathf.RoundToInt(position.y));  // Right
                        }
                    }
                    else if (xDecimal >= 0.5f)
                    {
                        xDecimal -= 0.5f;
                        yDecimal = 0.5f - yDecimal;

                        if (xDecimal > yDecimal)
                        {
                            _sideRotation = Vector3.zero;
                            return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y) + 0.5f);  // Up
                        }
                        else
                        {
                            _sideRotation = new Vector3(0f, 0f, 90f);
                            return new Vector3(Mathf.RoundToInt(position.x) - 0.5f, Mathf.RoundToInt(position.y));  // Left
                        }
                    }
                    else
                    {
                        if (xDecimal > yDecimal)
                        {
                            _sideRotation = new Vector3(0f, 0f, 90f);
                            return new Vector3(Mathf.RoundToInt(position.x) + 0.5f, Mathf.RoundToInt(position.y));  // Right
                        }
                        else
                        {
                            _sideRotation = Vector3.zero;
                            return new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y) + 0.5f);  // Up
                        }
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(buildMode), buildMode, null);
            }
        }
        
        public void BuildDoor()
        {
            var pos = RoundToBuildModePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), BuildMode.Side);

            if (Map.Instance.WorldModelMap.ContainsKey(pos))
            {
                return;
            }

            var go = Map.Instance.ModelCatalogue["door_stone_1"].Instantiate(pos, _sideRotation);
            Map.Instance.WorldModelMap.Add(pos, go);
        }

        public void BuildWall()
        {
            var pos = RoundToBuildModePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), BuildMode.Side);

            if (Map.Instance.WorldModelMap.ContainsKey(pos))
            {
                Debug.Log("Side already occupied!");
                return;
            }

            var go = Map.Instance.ModelCatalogue["wall_stone_1"].Instantiate(pos, _sideRotation);
            Map.Instance.WorldModelMap.Add(pos, go);
        }

        public void SetModeMove()
        {
            MoveMode = MoveMode.Move;
        }

        public void SetModeWall()
        {
            MoveMode = MoveMode.BuildWall;
        }

        public void SetModelDoor()
        {
            MoveMode = MoveMode.BuildDoor;
        }
    }
}