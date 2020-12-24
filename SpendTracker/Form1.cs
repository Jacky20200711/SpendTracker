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
        DailySpend totalSpend = new DailySpend();
        List<List<Label>> rowList = new List<List<Label>>();
        int currentPage = 1;

        public void ArrangeForm1()
        {
            // 取得螢幕的寬度
            string DesktopWidthStr = SystemInformation.PrimaryMonitorSize.Width.ToString();

            // 設置適應螢幕的寬度
            int RelativeWidth = (int)(Convert.ToInt32(DesktopWidthStr) * 0.8);

            // 調整主視窗的寬與高(將高度固定比較不容易在別台電腦跑版)
            Size = new Size(RelativeWidth, 690);

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
            comboBox1.Size = new Size((int)(Size.Width * 0.08), comboBox1.Height);

            // 設置相對位置
            int x = (int)(Size.Width * 0.495) - (comboBox1.Width) / 2;
            int y = (int)(Size.Height * 0.03);
            comboBox1.Location = (Point)new Size(x - 140, y);

            // 預設內容
            comboBox1.Text = DateTime.Now.ToString("yyyy年");
        }

        public void ArrangeComboBox2()
        {
            // 設置大小
            comboBox2.Size = new Size((int)(Size.Width * 0.06), comboBox1.Height);

            // 設置相對位置
            int x = (int)(Size.Width * 0.495) - (comboBox1.Width) / 2;
            int y = (int)(Size.Height * 0.03);
            comboBox2.Location = (Point)new Size(x, y);

            // 預設內容
            comboBox2.Text = DateTime.Now.ToString("MM月");
        }

        public void ArrangeSubmitButton()
        {
            // 設置大小
            SubmitButton.Size = new Size((int)(Size.Width * 0.08), comboBox1.Height);

            // 設置相對位置
            int x = (int)(Size.Width * 0.495) - (comboBox1.Width) / 2;
            int y = (int)(Size.Height * 0.03);
            SubmitButton.Location = (Point)new Size(x + 110, y);

            // 預設內容
            SubmitButton.Text = "送出查詢";
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

            // 創建標題列
            AddTitleBar();

            // 創建資料列
            AddDataRow();

            // 令表格和容器的高度相同
            table.Height = panel1.Height;
        }

        public void ArrangePageButton()
        {
            // 設置大小
            GoBackButton.Size = new Size((int)(table.Width * 0.1), comboBox1.Height);
            GoNextButton.Size = new Size((int)(table.Width * 0.1), comboBox1.Height);

            // 設置相對位置
            int x = (int)(Size.Width * 0.495) - (GoBackButton.Width) / 2;
            GoBackButton.Location = (Point)new Size(x - 80, 585);
            GoNextButton.Location = (Point)new Size(x + 80, 585);

            // 設置文字內容
            GoBackButton.Text = "前半月";
            GoNextButton.Text = "後半月";
        }

        // 主視窗的建構子，程序開始時會調用這個函數
        // 所有預設的UI配置和預設讀入的資料，都是透過這個函數
        public Form1()
        {
            InitializeComponent();
            ArrangeForm1();
            ArrangeComboBox1();
            ArrangeComboBox2();
            ArrangeSubmitButton();
            ArrangePanel1();
            ArrangeTable();
            ArrangePageButton();
            
            // 預設讀入現年現月的資料
            ReadDataToList(DateTime.Now.Year, DateTime.Now.Month);

            // 若現在的日期為後半月，則直接顯示第2頁的資料
            WriteDataToTable(DateTime.Now.Day < 17 ? 1 : 2);
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

        private void AddDataRow()
        {
            // 創建 17 列，每列有 7 個cell
            for (int index = 0; index < 17; index++)
            {
                // 動態新增一行
                table.RowCount++;

                // 設定這一行的高度
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
                int cellHeight = 24;

                // 令每個欄位都顯示邊框
                table.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;

                int j = table.RowCount - 1;

                // 建立這一列的 cell 並添加到表格
                rowList.Add(new List<Label>());
                for (int i = 0; i < 7; i++)
                {
                    rowList.Last().Add(new Label
                    {
                        Height = cellHeight,
                        Font = new Font("Microsoft JhengHei", 10, FontStyle.Regular),
                        TextAlign = ContentAlignment.MiddleCenter
                    });
                    table.Controls.Add(rowList.Last().Last(), i, j);
                }

                // 令各cell的寬度等於各欄位的寬度
                rowList.Last()[0].Width = (int)(table.Width * 0.1228);
                rowList.Last()[1].Width = (int)(table.Width * 0.1228);
                rowList.Last()[2].Width = (int)(table.Width * 0.1228);
                rowList.Last()[3].Width = (int)(table.Width * 0.1228);
                rowList.Last()[4].Width = (int)(table.Width * 0.1228);
                rowList.Last()[5].Width = (int)(table.Width * 0.3056);
                rowList.Last()[6].Width = (int)(table.Width * 0.0856);

            }

            // 若用迴圈綁定會有BUG，所以手動綁定 label & click 事件
            rowList[0][6].Click += delegate { GoEditPage(currentPage == 1 ? 0 : 16); };
            rowList[1][6].Click += delegate { GoEditPage(currentPage == 1 ? 1 : 17); };
            rowList[2][6].Click += delegate { GoEditPage(currentPage == 1 ? 2 : 18); };
            rowList[3][6].Click += delegate { GoEditPage(currentPage == 1 ? 3 : 19); };
            rowList[4][6].Click += delegate { GoEditPage(currentPage == 1 ? 4 : 20); };
            rowList[5][6].Click += delegate { GoEditPage(currentPage == 1 ? 5 : 21); };
            rowList[6][6].Click += delegate { GoEditPage(currentPage == 1 ? 6 : 22); };
            rowList[7][6].Click += delegate { GoEditPage(currentPage == 1 ? 7 : 23); };
            rowList[8][6].Click += delegate { GoEditPage(currentPage == 1 ? 8 : 24); };
            rowList[9][6].Click += delegate { GoEditPage(currentPage == 1 ? 9 : 25); };
            rowList[10][6].Click += delegate { GoEditPage(currentPage == 1 ? 10 : 26); };
            rowList[11][6].Click += delegate { GoEditPage(currentPage == 1 ? 11 : 27); };
            rowList[12][6].Click += delegate { GoEditPage(currentPage == 1 ? 12 : 28); };
            rowList[13][6].Click += delegate { GoEditPage(currentPage == 1 ? 13 : 29); };
            rowList[14][6].Click += delegate { GoEditPage(currentPage == 1 ? 14 : 30); };
            rowList[15][6].Click += delegate { GoEditPage(currentPage == 1 ? 15 : 31); };
        }

        public void GoEditPage(int index)
        {
            MessageBox.Show(index.ToString(), "Test");
        }

        public int GetDayOfCurrentMonth()
        {
            // 取得下拉選單的年與月
            // Todo

            // Todo

            // 透過月份取得天數
            // Todo

            // 若是二月且年分為閏年，則修改天數
            // Todo
            return 28;
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

                // 將測試用的假資料寫入類別 & 計算各項花費的加總
                int Food = 0;
                int Transportation = 0;
                int Other = 0;
                int TotalAmount = 0;
                int numOfDay = GetDayOfCurrentMonth();

                for (int i = 0; i < numOfDay; i++)
                {
                    dailySpends.Add(new DailySpend 
                    {
                        Date = "2020-12-23",
                        Food = i+1,
                        Transportation = 123,
                        Other = 456,
                        TotalAmount = 123456,
                        Remarks = "測試測試測試測試測試測試測試測試測試"
                    });

                    Food += dailySpends.Last().Food;
                    Transportation += dailySpends.Last().Transportation;
                    Other += dailySpends.Last().Other;
                    TotalAmount += dailySpends.Last().TotalAmount;
                }

                // 紀錄各項花費的加總結果
                totalSpend.Date = dailySpends[0].Date.Substring(0, 7);
                totalSpend.Food = Food;
                totalSpend.Transportation = Transportation;
                totalSpend.Other = Other;
                totalSpend.TotalAmount = TotalAmount;
                totalSpend.Remarks = "各項花費的加總";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void WriteDataToTable(int ShowPage = 1)
        {
            try
            {
                // 紀錄要呈現的分頁
                currentPage = ShowPage;

                // 第一頁會呈現16筆資料 & 加總
                // 第二頁會呈現(當月的天數 - 16)筆資料 & 加總
                int end = ShowPage == 1 ? 16 : dailySpends.Count - 16;

                // 透過頁數來決定要載入前半月或是後半月的資料到表格
                int index = ShowPage == 1 ? 0 : 16;

                // 將資料寫入表格
                for (int i = 0; i < end; i++)
                {
                    rowList[i][0].Text = dailySpends[index].Date;
                    rowList[i][1].Text = dailySpends[index].Food.ToString();
                    rowList[i][2].Text = dailySpends[index].Transportation.ToString();
                    rowList[i][3].Text = dailySpends[index].Other.ToString();
                    rowList[i][4].Text = dailySpends[index].TotalAmount.ToString();
                    rowList[i][5].Text = dailySpends[index].Remarks;
                    rowList[i][6].Text = "編輯";
                    index++;
                }

                // 清空多餘的日期資料
                for (int i = end; i < rowList.Count - 1; i++)
                {
                    rowList[i][0].Text = "------";
                    rowList[i][1].Text = "------";
                    rowList[i][2].Text = "------";
                    rowList[i][3].Text = "------";
                    rowList[i][4].Text = "------";
                    rowList[i][5].Text = "------";
                    rowList[i][6].Text = "----";
                }

                // 將花費加總寫入該頁的最後一列
                rowList[^1][0].Text = totalSpend.Date;
                rowList[^1][1].Text = totalSpend.Food.ToString();
                rowList[^1][2].Text = totalSpend.Transportation.ToString(); ;
                rowList[^1][3].Text = totalSpend.Other.ToString(); ;
                rowList[^1][4].Text = totalSpend.TotalAmount.ToString(); ;
                rowList[^1][5].Text = totalSpend.Remarks;
                rowList[^1][6].Text = "----";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.PadRight(30, ' '), "Hint", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GoBackButton_Click(object sender, EventArgs e)
        {
            // 重新載入前半個月的資料 & 呈現第一頁
            WriteDataToTable(1);
        }

        private void GoNextButton_Click(object sender, EventArgs e)
        {
            // 重新載入後半個月的資料 & 呈現第一頁
            WriteDataToTable(2);
        }

        // 令所有控件啟用 DoubleBuffering，解決修改表格數據時介面會閃爍並卡住的問題
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
    }
}
