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
    public partial class MainWindow : Form
    {
        TableLayoutPanel table = new TableLayoutPanel();
        List<Button> titleBar = new List<Button>();
        List<DailySpend> dailySpends = new List<DailySpend>();
        DailySpend totalSpend = new DailySpend();
        List<List<TextBox>> rowList = new List<List<TextBox>>();
        int currentPage = 1;

        public void ArrangeMainWindow()
        {
            // 取得螢幕的解析度
            string DesktopWidthStr = SystemInformation.PrimaryMonitorSize.Width.ToString();
            string DesktopHeightStr = SystemInformation.PrimaryMonitorSize.Height.ToString();

            // 調整主視窗的大小
            int RelativeWidth = (int)(Convert.ToInt32(DesktopWidthStr) * 0.8);
            int RelativeHeight = (int)(Convert.ToInt32(DesktopHeightStr) * 0.8);
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

        public void ArrangeSelectorOfYear()
        {
            // 設置大小
            SelectorOfYear.Size = new Size((int)(Size.Width * 0.08), SelectorOfYear.Height);

            // 設置相對位置
            int x = (int)(Size.Width * 0.495) - (SelectorOfYear.Width) / 2;
            int y = (int)(Size.Height * 0.03);
            SelectorOfYear.Location = (Point)new Size(x - (int)(Width * 0.16), y);

            // 預設內容為現在的年份
            SelectorOfYear.Text = DateTime.Now.ToString("yyyy年");

            // 只能選擇最近三年的年份，想查詢更舊的年分必須手動輸入
            int currentYear = DateTime.Now.Year;
            SelectorOfYear.Items.Add(currentYear.ToString() + "年");
            SelectorOfYear.Items.Add((currentYear - 1).ToString() + "年");
            SelectorOfYear.Items.Add((currentYear - 2).ToString() + "年");
        }

        public void ArrangeSelectorOfMonth()
        {
            // 設置大小
            SelectorOfMonth.Size = new Size((int)(Size.Width * 0.06), SelectorOfYear.Height);

            // 設置相對位置
            int x = (int)(Size.Width * 0.495) - (SelectorOfYear.Width) / 2;
            int y = (int)(Size.Height * 0.03);
            SelectorOfMonth.Location = (Point)new Size(x - (int)(Width * 0.06), y);

            // 預設內容為現在的月份
            SelectorOfMonth.Text = DateTime.Now.ToString("MM月");

            // 可以選擇1~12月
            for(int i = 12; i > 0; i--)
            {
                SelectorOfMonth.Items.Add(i.ToString() + "月");
            }
        }

        public void ArrangeSubmitButton()
        {
            // 設置大小
            SubmitButton.Size = new Size((int)(Size.Width * 0.08), SelectorOfYear.Height);

            // 設置相對位置
            int x = (int)(Size.Width * 0.495) - (SelectorOfYear.Width) / 2;
            int y = (int)(Size.Height * 0.03);
            SubmitButton.Location = (Point)new Size(x + (int)(Width * 0.02), y);

            // 預設內容
            SubmitButton.Text = "送出查詢";
        }

        public void ArrangePageButton()
        {
            // 設置大小
            GoBackButton.Size = new Size((int)(Size.Width * 0.08), SelectorOfYear.Height);
            GoNextButton.Size = new Size((int)(Size.Width * 0.08), SelectorOfYear.Height);

            // 設置相對位置
            int x = (int)(Size.Width * 0.495) - (SelectorOfYear.Width) / 2;
            int y = (int)(Size.Height * 0.03);
            GoBackButton.Location = (Point)new Size(x + (int)(Width * 0.117), y);
            GoNextButton.Location = (Point)new Size(x + (int)(Width * 0.214), y);

            // 設置文字內容
            GoBackButton.Text = "前半月";
            GoNextButton.Text = "後半月";
        }

        public void ArrangeContainerOfTable()
        {
            // 設置相對大小
            ContainerOfTable.Size = new Size((int)(Size.Width * 0.9), (int)(Size.Height * 0.78));

            // 設置相對位置
            int x = (int)(Size.Width * 0.495) - (ContainerOfTable.Width) / 2;
            int y = (int)(Size.Height * 0.089);
            ContainerOfTable.Location = (Point)new Size(x, y);
        }

        public void ArrangeTable()
        {
            // 添加表格到容器 & 令表格填滿容器
            ContainerOfTable.Controls.Add(table);
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
            table.Height = ContainerOfTable.Height;

            // 設置 cell 的邊框
            table.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;
        }

        // 主視窗的建構子，程序開始時會調用這個函數
        // 所有預設的UI配置、表格預設的呈現資料，都是透過這個函數來加載
        public MainWindow()
        {
            InitializeComponent();
            ArrangeMainWindow();
            ArrangeSelectorOfYear();
            ArrangeSelectorOfMonth();
            ArrangeSubmitButton();
            ArrangeContainerOfTable();
            ArrangeTable();
            ArrangePageButton();
            
            // 預設讀入現年現月的資料
            ReadDataToList(DateTime.Now.Year, DateTime.Now.Month);

            // 預設呈現現年現月的資料
            // 若現在的日期為後半月，則直接呈現後半月的資料
            WriteDataToTable(DateTime.Now.Day < 17 ? 1 : 2);
        }

        private void AddTitleBar()
        {
            try
            {
                // 動態新增一行
                table.RowCount++;

                // 設定行高
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

                // 創建按鈕並將按鈕添加到表格的第一列
                for(int i = 0; i < 7; i++)
                {
                    titleBar.Add(new Button
                    {
                        Height = 40,
                        Font = new Font("Microsoft JhengHei", 10, FontStyle.Regular),
                        TextAlign = ContentAlignment.MiddleCenter
                    });

                    // 添加到對應的cell
                    table.Controls.Add(titleBar.Last(), i, table.RowCount - 1);
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
            // 創建 17 列，每列有 7 個 cell
            for (int index = 0; index < 17; index++)
            {
                // 動態新增一行
                table.RowCount++;

                // 設定行高
                int cellHeight = 36;
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, cellHeight));

                // 建立這一列的 cell 並添加到表格
                rowList.Add(new List<TextBox>());
                for (int i = 0; i < 7; i++)
                {
                    rowList.Last().Add(new TextBox
                    {
                        Font = new Font("Microsoft JhengHei", 10, FontStyle.Regular),
                    });

                    // 添加到對應的cell
                    table.Controls.Add(rowList.Last().Last(), i, table.RowCount - 1);

                    // 修改 TextBox 的高度
                    rowList.Last().Last().Multiline = true;
                    rowList.Last().Last().Height = cellHeight;
                }

                // 令各個 cell 的寬度等於各欄位的寬度
                rowList.Last()[0].Width = (int)(table.Width * 0.1228);
                rowList.Last()[1].Width = (int)(table.Width * 0.1228);
                rowList.Last()[2].Width = (int)(table.Width * 0.1228);
                rowList.Last()[3].Width = (int)(table.Width * 0.1228);
                rowList.Last()[4].Width = (int)(table.Width * 0.1228);
                rowList.Last()[5].Width = (int)(table.Width * 0.3056);
                rowList.Last()[6].Width = (int)(table.Width * 0.0856);
            }
        }

        public int GetDayOfCurrentMonth()
        {
            // 取得下拉選單的年與月
            int year = Convert.ToInt32(SelectorOfYear.Text.Substring(0, 4));
            int month = Convert.ToInt32(SelectorOfMonth.Text.Substring(0, 2));

            // 透過月份取得天數
            int[] dayOfMonth = new int[] { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            // 若是2月且為閏年，則將天數修改為29
            if(month == 2)
            {
                if ((year % 4 == 0) && (year % 100 != 0) || (year % 400 == 0))
                {
                    return 29;
                }
            }

            return dayOfMonth[month];
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

                // 取得這個月的天數
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
            // 將前半月的資料寫入表格
            WriteDataToTable(1);
        }

        private void GoNextButton_Click(object sender, EventArgs e)
        {
            // 將後半月的資料寫入表格
            WriteDataToTable(2);
        }

        // 令所有控件啟用 Double Buffering，解決修改表格數據時介面會閃爍並卡住的問題
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            // _ = (string)comboBox1.SelectedItem;
        }
    }
}
