using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Rest;

static class AzureIoTHub
{
    //
    // Note: this connection string is specific to the device "TestDevice". To configure other devices,
    // see information on iothub-explorer at http://aka.ms/iothubgetstartedVSCS
    //
    const string connectionString = "<IoT Hub Connection String here>";

    //
    // To monitor messages sent to device "TestDevice" use iothub-explorer as follows:
    //    iothub-explorer HostName=fanie-hub.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=kWNJzx/q5s5CsrUsyUfEH5tttRUf+LwBlKrNJ6/DA2U= monitor-events "TestDevice"
    //

    // Refer to http://aka.ms/azure-iot-hub-vs-cs-wiki for more information on Connected Service for Azure IoT Hub

    public static async Task SendMessageAsync(string message)
    {

        var serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
        
        
        var msg = new Message(Encoding.ASCII.GetBytes(message));

        await serviceClient.SendAsync("TestDevice", msg);
    }

   
}
