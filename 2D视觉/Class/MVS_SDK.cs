using HalconDotNet;

using MvCamCtrl.NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace _6524
{
    public class MVS_SDK
    {
        private static object lockObject = new object();
        //  先连接相机Connect_Cam
        // 设置触发模式
        //注册回调
        //开始采流
        //停止采集
        //关闭相机
        //释放资源
        MyCamera.MV_CC_DEVICE_INFO_LIST m_pDeviceList;
        public MyCamera.cbOutputExdelegate ImageCallback;  //新建委托
        public MyCamera device = new MyCamera(); //创建一个新的相机对象

       
        public bool signal = false; //采图完成信号
        public HObject Himage = null; //采集的halcon格式图片
        public Bitmap bmpImage = null;//采集的bitmap格式图片
        int _State = 0;//0 未连接相机 1 连接未打开 2已经打开相机

        public int State
        {
            get
            {
                return _State;
            }

            set
            {
                _State = value;
            }
        }

        public MVS_SDK()
        {
            HOperatorSet.GenEmptyObj(out Himage);
            // ch:枚举设备 | en:Enum device
            MyCamera.MV_CC_DEVICE_INFO_LIST stDevList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            // List<CCameraInfo> stDevList = new List<CCameraInfo>();
            int nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref stDevList);

            // int nRet = MyCamera.EnumDevices(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref stDevList);
            if (MyCamera.MV_OK != nRet)
            {
                Console.WriteLine("Enum device failed:{0:x8}", nRet);
            
            }
        }

        /// <summary>
        /// 回调函数，将图片转Bitmap
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="pFrameInfo"></param>
        /// <param name="pUser"></param>
        private void ImageCallBack_Bitmap(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {

            // 将数据转换为图像格式
            byte[] byteData = new byte[pFrameInfo.nFrameLen];
            Marshal.Copy(pData, byteData, 0, (int)pFrameInfo.nFrameLen);
            bmpImage = new Bitmap((int)pFrameInfo.nWidth, (int)pFrameInfo.nHeight, (int)pFrameInfo.nWidth * 3,
               System.Drawing.Imaging.PixelFormat.Format24bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(byteData, 0));

            // 处理图像数据
            Console.WriteLine("Received image data. Width={0}, Height={1}, FrameNum={2}",
                pFrameInfo.nWidth, pFrameInfo.nHeight, pFrameInfo.nFrameNum);
            signal = true;
            // TODO: 在此处添加对图像的处理代码
        }

        /// <summary>
        /// 回调直接转Halcon的Hobject 图像格式
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="pFrameInfo"></param>
        /// <param name="pUser"></param>
        private void ImageCallback_Hobject(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {

            Himage.Dispose();
            // 将海康相机的图像数据转换成Halcon图像
            HOperatorSet.GenImage1(out Himage, "byte", pFrameInfo.nWidth, pFrameInfo.nHeight, pData);
            signal = true;
            // 处理图像数据
            Console.WriteLine("Received image data. Width={0}, Height={1}, FrameNum={2}",
                       pFrameInfo.nWidth, pFrameInfo.nHeight, pFrameInfo.nFrameNum);
             // 显示Halcon图像
            // HOperatorSet.DispObj(hoImage);
        }


        /// <summary>
        /// 连接相机
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public bool Connect_Cam(string IP)
        {
            try
            {
                // DeviceListAcq();
                // ch:枚举设备 | en:Enum device
                MyCamera.MV_CC_DEVICE_INFO_LIST stDevList = new MyCamera.MV_CC_DEVICE_INFO_LIST();

                int nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref stDevList);
                if (MyCamera.MV_OK != nRet)
                {
                    Console.WriteLine("Enum device failed:{0:x8}", nRet);
                    return false;
                }
                Console.WriteLine("Enum device count : " + Convert.ToString(stDevList.nDeviceNum));
                if (0 == stDevList.nDeviceNum)
                {
                    return false;
                }
                else
                {
                    MyCamera.MV_CC_DEVICE_INFO stDevInfo;                            // 通用设备信息
                    Int32 nDevIndex = 0;
                    // ch:打印设备信息 en:Print device info
                    for (Int32 i = 0; i < stDevList.nDeviceNum; i++)
                    {
                       
                        stDevInfo = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(stDevList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));

                        if (MyCamera.MV_GIGE_DEVICE == stDevInfo.nTLayerType)
                        {
                            MyCamera.MV_GIGE_DEVICE_INFO stGigEDeviceInfo = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(stDevInfo.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                            uint nIp1 = ((stGigEDeviceInfo.nCurrentIp & 0xff000000) >> 24);
                            uint nIp2 = ((stGigEDeviceInfo.nCurrentIp & 0x00ff0000) >> 16);
                            uint nIp3 = ((stGigEDeviceInfo.nCurrentIp & 0x0000ff00) >> 8);
                            uint nIp4 = (stGigEDeviceInfo.nCurrentIp & 0x000000ff);

                            Console.WriteLine("\n" + i.ToString() + ": [GigE] User Define Name : " + stGigEDeviceInfo.chUserDefinedName);
                            string deviceIP = nIp1 + "." + nIp2 + "." + nIp3 + "." + nIp4;
                            if (deviceIP == IP)
                            {
                                stDevInfo = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(stDevList.pDeviceInfo[nDevIndex], typeof(MyCamera.MV_CC_DEVICE_INFO));

                                // ch:创建设备 | en:Create device
                                nRet = device.MV_CC_CreateDevice_NET(ref stDevInfo);
                                if (MyCamera.MV_OK != nRet)
                                {
                                    Console.WriteLine("Create device failed:{0:x8}", nRet);
                                   
                                    return false;
                                }
                                else { State = 1; }
                                // ch:打开设备 | en:Open device
                                 nRet = device.MV_CC_OpenDevice_NET();
                                if (MyCamera.MV_OK != nRet)
                                {
                                    Console.WriteLine("Open device failed:{0:x8}", nRet);
                                   
                                    return false;
                                }
                                else
                                {
                                    State = 2;
                                    return true;
                                }

                            }
                            nDevIndex++;
                        }

                    }
                    return false;
                }


            }
            catch (Exception)
            {

                return false;
            }

        }


        /// <summary>
        /// 设置触发模式，0为off ,1为on
        /// </summary>
        /// <param name="TriggerMode"></param>
        /// <returns></returns>
        public bool Set_TriggerMode(uint TriggerMode)
        {
            try
            {
                // ch:设置触发模式为off || en:set trigger mode as off
                int nRet = device.MV_CC_SetEnumValue_NET("TriggerMode", TriggerMode);
                if (MyCamera.MV_OK != nRet)
                {
                    Console.WriteLine("Set TriggerMode failed!");
                    return false;
                }
                if (TriggerMode == 0)
                {
                    device.MV_CC_SetEnumValue_NET("TriggerSource", 7);
                }
                else
                {
                    device.MV_CC_SetEnumValue_NET("TriggerSource", 0);//0为Line0,1为line1，详情见SDK 
                }






                return true;
            }
            catch (Exception)
            {
                return false;

            }

        }


        /// <summary>
        /// / //事件触发源选择
        /// </summary>
        /// <param name="EventScoure"></param>
        /// <returns></returns>

        public bool Set_Event(uint EventScoure)
        {
            try
            {
                // ch:开启Event | en:Set Event of ExposureEnd On

                //事件触发源选择

                // 0 ：AcquisitionStart

                //1：AcquisitionEnd

                //2：AcquisitionActive

                //3：FrameStart

                //4：FrameEnd

                //5：FrameActive

                //6：FrameBurstStart

                //7：FrameBurstEnd

                //8：FrameBurstActive

                //9：LineStart

                //10：ExposureStart

                //11：ExposureEnd

                //12：ExposureActive


                int nRet = device.MV_CC_SetEnumValue_NET("TriggerSelector", EventScoure);
                //下面这个也可以，两种写法
                //int nRet = device.MV_CC_SetEnumValueByString_NET("EventSelector", "ExposureEnd");



                if (MyCamera.MV_OK != nRet)
                {
                    Console.WriteLine("Set EventSelector failed!");
                    return false;
                }

                ///设置事件通知开启
                nRet = device.MV_CC_SetEnumValueByString_NET("EventNotification", "On");
                if (MyCamera.MV_OK != nRet)
                {
                    Console.WriteLine("Set EventNotification failed!");
                    return false;
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }


        /// <summary>
        /// 设置回调，如何为0，则采集返回Bitmap ,1返回Hobject
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool Set_callback(int mode)
        {
            try
            {
                if (mode == 0)
                {
                    // ch:注册回调函数 | en:Register Event callback
                    ImageCallback = new MyCamera.cbOutputExdelegate(ImageCallBack_Bitmap);
                }
                else
                {
                    // ch:注册回调函数 | en:Register Event callback
                    ImageCallback = new MyCamera.cbOutputExdelegate(ImageCallback_Hobject);
                }

                //int   nRet = device.MV_CC_RegisterEventCallBackEx_NET("ExposureEnd", ImageCallback, IntPtr.Zero);
                // MV_CC_RegisterImageCallBackEx_NET(ImageCallback, IntPtr.Zero);
                int nRet = device.MV_CC_RegisterImageCallBackEx_NET(ImageCallback, IntPtr.Zero);

                if (MyCamera.MV_OK != nRet)
                {
                    Console.WriteLine("Register event callback failed!");
                    return false; ;
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Set_Exposure(string value)
        {
            try
            {

                int nRet = device.MV_CC_SetFloatValue_NET("ExposureTime", float.Parse(value));
                if (nRet != MyCamera.MV_OK)
                {
                    Console.WriteLine("Set Exposure Time Fail!", nRet);
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool OpenCamera()
        {
            try
            {
                // ch:打开设备 | en:Open device
                int nRet = device.MV_CC_OpenDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    Console.WriteLine("Open device failed:{0:x8}", nRet);
                    State = 2;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
            
        }
        public bool Get_Oneframe()
        {
            lock (lockObject)
            {
                try
                {
                    device.MV_CC_SetEnumValue_NET("TriggerSource", 0);
                    Thread.Sleep(100);

                    // // ch:获取包大小 || en: Get Payload Size
                    MyCamera.MVCC_INTVALUE stParam = new MyCamera.MVCC_INTVALUE();
                    int nRet = device.MV_CC_GetIntValue_NET("PayloadSize", ref stParam);
                    // if (MyCamera.MV_OK != nRet)
                    // {
                    //     Console.WriteLine("Get PayloadSize failed:{0:x8}", nRet);

                    // }
                    uint nBufSize = stParam.nCurValue;

                    IntPtr pBufForDriver = Marshal.AllocHGlobal((int)nBufSize); MyCamera.MV_FRAME_OUT_INFO_EX FrameInfo = new MyCamera.MV_FRAME_OUT_INFO_EX();


                    nRet = device.MV_CC_GetOneFrameTimeout_NET(pBufForDriver, nBufSize, ref FrameInfo, 1000);
                    if (MyCamera.MV_OK == nRet)
                    {
                        Console.WriteLine("Width:" + Convert.ToString(FrameInfo.nWidth) + " Height:" + Convert.ToString(FrameInfo.nHeight)
                            + " FrameNum:" + Convert.ToString(FrameInfo.nFrameNum));

                        Himage.Dispose();
                        // 将海康相机的图像数据转换成Halcon图像
                        HOperatorSet.GenImage1(out Himage, "byte", FrameInfo.nWidth, FrameInfo.nHeight, pBufForDriver);
                        // signal = true;

                    }
                    else
                    {
                        Console.WriteLine("No data:{0:x8}", nRet);
                    }

                    Marshal.FreeHGlobal(pBufForDriver);


                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
             
        }
        /// <summary>
        /// 开始取流
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            try
            {
               
                // ch:开启抓图 || en: start grab image
               int nRet = device.MV_CC_StartGrabbing_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    Console.WriteLine("Start grabbing failed:{0:x8}", nRet);
                    return false;
                }
                return true;


            }
            catch (Exception)
            {
                return false;

            }
        }


        /// <summary>
        /// 停止取流
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {

            try
            {
                // ch:停止抓图 | en:Stop grabbing
                int nRet = device.MV_CC_StopGrabbing_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    Console.WriteLine("Stop grabbing failed{0:x8}", nRet);
                    return false;
                }
                close();

                return true;
            }
            catch (Exception)
            {

                return false;
            }


        }

        public bool Set_win(IntPtr win)
        {
            try
            {
                int nRet = device.MV_CC_Display_NET(win);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }


        public bool getcamerainfo(ref string   ExposureTime, ref string Gain , ref string  ResultingFrameRate)
        {
           MyCamera.MVCC_FLOATVALUE stParam = new MyCamera.MVCC_FLOATVALUE();
            int nRet = device.MV_CC_GetFloatValue_NET("ExposureTime", ref stParam);
            if (MyCamera.MV_OK == nRet)
            {
                ExposureTime = stParam.fCurValue.ToString("F1");
                device.MV_CC_GetFloatValue_NET("Gain", ref stParam);
                Gain = stParam.fCurValue.ToString("F1");
                device.MV_CC_GetFloatValue_NET("ResultingFrameRate", ref stParam);
                ResultingFrameRate = stParam.fCurValue.ToString("F1");
                return true;
            }
            else
            { 
                return false;
            }

        }

        public bool SetCamera(string ExposureTime, string Gain, string ResultingFrameRate)
        {
            try
            {
                int nRet;
                device.MV_CC_SetEnumValue_NET("ExposureAuto", 0);

                try
                {
                    float.Parse(ExposureTime);
                    float.Parse(Gain);
                    float.Parse(ResultingFrameRate);
                }
                catch
                {
                    Console.WriteLine("Please enter correct type!", 0);
                    return false;
                }

                nRet = device.MV_CC_SetFloatValue_NET("ExposureTime", float.Parse(ExposureTime));
                if (nRet != MyCamera.MV_OK)
                {
                    Console.WriteLine("Set Exposure Time Fail!", nRet);
                }

                device.MV_CC_SetEnumValue_NET("GainAuto", 0);
                nRet = device.MV_CC_SetFloatValue_NET("Gain", float.Parse(Gain));
                if (nRet != MyCamera.MV_OK)
                {
                    Console.WriteLine("Set Gain Fail!", nRet);
                }

                nRet = device.MV_CC_SetFloatValue_NET("AcquisitionFrameRate", float.Parse(ResultingFrameRate));
                if (nRet != MyCamera.MV_OK)
                {
                    Console.WriteLine("Set Frame Rate Fail!", nRet);
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <returns></returns>
        public bool close()
        {
            try
            {
                // ch:关闭设备 | en:Close device
               int  nRet = device.MV_CC_CloseDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    Console.WriteLine("Close device failed{0:x8}", nRet);

                    return false;
                }
                else
                {
                    State = 1;
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        /// <summary>
        /// 释放对象
        /// </summary>
        /// <returns></returns>
        public bool dispose()
        {
            try
            {
                int   nRet = device.MV_CC_DestroyDevice_NET();
                if (MyCamera.MV_OK != nRet)
                {
                    Console.WriteLine("Destroy device failed:{0:x8}", nRet);
                    
                    return false;
                }
                State = 0;
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
   
    }
}
