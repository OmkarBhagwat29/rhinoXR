using Newtonsoft.Json;
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

        public List<RhinoObject> ObjectsAdded { get; set; } = new List<RhinoObject>();

        protected override void PostDrawObjects(DrawEventArgs e)
        {
            List<string> jsonObjects = new List<string>();

            foreach (var obj in this.ObjectsAdded)
            {
                var jsonObjectString = obj.Geometry.ToJSON(new SerializationOptions())
                + "_AUTO_" + obj.Id.ToString();

                jsonObjects.Add(jsonObjectString);
            }

            var jsonString = JsonConvert.SerializeObject(jsonObjects, Formatting.Indented);

           SocketCommand.EmitBuffer(SocketCommand.geometryAddKey, jsonString);

            this.Enabled = false;

            this.ObjectsAdded.Clear();

        }
    }
}
