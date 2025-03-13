using System.Resources;
using System.Runtime.CompilerServices;

namespace BeJeweledGame
{
    public partial class Form1 : Form
    {
        private const int WIDTH = 6;
        private const int HEIGHT = 6;
        private const int COUNT = 7;
        private const int TIME = 120000;
        private const int TIMEBONUS = 1500;
        private const int SCOREMULTIPLIER = 15;

        private Point boardLocation;
        private Size boardSize;

        public Point BoardLocation { get => boardLocation; }

        public Size BoardSize { get => boardSize; }

        public static Form1? Instance { get; private set; }

        struct Player
        {
            public string name;
            public int score;
            public int count;
            public Player(string name, int score, int count)
            {
                this.name = name;
                this.score = score;
                this.count = count;
            }
        }

        private List<Player> players;

        //private List<List<Jewel>> Board = new List<List<Jewel>>();

        public Form1()
        {
            InitializeComponent();

            players = new List<Player>();

            pnl_StartGame.Location = new Point(0, pnl_Control.Height - pnl_StartGame.Height);
            pnl_HighScore.Location = pnl_StartGame.Location;
            pnl_GameName.Location = new Point(0, 0);
            pnl_GameName.Size = new Size(pnl_Control.Width, pnl_Control.Height - pnl_StartGame.Height - 1);

            pnl_StartGame.Visible = true;
            pnl_HighScore.Visible = false;
            btn_Switch.Text = "High scores";

            btn_Save.Enabled = false;
            lbl_Congratulation.Visible = false;
            lbl_Sorry.Visible = false;
            lbl_Name.Visible = false;

            boardLocation = pnl_Board.Location;
            boardSize = pnl_Board.Size;

            Instance = this;

            if (COUNT > Enum.GetValues(typeof(Jewel.Type)).Length)
                throw new Exception("COUNT must be less than or equal to the number of Jewel.Type");

            string path = Application.ExecutablePath;
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            DirectoryInfo parentDir = dirInfo.Parent.Parent.Parent.Parent.Parent;

            GlobalConst.appFolder = parentDir.FullName;

            Initialize();
            InitializeHighScore();
        }

        private void Initialize()
        {

            combo_width.SelectedItem = WIDTH.ToString();
            combo_height.SelectedItem = HEIGHT.ToString();
            combo_count.SelectedItem = COUNT.ToString();
            combo_time.SelectedItem = (TIME / 1000).ToString();
            combo_bonus.SelectedIndex = 1;

            Jewel.InitializeBoard(pnl_Board, WIDTH, HEIGHT, COUNT, TIME, TIMEBONUS, SCOREMULTIPLIER);
        }

        private void InitializeHighScore()
        {
            GetHighScore(players);
            PrintHighScore(players);
            return;
        }

        private void GetHighScore(List<Player> players)
        {
            ResourceManager RM = HighScore.ResourceManager;
            players.Clear();
            for (int i = 0; i < 5; i++)
            {
                Player player = new Player();
                player.count = i + 1;
                player.name = RM.GetString("name" + (i + 1).ToString());
                player.score = int.Parse(RM.GetString("score" + (i + 1).ToString()));
                players.Add(player);
                players.Sort((p1, p2) => p2.score.CompareTo(p1.score));
            }
        }

        private void PrintHighScore(List<Player> players)
        {
            pnl_Score.Controls.Clear();
            int x = 10;
            int y = 13;
            for (int i = 0; i < 5; i++)
            {
                Label stt = new Label();
                stt.Text = (i + 1).ToString() + ".";
                stt.Font = new Font("Nunito", 12, FontStyle.Bold);
                stt.Location = new Point(x, y);
                stt.AutoSize = true;
                pnl_Score.Controls.Add(stt);

                Label name = new Label();
                name.Text = players[i].name;
                name.Font = new Font("Nunito", 12, FontStyle.Bold);
                name.Location = new Point(x + 45, y);
                name.AutoSize = true;
                pnl_Score.Controls.Add(name);

                Label score = new Label();
                if (players[i].score == 0)
                    score.Text = "_________";
                else score.Text = players[i].score.ToString();
                score.Font = new Font("Nunito", 12, FontStyle.Bold);
                score.Location = new Point(x + 155, y);
                score.AutoSize = true;
                pnl_Score.Controls.Add(score);
                y += 45;
            }
        }

        public void Endgame()
        {
            pnl_HighScore.Visible = true;
            btn_Switch.Text = "Start game";
            btn_Save.Enabled = true;
            btn_GameOver.Enabled = false;
        }

        private void btn_CustomNewGame_Click(object sender, EventArgs e)
        {
            int width;
            int height;
            int count;
            int time;
            int bonus;
            bool widthParsed = int.TryParse(combo_width.SelectedItem?.ToString(), out width);
            bool heightParsed = int.TryParse(combo_height.SelectedItem?.ToString(), out height);
            bool countParsed = int.TryParse(combo_count.SelectedItem?.ToString(), out count);
            bool timeParsed = int.TryParse(combo_time.SelectedItem?.ToString(), out time);
            bool bonusParsed = int.TryParse(combo_bonus.SelectedIndex.ToString(), out bonus);

            switch (bonus)
            {
                case 0:
                    bonus = 2000;
                    break;
                case 1:
                    bonus = 1500;
                    break;
                case 2:
                    bonus = 1000;
                    break;
            }

            //MessageBox.Show(time.ToString() + bonus.ToString());

            if (widthParsed && heightParsed && countParsed && timeParsed && bonusParsed)
            {
                Jewel.InitializeBoard(pnl_Board, width, height, count, time * 1000, bonus, 15);
            }
            else
            {
                MessageBox.Show("Invalid value");
                // Handle the error scenario, perhaps by showing a message to the user.
            }
        }

