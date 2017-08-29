using UnityEngine;

namespace World
{
    public class Door : MonoBehaviour, IWorldModel
    {
        public Vector3 TurnPoint;

        public bool IsOpen { get; private set; }

        public Tile SideA;
        public Tile SideB;
        public string Identifier { get; protected set; }

        public bool CanUseDoor { get; private set; }

        public void Configure(string identifier, Vector3 turnPoint)
        {
            Identifier = identifier;
            TurnPoint = turnPoint;
        }

        public void SetSides(Tile sideA, Tile sideB)
        {
            if (sideA != null && sideB != null)
            {
                CanUseDoor = true;
                SideA = sideA;
                SideB = sideB;
            }
            else
            {
                CanUseDoor = false;
            }
        }
        
        public void ToggleDoor()
        {
            if(!CanUseDoor) return;

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
            transform.RotateAround(TurnPoint, Vector3.forward, 90f);
            ConnectTiles();
        }

        public void CloseDoor()
        {
            IsOpen = false;
            transform.RotateAround(TurnPoint, Vector3.forward, -90f);
            DisconnectTiles();
        }

        // TODO: Block other tiles when door is open?

        private void ConnectTiles()
        {
            SideA.Vertice.NeighborList.Add(SideB.Vertice);
            SideB.Vertice.NeighborList.Add(SideA.Vertice);
        }

        private void DisconnectTiles()
        {
            SideA.Vertice.NeighborList.Remove(SideB.Vertice);
            SideB.Vertice.NeighborList.Remove(SideA.Vertice);
        }
    }
}