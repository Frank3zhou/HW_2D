using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;
using System.Diagnostics;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace _6524
{
    public partial class SetModel : Form
    {
        List<string> excelist = new List<string>();  
        DataTable t1 = new DataTable();
        DataTable t2 = new DataTable();
        private static string Param_Path = Application.StartupPath + "\\Param.ini";//配置表地址

        string DCpath ;
        public SetModel()
        {
            InitializeComponent();
       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = choose_DC();
                DCpath = textBox1.Text;
                DirectoryInfo folder = new DirectoryInfo(textBox1.Text);
                FileSystemInfo fileinfo1 = folder as FileSystemInfo;
                List<string> excelname = ListFiles(fileinfo1);
                IniAPI.INIWriteValue(Param_Path, "ModelExcel", "Path", DCpath);
                excelname.Clear();
                excelname = ListFiles(fileinfo1);
                listBox1.Items.Clear();
                for (int i = 0; i < excelname.Count; i++)
                {
                    listBox1.Items.Add(excelname[i].ToString());
                }

            }
            catch (Exception)
            {

                
            }
            
               
        }
        public string choose_DC()
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

                // 设置对话框的标题
                folderBrowserDialog1.Description = "选择文件夹";

                // 打开文件夹对话框并检查用户是否选择了文件夹
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    // 获取所选文件夹的路径
                    return folderBrowserDialog1.SelectedPath;

                    // 在控制台输出所选文件夹的路径
                    //Console.WriteLine("所选文件夹的路径是: " + selectedFolderPath);
                }
                else { return null; }
            }
            catch (Exception)
            {

                return null;
            }

        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputForm1 m_Inputform = new InputForm1("请输入新建配方表名称");
            m_Inputform.ShowDialog();
            IWorkbook workbook = new XSSFWorkbook();
            t1= Create_teachpoint1(1);
            t2 = Create_teachpoint2(1);
            string path = DCpath + "\\" + m_Inputform.Inputstr+".xlsx";

            //if (!File.Exists(path))
            //{
            //    //创建文件
            //    try
            //    {
            //        File.Create(path);
            //        File.Create(path).Close();

            //    }
            //    catch (Exception )
            //    {
            //    }
            //}

            if (!File.Exists(path))
            {
                using (File.Create(path))
                {
                }
            }


            TableToExcel(t1, t2, "Sheet1",   "Sheet2", path);
            t1 = ReadExcelToDataTable(path, "Sheet1", true);
            t2 = ReadExcelToDataTable(path, "Sheet2", true);
            dataGridView1.DataSource = t1;
            dataGridView2.DataSource = t2;

            DirectoryInfo folder = new DirectoryInfo(DCpath);
            FileSystemInfo fileinfo1 = folder as FileSystemInfo;
            List<string> excelname = ListFiles(fileinfo1);
            excelname.Clear();
            excelname = ListFiles(fileinfo1);
            listBox1.Items.Clear();
            for (int i = 0; i < excelname.Count; i++)
            {
                listBox1.Items.Add(excelname[i].ToString());
            }

        }

        public  List<string> ListFiles(FileSystemInfo info)
        {
            try
            {
                DirectoryInfo dir = info as DirectoryInfo;

                FileSystemInfo[] files = dir.GetFileSystemInfos();
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = files[i] as FileInfo;
                    //是文件
                    if (file != null)
                    {
                        string extension = Path.GetExtension(file.Name);
                        if (extension.ToUpper() == ".XLS")
                        {
                            excelist.Add(file.Name);
                        }
                        if (extension.ToUpper() == ".XLSX")
                        {
                            excelist.Add(file.Name);
                        }
                    }
                    else//对于子目录，进行递归调用
                        ListFiles(files[i]);
                }
                return excelist;
            }
            catch (Exception)
            {
                return null;
              //  throw;
            }
           
        }
        public DataTable Create_teachpoint1(int Number)
        {
            DataTable m_DataTable = new DataTable();

            m_DataTable.Columns.Add("运行顺序", Type.GetType("System.Int32"));//添加Id列，存储数据类型为Int
            m_DataTable.Columns.Add("等待拍照信号", Type.GetType("System.String"));//
            m_DataTable.Columns.Add("相机编号", Type.GetType("System.Int32"));//
            m_DataTable.Columns.Add("拍照完成信号", Type.GetType("System.String"));//
            m_DataTable.Columns.Add("视觉算子编号", Type.GetType("System.Int32"));//
            m_DataTable.Columns.Add("光源亮度", Type.GetType("System.Boolean"));
            m_DataTable.Columns.Add("是否存图", Type.GetType("System.Boolean"));//是否后台运行
            m_DataTable.Columns.Add("工位", Type.GetType("System.Boolean"));
            m_DataTable.Columns.Add("是否后台运行", Type.GetType("System.Boolean"));

            for (int i = 0; i < Number; i++)
            {
                DataRow newRow = m_DataTable.NewRow();
                newRow[0] = i + 1;
                newRow[1] = 0;
                newRow[2] = 0;
                newRow[3] = 0;
                newRow[4] = 0;
                newRow[5] = 0;
                newRow[6] = 0;
                newRow[7] = 0;
                newRow[8] = 0;

                m_DataTable.Rows.Add(newRow);
            }
            return m_DataTable;

        }
        public DataTable Create_teachpoint2(int Number)
        {
            DataTable m_DataTable = new DataTable();

     
            m_DataTable.Columns.Add("工位编号", Type.GetType("System.Int32"));//
            m_DataTable.Columns.Add("OK结果信号", Type.GetType("System.String"));//
            m_DataTable.Columns.Add("NG结果信号", Type.GetType("System.String"));//

            for (int i = 0; i < Number; i++)
            {
                DataRow newRow = m_DataTable.NewRow();
                newRow[0] = i + 1;
                newRow[1] = 0;
                newRow[2] = 0;
               
                //  newRow[6] = 0;

                m_DataTable.Rows.Add(newRow);
            }
            return m_DataTable;

        }



        //获取cell的数据，并设置为对应的数据类型
        public object GetCellValue(ICell cell)
        {
            object value = null;
            try
            {
                if (cell.CellType != CellType.Blank)
                {
                    switch (cell.CellType)
                    {
                        case CellType.Numeric:
                            // Date comes here
                            if (DateUtil.IsCellDateFormatted(cell))
                            {
                                value = cell.DateCellValue;
                            }
                            else
                            {
                                // Numeric type
                                value = cell.NumericCellValue;
                            }
                            break;
                        case CellType.Boolean:
                            // Boolean type
                            value = cell.BooleanCellValue;
                            break;
                        case CellType.Formula:
                            value = cell.CellFormula;
                            break;
                        default:
                            // String type
                            value = cell.StringCellValue;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                value = "";
            }

            return value;
        }


        //根据数据类型设置不同类型的cell
        public static void SetCellValue(ICell cell, object obj)
        {
            if (obj.GetType() == typeof(int))
            {
                cell.SetCellValue((int)obj);
            }
            else if (obj.GetType() == typeof(double))
            {
                cell.SetCellValue((double)obj);
            }
            else if (obj.GetType() == typeof(IRichTextString))
            {
                cell.SetCellValue((IRichTextString)obj);
            }
            else if (obj.GetType() == typeof(string))
            {
                cell.SetCellValue(obj.ToString());
            }
            else if (obj.GetType() == typeof(DateTime))
            {
                cell.SetCellValue((DateTime)obj);
            }
            else if (obj.GetType() == typeof(bool))
            {
                cell.SetCellValue((bool)obj);
            }
            else
            {
                cell.SetCellValue(obj.ToString());
            }
        }

    

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string filenanme = DCpath + "\\" + listBox1.SelectedItem.ToString();
                t1 = ReadExcelToDataTable(filenanme, "Sheet1", true);
                t2 = ReadExcelToDataTable(filenanme, "Sheet2", true);
                dataGridView1.DataSource = t1;
                dataGridView2.DataSource = t2;
            }
          

        }
        /// <summary>
        /// 将excel文件内容读取到DataTable数据表中
        /// </summary>
        /// <param name="fileName">文件完整路径名</param>
        /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名：true=是，false=否</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable ReadExcelToDataTable(string fileName, string sheetName = null, bool isFirstRowColumn = true)
        {
            //定义要返回的datatable对象
            DataTable data = new DataTable();
            //excel工作表
            ISheet sheet = null;
            //数据开始行(排除标题行)
            int startRow = 0;
            try
            {
                if (!File.Exists(fileName))
                {
                    throw new Exception("文件不存在");
                }
                //根据指定路径读取文件
                 FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                //根据文件流创建excel数据结构
                IWorkbook workbook = null;
                var fileType = Path.GetExtension(fileName).ToLower();
                //XSSFWorkbook 适用XLSX格式，HSSFWorkbook 适用XLS格式
                #region 判断Excel版本
                switch (fileType)
                {
                    //.XLSX是07版(或者07以上的)的Office Excel
                    case ".xlsx":
                        workbook = new XSSFWorkbook(fs);
                        break;
                    //.XLS是03版的Office Excel
                    case ".xls":
                        workbook = new HSSFWorkbook(fs);
                        break;
                    default:
                        throw new Exception("Excel文档格式有误");
                }
                #endregion

                //IWorkbook workbook = new HSSFWorkbook(fs);
                //如果有指定工作表名称
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                    //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    if (sheet == null)
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    //如果没有指定的sheetName，则尝试获取第一个sheet
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    //一行最后一个cell的编号 即总的列数
                    int cellCount = firstRow.LastCellNum;
                    //如果第一行是标题列名
                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null || row.FirstCellNum < 0) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();

                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {

                            //if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                            //    dataRow[j] = row.GetCell(j);

                            #region 格式转换 NPOI获取Excel单元格中不同类型的数据
                            ICell cell = row.GetCell(j);
                            if (cell != null)
                            {

                                //获取指定的单元格信息

                                switch (cell.CellType)
                                {
                                    //首先在NPOI中数字和日期都属于Numeric类型
                                    //通过NPOI中自带的DateUtil.IsCellDateFormatted判断是否为时间日期类型
                                    case CellType.Numeric when DateUtil.IsCellDateFormatted(cell):
                                        dataRow[j] = cell.DateCellValue;
                                        break;
                                    case CellType.Numeric:
                                        //其他数字类型
                                        dataRow[j] = cell.NumericCellValue;
                                        break;
                                    //空数据类型
                                    case CellType.Blank:
                                        dataRow[j] = "";
                                        break;
                                    //公式类型
                                    case CellType.Formula:
                                        {
                                            HSSFFormulaEvaluator eva = new HSSFFormulaEvaluator(workbook);
                                            dataRow[j] = eva.Evaluate(cell).StringValue;
                                            break;
                                        }
                                    //布尔类型
                                    case CellType.Boolean:
                                        dataRow[j] = row.GetCell(j).BooleanCellValue;
                                        break;
                                    //错误
                                    case CellType.Error:
                                       // dataRow[j] = HSSF Constants.GetText(row.GetCell(j).ErrorCellValue);
                                        break;
                                    //其他类型都按字符串类型来处理（未知类型CellType.Unknown，字符串类型CellType.String）
                                    default:
                                        dataRow[j] = cell.StringCellValue;
                                        break;
                                }

                            }
                            #endregion
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Datable导出成Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="file">导出路径(包括文件名与扩展名)</param>
        public static void TableToExcel(DataTable dt1, DataTable dt2, string sheetName1, string sheetName2, string file)
        {
            IWorkbook workbook;
            string fileExt = Path.GetExtension(file).ToLower();
            if (fileExt == ".xlsx")
            {
                workbook = new XSSFWorkbook();
            }
            else if (fileExt == ".xls")
            { 
                workbook = new HSSFWorkbook(); 
            } 
            else 
            {
                workbook = null; 
            }
            if (workbook == null)
            {
                return;
            }



            ISheet sheet1 =  workbook.CreateSheet(sheetName1);
            ISheet sheet2 = workbook.CreateSheet(sheetName2);

            //表头  
            IRow row1 = sheet1.CreateRow(0);
            for (int i = 0; i < dt1.Columns.Count; i++)
            {
                ICell cell = row1.CreateCell(i);
                cell.SetCellValue(dt1.Columns[i].ColumnName);
         
            }

            //数据  
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                IRow row2 = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt1.Columns.Count; j++)
                {
                    ICell cell = row2.CreateCell(j);
                    cell.SetCellValue(dt1.Rows[i][j].ToString());
                }
            }

            //表头  
            IRow row3 = sheet2.CreateRow(0);
            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                ICell cell = row3.CreateCell(i);
                cell.SetCellValue(dt2.Columns[i].ColumnName);

            }

            //数据  
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                IRow row4 = sheet2.CreateRow(i + 2);
                for (int j = 0; j < dt2.Columns.Count; j++)
                {
                    ICell cell = row4.CreateCell(j);
                    cell.SetCellValue(dt2.Rows[i][j].ToString());
                }
            }

            //转为字节数组  
            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            var buf = stream.ToArray();
            //Thread.Sleep(1000); 
            //保存为Excel文件  
            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
            }
        }

        public static void sheettoExcel(DataTable dt, IWorkbook workbook, string file, ISheet sheet)
        { }
            private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    string filenanme = DCpath + "\\" + listBox1.SelectedItem.ToString();
                   TableToExcel(t1, t2, "Sheet1" ,"Sheet2", filenanme);
              
                    MessageBox.Show("保存成功！");
                }
            }
            catch (Exception)
            {

                
            }
        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    string filenanme = DCpath + "\\" + listBox1.SelectedItem.ToString();
                    

                    MessageBox.Show("设置成功！");
                }
            }
            catch (Exception)
            {


            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string filenanme = DCpath + "\\" + listBox1.SelectedItem.ToString();

                if (File.Exists(filenanme))
                {
                    //创建文件
                    try
                    {
                        File.Delete(filenanme);

                     
                        DirectoryInfo folder = new DirectoryInfo(DCpath);
                        FileSystemInfo fileinfo1 = folder as FileSystemInfo;
                        List<string> excelname = ListFiles(fileinfo1);
                        excelname.Clear();
                        excelname = ListFiles(fileinfo1);
                        listBox1.Items.Clear();
                        for (int i = 0; i < excelname.Count; i++)
                        {
                            listBox1.Items.Add(excelname[i].ToString());
                        }

                    }
                    catch (Exception)
                    {
                    }
                }

            }
        }

        private void SetModel_Load(object sender, EventArgs e)
        {
            try
            {
                DCpath = IniAPI.INIGetStringValue(Param_Path, "ModelExcel", "Path", "C:\\Users\\Administrator\\Desktop\\6524");
                int cameranum = Convert.ToInt32(IniAPI.INIGetStringValue(Param_Path, "System", "CameraNum", "1"));
               bool robotenabled = Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "System", "RobotEnabled", "false"));
               bool  autochangemodel = Convert.ToBoolean(IniAPI.INIGetStringValue(Param_Path, "System", "AutoChangeModel", "false"));
                radioButton1.Checked = autochangemodel;
                radioButton4.Checked=robotenabled;
                numericUpDown1.Value = cameranum;
                textBox1.Text = DCpath;
                DirectoryInfo folder = new DirectoryInfo(DCpath);
                FileSystemInfo fileinfo1 = folder as FileSystemInfo;
                List<string> excelname = ListFiles(fileinfo1);

                excelname.Clear();
                excelname = ListFiles(fileinfo1);
                listBox1.Items.Clear();
                for (int i = 0; i < excelname.Count; i++)
                {
                    listBox1.Items.Add(excelname[i].ToString());
                }
            }
            catch (Exception)
            {

               // throw;
            }
           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                //string filenanme = DCpath + "\\" + listBox1.SelectedItem.ToString();
                IniAPI.INIWriteValue(Param_Path, "ModelExcel", "UsingExcelPath", listBox1.SelectedItem.ToString());
               
                MessageBox.Show("保存成功！");
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(Param_Path, "System", "AutoChangeModel", radioButton1.Checked.ToString());
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(Param_Path, "System", "AutoChangeModel", radioButton1.Checked.ToString());
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(Param_Path, "System", "CameraNum", numericUpDown1.Value.ToString());
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(Param_Path, "System", "RobotEnabled", radioButton4.Checked.ToString());
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            IniAPI.INIWriteValue(Param_Path, "System", "RobotEnabled", radioButton4.Checked.ToString());
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