        public void SetScore(int score)
        {
            this.lbl_ScoreValue.Text = score.ToString();
        }

        public void SetBoardSize(Size size)
        {
            int X = pnl_Board.Location.X + (pnl_Board.Width - size.Width) / 2;
            int Y = pnl_Board.Location.Y + (pnl_Board.Height - size.Height) / 2;
            pnl_Board.Location = new Point(X, Y);
            pnl_Board.Size = size;
        }

        private void btn_Hard_Click(object sender, EventArgs e)
        {
            if (GlobalFlags.isAnimating)
            {
                return;
            }
            Jewel.InitializeBoard(pnl_Board, 10, 10, 7, 120000, 1000, 20); //Hard 
            combo_width.SelectedItem = 10.ToString();
            combo_height.SelectedItem = 10.ToString();
            combo_count.SelectedItem = 7.ToString();
            combo_time.SelectedItem = 120.ToString();
            combo_bonus.SelectedIndex = 2;
        }

        private void btn_Normal_Click(object sender, EventArgs e)
        {
            if (GlobalFlags.isAnimating)
            {
                return;
            }
            Jewel.InitializeBoard(pnl_Board, 8, 8, 5, 180000, 1500, 15); //Normal
            combo_width.SelectedItem = 8.ToString();
            combo_height.SelectedItem = 8.ToString();
            combo_count.SelectedItem = 5.ToString();
            combo_time.SelectedItem = 180.ToString();
            combo_bonus.SelectedIndex = 1;
        }


        private void btn_Easy_Click(object sender, EventArgs e)
        {
            if (GlobalFlags.isAnimating)
            {
                return;
            }
            Jewel.InitializeBoard(pnl_Board, 7, 7, 4, 300000, 2000, 10); //Easy
            combo_width.SelectedItem = 7.ToString();
            combo_height.SelectedItem = 7.ToString();
            combo_count.SelectedItem = 4.ToString();
            combo_time.SelectedItem = 300.ToString();
            combo_bonus.SelectedIndex = 0;
        }

        public void SetTimer(int sessionvalue, int sesstionduration)
        {
            float ratio = (float)sessionvalue / sesstionduration;
            Timer.Width = (int)(ratio * TimerBackground.Width);
        }

        private void btn_Switch_Click(object sender, EventArgs e)
        {
            if (pnl_HighScore.Visible == true)
            {
                pnl_HighScore.Visible = false;
                btn_Switch.Text = "High score";
            }
            else
            {
                pnl_HighScore.Visible = true;
                btn_Switch.Text = "Start game";
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (box_Name.Text == "")
            {
                lbl_Name.Visible = true;
                return;
            }
            Player player = new Player();
            player.name = box_Name.Text;
            player.score = int.Parse(lbl_ScoreValue.Text);
            player.count = 6;
            players.Add(player);
            players.Sort((p1, p2) => p2.score.CompareTo(p1.score));
            players.RemoveRange(5, players.Count - 5);
            for (int i = 0; i < 5; i++)
            {
                if (players[i].count == 6)
                {
                    lbl_Congratulation.Text = "Congratulations! You are in top " + (i + 1).ToString();
                    lbl_Congratulation.Visible = true;
                    break;
                }
                if (i == 4)
                {
                    lbl_Sorry.Visible = true;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                Player player1 = new Player(players[i].name, players[i].score, i + 1);
                players[i] = player1;
            }
            string s = "";
            for (int i = 0; i < 5; i++)
            {
                s += players[i].name + " " + players[i].score.ToString() + players[i].count + "\n";
            }
            PrintHighScore(players);
            WriteData(players);
            btn_Save.Enabled = false;
            lbl_Name.Visible = false;
        }

        private void WriteData(List<Player> players)
        {
            string resxFile = GlobalConst.appFolder + @"\BeJeweledGame\HighScore.resx";

            using (ResXResourceWriter resxWriter = new ResXResourceWriter(resxFile))
            {
                for (int i = 0; i < 5; i++)
                {
                    resxWriter.AddResource("name" + (i + 1).ToString(), players[i].name);
                    resxWriter.AddResource("score" + (i + 1).ToString(), players[i].score.ToString());
                }
                resxWriter.Generate();
            }
        }

        public void Newgame()
        {
            GlobalFlags.isGameOver = false;
            GlobalFlags.isAnimating = false;
            btn_GameOver.Enabled = true;

            box_Name.Text = "";
            btn_Save.Enabled = false;
            lbl_Congratulation.Visible = false;
            lbl_Sorry.Visible = false;
            lbl_Name.Visible = false;
        }

        private void btn_GameOver_Click(object sender, EventArgs e)
        {
            if (GlobalFlags.isAnimating)
            {
                return;
            }
            Jewel.SetGameOver();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
