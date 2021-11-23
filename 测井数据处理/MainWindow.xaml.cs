using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 测井数据处理
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Attr attr = new Attr();
        public ObservableCollection<LoggingParameter> oc = new ObservableCollection<LoggingParameter>();
        public MainWindow()
        {
            InitializeComponent();
            
            this.DataContext = attr;
            this.datagrid.ItemsSource= oc;

        }

        /// <summary>
        /// 计算BI值按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_BI(object sender, RoutedEventArgs e)
        {
            LoggingParameter lp = Utils.receive_parameters(GR.Text, SP.Text, DT.Text, DEN.Text, CNL.Text);
            if (lp == null)
            {
                System.Windows.MessageBox.Show("请输入正确数值！");
            }
            else
            {
                double BI = Utils.compute_BI(lp);
                lp.BI = BI;
                oc.Add(lp);
            }
        }
           

        /// <summary>
        /// 从文件提交数据按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_submit(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "打开数据";
            dialog.Filter = "表格文件(*.xlsx *.txt *.xls)|*.xlsx;*.txt;*.xls|All files(*.*)|*.*"; 
            dialog.Multiselect = false;
            dialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filename = "";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = dialog.FileName;
            }
            try
            {
                if (filename.EndsWith("txt")) { Utils.readTxt(filename, oc); }
                else if (filename.EndsWith("xlsx") || filename.EndsWith("xls")) { Utils.ReadExcel(filename, oc); }
                else
                {
                    if (filename != "")
                    {
                        System.Windows.MessageBox.Show("无法打开此类文件！", "提示");
                    }
                }
            }catch (Exception ex)
            {
                System.Windows.MessageBox.Show("打开失败！","提示");
            }
            
        }


        /// <summary>
        /// 导出数据按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_export(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel 工作簿(*.xlsx)|*.xlsx|Excel 97-2003 工作簿(*.xls)|*.xls|所有文件|*.*";//设置文件类型
            sfd.FileName = "测井数据";//设置默认文件名
            sfd.DefaultExt = "xlsx";//设置默认格式（可以不设）
            sfd.AddExtension = true;//设置自动在文件名中添加扩展名
            //try { 
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                Utils.saveExecl(sfd.FileName, oc);
            }
            //}catch(Exception ex)
            //{
            //    System.Windows.MessageBox.Show("导出数据失败！", "提示");
            //}
        }

        /// <summary>
        /// 清除面板数据按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            oc.Clear();
        }
    }
}
