using Characters.PathFinding;
using Models.Components;
using UnityEngine;
using World;

namespace Characters.Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayableCharacter SelectedPlayerCharacter;

        public void Start()
        {

        }

        public void Update()
        {
            if (SelectedPlayerCharacter == null)
            {
                return;
            }

            UpdateMove();
        }

        public void UpdateMove()
        {
            if (SelectedPlayerCharacter.GameObject.transform.position == SelectedPlayerCharacter.NextVertice.FloorComponent.gameObject.transform.position && SelectedPlayerCharacter.Path.Count > 0)
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
                SelectedPlayerCharacter.NextVertice.FloorComponent.gameObject.transform.position, Time.deltaTime * SelectedPlayerCharacter.Speed);

            if (SelectedPlayerCharacter.GameObject.transform.position != SelectedPlayerCharacter.NextVertice.FloorComponent.gameObject.transform.position)
            {
                var vectorToTarget = SelectedPlayerCharacter.NextVertice.FloorComponent.gameObject.transform.position - SelectedPlayerCharacter.GameObject.transform.position;
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
            var target = FindObjectOfType<MapComponent>().GetComponentAt<FloorComponent>(targetPos);
            if (target == null || target.CanEnter == false) return;

            if (SelectedPlayerCharacter.GameObject.transform.position == SelectedPlayerCharacter.NextVertice.FloorComponent.gameObject.transform.position)
            {
                var startVertice = FindObjectOfType<MapComponent>().GetComponentAt<FloorComponent>(SelectedPlayerCharacter.GameObject.transform.position).Vertice;
                var endVertice = FindObjectOfType<MapComponent>().GetComponentAt<FloorComponent>(targetPos).Vertice;
                SelectedPlayerCharacter.Path = PathFinder.AStar(startVertice, endVertice);
            }
            else
            {
                var startVertice = FindObjectOfType<MapComponent>().GetComponentAt<FloorComponent>(SelectedPlayerCharacter.NextVertice.Position).Vertice;
                var endVertice = FindObjectOfType<MapComponent>().GetComponentAt<FloorComponent>(targetPos).Vertice;
                SelectedPlayerCharacter.Path = PathFinder.AStar(startVertice, endVertice);
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