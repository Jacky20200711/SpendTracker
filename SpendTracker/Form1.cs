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

        public void ArrangeForm1()
        {
            // 取得螢幕的寬度
            string DesktopWidthStr = SystemInformation.PrimaryMonitorSize.Width.ToString();

            // 設置適應螢幕的寬度
            int RelativeWidth = (int)(Convert.ToInt32(DesktopWidthStr) * 0.8);

            // 調整主視窗的寬與高(將高度固定比較不容易在別台電腦跑版)
            Size = new Size(RelativeWidth, 670);

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
            comboBox1.Size = new Size(93, comboBox1.Height);

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
            ArrangePageButton();
            ReadDataToList(DateTime.Now.Year, DateTime.Now.Month); // 預設讀入現年現月的資料
            //WriteDataToTable();
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

                // 將測試用的假資料寫入類別 & 計算各項花費的加總
                int Food = 0;
                int Transportation = 0;
                int Other = 0;
                int TotalAmount = 0;

                for (int i = 0; i < 31; i++)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void WriteDataToTable(int CurrentPage = 1)
        {
            try
            {
                //int startIndex = CurrentPage == 1 ? 0 : 16;

                //// 每一頁呈現17筆資料(第17筆為花費加總)
                //for (int index = startIndex; index < startIndex+17; index++)
                //{
                //    // 令各cell的寬度等於各欄位的寬度
                //    labels[0].Width = (int)(table.Width * 0.1228);
                //    labels[1].Width = (int)(table.Width * 0.1228);
                //    labels[2].Width = (int)(table.Width * 0.1228);
                //    labels[3].Width = (int)(table.Width * 0.1228);
                //    labels[4].Width = (int)(table.Width * 0.1228);
                //    labels[5].Width = (int)(table.Width * 0.3056);
                //    labels[6].Width = (int)(table.Width * 0.0856);

                //    // 將對應日期的資料寫入到各個cell
                //    labels[0].Text = dailySpends[index].Date;
                //    labels[1].Text = dailySpends[index].Food.ToString();
                //    labels[2].Text = dailySpends[index].Transportation.ToString();
                //    labels[3].Text = dailySpends[index].Other.ToString();
                //    labels[4].Text = dailySpends[index].TotalAmount.ToString();
                //    labels[5].Text = dailySpends[index].Remarks;
                //    labels[6].Text = "編輯";

                //    // 將編輯用的cell連動到類別對應的資料，以方便之後抓取按鈕事件
                //    dailySpends[index].EditField = labels[6];

                //    // 最後一列用來呈現這個月的加總結果
                //    if(index == startIndex + 16)
                //    {
                //        labels[0].Text = totalSpend.Date;
                //        labels[1].Text = totalSpend.Food.ToString();
                //        labels[2].Text = totalSpend.Transportation.ToString();
                //        labels[3].Text = totalSpend.Other.ToString();
                //        labels[4].Text = totalSpend.TotalAmount.ToString();
                //        labels[5].Text = "這個月的各項加總";
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.PadRight(30, ' '), "Hint", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ArrangePageButton()
        {
            // 設置大小
            GoBackButton.Size = new Size((int)(table.Width * 0.1), 24);
            GoNextButton.Size = new Size((int)(table.Width * 0.1), 24);

            // 設置相對位置
            int x = (int)(Size.Width * 0.495) - (GoBackButton.Width) / 2;
            GoBackButton.Location = (Point)new Size(x - 60, 585);
            GoNextButton.Location = (Point)new Size(x + 60, 585);

            // 設置文字內容
            GoBackButton.Text = "上一頁";
            GoNextButton.Text = "下一頁";
        }

        private void GoBackButton_Click(object sender, EventArgs e)
        {
            // 寫入前半個月的資料
            WriteDataToTable(1);
        }

        private void GoNextButton_Click(object sender, EventArgs e)
        {
            // 寫入後半個月的資料
            WriteDataToTable(2);
        }
    }
}
