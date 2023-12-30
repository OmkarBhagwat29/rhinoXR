using System;
using System.IO;
using System.Threading.Tasks;
using Rhino;
using Rhino.Commands;

namespace autoXR
{
    public class XrOff_Command : SocketCommand
    {
        public XrOff_Command()
        {
            Instance = this;
        }

        ///<summary>The only instance of the MyCommand command.</summary>
        public static XrOff_Command Instance { get; private set; }

        public override string EnglishName => "XrOff";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            Disconnect();

            RhinoDoc.AddRhinoObject -= RhinoDoc_AddRhinoObject;
            RhinoDoc.LayerTableEvent -= RhinoDoc_LayerTableEvent;
            RhinoDoc.ModifyObjectAttributes -= RhinoDoc_ModifyObjectAttributes;
            RhinoDoc.DeleteRhinoObject -= RhinoDoc_DeleteRhinoObject;
            RhinoDoc.BeforeTransformObjects -= RhinoDoc_BeforeTransformObjects;

            RhinoApp.WriteLine("XR is OFF!!!");

            doc.Views.Redraw();
            return Result.Success;
        }

    }
}