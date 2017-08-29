using System.Collections.Generic;
using Characters.PathFinding;
using UnityEngine;

namespace Characters
{
    public class Character
    {
        public string Name { get; protected set; }
        public float Speed { get; protected set; }

        public GameObject GameObject { get; set; }
        
        public LinkedList<Vertice> Path = new LinkedList<Vertice>();
        public Vertice NextVertice;

        public Character()
        {
            Speed = 1;
        }
    }
}
