using System;
using System.Collections.Generic;
using Code.Characters.PathFinding;
using UnityEngine;

namespace Code.Characters.Npc
{
    public class NpcController : MonoBehaviour {

        public static List<Npc> NpcList = new List<Npc>();
        
        private float _nextTimer;

        // Use this for initialization
        void Start () {
            for (int i = 0; i < 5; i++)
            {
                Npc npc = new Npc("test.npc" + i, "Npc #" + i, 1);
                npc.NpcGameObject.transform.parent = gameObject.transform;
                NpcList.Add(npc);
            }
            _nextTimer = Time.fixedTime + 5f;
        }

        // Update is called once per frame
        void Update () {
            if (GameState.GameActionState != GameActionState.Combat && Time.fixedTime >= _nextTimer)
            {
                foreach (var npc in NpcList)
                {
                    npc.Move(GameState.Rand.Next(0, 10), GameState.Rand.Next(0, 10));
                }

                _nextTimer += 5f;
            }

            MoveRandom();
        }

        private void MoveRandom()
        {
            foreach (var npc in NpcList)
            {
                if (npc.Transform.position == npc.Position && npc.Path.Count > 0)
                {
                    npc.NextTile = npc.Path.Pop();
                    switch (npc.NextTile.Direction)
                    {
                        case PathFinderDirection.Up:
                            npc.Position += Vector3.up;
                            break;
                        case PathFinderDirection.UpRight:
                            npc.Position += Vector3.up;
                            npc.Position += Vector3.right;
                            break;
                        case PathFinderDirection.Right:
                            npc.Position += Vector3.right;
                            break;
                        case PathFinderDirection.DownRight:
                            npc.Position += Vector3.right;
                            npc.Position += Vector3.down;
                            break;
                        case PathFinderDirection.Down:
                            npc.Position += Vector3.down;
                            break;
                        case PathFinderDirection.DownLeft:
                            npc.Position += Vector3.down;
                            npc.Position += Vector3.left;
                            break;
                        case PathFinderDirection.Left:
                            npc.Position += Vector3.left;
                            break;
                        case PathFinderDirection.UpLeft:
                            npc.Position += Vector3.left;
                            npc.Position += Vector3.up;
                            break;
                        case PathFinderDirection.Stay:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                npc.NpcGameObject.transform.position = Vector3.MoveTowards(npc.NpcGameObject.transform.position, npc.Position, Time.deltaTime * npc.Speed);
            }
        }
    }
}
