using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Characters.Player;
using GameEditor.MapEditor;
using Global;
using Models.Components;
using UnityEngine;
using World;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Models.MapModels
{
    public class MapModelConverter : IGameSerializable
    {
        public string Reference { get; set; }
        
        public List<MapModel> Models { get; set; } = new List<MapModel>();

        public void CreateMapFromModel()
        {
            // TODO: Data Models to creatre and configure from MapModels and SaveModels as well.
            // Pass nothing when building new
            // Pass map data when loading a map in editor
            // Pass map data and save data when loading a map in game

            var mapBuilder = Object.FindObjectOfType<MapBuilder>();
            var map = Object.FindObjectOfType<MapComponent>();
            foreach (var floor in Models.OfType<FloorMapModel>())
            {
                var go = GameRegistry.ModelRegistry[floor.Identifier].Instantiate(new Vector3(floor.X, floor.Y));
                go.GetComponent<FloorComponent>().CanEnter = floor.CanEnter;
                go.GetComponent<FloorComponent>().References = floor.References;
                map.ModelsMap.Add(go.transform.position, go);
            }
            foreach (var wall in Models.OfType<WallMapModel>())
            {
                Vector3 rotation;
                mapBuilder.RoundToBuildModePosition(new Vector3(wall.X, wall.Y), BuildPositionMode.Side, out rotation);

                var go = GameRegistry.ModelRegistry[wall.Identifier].Instantiate(new Vector3(wall.X, wall.Y), rotation);

                go.GetComponent<WallComponent>().References = wall.References;

                map.ModelsMap.Add(go.transform.position, go);
            }
            foreach (var door in Models.OfType<DoorMapModel>())
            {
                Vector3 rotation;
                mapBuilder.RoundToBuildModePosition(new Vector3(door.X, door.Y), BuildPositionMode.Side, out rotation);

                var go = GameRegistry.ModelRegistry[door.Identifier].Instantiate(new Vector3(door.X, door.Y), rotation);
                go.GetComponent<DoorComponent>().IsOpen = door.IsOpen;
                go.GetComponent<DoorComponent>().LoadOtherLevelOnUse = door.LoadOtherLevelOnUse;
                go.GetComponent<DoorComponent>().MapReference = door.MapReference;
                go.GetComponent<DoorComponent>().SpawnPointReference = door.SpawnReference;
                go.GetComponent<DoorComponent>().References = door.References;
                map.ModelsMap.Add(go.transform.position, go);
            }

            // Add references to map
            foreach (var model in Models)
            {
                foreach (var reference in model.References)
                {
                    Object.FindObjectOfType<MapComponent>().ReferenceMap.Add(reference, model);
                }
            }

            map.GenerateGraph();
        }

        public void CreateModelFromMap()
        {
            var map = Object.FindObjectOfType<MapComponent>();

            foreach (var go in map.ModelsMap.Values)
            {
                var comp = go.GetComponent<IWorldComponent>();
                if (comp is FloorComponent)
                {
                    Models.Add(new FloorMapModel(comp as FloorComponent));
                }
                else if (comp is DoorComponent)
                {
                    Models.Add(new DoorMapModel(comp as DoorComponent));
                }
                else if (comp is WallComponent)
                {
                    Models.Add(new WallMapModel(comp as WallComponent));
                }
                else
                {
                    Debug.LogError("Non-supported type!");
                }
            }
        }
    }
}