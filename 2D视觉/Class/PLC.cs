using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HslCommunication;
using HslCommunication.ModBus;
using HslCommunication.Profinet;
using HslCommunication.Profinet.Melsec;
using HslCommunication.Profinet.Omron;
using HslCommunication.Profinet.Siemens;

namespace _6524.Class
{
    public class basePLC
    {
        public PLCMode pLCMode; //"MC","ModbusTCP","FinsTCP""SiemensS7"
        private MelsecMcNet melsec_net;
        private OmronFinsNet omronFinsNet;
        private ModbusTcpNet Modbus_PLC;
        private SiemensS7Net Siemens_PLC;  // 西门子初始化要去对应一下型号默认S71200
        public string IP;
        public int Port;

        #region 初始化
        public bool init()
        {
            try
            {
                if (pLCMode == PLCMode.ModbusTCP)
                {
                    Modbus_PLC = new ModbusTcpNet(IP, Port);
                    Modbus_PLC.ConnectTimeOut = 2000;
                    OperateResult connect = Modbus_PLC.ConnectServer();
                    if (connect.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (pLCMode == PLCMode.MC)
                {
                    melsec_net = new MelsecMcNet(IP, Port);
                    melsec_net.ConnectTimeOut = 2000;
                    OperateResult connect = melsec_net.ConnectServer();
                    if (connect.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (pLCMode == PLCMode.FinsTCP)
                {
                    omronFinsNet = new OmronFinsNet(IP, Port);
                    omronFinsNet.ConnectTimeOut = 2000;
                    OperateResult connect = omronFinsNet.ConnectServer();
                    if (connect.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (pLCMode == PLCMode.SiemensS7)
                {
                    Siemens_PLC = new SiemensS7Net(SiemensPLCS.S1200, IP);
                    Siemens_PLC.ConnectTimeOut = 2000;
                    OperateResult connect = Siemens_PLC.ConnectServer();
                    if (connect.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion

        #region 读
        public OperateResult<bool[]> ReadBool(string address, ushort length)
        {
            OperateResult<bool[]> result = new OperateResult<bool[]>();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.ReadBool(address, length);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.ReadBool(address, length);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.ReadBool(address, length);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.ReadBool(address, length);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public OperateResult<float[]> ReadFloat(string address, ushort length)
        {
            OperateResult<float[]> result = new OperateResult<float[]>();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.ReadFloat(address, length);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.ReadFloat(address, length);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.ReadFloat(address, length);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.ReadFloat(address, length);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public OperateResult<byte[]> Read(string address, ushort length)
        {
            OperateResult<byte[]> result = new OperateResult<byte[]>();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.Read(address, length);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.Read(address, length);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.Read(address, length);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.Read(address, length);
            }
            else
            {
                result = null;
            }
            return result;
        }
        public OperateResult<Int32[]> ReadInt32(string address, ushort length)
        {
            OperateResult<Int32[]> result = new OperateResult<Int32[]>();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.ReadInt32(address, length);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.ReadInt32(address, length);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.ReadInt32(address, length);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.ReadInt32(address, length);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public OperateResult<Int16[]> ReadInt16(string address, ushort length)
        {
            OperateResult<Int16[]> result = new OperateResult<Int16[]>();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.ReadInt16(address, length);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.ReadInt16(address, length);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.ReadInt16(address, length);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.ReadInt16(address, length);
            }
            else
            {
                result = null;
            }
            return result;
        }
        public OperateResult<UInt16[]> ReadUInt16(string address, ushort length)
        {
            OperateResult<UInt16[]> result = new OperateResult<UInt16[]>();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.ReadUInt16(address, length);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.ReadUInt16(address, length);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.ReadUInt16(address, length);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.ReadUInt16(address, length);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public OperateResult<UInt32[]> ReadUInt32(string address, ushort length)
        {
            OperateResult<UInt32[]> result = new OperateResult<UInt32[]>();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.ReadUInt32(address, length);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.ReadUInt32(address, length);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.ReadUInt32(address, length);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.ReadUInt32(address, length);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public OperateResult<Double[]> ReadDouble(string address, ushort length)
        {
            OperateResult<Double[]> result = new OperateResult<Double[]>();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.ReadDouble(address, length);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.ReadDouble(address, length);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.ReadDouble(address, length);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.ReadDouble(address, length);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public OperateResult<String> ReadString(string address, ushort length)
        {
            OperateResult<String> result = new OperateResult<String>();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.ReadString(address, length);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.ReadString(address, length);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.ReadString(address, length);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.ReadString(address, length);
            }
            else
            {
                result = null;
            }
            return result;
        }
        #endregion

        #region 写

        public OperateResult Write(string address, bool value)
        {
            OperateResult result = new OperateResult();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.Write(address, value);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.Write(address, value);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.Write(address, value);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.Write(address, value);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public OperateResult Write(string address, byte[] value)
        {
            OperateResult result = new OperateResult();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.Write(address, value);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.Write(address, value);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.Write(address, value);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.Write(address, value);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public OperateResult Write(string address, bool[] value)
        {
            OperateResult result = new OperateResult();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.Write(address, value);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.Write(address, value);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.Write(address, value);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.Write(address, value);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public OperateResult Write(string address, int value)
        {
            OperateResult result = new OperateResult();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.Write(address, value);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.Write(address, value);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.Write(address, value);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.Write(address, value);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public OperateResult Write(string address, long value)
        {
            OperateResult result = new OperateResult();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.Write(address, value);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.Write(address, value);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.Write(address, value);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.Write(address, value);
            }
            else
            {
                result = null;
            }
            return result;
        }
        public OperateResult Write(string address, ulong value)
        {
            OperateResult result = new OperateResult();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.Write(address, value);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.Write(address, value);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.Write(address, value);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.Write(address, value);
            }
            else
            {
                result = null;
            }
            return result;
        }
        public OperateResult Write(string address, ushort value)
        {
            OperateResult result = new OperateResult();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.Write(address, value);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.Write(address, value);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.Write(address, value);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.Write(address, value);
            }
            else
            {
                result = null;
            }
            return result;
        }
        public OperateResult Write(string address, string value)
        {
            OperateResult result = new OperateResult();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.Write(address, value);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.Write(address, value);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.Write(address, value);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.Write(address, value);
            }
            else
            {
                result = null;
            }
            return result;
        }
        public OperateResult Write(string address, uint value)
        {
            OperateResult result = new OperateResult();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.Write(address, value);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.Write(address, value);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.Write(address, value);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.Write(address, value);
            }
            else
            {
                result = null;
            }
            return result;
        }
        public OperateResult Write(string address, double value)
        {
            OperateResult result = new OperateResult();
            if (pLCMode == PLCMode.ModbusTCP)
            {
                result = Modbus_PLC.Write(address, value);
            }
            else if (pLCMode == PLCMode.MC)
            {
                result = melsec_net.Write(address, value);
            }
            else if (pLCMode == PLCMode.FinsTCP)
            {
                result = omronFinsNet.Write(address, value);
            }
            else if (pLCMode == PLCMode.SiemensS7)
            {
                result = Siemens_PLC.Write(address, value);
            }
            else
            {
                result = null;
            }
            return result;
        }
        #endregion


        #region 关闭连接 
        //长连接切换至短链接

        public bool ConnectClose()
        {
            try
            {
                if (pLCMode == PLCMode.ModbusTCP)
                {
                    Modbus_PLC.ConnectClose();
                    return true;
                }
                else if (pLCMode == PLCMode.MC)
                {
                    melsec_net.ConnectClose();
                    return true;
                }
                else if (pLCMode == PLCMode.FinsTCP)
                {
                    omronFinsNet.ConnectClose();
                    return true;

                }
                else if (pLCMode == PLCMode.SiemensS7)
                {
                    Siemens_PLC.ConnectClose();
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion

        #region 释放
        public bool dispose()
        {
            try
            {
                if (pLCMode == PLCMode.ModbusTCP)
                {
                    Modbus_PLC.Dispose();
                    return true;
                }
                else if (pLCMode == PLCMode.MC)
                {
                    melsec_net.Dispose();
                    return true;
                }
                else if (pLCMode == PLCMode.FinsTCP)
                {
                    omronFinsNet.Dispose();
                    return true;

                }
                else if (pLCMode == PLCMode.SiemensS7)
                {
                    Siemens_PLC.Dispose();
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion
    }

    public enum PLCMode
    {
        ModbusTCP,
        MC,
        FinsTCP,
        SiemensS7
    }
}
