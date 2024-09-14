using Rhino;
using Rhino.Commands;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.Runtime.InProcess;
using Rhino.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace autoXR
{
    public class XrOn_Command : SocketCommand
    {

        public XrOn_Command()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;


        }


        ///<summary>The only instance of this command.</summary>
        public static XrOn_Command Instance { get; private set; }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName => "xrOn";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {

            _socket = new SocketIOClient.SocketIO(host);

            RhinoDoc.AddRhinoObject += RhinoDoc_AddRhinoObject;
            RhinoDoc.ModifyObjectAttributes += RhinoDoc_ModifyObjectAttributes;
            RhinoDoc.DeleteRhinoObject += RhinoDoc_DeleteRhinoObject;
            RhinoDoc.BeforeTransformObjects += RhinoDoc_BeforeTransformObjects;
         

            RhinoDoc.ActiveDocumentChanged += RhinoDoc_ActiveDocumentChanged;
          
            //SetBytes();
            //EmitBuffer();

            RhinoApp.WriteLine("XR is ON!!!");

            return Result.Success;
        }



        private void RhinoDoc_ActiveDocumentChanged(object sender, DocumentEventArgs e)
        {
            Disconnect();
        }
    }
}
