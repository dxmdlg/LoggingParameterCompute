using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 测井数据处理
{

    /// <summary>
    /// 工具类
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// 处理接收的参数，把合法参数转成对象返回
        /// </summary>
        /// <param name="GR"></param>
        /// <param name="SP"></param>
        /// <param name="DT"></param>
        /// <param name="DEN"></param>
        /// <param name="CNL"></param>
        /// <returns></returns>
        public static LoggingParameter receive_parameters(String GR, String SP, String DT, String DEN, String CNL)
        {
            double gr, sp, dt, den, cnl;
            bool b_gr = double.TryParse(GR, out gr);
            bool b_sp = double.TryParse(SP, out sp);
            bool b_dt = double.TryParse(DT, out dt);
            bool b_den = double.TryParse(DEN, out den);
            bool b_cnl = double.TryParse(CNL, out cnl);
            if (!(b_gr && b_sp && b_dt && b_den && b_cnl))
            {
                return null;
            }
            return new LoggingParameter(gr, sp, dt, den, cnl);
        }

        /// <summary>
        /// 输入lp参数计算出BI值得
        /// </summary>
        /// <param name="lp"></param>
        /// <returns></returns>
        public static double compute_BI(LoggingParameter lp)
        {
            double[] lp_pac = new double[3];
            lp_pac = pca(lp);//使用pac降维
            double BI = input_model(lp_pac);//返回模型计算的BI值
            return BI;
        }

        /// <summary>
        /// PCA方法降维 ，待实现
        /// </summary>
        /// <param name="lp"></param>
        /// <returns>PCA处理结果</returns>
        private static double[] pca(LoggingParameter lp)
        {
            double[] lp_pca = new double[3];
            double[] lp_p = new double[5] { lp.GR, lp.SP, lp.DT, lp.DEN, lp.CNL };
            PCA pca = new PCA();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    lp_pca[i] += pca.Matrix[i][j] * lp_p[j];
                }
            }
            return lp_pca;
        }

        /// <summary>
        /// 输入参数得出BI值，待实现
        /// </summary>
        /// <param name="lp_pca"></param>
        /// <returns></returns>
        private static double input_model(double[] lp_pca)
        {
            return (lp_pca[0] + lp_pca[1] + lp_pca[2]) / 3.0;
        }

        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="oc"></param>
        public static void ReadExcel(String strFileName, ObservableCollection<LoggingParameter> oc)
        {
            List<LoggingParameter> list = new List<LoggingParameter>();
            string[] p = new string[5];
            LoggingParameter lp = null;
            Workbook book = new Workbook(strFileName);
            Worksheet sheet = book.Worksheets[0];
            Cells cells = sheet.Cells;
            DataTable dt = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    p[j] = dt.Rows[i][j].ToString();
                }
                lp = receive_parameters(p[0], p[1], p[2], p[3], p[4]);
                if (lp != null)
                {
                    lp.BI = compute_BI(lp);
                    oc.Add(lp);
                }
            }
            System.Windows.MessageBox.Show("计算完毕!共" + oc.Count + "条数据", "提示");
        }

        /// <summary>
        /// 读取txt文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="oc"></param>
        public static void readTxt(String filename, ObservableCollection<LoggingParameter> oc)
        {
            List<LoggingParameter> list = new List<LoggingParameter>();
            FileStream fs = new FileStream(filename, FileMode.Open);
            StreamReader sr = new StreamReader(fs); // 读文件
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] data = line.Split('\t');
                LoggingParameter lp = Utils.receive_parameters(data[0], data[1], data[2], data[3], data[4]);
                if (lp != null)
                {
                    lp.BI = compute_BI(lp);
                    oc.Add(lp);
                }
            }
            list.Clear();
            fs.Close();
            sr.Close();
            System.Windows.MessageBox.Show("计算完毕!共" + oc.Count + "条数据", "提示");
        }

        /// <summary>
        /// 保存为Excel
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="oc"></param>
        public static void saveExecl(string filename, ObservableCollection<LoggingParameter> oc)
        {
            string license = @"C:\Users\19870\Desktop\vs2022work\测井数据处理\测井数据处理\resource\License.lic";
            //string license = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Aspose.Total.lic";
            Aspose.Cells.License license4 = new Aspose.Cells.License();
            license4.SetLicense(license);
            Workbook book = new Workbook();
            Worksheet sheet = book.Worksheets[0];
            Cells cells = sheet.Cells;
            List<string> header = new List<string> { "GR", "SP", "DT", "DEN", "CNL", "BI" };
            System.Data.DataTable table = new DataTable("测井数据");
            DataColumn column;
            DataRow row;
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.ColumnName = "GR";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.ColumnName = "SP";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.ColumnName = "DT";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.ColumnName = "DEN";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.ColumnName = "CNL";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.ColumnName = "BI";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            for (int i = 0; i < oc.Count; i++)
            {
                table.Rows.Add();
                table.Rows[i][0] = oc[i].GR;
                table.Rows[i][1] = oc[i].SP;
                table.Rows[i][2] = oc[i].DT;
                table.Rows[i][3] = oc[i].DEN;
                table.Rows[i][4] = oc[i].CNL;
                table.Rows[i][5] = oc[i].BI;
            }
            //列头样式
            Style styleHeader = book.CreateStyle();//新增样式 
            styleHeader.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            styleHeader.Font.Name = "宋体";//文字字体 
            styleHeader.Font.Size = 14;//文字大小 
            styleHeader.Font.IsBold = true;//粗体 

            //普通单元格样式
            Style styleContent = book.CreateStyle();//新增样式 
            styleContent.HorizontalAlignment = TextAlignmentType.Left;//文字靠左
            styleContent.Font.Name = "宋体";//文字字体 
            styleContent.Font.Size = 12;//文字大小 

            //存数据
            int headerNum = 0;//当前表头所在列
            foreach (string item in header)
            {
                cells[0, headerNum].PutValue(item);
                cells[0, headerNum].SetStyle(styleHeader);
                cells.SetColumnWidthPixel(headerNum, 100);//设置单元格200宽度
                cells.SetRowHeight(0, 30);//第一行，30px高
                headerNum++;
            }
            for (int i = 0; i < table.Rows.Count; i++)
            {
                cells.SetRowHeight(1 + i, 24);
                for (int j = 0; j < 6; j++)
                {
                    cells[1 + i, j].PutValue(table.Rows[i][j]);
                    cells[1 + i, j].SetStyle(styleContent);
                }
            }
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding1 = Encoding.GetEncoding(1252);
            Console.WriteLine(encoding1.WebName);
            Encoding encoding2 = Encoding.GetEncoding("GB2312");
            book.Save(filename);
            System.Windows.MessageBox.Show("导出数据成功", "提示");
        }
    }
}
