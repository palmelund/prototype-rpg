using System.Collections.Generic;

namespace Models.Components
{
    public interface IWorldComponent
    {
        string Identifier { get; set; }
        
        List<string> References { get; set; }

        void OpenEditorWindow();
    }
}