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

        public void ArrangePanel1()
        {
            // 設置相對寬度與高度
            panel1.Size = new Size((int)(Size.Width * 0.9), (int)(Size.Height * 0.7));

            // 設置相對位置
            int x = (int)(Size.Width * 0.495) - (panel1.Width) / 2;
            int y = (int)(Size.Height * 0.09);
            panel1.Location = (Point)new Size(x, y);
        }

        public Form1()
        {
            InitializeComponent();
            ArrangeForm1();
            ArrangeComboBox1();
            ArrangePanel1();
        }

        TableLayoutPanel table = new TableLayoutPanel();

        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.Controls.Add(table);     // 添加表格到容器
            table.Dock = DockStyle.Top;     // 令表格填滿容器

            // 利用百分比來分配每個欄位的寬度
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, table.Width * 0.1228f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, table.Width * 0.1228f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, table.Width * 0.1228f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, table.Width * 0.1228f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, table.Width * 0.1228f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, table.Width * 0.3056f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, table.Width * 0.0856f));

            // 創建並調整標題列
            AddTitleBar();

            // 創建並插入支出資料(預設為當年當月)
            AddDailySpend(DateTime.Now.Year, DateTime.Now.Month);

            // 令表格和容器高度相同
            table.Height = panel1.Height;
        }

        private void AddTitleBar()
        {
            try
            {
                // 動態新增一行
                table.RowCount++;

                // 設定這一行的高度
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

                // 令每個欄位都顯示邊框
                table.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;

                int j = table.RowCount - 1;

                // Height 可以影響字體在格子中的垂直位置，設太大也不會影響顯示
                // Width 可以影響字體在格子中的水平位置
                int horizontalShift = (int)(Size.Width / 4.8);
                int verticalShift = 100;
                int fontSize = 10;

                List<Button> Buttons = new List<Button>();

                for(int i = 0; i < 7; i++)
                {
                    Buttons.Add(new Button
                    {
                        Width = horizontalShift * 2,
                        Height = verticalShift,
                        Font = new Font("Microsoft JhengHei", fontSize, FontStyle.Regular),
                        TextAlign = ContentAlignment.MiddleCenter
                    });
                    table.Controls.Add(Buttons.Last(), i, j);
                }

                Buttons[0].Text = "日期";
                Buttons[1].Text = "伙食費";
                Buttons[2].Text = "交通費";
                Buttons[3].Text = "學雜費";
                Buttons[4].Text = "總花費";
                Buttons[5].Text = "備註";
                Buttons[6].Text = "編輯";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.PadRight(30, ' '), "Hint", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddDailySpend(int year, int month)
        {
            try
            {
                // 預設為第一頁，寫入18筆資料
                for (int day = 1; day <= 18; day++)
                {
                    // 動態新增一行
                    table.RowCount++;

                    // 設定這一行的高度
                    table.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));

                    // 令每個欄位都顯示邊框
                    table.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;

                    int j = table.RowCount - 1;

                    // Height 可以影響字體在格子中的垂直位置，設太大也不會影響顯示
                    // Width 可以影響字體在格子中的水平位置
                    int horizontalShift = (int)(Size.Width / 4.8);
                    int verticalShift = 100;
                    int fontSize = 10;

                    List<Label> labels = new List<Label>();
                    for (int i = 0; i < 7; i++)
                    {
                        labels.Add(new Label
                        {
                            Width = horizontalShift,
                            Height = verticalShift,
                            Font = new Font("Microsoft JhengHei", fontSize, FontStyle.Regular),
                            TextAlign = ContentAlignment.MiddleCenter
                        });
                        table.Controls.Add(labels.Last(), i, j);
                    }

                    // 載入該日期的資料
                    labels[0].Text = "2020-12-21";
                    labels[1].Text = day.ToString();
                    labels[2].Text = "123";
                    labels[3].Text = "123";
                    labels[4].Text = "123";
                    labels[5].Text = "測試測試測試測試測試測試測試測試測試測試";
                    labels[6].Text = "編輯";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.PadRight(30, ' '), "Hint", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
