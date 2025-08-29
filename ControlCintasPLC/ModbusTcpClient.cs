using System;
using System.Drawing;
using System.Net.Mime;
using System.Net.Sockets;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ControlCintasPLC
{
    public class ModbusTcpClient
    {
        private TcpClient tcpClient;
        private NetworkStream stream;
        private ushort transactionId = 0;

        public void Connect(string ip, int port)
        {
            try
            {
                tcpClient = new TcpClient
                {
                    ReceiveTimeout = 1000,
                    SendTimeout = 1000
                };
                tcpClient.Connect(ip, port);
                stream = tcpClient.GetStream();
            }
            catch (SocketException ex)
            {
                throw new Exception($"Error de conexión al PLC: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al conectar: {ex.Message}");
            }
        }

        public void Disconnect()
        {
            try
            {
                stream?.Close();
                tcpClient?.Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al desconectar: {ex.Message}");
            }
        }

        private byte[] CreateRequest(byte functionCode, byte[] data)
        {
            transactionId++;
            byte[] header = new byte[6];
            header[0] = (byte)(transactionId >> 8);
            header[1] = (byte)(transactionId & 0xFF);
            header[2] = 0;
            header[3] = 0;
            ushort length = (ushort)(data.Length + 2);
            header[4] = (byte)(length >> 8);
            header[5] = (byte)(length & 0xFF);
            byte[] request = new byte[header.Length + length];
            Array.Copy(header, 0, request, 0, header.Length);
            request[6] = 0;
            request[7] = functionCode;
            Array.Copy(data, 0, request, 8, data.Length);
            return request;
        }

        private byte[] ReadResponse(int expectedDataLength)
        {
            try
            {
                byte[] response = new byte[9 + expectedDataLength];
                int bytesRead = stream.Read(response, 0, response.Length);
                if (bytesRead != response.Length)
                    throw new Exception("Respuesta incompleta desde el PLC");
                if (response[7] > 0x80)
                    throw new Exception($"Error Modbus: Código {response[8]}");
                if (response[0] != (byte)(transactionId >> 8) || response[1] != (byte)(transactionId & 0xFF))
                    throw new Exception("ID de transacción no coincide");
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al leer respuesta: {ex.Message}");
            }
        }

        public bool[] ReadCoils(ushort startAddress, ushort count)
        {
            try
            {
                byte[] data = new byte[4];
                data[0] = (byte)(startAddress >> 8);
                data[1] = (byte)(startAddress & 0xFF);
                data[2] = (byte)(count >> 8);
                data[3] = (byte)(count & 0xFF);
                byte[] request = CreateRequest(1, data);
                stream.Write(request, 0, request.Length);
                byte[] response = ReadResponse((count + 7) / 8);
                byte byteCount = response[8];
                bool[] coils = new bool[count];
                for (int i = 0; i < count; i++)
                {
                    coils[i] = (response[9 + (i / 8)] & (1 << (i % 8))) != 0;
                }
                return coils;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al leer coils: {ex.Message}");
            }
        }

        public void WriteSingleCoil(ushort address, bool value)
        {
            try
            {
                byte[] data = new byte[4];
                data[0] = (byte)(address >> 8);
                data[1] = (byte)(address & 0xFF);
                data[2] = (byte)(value ? 0xFF : 0x00);
                data[3] = 0;
                byte[] request = CreateRequest(5, data);
                stream.Write(request, 0, request.Length);
                ReadResponse(4);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al escribir coil: {ex.Message}");
            }
        }
    }
}

