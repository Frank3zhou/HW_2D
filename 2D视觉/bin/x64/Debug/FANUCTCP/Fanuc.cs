
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


using Microsoft.VisualBasic;

namespace FANUCTCP
{
    public class Fanuc
    {

        Array config = new short[7];
        Array joint = new float[9];
        //定义对象
        public string HostName;

        private const string cnstApp = "frrjiftest";
        private const string cnstSection = "setting";

        private Random rnd = new Random();

        private FRRJIf.Core mobjCore;
        private FRRJIf.DataTable mobjDataTable;
        private FRRJIf.DataTable mobjDataTable2;
        private FRRJIf.DataCurPos mobjCurPos;
        private FRRJIf.DataCurPos mobjCurPosUF;
        private FRRJIf.DataCurPos mobjCurPos2;
        private FRRJIf.DataTask mobjTask;
        private FRRJIf.DataTask mobjTaskIgnoreMacro;
        private FRRJIf.DataTask mobjTaskIgnoreKarel;
        private FRRJIf.DataTask mobjTaskIgnoreMacroKarel;
        private FRRJIf.DataPosReg mobjPosReg;
        private FRRJIf.DataPosReg mobjPosReg2;
        private FRRJIf.DataPosRegXyzwpr mobjPosRegXyzwpr;
        private FRRJIf.DataPosRegMG mobjPosRegMG;
        private FRRJIf.DataSysVar mobjSysVarInt;
        private FRRJIf.DataSysVar mobjSysVarInt2;
        private FRRJIf.DataSysVar mobjSysVarReal;
        private FRRJIf.DataSysVar mobjSysVarReal2;
        private FRRJIf.DataSysVar mobjSysVarString;
        private FRRJIf.DataSysVarPos mobjSysVarPos;
        private FRRJIf.DataSysVar[] mobjSysVarIntArray;
        private FRRJIf.DataNumReg mobjNumReg;
        private FRRJIf.DataNumReg mobjNumReg2;
        private FRRJIf.DataNumReg mobjNumReg3;
        private FRRJIf.DataAlarm mobjAlarm;
        private FRRJIf.DataAlarm mobjAlarmCurrent;
        private FRRJIf.DataSysVar mobjVarString;
        private FRRJIf.DataString mobjStrReg;
        private FRRJIf.DataString mobjStrRegComment;



