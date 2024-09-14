using Rhino.FileIO;
using Rhino;
using Rhino.Commands;
using System.Threading;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Rhino.DocObjects;
using System.Linq;
using Rhino.Display;
using autoXR;

public abstract class SocketCommand : Command
{
    public static string host = "http://localhost:3001";
    //15.206.149.105
    //ssh -i om.pem ubuntu@15.206.149.105
    //pkill -f "nodemon index.js"
    public static SocketIOClient.SocketIO _socket;

    //public static string JsonAttributes = null;

    public const string geometryAddKey = "geometry_added";
    public const string geometryDeleteKey = "geometry_delete";

    static bool _deleteObjectRegistered = false;


    public static string filePath = Path.Combine(@"C:\Users\Om\OneDrive\Desktop\RhTest", Guid.NewGuid().ToString() + ".3dm");

    static CustomXRDisplay _cDisplay;
    protected SocketCommand()
    {
        RhinoDoc.DeleteRhinoObject += RhinoDoc_DeleteRhinoObject;

        _cDisplay = new CustomXRDisplay();

        _deleteObjectRegistered = true;
    }


    public static void EmitBuffer(string emitKey, string jsonObjString)
    {
        if (jsonObjString != null)
        {

            try
            {
                Task.Run(async () => {
                    await _socket.ConnectAsync();
                    if (_socket.Connected)
                    {

                            await _socket.EmitAsync(emitKey, jsonObjString);


                        RhinoApp.WriteLine($"Geometry Sent!");

                        File.Delete(filePath);
                    }
                });
            }
            catch (Exception e)
            {

                RhinoApp.WriteLine(e.Message);
            }

            try
            {
                _socket.On("offMe", async (data) => {

                    if (!_socket.Connected)
                        return;

                    //disconnect
                    //RhinoApp.WriteLine("disconnected");
                    await _socket.DisconnectAsync();
                });
            }
            catch
            {

            }

        }
    }

    public static void Disconnect()
    {
        if(_socket == null ) {
            return;
        }

        Task.Run(async () => { await _socket.DisconnectAsync();
            if (!_socket.Connected)
            {
                RhinoApp.WriteLine("Disconnected!!!");
            } });

    }

    public static void RhinoDoc_AddRhinoObject(object sender, RhinoObjectEventArgs e)
    {
        _cDisplay.Enabled = true;

        _cDisplay.RhObjAdded = e.TheObject;


        if (!_deleteObjectRegistered)
        {
            RhinoDoc.DeleteRhinoObject += RhinoDoc_DeleteRhinoObject;
            _deleteObjectRegistered = true;
        }
    }

    public static void RhinoDoc_ModifyObjectAttributes(object sender, Rhino.DocObjects.RhinoModifyObjectAttributesEventArgs e)
    {

            //SetBytes(e);
            //EmitBuffer();

    }

    public static void RhinoDoc_DeleteRhinoObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
    {
        var id = e.ObjectId.ToString();
        EmitBuffer(geometryDeleteKey,id);
    }

    public static void RhinoDoc_BeforeTransformObjects(object sender,
        Rhino.DocObjects.RhinoTransformObjectsEventArgs e)
    {


        RhinoDoc.DeleteRhinoObject -= RhinoDoc_DeleteRhinoObject;
        _deleteObjectRegistered = false;

    }
}