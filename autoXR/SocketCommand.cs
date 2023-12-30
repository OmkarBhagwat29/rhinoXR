using Rhino.FileIO;
using Rhino;
using Rhino.Commands;
using System.Threading;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

public abstract class SocketCommand : Command
{
    public static string host = "http://13.200.235.95:3001";
    //15.206.149.105
    //ssh -i om.pem ubuntu@15.206.149.105
    //pkill -f "nodemon index.js"
    public static SocketIOClient.SocketIO _socket;
    public static byte[] buffer;
    public const string emmitKey = "doc";

    public static string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".3dm");

    public static void SetBytes()
    {
        if (RhinoDoc.ActiveDoc == null)
        {
            buffer = null;
            return;
        }

        // Ensure that the document is saved
       var saved = RhinoDoc
            .ActiveDoc
            .Write3dmFile(filePath, new FileWriteOptions { SuppressDialogBoxes = true });

        if (!saved)
        {
            buffer = null;
            return;
        }

        // Read the saved file into a byte array
        var rhFile = File3dm.Read(filePath);
        buffer = rhFile.ToByteArray();
    }

    public static void EmitBuffer(string emitKeyName = emmitKey)
    {
        if (buffer != null)
        {

            Task.Run(async () => { 
                await _socket.ConnectAsync();
                if (_socket.Connected)
                {
                    string base64Data = Convert.ToBase64String(buffer);
                    await _socket.EmitAsync(emitKeyName, base64Data);
                    RhinoApp.WriteLine($"Geometry Sent!");
                }
            });

            _socket.On("offMe", async (data) => {

                if (!_socket.Connected)
                    return;

                //disconnect
                //RhinoApp.WriteLine("disconnected");
                await _socket.DisconnectAsync();
            });
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

    public static void RhinoDoc_AddRhinoObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
    {
        SetBytes();

        EmitBuffer();

        RhinoDoc.DeleteRhinoObject += RhinoDoc_DeleteRhinoObject;
    }

    public static void RhinoDoc_LayerTableEvent(object sender, Rhino.DocObjects.Tables.LayerTableEventArgs e)
    {
        SetBytes();
        EmitBuffer();

    }

    public static void RhinoDoc_ModifyObjectAttributes(object sender, Rhino.DocObjects.RhinoModifyObjectAttributesEventArgs e)
    {
        SetBytes();
        EmitBuffer();
    }

    public static void RhinoDoc_DeleteRhinoObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
    {
        SetBytes();
        EmitBuffer();
    }

    public static void RhinoDoc_BeforeTransformObjects(object sender,
        Rhino.DocObjects.RhinoTransformObjectsEventArgs e)
    {
        RhinoDoc.DeleteRhinoObject -= RhinoDoc_DeleteRhinoObject;

    }
}