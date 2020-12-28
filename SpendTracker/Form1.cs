using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        int IntervalOfTopTool = 20;
        float[] cellWidthOfRow;

        // 紀錄當前載入的是何年何月的資料，預設為現年現月
        int currentYear = DateTime.Now.Year;
        int currentMonth = DateTime.Now.Month;

        public void ArrangeMainWindow()
        {
            // 取得螢幕的解析度
            string DesktopWidthStr = SystemInformation.PrimaryMonitorSize.Width.ToString();
            string DesktopHeightStr = SystemInformation.PrimaryMonitorSize.Height.ToString();

            // 調整主視窗的大小
            int RelativeWidth = (int)(Convert.ToInt32(DesktopWidthStr) * 0.8);
            int RelativeHeight = (int)(Convert.ToInt32(DesktopHeightStr) * 0.8);
            Size = new Size(RelativeWidth, RelativeHeight);

            // 調整最上方工具列的UI間距
            IntervalOfTopTool = (int)(Size.Width * 0.02);

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
            SelectorOfYear.Location = (Point)new Size(x - (int)(Width * 0.23), y);

            // 預設內容為現在的年份
            SelectorOfYear.Text = DateTime.Now.ToString("yyyy年");

            // 只能選擇最近五年的年份，想查詢更舊的年分必須手動輸入
            SelectorOfYear.Items.Add(currentYear.ToString() + "年");
            SelectorOfYear.Items.Add((currentYear - 1).ToString() + "年");
            SelectorOfYear.Items.Add((currentYear - 2).ToString() + "年");
            SelectorOfYear.Items.Add((currentYear - 3).ToString() + "年");
            SelectorOfYear.Items.Add((currentYear - 4).ToString() + "年");
        }

        public void ArrangeSelectorOfMonth()
        {
            // 設置大小
            SelectorOfMonth.Size = new Size((int)(Size.Width * 0.06), SelectorOfYear.Height);

            // 設置相對位置(水平貼齊年分的選擇器，然後稍微往右移動)
            int x = SelectorOfYear.Location.X + SelectorOfYear.Width + IntervalOfTopTool;
            int y = SelectorOfYear.Location.Y;
            SelectorOfMonth.Location = (Point)new Size(x, y);

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

            // 設置相對位置(水平貼齊月分的選擇器，然後稍微往右移動)
            int x = SelectorOfMonth.Location.X + SelectorOfMonth.Width + IntervalOfTopTool;
            int y = SelectorOfMonth.Location.Y;
            SubmitButton.Location = (Point)new Size(x, y);

            // 預設內容
            SubmitButton.Text = "送出查詢";
        }

        public void ArrangGoBackButton()
        {
            // 設置大小
            GoBackButton.Size = new Size(SubmitButton.Size.Width, SelectorOfYear.Height);

            // 設置相對位置(水平貼齊送出按鈕，然後稍微往右移動)
            int x = SubmitButton.Location.X + SubmitButton.Width + IntervalOfTopTool;
            int y = SubmitButton.Location.Y;
            GoBackButton.Location = (Point)new Size(x, y);

            // 設置文字內容
            GoBackButton.Text = "前半月";
        }

        public void ArrangGoNextButton()
        {
            // 設置大小
            GoNextButton.Size = new Size(SubmitButton.Size.Width, SelectorOfYear.Height);

            // 設置相對位置(水平貼齊前半月的按鈕，然後稍微往右移動)
            int x = GoBackButton.Location.X + GoBackButton.Width + IntervalOfTopTool;
            int y = GoBackButton.Location.Y;
            GoNextButton.Location = (Point)new Size(x, y);

            // 設置文字內容
            GoNextButton.Text = "後半月";
        }

        public void ArrangeSaveButton()
        {
            // 設置大小
            SaveButton.Size = new Size(SubmitButton.Size.Width, SelectorOfYear.Height);

            // 設置相對位置(水平貼齊後半月的按鈕，然後稍微往右移動)
            int x = GoNextButton.Location.X + GoNextButton.Width + IntervalOfTopTool;
            int y = GoNextButton.Location.Y;
            SaveButton.Location = (Point)new Size(x, y);

            // 設置文字內容
            SaveButton.Text = "儲存變更";
        }

        public void ArrangeContainerOfTable()
        {
            // 設置相對大小
            ContainerOfTable.Size = new Size((int)(Size.Width * 0.9), (int)(Size.Height * 0.81));

            // 設置相對位置(x接近置中 & 令表格和上方工具列的間距近似於上方工具列到視窗頂部的間距)
            int x = (int)(Size.Width * 0.495) - (ContainerOfTable.Width) / 2;
            int y = SelectorOfYear.Location.Y + SubmitButton.Height + SelectorOfYear.Location.Y - 6;
            ContainerOfTable.Location = (Point)new Size(x, y);
        }

        public void ArrangeTable()
        {
            // 添加表格到容器 & 令表格填滿容器
            ContainerOfTable.Controls.Add(table);
            table.Dock = DockStyle.Top;

            // 此變數會連動各個欄位的寬度 & 欄位內的控件寬度
            cellWidthOfRow = new float[6]
            {
                table.Width * 0.123f,
                table.Width * 0.123f,
                table.Width * 0.123f,
                table.Width * 0.123f,
                table.Width * 0.123f,
                table.Width * 0.385f,
            };

            // 利用百分比來分配每個欄位的寬度
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cellWidthOfRow[0]));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cellWidthOfRow[1]));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cellWidthOfRow[2]));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cellWidthOfRow[3]));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cellWidthOfRow[4]));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cellWidthOfRow[5]));

            // 創建標題列
            AddTitleBar();

            // 創建資料列
            AddDataRow();

            // 令表格和容器的高度相同
            table.Height = ContainerOfTable.Height;
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
            ArrangGoBackButton();
            ArrangGoNextButton();
            ArrangeSaveButton();
            ArrangeContainerOfTable();
            ArrangeTable();
            ReadDataToList(currentYear, currentMonth);
            WriteDataToTable(DateTime.Now.Day < 17 ? 1 : 2); // 若現在的日期為後半月，則直接呈現後半月的資料
        }

        private void AddTitleBar()
        {
            // 動態新增一行
            table.RowCount++;

            // 設定行高
            int cellHeight = 36;
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, cellHeight));

            // 創建按鈕並將按鈕添加到表格的第一列
            for(int i = 0; i < 6; i++)
            {
                titleBar.Add(new Button
                {
                    Height = cellHeight,
                    Font = new Font("Microsoft JhengHei", 10, FontStyle.Regular),
                    TextAlign = ContentAlignment.MiddleCenter
                });

                // 添加到對應的cell
                table.Controls.Add(titleBar.Last(), i, table.RowCount - 1);
            }

            // 令各按鈕的寬度等於各欄位的寬度
            titleBar[0].Width = (int)(cellWidthOfRow[0]);
            titleBar[1].Width = (int)(cellWidthOfRow[1]);
            titleBar[2].Width = (int)(cellWidthOfRow[2]);
            titleBar[3].Width = (int)(cellWidthOfRow[3]);
            titleBar[4].Width = (int)(cellWidthOfRow[4]);
            titleBar[5].Width = (int)(cellWidthOfRow[5]);

            // 設置各按鈕的文字內容
            titleBar[0].Text = "日期";
            titleBar[1].Text = "伙食費";
            titleBar[2].Text = "交通費";
            titleBar[3].Text = "學雜費";
            titleBar[4].Text = "總花費";
            titleBar[5].Text = "備註";
        }

        private void AddDataRow()
        {
            // 創建 17 列，每列有 6 個 cell
            for (int index = 0; index < 17; index++)
            {
                // 動態新增一行
                table.RowCount++;

                // 設定行高
                int cellHeight = (int)((ContainerOfTable.Height - titleBar[0].Height) / 17);
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, cellHeight));

                // 建立這一列的 cell 並添加到表格
                rowList.Add(new List<TextBox>());
                for (int i = 0; i < 6; i++)
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
                rowList.Last()[0].Width = (int)(cellWidthOfRow[0]);
                rowList.Last()[1].Width = (int)(cellWidthOfRow[1]);
                rowList.Last()[2].Width = (int)(cellWidthOfRow[2]);
                rowList.Last()[3].Width = (int)(cellWidthOfRow[3]);
                rowList.Last()[4].Width = (int)(cellWidthOfRow[4]);
                rowList.Last()[5].Width = (int)(cellWidthOfRow[5]);
            }
        }

        public int GetDayOfTargetMonth(int year, int month)
        {
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
                // 更新當前載入的資料日期
                currentYear = year;
                currentMonth = month;

                // 要裝載某個月的資料前先清空容器
                dailySpends.Clear();

                // 取得該年該月份的天數
                int numOfDay = GetDayOfTargetMonth(year, month);

                // 進入該年的資料夾(若不存在則創建)
                string TargetDir = year.ToString();

                if (!Directory.Exists(TargetDir))
                {
                    Directory.CreateDirectory(TargetDir);
                }

                Directory.SetCurrentDirectory(TargetDir);

                // 開啟該月的檔案(若不存在則創建)
                string monthStr = month.ToString("D2");
                string targetFile = $"{year}-{monthStr}.txt";
                StreamWriter fileForWrite;

                if (!File.Exists(targetFile))
                {
                    // 新增檔案
                    fileForWrite = new StreamWriter(targetFile);

                    // 透過串流寫入每一日的預設資料(資料筆數 = 該月的天數)
                    for (int i = 0; i < numOfDay; i++)
                    {
                        string currentDay = (i+1).ToString("D2");
                        fileForWrite.WriteLine($"{year}-{monthStr}-{currentDay},0,0,0,0,無");
                    }

                    // 關閉檔案
                    fileForWrite.Close();
                }

                // 讀取檔案內容(逐行讀取)
                StreamReader fileForRead = new StreamReader(targetFile);
                string line;
                int Food = 0;
                int Transportation = 0;
                int Other = 0;
                int TotalAmount = 0;

                while ((line = fileForRead.ReadLine()) != null)
                {
                    string[] dataOfDay = line.Split(',');
                    List<string> splitedRemarks = new List<string>();

                    // 備註裡面可能含有逗號，若被切割則必須組裝回去
                    for(int i = 5; i < dataOfDay.Length; i++)
                    {
                        splitedRemarks.Add(dataOfDay[i]);
                    }

                    string remark = string.Join("", splitedRemarks);

                    dailySpends.Add(new DailySpend
                    {
                        Date = dataOfDay[0],
                        Food = Convert.ToInt32(dataOfDay[1]),
                        Transportation = Convert.ToInt32(dataOfDay[2]),
                        Other = Convert.ToInt32(dataOfDay[3]),
                        TotalAmount = Convert.ToInt32(dataOfDay[4]),
                        Remarks = remark
                    });

                    // 累計各項花費
                    Food += dailySpends.Last().Food;
                    Transportation += dailySpends.Last().Transportation;
                    Other += dailySpends.Last().Other;
                    TotalAmount += dailySpends.Last().TotalAmount;
                }

                fileForRead.Close();

                // 紀錄各項花費的加總
                totalSpend.Date = dailySpends[0].Date.Substring(0, 7);
                totalSpend.Food = Food;
                totalSpend.Transportation = Transportation;
                totalSpend.Other = Other;
                totalSpend.TotalAmount = TotalAmount;
                totalSpend.Remarks = "各項花費的加總";
            }
            catch (Exception)
            {
                string monthStr = currentMonth.ToString("D2");
                string targetFile = $"{currentYear}-{monthStr}.txt";
                string ErrorMessage = $"讀取失敗，請到資料夾{currentYear}下查看{targetFile}的資料是否異常!";
                MessageBox.Show(ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                }

                // 將花費加總寫入該頁的最後一列
                rowList[^1][0].Text = totalSpend.Date;
                rowList[^1][1].Text = totalSpend.Food.ToString();
                rowList[^1][2].Text = totalSpend.Transportation.ToString(); ;
                rowList[^1][3].Text = totalSpend.Other.ToString(); ;
                rowList[^1][4].Text = totalSpend.TotalAmount.ToString(); ;
                rowList[^1][5].Text = totalSpend.Remarks;
            }
            catch (Exception)
            {
                string monthStr = currentMonth.ToString("D2");
                string targetFile = $"{currentYear}-{monthStr}.txt";
                string ErrorMessage = $"讀取失敗，請到資料夾{currentYear}下查看{targetFile}的資料是否異常!";
                MessageBox.Show(ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
