using Rhino.Display;
using Rhino.DocObjects;
using Rhino.FileIO;
using System.Collections.Generic;


namespace autoXR
{
    public class CustomXRDisplay : DisplayConduit
    {
        public CustomXRDisplay() 
        {
            
        }

        public List<string> ObjectsAdded { get; set; } = new List<string>();

        protected override void PostDrawObjects(DrawEventArgs e)
        {

           // string geomJsonString = RhObjAdded.Geometry.ToJSON(new SerializationOptions());

           // string combinedString = geomJsonString + "_AUTO_" + RhObjAdded.Id.ToString();

           // SocketCommand.EmitBuffer(SocketCommand.geometryAddKey, combinedString);

            this.Enabled = false;

        }
    }
}
