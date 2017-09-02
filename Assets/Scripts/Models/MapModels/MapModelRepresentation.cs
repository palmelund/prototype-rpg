using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using GameEditor.MapEditor;
using UnityEngine;
using World;

namespace Models.MapModels
{
    public class MapModelRepresentation
    {
        public List<FloorMapModel> FloorModels { get; set; } = new List<FloorMapModel>();
        public List<WallMapModel> WallModels { get; set; } = new List<WallMapModel>();
        public List<DoorMapModel> DoorModels { get; set; } = new List<DoorMapModel>();

        public void Serialize(string file)
        {
            var serializer = new XmlSerializer(typeof(MapModelRepresentation));
            using (var stream = new FileStream(file, FileMode.Create))
            {
                serializer.Serialize(stream, this);
            }
        }

        public static MapModelRepresentation DeserializeFromFile(string file)
        {
            var serializer = new XmlSerializer(typeof(MapModelRepresentation));
            using (var stream = new FileStream(file, FileMode.Open))
            {
                return serializer.Deserialize(stream) as MapModelRepresentation;
            }
        }

        public void CreateMapFromModel()
        {
            // TODO: Data Models to creatre and configure from MapModels and SaveModels as well.
            // Pass nothing when building new
            // Pass map data when loading a map in editor
            // Pass map data and save data when loading a map in game

            var mapBuilder = Object.FindObjectOfType<MapBuilder>();
            var map = Object.FindObjectOfType<WorldComponent>();
            foreach (var floor in FloorModels)
            {
                var go = GameRegistry.FloorDataModelRegistry[floor.Identifier].InstantiateGame(new Vector3(floor.X, floor.Y));
                var floorComponent = go.GetComponent<FloorComponent>();
                floorComponent.CanEnter = floor.CanEnter;
                map.TileMap.Add(go.transform.position, floorComponent);
            }
            foreach (var wall in WallModels)
            {
                Vector3 rotation;
                mapBuilder.RoundToBuildModePosition(new Vector3(wall.X, wall.Y), BuildPositionMode.Side, out rotation);

                var go = GameRegistry.WallDataModelRegistry[wall.Identifier].InstantiateGame(new Vector3(wall.X, wall.Y), rotation);
                map.WorldModelMap.Add(go.transform.position, go);
            }
            foreach (var door in DoorModels)
            {
                Vector3 rotation;
                mapBuilder.RoundToBuildModePosition(new Vector3(door.X, door.Y), BuildPositionMode.Side, out rotation);

                var go = GameRegistry.DoorDataModelRegistry[door.Identifier].InstantiateGame(new Vector3(door.X, door.Y), rotation);
                var doorComponent = go.GetComponent<DoorComponent>();
                doorComponent.IsOpen = door.IsOpen;
                map.WorldModelMap.Add(go.transform.position, go);
            }

            map.GenerateGraph();
        }

        public void CreateModelFromMap()
        {
            var map = Object.FindObjectOfType<WorldComponent>();

            foreach (var tile in map.TileMap.Values)
            {
                FloorModels.Add(new FloorMapModel(tile));
            }

            foreach (var obj in map.WorldModelMap.Values)
            {
                var comp = obj.GetComponent<IWorldComponent>();
                
                if (comp is DoorComponent)
                {
                    DoorModels.Add(new DoorMapModel(comp as DoorComponent));
                }
                else if (comp is WallComponent)
                {
                    WallModels.Add(new WallMapModel(comp as WallComponent));
                }
                else if (comp is FloorComponent)
                {
                    // TODO: Support me
                }
                else
                {
                    Debug.LogError(comp.Identifier + " does not fit any MapModel conversions!");
                }
            }
        }
    }
}