using GameEditor.MapEditor;
using UnityEngine;

namespace Models
{
    public abstract class BaseModel : MonoBehaviour
    {
        public string IdName;
        public string DisplayName;

        public abstract GameObject Instantiate(Vector3 position);
        public abstract GameObject Instantiate(Vector3 position, Vector3 rotation);

        public void OnEditorBuilderSelect()
        {
            Debug.Log($"Setting {IdName} as current buildable");
            Object.FindObjectOfType<MapBuilder>().SelectedDataModel = this;
        }
    }
}