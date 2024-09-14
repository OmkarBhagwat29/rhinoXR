using Rhino.Display;
using Rhino.DocObjects;
using Rhino.FileIO;


namespace autoXR
{
    public class CustomXRDisplay : DisplayConduit
    {
        public CustomXRDisplay() 
        {
            
        }

        public RhinoObject RhObjAdded { get; set; }

        protected override void PostDrawObjects(DrawEventArgs e)
        {
            //string rhinoObjectJsonString = RhObjAdded.ToJSON(new SerializationOptions());

            string geomJsonString = RhObjAdded.Geometry.ToJSON(new SerializationOptions());

            string combinedString = geomJsonString + "_AUTO_" + RhObjAdded.Id.ToString();

            SocketCommand.EmitBuffer(SocketCommand.geometryAddKey, combinedString);

            this.Enabled = false;

        }
    }
}
