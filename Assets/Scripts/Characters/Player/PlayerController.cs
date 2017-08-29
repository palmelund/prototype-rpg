using System;
using System.Collections.Generic;
using Characters.PathFinding;
using UnityEngine;
using UnityEngine.EventSystems;
using World;
using Debug = UnityEngine.Debug;

namespace Characters.Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController ControllerInstance;
        public PlayableCharacter SelectedPlayerCharacter;

        public void Start()
        {
            ControllerInstance = this;
            SelectedPlayerCharacter = new PlayableCharacter();

            SelectedPlayerCharacter.GameObject = new GameObject("player_go");
            var spriteRenderer = SelectedPlayerCharacter.GameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("player");
            spriteRenderer.sortingLayerName = "Characters";
            SelectedPlayerCharacter.GameObject.transform.position = new Vector3(0, 0);

            SelectedPlayerCharacter.NextVertice = Map.Instance.GetTileAt(SelectedPlayerCharacter.GameObject.transform.position)?.Vertice;

            SelectedPlayerCharacter.GameObject.AddComponent<BoxCollider2D>();
        }

        public void Update()
        {
            UpdateMove();
        }

        public void UpdateMove()
        {
            // Prevent character from moving while transitioning to new graph
            if (SelectedPlayerCharacter.NextVertice.Position.Equals(Vector3.positiveInfinity))
            {
                return; // Ensure no movement while generating graph
                // TODO: This is not how this should work in the future
                // TODO: Be aware of == when working with vectors. It is not the same as Equal
            }

            if (SelectedPlayerCharacter.GameObject.transform.position == SelectedPlayerCharacter.NextVertice.Tile.gameObject.transform.position && SelectedPlayerCharacter.Path.Count > 0)
            {
                var currentVertice = SelectedPlayerCharacter.NextVertice;
                SelectedPlayerCharacter.NextVertice = SelectedPlayerCharacter.Path.First.Value;
                SelectedPlayerCharacter.Path.RemoveFirst();

                if (currentVertice != SelectedPlayerCharacter.NextVertice && !currentVertice.NeighborList.Contains(SelectedPlayerCharacter.NextVertice))
                {
                    SelectedPlayerCharacter.NextVertice = currentVertice;
                }
            }

            SelectedPlayerCharacter.GameObject.transform.position = Vector3.MoveTowards(SelectedPlayerCharacter.GameObject.transform.position,
                SelectedPlayerCharacter.NextVertice.Tile.gameObject.transform.position, Time.deltaTime * SelectedPlayerCharacter.Speed);

            if (SelectedPlayerCharacter.GameObject.transform.position != SelectedPlayerCharacter.NextVertice.Tile.gameObject.transform.position)
            {
                var vectorToTarget = SelectedPlayerCharacter.NextVertice.Tile.gameObject.transform.position - SelectedPlayerCharacter.GameObject.transform.position;
                var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;

                SelectedPlayerCharacter.GameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        public void SetSelectedPlayerCharacterPath(Vector3 mousePosition)
        {
            var clickPos = Camera.main.ScreenToWorldPoint(mousePosition);
            var x = Mathf.RoundToInt(clickPos.x);
            var y = Mathf.RoundToInt(clickPos.y);
            var targetPos = new Vector3(x, y);
            var target = Map.Instance.GetTileAt(targetPos);
            if (target == null || target.CanEnter == false) return;

            if (SelectedPlayerCharacter.GameObject.transform.position == SelectedPlayerCharacter.NextVertice.Tile.gameObject.transform.position)
            {
                SelectedPlayerCharacter.Path = PathFinder.AStar(Map.Instance.GetTileAt(SelectedPlayerCharacter.GameObject.transform.position).Vertice, Map.Instance.GetTileAt(targetPos).Vertice);
            }
            else
            {
                SelectedPlayerCharacter.Path = PathFinder.AStar(Map.Instance.GetTileAt(SelectedPlayerCharacter.NextVertice.Position).Vertice, Map.Instance.GetTileAt(targetPos).Vertice);
            }
        }

        public void StopSelectedPlayerCharacter()
        {
            SelectedPlayerCharacter.Path.Clear(); // = new LinkedList<Vertice>();
        }

        public void MoveSelectedPlayerCharacterToTarget()
        {
            SetSelectedPlayerCharacterPath(Input.mousePosition);
        }
    }
}