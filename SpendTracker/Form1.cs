using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpendTracker
{
    public partial class Form1 : Form
    {
        public void ArrangeForm1()
        {
            // 取得螢幕解析度
            string DesktopWidthStr = SystemInformation.PrimaryMonitorSize.Width.ToString();
            string DesktopHeightStr = SystemInformation.PrimaryMonitorSize.Height.ToString();

            // 計算主視窗對應該解析度的寬與高
            int RelativeWidth = (int)(Convert.ToInt32(DesktopWidthStr) * 0.8);
            int RelativeHeight = (int)(Convert.ToInt32(DesktopHeightStr) * 0.8);

            // 調整主視窗的大小
            Size = new Size(RelativeWidth, RelativeHeight);

            // 令主視窗居中顯示
            int x = (SystemInformation.WorkingArea.Width - Size.Width) / 2;
            int y = (SystemInformation.WorkingArea.Height - Size.Height) / 2;
            StartPosition = FormStartPosition.Manual;
            Location = (Point)new Size(x, y);

            // 禁止視窗最大化
            MaximizeBox = false;

            // 禁止使用者手動延伸視窗邊界
            FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        public void ArrangeComboBox1()
        {
            // 設置相對寬度
            comboBox1.Size = new Size((int)(Size.Width * 0.2), comboBox1.Height);

            // 設置相對位置
            int x = (int)(Size.Width * 0.495) - (comboBox1.Width) / 2;
            int y = (int)(Size.Height * 0.03);
            comboBox1.Location = (Point)new Size(x, y);
        }

        public Form1()
        {
            InitializeComponent();
            ArrangeForm1();
            ArrangeComboBox1();
        }
    }
}
