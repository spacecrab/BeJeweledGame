namespace BeJeweledGame
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pnl_Board = new Panel();
            pnl_HighScore = new Panel();
            pnl_NewScore = new Panel();
            lbl_Sorry = new Label();
            lbl_Congratulation = new Label();
            lbl_Name = new Label();
            label7 = new Label();
            box_Name = new TextBox();
            btn_Save = new Button();
            pnl_Score = new Panel();
            label9 = new Label();
            pnl_StartGame = new Panel();
            panel2 = new Panel();
            label3 = new Label();
            lbl_ScoreValue = new Label();
            panel3 = new Panel();
            combo_count = new ComboBox();
            combo_time = new ComboBox();
            label2 = new Label();
            combo_bonus = new ComboBox();
            label1 = new Label();
            combo_height = new ComboBox();
            label5 = new Label();
            combo_width = new ComboBox();
            label4 = new Label();
            label6 = new Label();
            label8 = new Label();
            btn_GameOver = new Button();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            pnl_GameName = new Panel();
            label10 = new Label();
            TimerBackground = new Button();
            Timer = new Button();
            btn_Switch = new Button();
            pnl_Control = new Panel();
            button5 = new Button();
            pnl_HighScore.SuspendLayout();
            pnl_NewScore.SuspendLayout();
            pnl_StartGame.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            pnl_GameName.SuspendLayout();
            pnl_Control.SuspendLayout();
            SuspendLayout();
            // 
            // pnl_Board
            // 
            pnl_Board.Anchor = AnchorStyles.None;
            pnl_Board.BackColor = SystemColors.ActiveCaption;
            pnl_Board.Location = new Point(403, 26);
            pnl_Board.Name = "pnl_Board";
            pnl_Board.Size = new Size(850, 850);
            pnl_Board.TabIndex = 0;
            // 
            // pnl_HighScore
            // 
            pnl_HighScore.Controls.Add(pnl_NewScore);
            pnl_HighScore.Controls.Add(pnl_Score);
            pnl_HighScore.Controls.Add(label9);
            pnl_HighScore.Location = new Point(118, 184);
            pnl_HighScore.Name = "pnl_HighScore";
            pnl_HighScore.Size = new Size(335, 607);
            pnl_HighScore.TabIndex = 6;
            // 
            // pnl_NewScore
            // 
            pnl_NewScore.BackColor = Color.Gainsboro;
            pnl_NewScore.Controls.Add(lbl_Sorry);
            pnl_NewScore.Controls.Add(lbl_Congratulation);
            pnl_NewScore.Controls.Add(lbl_Name);
            pnl_NewScore.Controls.Add(label7);
            pnl_NewScore.Controls.Add(box_Name);
            pnl_NewScore.Controls.Add(btn_Save);
            pnl_NewScore.Location = new Point(19, 374);
            pnl_NewScore.Name = "pnl_NewScore";
            pnl_NewScore.Size = new Size(295, 219);
            pnl_NewScore.TabIndex = 11;
            // 
            // lbl_Sorry
            // 
            lbl_Sorry.AutoSize = true;
            lbl_Sorry.Location = new Point(32, 109);
            lbl_Sorry.Name = "lbl_Sorry";
            lbl_Sorry.Size = new Size(195, 40);
            lbl_Sorry.TabIndex = 12;
            lbl_Sorry.Text = "You didn't make it to top 5\r\nGood luck next try.";
            // 
            // lbl_Congratulation
            // 
            lbl_Congratulation.AutoSize = true;
            lbl_Congratulation.Location = new Point(32, 109);
            lbl_Congratulation.Name = "lbl_Congratulation";
            lbl_Congratulation.Size = new Size(115, 20);
            lbl_Congratulation.TabIndex = 11;
            lbl_Congratulation.Text = "Congratulation";
            // 
            // lbl_Name
            // 
            lbl_Name.AutoSize = true;
            lbl_Name.Location = new Point(32, 109);
            lbl_Name.Name = "lbl_Name";
            lbl_Name.Size = new Size(177, 20);
            lbl_Name.TabIndex = 10;
            lbl_Name.Text = "PLease enter your name";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Nunito", 12F, FontStyle.Bold);
            label7.Location = new Point(25, 21);
            label7.Name = "label7";
            label7.Size = new Size(67, 28);
            label7.TabIndex = 8;
            label7.Text = "Name";
            // 
            // box_Name
            // 
            box_Name.Font = new Font("Nunito", 12F, FontStyle.Bold);
            box_Name.Location = new Point(34, 67);
            box_Name.Name = "box_Name";
            box_Name.Size = new Size(223, 35);
            box_Name.TabIndex = 7;
            // 
            // btn_Save
            // 
            btn_Save.BackColor = SystemColors.ScrollBar;
            btn_Save.FlatAppearance.BorderSize = 0;
            btn_Save.FlatStyle = FlatStyle.Flat;
            btn_Save.Font = new Font("Nunito", 11F, FontStyle.Bold);
            btn_Save.Location = new Point(34, 156);
            btn_Save.Name = "btn_Save";
            btn_Save.Size = new Size(223, 39);
            btn_Save.TabIndex = 9;
            btn_Save.Text = "Save";
            btn_Save.UseVisualStyleBackColor = false;
            btn_Save.Click += btn_Save_Click;
            // 
            // pnl_Score
            // 
            pnl_Score.BackColor = Color.Gainsboro;
            pnl_Score.Location = new Point(19, 126);
            pnl_Score.Name = "pnl_Score";
            pnl_Score.Size = new Size(295, 232);
            pnl_Score.TabIndex = 10;
            // 
            // label9
            // 
            label9.Anchor = AnchorStyles.Top;
            label9.AutoSize = true;
            label9.Font = new Font("Nunito", 20F, FontStyle.Bold);
            label9.Location = new Point(68, 55);
            label9.Name = "label9";
            label9.Size = new Size(193, 47);
            label9.TabIndex = 6;
            label9.Text = "High Score";
            // 
            // pnl_StartGame
            // 
            pnl_StartGame.BackColor = SystemColors.Control;
            pnl_StartGame.BorderStyle = BorderStyle.FixedSingle;
            pnl_StartGame.Controls.Add(panel2);
            pnl_StartGame.Controls.Add(panel3);
            pnl_StartGame.Controls.Add(label8);
            pnl_StartGame.Controls.Add(btn_GameOver);
            pnl_StartGame.Controls.Add(button1);
            pnl_StartGame.Controls.Add(button2);
            pnl_StartGame.Controls.Add(button3);
            pnl_StartGame.Controls.Add(button4);
            pnl_StartGame.Location = new Point(24, 294);
            pnl_StartGame.Name = "pnl_StartGame";
            pnl_StartGame.Size = new Size(335, 697);
            pnl_StartGame.TabIndex = 7;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Gainsboro;
            panel2.Controls.Add(label3);
            panel2.Controls.Add(lbl_ScoreValue);
            panel2.Location = new Point(19, 613);
            panel2.Name = "panel2";
            panel2.Size = new Size(295, 66);
            panel2.TabIndex = 11;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Nunito ExtraBold", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(34, 14);
            label3.Name = "label3";
            label3.Size = new Size(106, 38);
            label3.TabIndex = 4;
            label3.Text = "Score :";
            // 
            // lbl_ScoreValue
            // 
            lbl_ScoreValue.AutoSize = true;
            lbl_ScoreValue.Font = new Font("Nunito", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_ScoreValue.Location = new Point(168, 14);
            lbl_ScoreValue.Name = "lbl_ScoreValue";
            lbl_ScoreValue.Size = new Size(86, 38);
            lbl_ScoreValue.TabIndex = 5;
            lbl_ScoreValue.Text = "score";
            // 
            // panel3
            // 
            panel3.BackColor = Color.Gainsboro;
            panel3.Controls.Add(combo_count);
            panel3.Controls.Add(combo_time);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(combo_bonus);
            panel3.Controls.Add(label1);
            panel3.Controls.Add(combo_height);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(combo_width);
            panel3.Controls.Add(label4);
            panel3.Controls.Add(label6);
            panel3.Location = new Point(19, 125);
            panel3.Name = "panel3";
            panel3.Size = new Size(295, 232);
            panel3.TabIndex = 11;
            // 
            // combo_count
            // 
            combo_count.Font = new Font("Segoe UI", 14F);
            combo_count.FormattingEnabled = true;
            combo_count.Items.AddRange(new object[] { "2", "3", "4", "5", "6", "7" });
            combo_count.Location = new Point(132, 76);
            combo_count.Name = "combo_count";
            combo_count.Size = new Size(75, 39);
            combo_count.TabIndex = 2;
            // 
            // combo_time
            // 
            combo_time.Font = new Font("Segoe UI", 14F);
            combo_time.FormattingEnabled = true;
            combo_time.Items.AddRange(new object[] { "7", "15", "90", "120", "150", "180", "300" });
            combo_time.Location = new Point(132, 121);
            combo_time.Name = "combo_time";
            combo_time.Size = new Size(109, 39);
            combo_time.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Nunito SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(27, 41);
            label2.Name = "label2";
            label2.Size = new Size(57, 23);
            label2.TabIndex = 3;
            label2.Text = "Board";
            // 
            // combo_bonus
            // 
            combo_bonus.Font = new Font("Segoe UI", 14F);
            combo_bonus.FormattingEnabled = true;
            combo_bonus.Items.AddRange(new object[] { "High", "Normal", "Low" });
            combo_bonus.Location = new Point(132, 166);
            combo_bonus.Name = "combo_bonus";
            combo_bonus.Size = new Size(137, 39);
            combo_bonus.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Nunito SemiBold", 10.7999992F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(27, 85);
            label1.Name = "label1";
            label1.Size = new Size(99, 24);
            label1.TabIndex = 3;
            label1.Text = "Jewel type";
            // 
            // combo_height
            // 
            combo_height.Font = new Font("Segoe UI", 14F);
            combo_height.FormattingEnabled = true;
            combo_height.Items.AddRange(new object[] { "6", "7", "8", "9", "10" });
            combo_height.Location = new Point(199, 31);
            combo_height.Name = "combo_height";
            combo_height.Size = new Size(62, 39);
            combo_height.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Nunito SemiBold", 10.7999992F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(74, 130);
            label5.Name = "label5";
            label5.Size = new Size(52, 24);
            label5.TabIndex = 3;
            label5.Text = "Time";
            // 
            // combo_width
            // 
            combo_width.Font = new Font("Segoe UI", 14F);
            combo_width.FormattingEnabled = true;
            combo_width.Items.AddRange(new object[] { "6", "7", "8", "9", "10" });
            combo_width.Location = new Point(98, 31);
            combo_width.Name = "combo_width";
            combo_width.Size = new Size(62, 39);
            combo_width.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Nunito", 19.7999973F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(161, 24);
            label4.Name = "label4";
            label4.Size = new Size(38, 46);
            label4.TabIndex = 3;
            label4.Text = "x";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Nunito SemiBold", 10.7999992F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(19, 175);
            label6.Name = "label6";
            label6.Size = new Size(107, 24);
            label6.TabIndex = 3;
            label6.Text = "Time bonus";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Nunito", 20F, FontStyle.Bold);
            label8.Location = new Point(67, 61);
            label8.Name = "label8";
            label8.Size = new Size(197, 47);
            label8.TabIndex = 6;
            label8.Text = "Start game";
            // 
            // btn_GameOver
            // 
            btn_GameOver.BackColor = SystemColors.ScrollBar;
            btn_GameOver.FlatAppearance.BorderSize = 0;
            btn_GameOver.FlatStyle = FlatStyle.Flat;
            btn_GameOver.Font = new Font("Nunito", 12F, FontStyle.Bold);
            btn_GameOver.ForeColor = Color.Black;
            btn_GameOver.Location = new Point(19, 544);
            btn_GameOver.Name = "btn_GameOver";
            btn_GameOver.Size = new Size(294, 48);
            btn_GameOver.TabIndex = 0;
            btn_GameOver.Text = "End game";
            btn_GameOver.UseVisualStyleBackColor = false;
            btn_GameOver.Click += btn_GameOver_Click;
            // 
            // button1
            // 
            button1.BackColor = SystemColors.ScrollBar;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Nunito", 12F, FontStyle.Bold);
            button1.ForeColor = Color.Black;
            button1.Location = new Point(20, 373);
            button1.Name = "button1";
            button1.Size = new Size(294, 48);
            button1.TabIndex = 0;
            button1.Text = "Custom New Game";
            button1.UseVisualStyleBackColor = false;
            button1.Click += btn_CustomNewGame_Click;
            // 
            // button2
            // 
            button2.BackColor = SystemColors.ScrollBar;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Nunito", 11F, FontStyle.Bold);
            button2.ForeColor = Color.Black;
            button2.Location = new Point(226, 435);
            button2.Name = "button2";
            button2.Size = new Size(88, 95);
            button2.TabIndex = 0;
            button2.Text = "Hard";
            button2.UseVisualStyleBackColor = false;
            button2.Click += btn_Hard_Click;
            // 
            // button3
            // 
            button3.BackColor = SystemColors.ScrollBar;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Nunito", 11F, FontStyle.Bold);
            button3.ForeColor = Color.Black;
            button3.Location = new Point(123, 435);
            button3.Name = "button3";
            button3.Size = new Size(88, 95);
            button3.TabIndex = 0;
            button3.Text = "Normal";
            button3.UseVisualStyleBackColor = false;
            button3.Click += btn_Normal_Click;
            // 
            // button4
            // 
            button4.BackColor = SystemColors.ScrollBar;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("Nunito", 11F, FontStyle.Bold);
            button4.ForeColor = Color.Black;
            button4.Location = new Point(20, 435);
            button4.Name = "button4";
            button4.Size = new Size(88, 95);
            button4.TabIndex = 0;
            button4.Text = "Easy";
            button4.UseVisualStyleBackColor = false;
            button4.Click += btn_Easy_Click;
            // 
            // pnl_GameName
            // 
            pnl_GameName.BorderStyle = BorderStyle.FixedSingle;
            pnl_GameName.Controls.Add(label10);
            pnl_GameName.Location = new Point(0, 0);
            pnl_GameName.Name = "pnl_GameName";
            pnl_GameName.Size = new Size(333, 150);
            pnl_GameName.TabIndex = 9;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Nunito", 30F, FontStyle.Bold);
            label10.Location = new Point(7, 40);
            label10.Name = "label10";
            label10.Size = new Size(325, 69);
            label10.TabIndex = 6;
            label10.Text = "BEJEWELED";
            // 
            // TimerBackground
            // 
            TimerBackground.BackColor = Color.Transparent;
            TimerBackground.FlatAppearance.BorderSize = 5;
            TimerBackground.FlatStyle = FlatStyle.Flat;
            TimerBackground.Location = new Point(403, 905);
            TimerBackground.Name = "TimerBackground";
            TimerBackground.Size = new Size(850, 52);
            TimerBackground.TabIndex = 2;
            TimerBackground.UseVisualStyleBackColor = false;
            // 
            // Timer
            // 
            Timer.BackColor = Color.Black;
            Timer.FlatAppearance.BorderSize = 5;
            Timer.FlatStyle = FlatStyle.Flat;
            Timer.Location = new Point(403, 905);
            Timer.Name = "Timer";
            Timer.Size = new Size(850, 52);
            Timer.TabIndex = 3;
            Timer.UseVisualStyleBackColor = false;
            // 
            // btn_Switch
            // 
            btn_Switch.BackColor = SystemColors.ScrollBar;
            btn_Switch.FlatAppearance.BorderSize = 0;
            btn_Switch.FlatStyle = FlatStyle.Flat;
            btn_Switch.Font = new Font("Nunito", 12F, FontStyle.Bold);
            btn_Switch.ForeColor = Color.Black;
            btn_Switch.Location = new Point(109, 905);
            btn_Switch.Name = "btn_Switch";
            btn_Switch.Size = new Size(263, 52);
            btn_Switch.TabIndex = 4;
            btn_Switch.Text = "High Score";
            btn_Switch.UseVisualStyleBackColor = false;
            btn_Switch.Click += btn_Switch_Click;
            // 
            // pnl_Control
            // 
            pnl_Control.BackColor = SystemColors.Control;
            pnl_Control.BorderStyle = BorderStyle.FixedSingle;
            pnl_Control.Controls.Add(pnl_HighScore);
            pnl_Control.Controls.Add(pnl_StartGame);
            pnl_Control.Controls.Add(pnl_GameName);
            pnl_Control.Location = new Point(37, 26);
            pnl_Control.Name = "pnl_Control";
            pnl_Control.Size = new Size(335, 850);
            pnl_Control.TabIndex = 10;
            // 
            // button5
            // 
            button5.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button5.Image = (Image)resources.GetObject("button5.Image");
            button5.Location = new Point(37, 905);
            button5.Name = "button5";
            button5.Size = new Size(53, 52);
            button5.TabIndex = 11;
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1280, 987);
            Controls.Add(button5);
            Controls.Add(pnl_Control);
            Controls.Add(btn_Switch);
            Controls.Add(Timer);
            Controls.Add(TimerBackground);
            Controls.Add(pnl_Board);
            Font = new Font("Nunito", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            pnl_HighScore.ResumeLayout(false);
            pnl_HighScore.PerformLayout();
            pnl_NewScore.ResumeLayout(false);
            pnl_NewScore.PerformLayout();
            pnl_StartGame.ResumeLayout(false);
            pnl_StartGame.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            pnl_GameName.ResumeLayout(false);
            pnl_GameName.PerformLayout();
            pnl_Control.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnl_Board;
        private Button button1;
        private ComboBox combo_width;
        private ComboBox combo_count;
        private Label label2;
        private Label label1;
        private Label lbl_ScoreValue;
        private Label label3;
        private Button TimerBackground;
        private Button button4;
        private Button button3;
        private Button button2;
        private Button Timer;
        private ComboBox combo_height;
        private Label label4;
        private Label label6;
        private Label label5;
        private ComboBox combo_bonus;
        private ComboBox combo_time;
        private Button btn_Switch;
        private Panel pnl_HighScore;
        private Panel pnl_StartGame;
        private Label label8;
        private Label label9;
        private Panel pnl_GameName;
        private Label label10;
        private Panel pnl_Control;
        private Label label7;
        private TextBox box_Name;
        private Panel pnl_Score;
        private Button btn_Save;
        private Panel pnl_NewScore;
        private Panel panel2;
        private Panel panel3;
        private Button btn_GameOver;
        private Label lbl_Name;
        private Label lbl_Congratulation;
        private Label lbl_Sorry;
        private Button button5;
    }
}
