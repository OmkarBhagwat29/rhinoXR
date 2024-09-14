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
using System.Collections.Generic;
using System.Numerics;

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


    static CustomXRDisplay _cDisplay;
    protected SocketCommand()
    {
        // RhinoDoc.DeleteRhinoObject += RhinoDoc_DeleteRhinoObject;

        _cDisplay = new CustomXRDisplay();

    }


    public static void EmitBuffer(string emitKey, object jsonObjStrings)
    {
        if (jsonObjStrings != null)
        {

            try
            {

                Task.Run(async () =>
                {
                    await _socket.ConnectAsync();
                    if (_socket.Connected)
                    {

                        await _socket.EmitAsync(emitKey, jsonObjStrings);

                        RhinoApp.WriteLine($"Geometry Sent!");

                    }
                });
            }
            catch (Exception e)
            {

                RhinoApp.WriteLine(e.Message);
            }

            try
            {
                _socket.On("offMe", async (data) =>
                {

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


    public static async Task EmitBufferAsync(string emitKey, object jsonObjectString)
    {
        try
        {
            if (jsonObjectString == null)
                return;

            await _socket.ConnectAsync();
            if (_socket.Connected)
            {
                await _socket.EmitAsync(emitKey, jsonObjectString);
            }

        }
        catch
        {

        }
    }

    public static void Disconnect()
    {
        if (_socket == null)
        {
            return;
        }

        Task.Run(async () =>
        {
            await _socket.DisconnectAsync();
            if (!_socket.Connected)
            {
                RhinoApp.WriteLine("Disconnected!!!");
            }
        });

    }

    public static void RhinoDoc_AddRhinoObject(object sender, RhinoObjectEventArgs e)
    {
        // Wait for RhinoDoc_DeleteRhinoObject to complete

        _cDisplay.Enabled = true;

        _cDisplay.ObjectsAdded.Add(e.TheObject);

    }

    public static void RhinoDoc_ModifyObjectAttributes(object sender, Rhino.DocObjects.RhinoModifyObjectAttributesEventArgs e)
    {

        //SetBytes(e);
        //EmitBuffer();

    }


    public static async void RhinoDoc_DeleteRhinoObject(object sender, Rhino.DocObjects.RhinoObjectEventArgs e)
    {
        var id = e.ObjectId.ToString();

        // Reset the TaskCompletionSource

        await EmitBufferAsync(geometryDeleteKey, id);

    }

    public static void RhinoDoc_BeforeTransformObjects(object sender, RhinoTransformObjectsEventArgs e)
    {
        
    }

}