        public bool InterfaceConnect(string ip)
        {
            //bool conRes= new FRRJIf.Core();

            try
            {
                bool blnRes = false;
                string strHost = null;
                int lngTmp = 0;


                //   System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                mobjCore = new FRRJIf.Core();

                // You need to set data table before connecting.
                mobjDataTable = mobjCore.DataTable;

                {
                    mobjAlarm = mobjDataTable.AddAlarm(FRRJIf.FRIF_DATA_TYPE.ALARM_LIST, 5, 0);
                    mobjAlarmCurrent = mobjDataTable.AddAlarm(FRRJIf.FRIF_DATA_TYPE.ALARM_CURRENT, 1, 0);
                    mobjCurPos = mobjDataTable.AddCurPos(FRRJIf.FRIF_DATA_TYPE.CURPOS, 1);
                    mobjCurPosUF = mobjDataTable.AddCurPosUF(FRRJIf.FRIF_DATA_TYPE.CURPOS, 1, 15);
                    mobjCurPos2 = mobjDataTable.AddCurPos(FRRJIf.FRIF_DATA_TYPE.CURPOS, 2);
                    mobjTask = mobjDataTable.AddTask(FRRJIf.FRIF_DATA_TYPE.TASK, 1);
                    mobjTaskIgnoreMacro = mobjDataTable.AddTask(FRRJIf.FRIF_DATA_TYPE.TASK_IGNORE_MACRO, 1);
                    mobjTaskIgnoreKarel = mobjDataTable.AddTask(FRRJIf.FRIF_DATA_TYPE.TASK_IGNORE_KAREL, 1);
                    mobjTaskIgnoreMacroKarel = mobjDataTable.AddTask(FRRJIf.FRIF_DATA_TYPE.TASK_IGNORE_MACRO_KAREL, 1);
                    mobjPosReg = mobjDataTable.AddPosReg(FRRJIf.FRIF_DATA_TYPE.POSREG, 1, 1, 10);
                    mobjPosReg2 = mobjDataTable.AddPosReg(FRRJIf.FRIF_DATA_TYPE.POSREG, 2, 1, 4);
                    mobjSysVarInt = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$FAST_CLOCK");
                    mobjSysVarInt2 = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[10].$TIMER_VAL");
                    mobjSysVarReal = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_REAL, "$MOR_GRP[1].$CURRENT_ANG[1]");
                    mobjSysVarReal2 = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_REAL, "$DUTY_TEMP");
                    mobjSysVarString = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_STRING, "$TIMER[10].$COMMENT");
                    mobjSysVarPos = mobjDataTable.AddSysVarPos(FRRJIf.FRIF_DATA_TYPE.SYSVAR_POS, "$MNUTOOL[1,1]");
                    mobjVarString = mobjDataTable.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_STRING, "$[HTTPKCL]CMDS[1]");
                    mobjNumReg = mobjDataTable.AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_INT, 1, 50);
                    mobjNumReg2 = mobjDataTable.AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_REAL, 6, 10);
                    mobjPosRegXyzwpr = mobjDataTable.AddPosRegXyzwpr(FRRJIf.FRIF_DATA_TYPE.POSREG_XYZWPR, 1, 1, 10);
                    mobjPosRegMG = mobjDataTable.AddPosRegMG(FRRJIf.FRIF_DATA_TYPE.POSREGMG, "C,J6", 1, 10);
                    mobjStrReg = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.STRREG, 1, 3);
                    mobjStrRegComment = mobjDataTable.AddString(FRRJIf.FRIF_DATA_TYPE.STRREG_COMMENT, 1, 3);
                    Debug.Assert(mobjStrRegComment != null);
                }

                // 2nd data table.
                // You must not set the first data table.
                mobjDataTable2 = mobjCore.DataTable2;
                mobjNumReg3 = mobjDataTable2.AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_INT, 1, 5);
                mobjSysVarIntArray = new FRRJIf.DataSysVar[10];
                mobjSysVarIntArray[0] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[1].$TIMER_VAL");
                mobjSysVarIntArray[1] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[2].$TIMER_VAL");
                mobjSysVarIntArray[2] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[3].$TIMER_VAL");
                mobjSysVarIntArray[3] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[4].$TIMER_VAL");
                mobjSysVarIntArray[4] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[5].$TIMER_VAL");
                mobjSysVarIntArray[5] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[6].$TIMER_VAL");
                mobjSysVarIntArray[6] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[7].$TIMER_VAL");
                mobjSysVarIntArray[7] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[8].$TIMER_VAL");
                mobjSysVarIntArray[8] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[9].$TIMER_VAL");
                mobjSysVarIntArray[9] = mobjDataTable2.AddSysVar(FRRJIf.FRIF_DATA_TYPE.SYSVAR_INT, "$TIMER[10].$TIMER_VAL");

                //get host name
                if (string.IsNullOrEmpty(HostName))
                {
                    //  strHost = Interaction.GetSetting(cnstApp, cnstSection, "HostName", "");
                    strHost = ip;
                    if (string.IsNullOrEmpty(strHost))
                    {
                        System.Environment.Exit(0);
                    }
                    //  Interaction.SaveSetting(cnstApp, cnstSection, "HostName", strHost);
                    HostName = strHost;
                }
                else
                {
                    strHost = HostName;
                }

                //get time out value
                //  lngTmp = Convert.ToInt32(Interaction.GetSetting(cnstApp, cnstSection, "TimeOut", "-1"));

                //connect
                if (lngTmp > 0)
                    mobjCore.TimeOutValue = lngTmp;
                blnRes = mobjCore.Connect(strHost);

                if (blnRes)
                {
                    Array xyzwpr1121 = new float[9];
                    if (getnowpoint(ref xyzwpr1121))
                    {
                        return true;
                    }
                    return false;

                }
                else { return false; }






                ////连接，输入IP地址
                //bool res = mobjCore.Connect(ip);
                ////Console.WriteLine(res);

                ////读取SDI
                //Array intSDI = new short[10];
                //bool blnSDI = mobjCore.ReadSDI(1, ref intSDI, 10);

                ////使用前先刷新
                //bool blnDT = mobjDataTable.Refresh();

                //Array xyzwpr = new float[9];
                //Array config = new short[7];
                //Array joint = new float[9];
                //short intUF = 0;
                //short intUT = 0;
                //short intValidC = 0;
                //short intValidJ = 0;
                //object objTmp = null;

                ////查看是否有效
                //bool aa = mobjPosReg.Valid;

                ////bool result = mobjSysVarPos.GetValue(xyzwpr, config, joint, intUF,intUT, intValidC, intValidJ);

                ////bool resul = mobjCurPos.GetValue(xyzwpr, config, joint, intUF, intUT, intValidC, intValidJ);

                //// bool resu =  mobjPosReg.GetValue(1, ref xyzwpr,ref config, ref joint,ref  intUF, ref intUT, ref intValidC, ref intValidJ);

                ////float x = (float)xyzwpr.GetValue(0);

                ////读PR6
                //bool res_r = mobjPosReg.GetValue(6, ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ);

                //float x = (float)xyzwpr.GetValue(0);
                //float y = (float)xyzwpr.GetValue(1);
                //float z = (float)xyzwpr.GetValue(2);

                ////写PR11 将PR6的值写进PR11
                ////bool res_w = mobjPosReg.SetValueXyzwpr(11, ref xyzwpr, ref config, -1, -1);
                //return true;
            }
            catch (Exception e)
            {

                // MessageBox.Show(e.Message);
                return false;
            }

        }

        public bool readPR(int index, out Array xyzwpr)
        {
            try
            {
                bool blnDT = mobjDataTable.Refresh();

                xyzwpr = new float[9];
          
                short intUF = 0;
                short intUT = 0;
                short intValidC = 0;
                short intValidJ = 0;
                object objTmp = null;

                //读PR
                bool res_r = mobjPosReg.GetValue(index, ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ);
                return true;
            }
            catch (Exception)
            {
                xyzwpr = null;
                return false;
            }
        }

        public bool getnowpoint(ref Array xyzwpr)
        {
            //使用前先刷新
            bool blnDT = mobjDataTable.Refresh();

             xyzwpr = new float[9];
          
            short intUF = 0;
            short intUT = 0;
            short intValidC = 0;
            short intValidJ = 0;
            object objTmp = null;
            if (mobjCurPos.GetValue(ref xyzwpr, ref config, ref joint, ref intUF, ref intUT, ref intValidC, ref intValidJ))
            {
               return true;
            }
            else
            {
                return false;
            }
        }

        public bool writePR(int index, Array xyzwpr)
        {
            try
            {
                //使用前先刷新
                bool blnDT = mobjDataTable.Refresh();


              
                short intUF = 0;
                short intUT = 0;
                short intValidC = 0;
                short intValidJ = 0;
                object objTmp = null;
                bool res_w = mobjPosReg.SetValueXyzwpr(index, ref xyzwpr, ref config, -1, -1);
                return res_w;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool readRDI(int index ,ref object intR)
        {
            try
            {
                //使用前先刷新
                bool blnDT = mobjDataTable.Refresh();
                if (mobjNumReg.GetValue(index, ref intR) == true)
                {
                  
                    return true;
                }
                else
                {
                   
                    return false;
                }
               
            }
            catch (Exception)
            {
                intR = null;
                return false;
            }
        }


        public bool writeRDI(int index, Int32  intR)
        {
            try
            {
                //使用前先刷新
                bool blnDT = mobjDataTable.Refresh();
                int[] intValues = new int[1];
                intValues[0] = intR;
                if (mobjNumReg.SetValues(index, intValues, 1))
                {
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
        public void disconnect ()
        {
            msubClearVars();
        
        }

        private void msubClearVars()
        {
            try
            {
                mobjCore.Disconnect();

                mobjCore = null;
                mobjDataTable = null;
                mobjDataTable2 = null;
                mobjAlarm = null;
                mobjAlarmCurrent = null;
                mobjCurPos = null;
                mobjCurPos2 = null;
                mobjTask = null;
                mobjTaskIgnoreMacro = null;
                mobjTaskIgnoreKarel = null;
                mobjTaskIgnoreMacroKarel = null;
                mobjPosReg = null;
                mobjPosReg2 = null;
                mobjSysVarInt = null;
                mobjSysVarReal = null;
                mobjSysVarReal2 = null;
                mobjSysVarString = null;
                mobjSysVarPos = null;
                mobjNumReg = null;
                mobjNumReg2 = null;
                mobjNumReg3 = null;
                mobjVarString = null;
                mobjStrReg = null;
                mobjStrRegComment = null;
                for (int ii = mobjSysVarIntArray.GetLowerBound(0); ii <= mobjSysVarIntArray.GetUpperBound(0); ii++)
                {
                    mobjSysVarIntArray[ii] = null;
                }
            }
            catch (Exception)
            {

              //  throw;
            }

          

        }
    }
}
