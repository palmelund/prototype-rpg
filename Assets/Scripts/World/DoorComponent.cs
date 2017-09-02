using UnityEngine;

namespace World
{
    public class DoorComponent : MonoBehaviour, IWorldComponent, IWallBehavior
    {
        // TODO: Better way to do this!

        public Vector3 TurnPoint { get; set; }

        public bool IsOpen { get; set; }

        private FloorComponent _sideA;
        private FloorComponent _sideB;
        public string Identifier { get; set; }

        public bool CanUseDoor { get; set; } = true;

        private GameObject MovingPart { get; set; }

        public void Configure(string identifier, Vector3 turnPoint, GameObject movingPart)
        {
            Identifier = identifier;
            TurnPoint = turnPoint;
            MovingPart = movingPart;
        }

        public void SetSides(FloorComponent sideA, FloorComponent sideB)
        {
            if (sideA != null && sideB != null)
            {
                CanUseDoor = true;
                _sideA = sideA;
                _sideB = sideB;
            }
            else
            {
                CanUseDoor = false;
            }
        }

        public void ToggleDoor()
        {
            if (!CanUseDoor) return;

            if (IsOpen)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }

        public void OpenDoor()
        {
            IsOpen = true;
            MovingPart.transform.RotateAround(TurnPoint, Vector3.forward, 90f);
            GetComponentInChildren<BoxCollider2D>().offset = new Vector2(MovingPart.transform.localPosition.x, MovingPart.transform.localPosition.y);
            ConnectTiles();
        }

        public void CloseDoor()
        {
            IsOpen = false;
            MovingPart.transform.RotateAround(TurnPoint, Vector3.forward, -90f);
            GetComponentInChildren<BoxCollider2D>().offset = Vector2.zero;
            DisconnectTiles();
        }

        // TODO: Block other tiles when door is open?

        private void ConnectTiles()
        {
            _sideA.Vertice.NeighborList.Add(_sideB.Vertice);
            _sideB.Vertice.NeighborList.Add(_sideA.Vertice);
        }

        private void DisconnectTiles()
        {
            _sideA.Vertice.NeighborList.Remove(_sideB.Vertice);
            _sideB.Vertice.NeighborList.Remove(_sideA.Vertice);
        }
    }
}