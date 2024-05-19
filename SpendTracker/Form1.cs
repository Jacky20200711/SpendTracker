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
        // 用來儲存當前呈現的月份資料，這個變數可以視為使用者和表格資料之間的API
        // 使用者點選排序後會先排序這個變數，再從這個變數讀取資料到表格
        // 使用者切換頁面時，也是從這個變數讀取對應的資料到表格
        List<DailySpend> dailySpends = new List<DailySpend>();   

        TableLayoutPanel table = new TableLayoutPanel();
        List<Button> titleBar = new List<Button>();
        DailySpend totalSpend = new DailySpend();
        List<List<TextBox>> rowList = new List<List<TextBox>>();
        int currentPage = 1;
        int IntervalOfTopTool = 20;
        int fontSize = 12;
        string projectDir = Directory.GetCurrentDirectory();
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

            // 根據螢幕解析度調整字體
            if(SystemInformation.PrimaryMonitorSize.Width == 1920)
            {
                fontSize = 18;
            }

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
            SelectorOfYear.Text = DateTime.Now.ToString("yyyy");

            // 只能選擇最近五年的年份，想查詢更舊的年分必須手動輸入
            SelectorOfYear.Items.Add(currentYear.ToString());
            SelectorOfYear.Items.Add((currentYear - 1).ToString());
            SelectorOfYear.Items.Add((currentYear - 2).ToString());
            SelectorOfYear.Items.Add((currentYear - 3).ToString());
            SelectorOfYear.Items.Add((currentYear - 4).ToString());
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
            SelectorOfMonth.Text = DateTime.Now.Month.ToString();

            // 可以選擇1~12月
            for(int i = 12; i > 0; i--)
            {
                SelectorOfMonth.Items.Add(i.ToString());
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

            // 設置樣式
            SubmitButton.BackColor = Color.RoyalBlue;
            SubmitButton.ForeColor = Color.White;
            SubmitButton.FlatStyle = FlatStyle.Flat;
            SubmitButton.FlatAppearance.MouseOverBackColor = Color.RoyalBlue;
            SubmitButton.TabStop = false;
            SubmitButton.FlatAppearance.BorderSize = 0;

            // 預設的焦點為年分的選擇器，若不修改則其內容會在程式開啟時被反白
            // 將程式開啟後的焦點轉移到這個按鈕
            SubmitButton.Select();
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

            // 設置樣式
            GoBackButton.BackColor = Color.SeaGreen;
            GoBackButton.ForeColor = Color.White;
            GoBackButton.FlatStyle = FlatStyle.Flat;
            GoBackButton.FlatAppearance.MouseOverBackColor = Color.SeaGreen;
            GoBackButton.TabStop = false;
            GoBackButton.FlatAppearance.BorderSize = 0;
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

            // 設置樣式
            GoNextButton.BackColor = Color.SeaGreen;
            GoNextButton.ForeColor = Color.White;
            GoNextButton.FlatStyle = FlatStyle.Flat;
            GoNextButton.FlatAppearance.MouseOverBackColor = Color.SeaGreen;
            GoNextButton.TabStop = false;
            GoNextButton.FlatAppearance.BorderSize = 0;
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

            // 設置樣式
            SaveButton.BackColor = Color.MediumOrchid;
            SaveButton.ForeColor = Color.White;
            SaveButton.FlatStyle = FlatStyle.Flat;
            SaveButton.FlatAppearance.MouseOverBackColor = Color.MediumOrchid;
            SaveButton.TabStop = false;
            SaveButton.FlatAppearance.BorderSize = 0;
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
                // 使用 Pixel 可以固定字型大小，比較不容易在別的解析度跑版
                titleBar.Add(new Button
                {
                    Height = cellHeight,
                    Font = new Font("Microsoft JhengHei", fontSize, FontStyle.Regular, GraphicsUnit.Pixel),
                    TextAlign = ContentAlignment.MiddleCenter
                });

                // 添加到對應的cell
                table.Controls.Add(titleBar.Last(), i, table.RowCount - 1);
            }

            // 令各按鈕的寬度等於各欄位的寬度
            for(int i = 0; i < titleBar.Count; i++)
            {
                titleBar[i].Width = (int)(cellWidthOfRow[i]);
            }

            // 設置各按鈕的文字內容
            titleBar[0].Text = "日期";
            titleBar[1].Text = "伙食費";
            titleBar[2].Text = "交通費";
            titleBar[3].Text = "學雜費";
            titleBar[4].Text = "總花費";
            titleBar[5].Text = "備註";

            // 綁定按鈕的點擊事件
            titleBar[0].Click += delegate { SortData("Date"); };
            titleBar[1].Click += delegate { SortData("Food"); };
            titleBar[2].Click += delegate { SortData("Transportation"); };
            titleBar[3].Click += delegate { SortData("Other"); };
            titleBar[4].Click += delegate { SortData("TotalAmount"); };

            // 設置樣式
            for (int i = 0; i < titleBar.Count; i++)
            {
                titleBar[i].BackColor = Color.Sienna;
                titleBar[i].ForeColor = Color.White;
                titleBar[i].FlatStyle = FlatStyle.Flat;
                titleBar[i].FlatAppearance.BorderColor = Color.Sienna;
                titleBar[i].FlatAppearance.MouseOverBackColor = Color.Sienna;
            }
        }

        public void SortData(string order)
        {
            // 按照傳入的參數來決定如何排序資料
            switch (order)
            {
                case "Date":
                    dailySpends = dailySpends.OrderBy(d => d.Date).ToList();
                    break;

                case "Food":
                    dailySpends = dailySpends.OrderByDescending(d => d.Food).ToList();
                    break;

                case "Transportation":
                    dailySpends = dailySpends.OrderByDescending(d => d.Transportation).ToList();
                    break;

                case "Other":
                    dailySpends = dailySpends.OrderByDescending(d => d.Other).ToList();
                    break;

                case "TotalAmount":
                    dailySpends = dailySpends.OrderByDescending(d => d.TotalAmount).ToList();
                    break;
            }

            // 將排序後的資料寫入表格 & 預設呈現到第一頁
            WriteDataToTable(1);
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
                    // 使用 Pixel 可以固定字型大小，比較不容易在別的解析度跑版
                    rowList.Last().Add(new TextBox
                    {
                        Font = new Font("Microsoft JhengHei", fontSize, FontStyle.Regular, GraphicsUnit.Pixel),
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

                // 禁止編輯日期欄位 
                rowList.Last()[0].ReadOnly = true;

                // 禁止編輯總花費欄位 
                rowList.Last()[4].ReadOnly = true;
            }

            // 禁止編輯最後一列(用來顯示總花費)
            rowList.Last()[0].ReadOnly = true;
            rowList.Last()[1].ReadOnly = true;
            rowList.Last()[2].ReadOnly = true;
            rowList.Last()[3].ReadOnly = true;
            rowList.Last()[4].ReadOnly = true;
            rowList.Last()[5].ReadOnly = true;
        }

        public int GetNumOfDay(int year, int month)
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
                int numOfDay = GetNumOfDay(year, month);

                // 進入該年的資料夾(若不存在則創建)
                string TargetDir = @$"{projectDir}\{year}";

                if (!Directory.Exists(TargetDir))
                {
                    Directory.CreateDirectory(TargetDir);
                }

                // 開啟該月的檔案(若不存在則創建)
                string monthStr = month.ToString("D2");
                string targetFile = @$"{TargetDir}\{year}-{monthStr}.txt";
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

                while ((line = fileForRead.ReadLine()) != null)
                {
                    string[] dataOfDay = line.Split(',');
                    List<string> splitedRemarks = new List<string>();

                    // 備註裡面可能含有逗號，若被切割則必須組裝回去
                    for(int i = 5; i < dataOfDay.Length; i++)
                    {
                        splitedRemarks.Add(dataOfDay[i]);
                    }

                    string remark = string.Join(",", splitedRemarks);

                    dailySpends.Add(new DailySpend
                    {
                        Date = $"{dataOfDay[0]}({GetWeek(dataOfDay[0])})",
                        Food = Convert.ToInt32(dataOfDay[1]),
                        Transportation = Convert.ToInt32(dataOfDay[2]),
                        Other = Convert.ToInt32(dataOfDay[3]),
                        TotalAmount = Convert.ToInt32(dataOfDay[4]),
                        Remarks = remark
                    });
                }

                fileForRead.Close();

                // 紀錄各項花費的加總
                totalSpend.Date = dailySpends[0].Date.Substring(0, 7);
                totalSpend.Food = dailySpends.Sum(d => d.Food);
                totalSpend.Transportation = dailySpends.Sum(d => d.Transportation);
                totalSpend.Other = dailySpends.Sum(d => d.Other);
                totalSpend.TotalAmount = dailySpends.Sum(d => d.TotalAmount);
                totalSpend.Remarks = "各項花費的加總";
            }
            catch (Exception)
            {
                string monthStr = currentMonth.ToString("D2");
                string targetFile = $"{currentYear}-{monthStr}.txt";
                string ErrorMessage = $"讀取失敗，請到資料夾{currentYear}下查看{targetFile}的資料是否異常!";
                MessageBox.Show(ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private static string GetWeek(string dateStr)
        {
            try
            {
                int year = int.Parse(dateStr.Split('-')[0]);
                int month = int.Parse(dateStr.Split('-')[1]);
                int day = int.Parse(dateStr.Split('-')[2]);
                DateTime date = new DateTime(year, month, day);
                int week = (int)date.DayOfWeek;
                return week switch
                {
                    1 => "一",
                    2 => "二",
                    3 => "三",
                    4 => "四",
                    5 => "五",
                    6 => "六",
                    _ => "日",
                };
            }
            catch
            {
                return "";
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

                // 根據頁數調整UI顯示
                if(currentPage == 2)
                {
                    for (int i = end; i < rowList.Count - 1; i++)
                    {
                        // 標記多餘的日期資料
                        rowList[i][0].Text = "--------------------";
                        rowList[i][1].Text = "--------------------";
                        rowList[i][2].Text = "--------------------";
                        rowList[i][3].Text = "--------------------";
                        rowList[i][4].Text = "--------------------";
                        rowList[i][5].Text = "--------------------------------------------------------------------------";

                        // 禁止編輯多餘的欄位
                        rowList[i][1].ReadOnly = true;
                        rowList[i][2].ReadOnly = true;
                        rowList[i][3].ReadOnly = true;
                        rowList[i][4].ReadOnly = true;
                        rowList[i][5].ReadOnly = true;
                    }
                }
                else
                {
                    // 解禁編輯，讓回到第一頁的使用者可以編輯這些欄位
                    // rowList[i][4]為總花費，保持禁止編輯
                    for (int i = 0; i < end; i++)
                    {
                        rowList[i][1].ReadOnly = false;
                        rowList[i][2].ReadOnly = false;
                        rowList[i][3].ReadOnly = false;
                        rowList[i][5].ReadOnly = false;
                    }
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
                Environment.Exit(0);
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

        private bool IsSpendFieldValid()
        {
            // 取得要檢查的資料筆數
            int numOfData = currentPage == 1 ? 16 : dailySpends.Count - 16;

            for (int i = 0; i < numOfData; i++)
            {
                // 檢查可以編輯的花費
                int EditableSpend = 3;
                for (int j = 1; j <= EditableSpend; j++)
                {
                    // 若欄位為空則補上0
                    if(rowList[i][j].Text.Trim().Length == 0)
                    {
                        rowList[i][j].Text = "0";
                    }
                    else
                    {
                        if (!int.TryParse(rowList[i][j].Text, out _))
                        {
                            MessageBox.Show($"儲存失敗，請檢查 {rowList[i][0].Text} 各項花費的格式!");
                            return false;
                        }
                    }
                }
            }

            // 檢查備註長度
            int remarkMaxLen = 50;
            for (int i = 0; i < numOfData; i++)
            {
                // 若為空則補"無"
                if(rowList[i][^1].Text.Length < 1)
                {
                    rowList[i][^1].Text = "無";
                }
                else if (rowList[i][^1].Text.Length > remarkMaxLen)
                {
                    MessageBox.Show($"儲存失敗，請檢查 {rowList[i][0].Text} 的備註長度(限{remarkMaxLen}字)");
                    return false;
                }
            }

            return true;
        }

        private bool IsComboBoxValueValid()
        {
            // 取得 comboBox 的內容
            string yearStr = SelectorOfYear.Text;
            string monthStr = SelectorOfMonth.Text;

            // 設置年份範圍
            int minYear = 2000;
            int maxYear = 9999;

            // 檢查輸入的年份
            if (int.TryParse(yearStr, out _))
            {
                int year = int.Parse(yearStr);
                if(year < minYear || year > maxYear)
                {
                    MessageBox.Show($"查詢失敗，輸入的年份必須是{minYear}~{maxYear}");
                    return false;
                }
            }
            else
            {
                MessageBox.Show($"查詢失敗，輸入的年份必須是{minYear}~{maxYear}");
                return false;
            }

            // 檢查輸入的月份
            if (int.TryParse(monthStr, out _))
            {
                int month = int.Parse(monthStr);
                if (month < 1 || month > 12)
                {
                    MessageBox.Show($"查詢失敗，輸入的月份必須是1~12");
                    return false;
                }
            }
            else
            {
                MessageBox.Show($"查詢失敗，輸入的月份必須是1~12");
                return false;
            }

            return true;
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (IsComboBoxValueValid())
            {
                ReadDataToList(int.Parse(SelectorOfYear.Text), int.Parse(SelectorOfMonth.Text));
                WriteDataToTable(1);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (IsSpendFieldValid())
            {
                // 讀取當前分頁的表格資料
                int numOfData = currentPage == 1 ? 16 : dailySpends.Count - 16;
                int NewDataStart = currentPage == 1 ? 0 : 16;

                for (int i = 0; i < numOfData; i++, NewDataStart++)
                {
                    // 更新串列中，對應各日期欄位的資料
                    dailySpends[NewDataStart].Date = rowList[i][0].Text;
                    dailySpends[NewDataStart].Food = Convert.ToInt32(rowList[i][1].Text);
                    dailySpends[NewDataStart].Transportation = Convert.ToInt32(rowList[i][2].Text);
                    dailySpends[NewDataStart].Other = Convert.ToInt32(rowList[i][3].Text);
                    dailySpends[NewDataStart].TotalAmount = dailySpends[NewDataStart].Food + dailySpends[NewDataStart].Transportation + dailySpends[NewDataStart].Other;
                    dailySpends[NewDataStart].Remarks = rowList[i][5].Text;
                }

                // 複製該串列 & 使用日期排序
                List<DailySpend> NewData = dailySpends.OrderBy(d => d.Date).ToList();

                // 開啟檔案
                string monthStr = currentMonth.ToString("D2");
                string targetFile = @$"{projectDir}\{currentYear}\{currentYear}-{monthStr}.txt";
                StreamWriter file = new StreamWriter(targetFile);

                // 將更新過並依照日期排序的月份資料，寫入到對應的檔案
                for (int i = 0; i < NewData.Count; i++)
                {
                    file.WriteLine($"{NewData[i].Date},{NewData[i].Food},{NewData[i].Transportation},{NewData[i].Other},{NewData[i].TotalAmount},{NewData[i].Remarks}");
                }

                // 關閉檔案
                file.Close();

                // 重新計算各項花費的加總
                totalSpend.Food = dailySpends.Sum(d => d.Food);
                totalSpend.Transportation = dailySpends.Sum(d => d.Transportation);
                totalSpend.Other = dailySpends.Sum(d => d.Other);
                totalSpend.TotalAmount = dailySpends.Sum(d => d.TotalAmount);

                // 將更新後的類別資料寫入表格
                WriteDataToTable(currentPage);
            }
        }
    }
}
