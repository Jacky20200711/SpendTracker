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
        TableLayoutPanel table = new TableLayoutPanel();
        List<Button> titleBar = new List<Button>();
        List<DailySpend> dailySpends = new List<DailySpend>();

        public void ArrangeForm1()
        {
            // 取得螢幕的寬度
            string DesktopWidthStr = SystemInformation.PrimaryMonitorSize.Width.ToString();

            // 設置適應螢幕的寬度
            int RelativeWidth = (int)(Convert.ToInt32(DesktopWidthStr) * 0.8);

            // 調整主視窗的寬與高(將高度固定比較不容易在別台電腦跑版)
            Size = new Size(RelativeWidth, 700);

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
            // 設置大小
            comboBox1.Size = new Size(94, comboBox1.Height);

            // 設置相對位置
            int x = (int)(Size.Width * 0.495) - (comboBox1.Width) / 2;
            comboBox1.Location = (Point)new Size(x, 23);

            // 預設內容
            comboBox1.Text = DateTime.Now.ToString("yyyy年MM月");
        }

        public void ArrangePanel1()
        {
            // 設置相對寬度 & 將高度固定比較不容易跑版
            panel1.Size = new Size((int)(Size.Width * 0.9), 504);

            // 設置相對位置
            int x = (int)(Size.Width * 0.495) - (panel1.Width) / 2;
            int y = (int)(Size.Height * 0.09);
            panel1.Location = (Point)new Size(x, y);
        }

        public void ArrangeTable()
        {
            // 添加表格到容器 & 令表格填滿容器
            panel1.Controls.Add(table);
            table.Dock = DockStyle.Top;

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

            // 載入現年現月的資料
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;
            ReadDataToList(currentYear, currentMonth);

            // 將現年現月的資料寫入表格
            WriteDataToTable();

            // 令表格和容器的高度相同
            table.Height = panel1.Height;
        }

        // 主視窗的建構子，程序開始時會調用這個函數
        // 我們可以在這個函數內配置各個控制元件
        public Form1()
        {
            InitializeComponent();
            ArrangeForm1();
            ArrangeComboBox1();
            ArrangePanel1();
            ArrangeTable();
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

                // 創建按鈕並將按鈕添加到表格的第一列
                for(int i = 0; i < 7; i++)
                {
                    titleBar.Add(new Button
                    {
                        Height = 40,
                        Font = new Font("Microsoft JhengHei", 10, FontStyle.Regular),
                        TextAlign = ContentAlignment.MiddleCenter
                    });
                    table.Controls.Add(titleBar.Last(), i, j);
                }

                // 令各按鈕的寬度等於各欄位的寬度
                titleBar[0].Width = (int)(table.Width * 0.1228);
                titleBar[1].Width = (int)(table.Width * 0.1228);
                titleBar[2].Width = (int)(table.Width * 0.1228);
                titleBar[3].Width = (int)(table.Width * 0.1228);
                titleBar[4].Width = (int)(table.Width * 0.1228);
                titleBar[5].Width = (int)(table.Width * 0.3056);
                titleBar[6].Width = (int)(table.Width * 0.0856);

                // 設置各按鈕的文字內容
                titleBar[0].Text = "日期";
                titleBar[1].Text = "伙食費";
                titleBar[2].Text = "交通費";
                titleBar[3].Text = "學雜費";
                titleBar[4].Text = "總花費";
                titleBar[5].Text = "備註";
                titleBar[6].Text = "編輯";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.PadRight(30, ' '), "Hint", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReadDataToList(int year, int month)
        {
            try
            {
                // 要裝載某個月的資料前先清空容器
                dailySpends.Clear();

                // 進入當前年月的資料夾(若該資料夾不存在則創建)
                // Todo ...

                // 取得各個檔名並將檔名排序(若該檔案不存在則創建)
                // Todo ...

                // 取得各個檔名並將檔名排序
                // Todo ...

                // 讀取檔案並寫入類別
                // Todo ...

                // 將測試用的假資料寫入類別
                for (int i = 0; i <= 17; i++)
                {
                    dailySpends.Add(new DailySpend 
                    {
                        Date = "2020-12-22",
                        Food = i+1,
                        Transportation = 123,
                        Other = 456,
                        TotalAmount = 123456,
                        Remarks = "測試測試測試測試測試測試測試測試測試",
                        EditField = new Label()
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void WriteDataToTable()
        {
            try
            {
                for (int index = 0; index < dailySpends.Count; index++)
                {
                    // 動態新增一行
                    table.RowCount++;

                    // 設定這一行的高度
                    table.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
                    int cellHeight = 24;

                    // 令每個欄位都顯示邊框
                    table.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;

                    int j = table.RowCount - 1;


                    // 建立這筆資料的各個cell並添加到表格
                    List<Label> labels = new List<Label>();
                    for (int i = 0; i < 7; i++)
                    {
                        labels.Add(new Label
                        {
                            Height = cellHeight,
                            Font = new Font("Microsoft JhengHei", 10, FontStyle.Regular),
                            TextAlign = ContentAlignment.MiddleCenter
                        });
                        table.Controls.Add(labels.Last(), i, j);
                    }

                    // 令各cell的寬度等於各欄位的寬度
                    labels[0].Width = (int)(table.Width * 0.1228);
                    labels[1].Width = (int)(table.Width * 0.1228);
                    labels[2].Width = (int)(table.Width * 0.1228);
                    labels[3].Width = (int)(table.Width * 0.1228);
                    labels[4].Width = (int)(table.Width * 0.1228);
                    labels[5].Width = (int)(table.Width * 0.3056);
                    labels[6].Width = (int)(table.Width * 0.0856);

                    // 將對應日期的資料寫入到各個cell
                    labels[0].Text = dailySpends[index].Date;
                    labels[1].Text = dailySpends[index].Food.ToString();
                    labels[2].Text = dailySpends[index].Transportation.ToString();
                    labels[3].Text = dailySpends[index].Other.ToString();
                    labels[4].Text = dailySpends[index].TotalAmount.ToString();
                    labels[5].Text = dailySpends[index].Remarks;
                    labels[6].Text = "編輯";

                    // 將編輯用的cell連動到類別對應的資料，以方便之後抓取按鈕事件
                    dailySpends[index].EditField = labels[6];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.PadRight(30, ' '), "Hint", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
