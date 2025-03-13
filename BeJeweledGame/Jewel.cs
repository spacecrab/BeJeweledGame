using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WinFormsTimer = System.Windows.Forms.Timer;

namespace BeJeweledGame
{
    internal class Jewel
    {
        internal enum Type
        {
            Red, // Ruby
            Blue, // Sapphire
            Green, // Emerald
            Yellow, // Citrine
            Orange, // Topaz
            Purple, // Amethyst
            White, // Diamond
        }

        internal enum Style
        {
            Normal,
            Bomb,
            Cross,
            Star,
        }

        internal enum IsMarkedBy
        {
            None,
            Normal,
            Match,
            Bomb,
            Cross,
            Star,
        }

        private static Color[] Colors = new Color[7]
        {
            Color.Red,
            Color.Blue,
            Color.Green,
            Color.Yellow,
            Color.Orange,
            Color.Purple,
            Color.White
        };

        private const int timeMoveTile = 60; // milliseconds
        private const int stepTotal = 15;
        private const int boardMargin = 20;
        private const int jewelSpacing = 10;
        private const int Penalty = -200;

        private static int BorderWidth;
        private static int WIDTH;
        private static int HEIGHT;
        private static int COUNT;
        private static int jewelWidth;
        private static Point firstLocation;
        private static int stepTile;
        private static int timerSessionDuration;
        private static int timeBonus;
        private static int scoreMultiplier;

        private static Panel pnl_Board = new Panel();
        private static List<List<Jewel>> Board = new List<List<Jewel>>();
        private static List<List<Jewel>> Pending = new List<List<Jewel>>();
        private static Jewel? Jewel1st;
        private static Jewel? Jewel2nd;
        private static int Score = 0;
        private static WinFormsTimer timerSession = new WinFormsTimer();
        private static int timerSessionValue;
        private static Form darkenAreaForm = new Form();

        private Button btn_Jewel;
        private Type jewelType;
        private Style jewelStyle;

        private int stepCount;

        private Point LocationUnit;
        private Point LocationPixel;
        private Point DestinationPixel;
        private Point DestinationUnit;

        private TaskCompletionSource<bool> timerMoveTileCompleteCheck;
        private TaskCompletionSource<bool> timerRemoveTileCompleteCheck;
        private TaskCompletionSource<bool> timerMakeTileCompleteCheck;

        private WinFormsTimer timerMoveTile;
        private WinFormsTimer timerRemoveTile;
        private WinFormsTimer timerMakeTile;

        private IsMarkedBy isMarked;
        private bool isMatchedVertical;
        private bool isMatchedHorizontal;

        private Style toStyle;

        private WinFormsTimer timerBombAnimation = new WinFormsTimer() { Interval = 1};
        private WinFormsTimer timerCrossAnimation = new WinFormsTimer() { Interval = 1};

        private Button btn1_BombAnimation;
        private Button btn2_BombAnimation;
        private Button btn3_BombAnimation;
        private Button btn4_BombAnimation;

        private Button btn1_CrossAnimation;
        private Button btn2_CrossAnimation;
        private Button btn3_CrossAnimation;
        private Button btn4_CrossAnimation;

        private bool isBombAnimationFinished;
        private bool isCrossAnimationFinished;

        public Jewel(Point locationpixel, Type jeweltype, Point locationunit)
        {
            btn_Jewel = CreateButton();

            pnl_Board.Controls.Add(btn_Jewel);

            btn_Jewel.Location = locationpixel;
            jewelType = jeweltype;
            jewelStyle = Style.Normal;
            
            btn_Jewel.BackgroundImage = Asset.ResourceManager.GetObject(jeweltype.ToString()) as Image;

            LocationUnit = locationunit;
            LocationPixel = locationpixel;

            timerMoveTileCompleteCheck = new TaskCompletionSource<bool>();
            timerRemoveTileCompleteCheck = new TaskCompletionSource<bool>();
            timerMakeTileCompleteCheck = new TaskCompletionSource<bool>();

            //timerMoveTile = new WinFormsTimer() { Interval = timeMoveTile/stepTotal };
            timerMoveTile = new WinFormsTimer() { Interval = 1 };
            timerRemoveTile = new WinFormsTimer() { Interval = 3 };
            timerMakeTile = new WinFormsTimer() { Interval = 3 };

            isMarked = IsMarkedBy.None;
            isMatchedVertical = false;
            isMatchedHorizontal = false;

            toStyle = Style.Normal;

            btn1_BombAnimation = new Button();
            btn2_BombAnimation = new Button();
            btn3_BombAnimation = new Button();
            btn4_BombAnimation = new Button();

            btn1_CrossAnimation = new Button();
            btn2_CrossAnimation = new Button();

            isBombAnimationFinished = false;
            isCrossAnimationFinished = false;
            //UpdateInfo();

            Init();
        }



        private void UpdateInfo()
        {
            btn_Jewel.Text = jewelStyle.ToString();
        }

        private Button CreateButton()
        {
            Button button = new Button();

            button.BackgroundImageLayout = ImageLayout.Zoom;
            button.Size = new Size(jewelWidth, jewelWidth);
            button.BackColor = Color.Transparent;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;

            return button;
        }

        private void Init()
        {
            btn_Jewel.Click += btn_Jewel_Click;
            btn_Jewel.MouseEnter += Button_MouseEnter;
            btn_Jewel.MouseLeave += Button_MouseLeave;

            timerMoveTile.Tick += TimerMoveTile_Tick;
            timerRemoveTile.Tick += TimerRemoveTile_Tick;
            timerMakeTile.Tick += TimerMakeTile_Tick;
        }

        private static async Task SwapJewel(Jewel jewel1, Jewel jewel2)
        {
           // GlobalFlags.isAnimating = true;

            Task moveJewel1 = jewel1.GoToLocation(jewel2.LocationUnit);
            Task moveJewel2 = jewel2.GoToLocation(jewel1.LocationUnit);

            await Task.WhenAll(moveJewel1, moveJewel2);

            await Task.Delay(200);
            //GlobalFlags.isAnimating = false;
        }

        private async Task GoToLocation(Point destinationunit)                                                                                                                                             
        {
            Board[LocationUnit.Y][LocationUnit.X] = null;

            timerMoveTileCompleteCheck = new TaskCompletionSource<bool>();

            DestinationPixel = new Point
            (
                firstLocation.X + destinationunit.X * (jewelWidth + jewelSpacing),
                firstLocation.Y + destinationunit.Y * (jewelWidth + jewelSpacing)
            );
            DestinationUnit = destinationunit;

            stepCount = 0;
            timerMoveTile.Start();

            await timerMoveTileCompleteCheck.Task; 
        }

        private void TimerMoveTile_Tick(object? sender, EventArgs? e)
        {
            if (stepCount < stepTotal - 1)
            {
                MoveTile(stepCount);
                stepCount++;
            }
            else
            {
                MoveTileComplete();
            }
        }

        private void MoveTile(int stepcount)
        {
            Point location = new Point
            (
                LocationPixel.X + (int)Math.Round((double)(DestinationPixel.X - LocationPixel.X) / stepTotal * stepcount),
                LocationPixel.Y + (int)Math.Round((double)(DestinationPixel.Y - LocationPixel.Y) / stepTotal * stepcount)
            );
            btn_Jewel.Location = location;
        }

        private void MoveTileComplete()
        {
            timerMoveTile.Stop();
            SetLocation(DestinationPixel, DestinationUnit);
            btn_Jewel.FlatAppearance.BorderSize = 0;
            timerMoveTileCompleteCheck.TrySetResult(true);
        }

        private void SetLocation(Point destinationpixel, Point destinationunit)
        {
            btn_Jewel.Location = destinationpixel;
            LocationPixel = destinationpixel;
            LocationUnit = destinationunit;
            Board[destinationunit.Y][destinationunit.X] = this;
        }

        private async void btn_Jewel_Click(object? sender, EventArgs e)
        {
            if (GlobalFlags.isAnimating || GlobalFlags.isGameOver)
            {
                return;
            }
            if (Jewel1st == null)
            {
                Jewel1st = this;
                btn_Jewel.FlatAppearance.BorderSize = BorderWidth;
                return;
            }
            if (Jewel1st == this)
            {
                Jewel1st = null;
                btn_Jewel.FlatAppearance.BorderSize = 0;
                return;
            }

            Jewel2nd = this;

            if (isValidMove(Jewel1st, Jewel2nd) == false)
            {
                Jewel1st.btn_Jewel.FlatAppearance.BorderSize = 0;
                Jewel1st = null;
                Jewel2nd = null;
                return;
            }
            
            await MakeMove(Board, Jewel1st, Jewel2nd);
            Jewel1st = null;
            Jewel2nd = null;
            return;
        }

        private static bool isValidMove(Jewel jewel1, Jewel jewel2)
        {
            return Math.Abs(jewel1.LocationUnit.X - jewel2.LocationUnit.X) + Math.Abs(jewel1.LocationUnit.Y - jewel2.LocationUnit.Y) < 2;
        }

        private static async Task MakeMove(List<List<Jewel>>board, Jewel jewel1, Jewel jewel2)
        {
            GlobalFlags.isAnimating = true;
            bool hasMatch = false;
            bool isSpecialMove = false;

            await SwapJewel(jewel1, jewel2);
            GlobalFlags.isAnimating = true;

            CalculateBoardSimple(new List<List<bool>>(), out hasMatch);
            IsSpecialMove(jewel1, jewel2, out isSpecialMove);

            if (hasMatch == false && isSpecialMove == false)
            {
                CalculateScore(-1);
                await SwapJewel(jewel1, jewel2);
                GlobalFlags.isAnimating = false;
                return;
            }

            

            if (isSpecialMove)
            {
                hasMatch = await SpecialMove(board, jewel1, jewel2, hasMatch);
            }
            else
            {
                hasMatch = await FirstNormalMatch(board, jewel1, jewel2, hasMatch);
            }

            while (hasMatch)
            {
                hasMatch = await NextNormalMatch(board, hasMatch);
            }

            GlobalFlags.isAnimating = false;
        }

        private static void IsSpecialMove(Jewel jewel1, Jewel jewel2, out bool isSpecialMove)
        {
            isSpecialMove = false;
            if (jewel1.jewelStyle == Style.Star || jewel2.jewelStyle == Style.Star)
            {
                isSpecialMove = true;
                return;
            }

        }

        private static async Task<bool> SpecialMove(List<List<Jewel>> board, Jewel jewel1, Jewel jewel2, bool hasMatch)
        {
            ClearMarkedJewel(board);

            List<Jewel> MarkedJewel = new List<Jewel>();

            await RemoveClickedSpecialMoveJewel(board, jewel1, jewel2, MarkedJewel);

            while (MarkedJewel.Count > 0)
            {
                await RemoveMarkedJewel(board, MarkedJewel);
            }

            await FillBoardDrop(board);

            CalculateBoardSimple(new List<List<bool>>(), out hasMatch);

            //await Task.Delay(200);

            return hasMatch;


        }

        private static async Task<bool> FirstNormalMatch(List<List<Jewel>> board, Jewel jewel1, Jewel jewel2, bool hasMatch)
        {
            ClearMarkedJewel(board);

            List<List<Jewel>> ClickedMatches = new List<List<Jewel>>();
            List<Jewel> MarkedJewel = new List<Jewel>();

            MarkMatchedJewel(board, out ClickedMatches, out MarkedJewel, out hasMatch);

            if (hasMatch == false)
            {
                await Task.Delay(100);
                return hasMatch;
            }

            MarkSpecialJewelClicked(board, jewel1, jewel2, MarkedJewel, ClickedMatches);

            while (MarkedJewel.Count > 0)
            {
                await RemoveMarkedJewel(board, MarkedJewel);
            }

            await FillBoardDrop(board);

            //await Task.Delay(200);

            return hasMatch;
        }

        private static async Task<bool> NextNormalMatch(List<List<Jewel>> board, bool hasMatch)
        {
            ClearMarkedJewel(board);

            List<List<Jewel>> Matches;
            List<Jewel> MarkedJewel;

            MarkMatchedJewel(board, out Matches, out MarkedJewel, out hasMatch);

            if (hasMatch == false)
            {
                await Task.Delay(100);
                return hasMatch;
            }

            MarkSpecialJewel(board, MarkedJewel, Matches);

            while (MarkedJewel.Count > 0)
            {
                await RemoveMarkedJewel(board, MarkedJewel);
            }


            await FillBoardDrop(board);
            //await Task.Delay(200);

            return hasMatch;
        }

        private static async Task RemoveClickedSpecialMoveJewel(List<List<Jewel>> board, Jewel jewel1, Jewel jewel2, List<Jewel> MarkedJewel)
        {
            MarkedJewel = new List<Jewel>();

            if (jewel1.jewelStyle == Style.Star && jewel2.jewelStyle == Style.Normal)
            {
                await RemoveCLickedStarJewel(board, jewel1, jewel2, MarkedJewel);
            }
            else if (jewel1.jewelStyle == Style.Normal && jewel2.jewelStyle == Style.Star)
            {
                await RemoveCLickedStarJewel(board, jewel2, jewel1, MarkedJewel);
            }
            else if (jewel1.jewelStyle == Style.Star && jewel2.jewelStyle == Style.Bomb)
            {
                await RemoveCLickedStarBombJewel(board, jewel1, jewel2, MarkedJewel);
            }
            else if (jewel1.jewelStyle == Style.Bomb && jewel2.jewelStyle == Style.Star)
            {
                await RemoveCLickedStarBombJewel(board, jewel2, jewel1, MarkedJewel);
            }
            else if (jewel1.jewelStyle == Style.Star && jewel2.jewelStyle == Style.Cross)
            {
                await RemoveCLickedStarCrossJewel(board, jewel1, jewel2, MarkedJewel);
            }
            else if (jewel1.jewelStyle == Style.Cross && jewel2.jewelStyle == Style.Star) 
            {
                await RemoveCLickedStarCrossJewel(board, jewel2, jewel1, MarkedJewel);
            }

            while (MarkedJewel.Count > 0)
            {
                await RemoveMarkedJewel(board, MarkedJewel);
            }

            await FillBoardDrop(board);
        }

        private static void MarkMatchedJewel(List<List<Jewel>> board, out List<List<Jewel>> Matches, out List<Jewel> MarkedJewel, out bool hasMatch)
        {

            Matches = new List<List<Jewel>>();
            MarkedJewel = new List<Jewel>();
            hasMatch = false;

            for (int i = 1; i <= HEIGHT; i++)
            {
                for (int j = 1; j <= WIDTH; j++)
                {
                    if (board[i][j].isMatchedVertical == false)
                    {
                        List<Jewel> match = new List<Jewel>();
                        Type jeweltype = board[i][j].jewelType;
                        int ii = i;
                        while (ii <= HEIGHT && board[ii][j].jewelType == jeweltype)
                        {
                            match.Add(board[ii][j]);
                            ii++;
                        }
                        if (match.Count >= 3)
                        {
                            Matches.Add(match);
                            foreach (Jewel jewel in match)
                            {
                                board[jewel.LocationUnit.Y][jewel.LocationUnit.X].isMatchedVertical = true;
                                board[jewel.LocationUnit.Y][jewel.LocationUnit.X].isMarked= IsMarkedBy.Match;
                            }
                        }
                    }

                    ///////////

                    if (board[i][j].isMatchedHorizontal == false)
                    {
                        List<Jewel> match = new List<Jewel>();
                        Type jeweltype = board[i][j].jewelType;
                        int jj = j;
                        while (jj <= WIDTH && board[i][jj].jewelType == jeweltype)
                        {
                            match.Add(board[i][jj]);
                            jj++;
                        }
                        if (match.Count >= 3)
                        {
                            Matches.Add(match);
                            foreach (Jewel jewel in match)
                            {
                                board[jewel.LocationUnit.Y][jewel.LocationUnit.X].isMatchedHorizontal = true;
                                board[jewel.LocationUnit.Y][jewel.LocationUnit.X].isMarked = IsMarkedBy.Match;
                            }
                        }
                    }
                }
            }

            for (int i = 1; i <= HEIGHT; i++)
            {
                for (int j = 1; j <= WIDTH; j++)
                {
                    if (board[i][j].isMarked != IsMarkedBy.None)
                    {
                        MarkedJewel.Add(board[i][j]);
                    }
                }
            }

            if (MarkedJewel.Count > 0)
            {
                hasMatch = true;
            }
        }

        private static void ClearMarkedJewel(List<List<Jewel>> board)
        {
            for (int i = 1; i <= HEIGHT; i++)
            {
                for (int j = 1; j <= WIDTH; j++)
                {
                    board[i][j].isMarked = IsMarkedBy.None;
                    board[i][j].isMatchedVertical = false;
                    board[i][j].isMatchedHorizontal = false;
                    board[i][j].toStyle = Style.Normal;
                }
            }
        }
        private static void MarkSpecialJewelClicked(List<List<Jewel>> board, Jewel jewel1, Jewel jewel2, List<Jewel> MarkedJewel, List<List<Jewel>> ClickedMatches)
        {
            foreach (List<Jewel> match in ClickedMatches)
            {

                if (match.Count == 3)
                {

                }
                else if (match.Count == 4)
                {
                    foreach (Jewel jewel in match)
                    {
                        if (jewel == Jewel1st || jewel == Jewel2nd)
                        {
                            jewel.MarkOneSpecialJewel(Style.Bomb);
                        }
                    }
                }
                
                else if (match.Count >= 5)
                {
                    foreach (Jewel jewel in match)
                    {
                        if (jewel == Jewel1st || jewel == Jewel2nd)
                        {
                            jewel.MarkOneSpecialJewel(Style.Star);
                        }
                    }
                }
                
            }

            foreach (Jewel jewel in MarkedJewel)
            {
                if (jewel.isMatchedHorizontal && jewel.isMatchedVertical)
                {
                    if (jewel.toStyle == Style.Cross || jewel.toStyle == Style.Star)
                    {
                        continue;
                    }
                    jewel.MarkOneSpecialJewel(Style.Cross);
                }
            }
        }

        private static void MarkSpecialJewel(List<List<Jewel>> board, List<Jewel> MarkedJewel, List<List<Jewel>> Matches)
        {
            Random random = new Random();

            foreach (Jewel jewel in MarkedJewel)
            {
                if (jewel.isMatchedHorizontal && jewel.isMatchedVertical)
                {
                    if (jewel.toStyle == Style.Cross || jewel.toStyle == Style.Star)
                    {
                        continue;
                    }
                    jewel.MarkOneSpecialJewel(Style.Cross);
                }
            }

            foreach (List<Jewel> match in Matches)
            {
                if (match.Count == 3)
                {

                }
                else if (match.Count == 4)
                {
                    bool flag = false;
                    for (int i = 0; i < 4; i++)
                    {
                       if (match[i].toStyle != Style.Normal)
                       {
                           flag = true;
                       }
                    }
                    if (flag)
                    {
                        continue;
                    }
                    int randomNumber = random.Next(0, 4);
                    match[randomNumber].MarkOneSpecialJewel(Style.Bomb);
                }
                
                else if (match.Count >= 5)
                {
                    int flag = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (match[i].toStyle != Style.Normal)
                        {
                            flag++;
                        }
                    }
                    if (flag == 5)
                    {
                        continue;
                    }
                    while(true)
                    {
                        int randomNumber = random.Next(0, 5);
                        if (match[randomNumber].toStyle == Style.Normal)
                        {
                            match[randomNumber].MarkOneSpecialJewel(Style.Star);
                            break;
                        }
                    }
                }
                
            }
        }

        private static async Task RemoveCLickedStarJewel(List<List<Jewel>> board, Jewel jewel1, Jewel jewel2, List<Jewel> MarkedJewel)
        {
            List <Jewel> NextMarkedJewel = new List<Jewel>();

            jewel1.isMarked = IsMarkedBy.Normal;

            jewel1.ActivateOneStar(board, jewel1, jewel2.jewelType, NextMarkedJewel);
            await jewel1.ShrinkJewel();
            

            MarkedJewel.Clear();

            foreach (Jewel jewel in NextMarkedJewel)
            {
                MarkedJewel.Add(jewel);
            }
        }

        private static async Task RemoveCLickedStarBombJewel(List<List<Jewel>> board, Jewel jewel1, Jewel jewel2, List<Jewel> MarkedJewel)
        {
            List<Jewel> NextMarkedJewel = new List<Jewel>();

            jewel1.isMarked = IsMarkedBy.Normal;
        }

        private static async Task RemoveCLickedStarCrossJewel(List<List<Jewel>> board, Jewel jewel1, Jewel jewel2, List<Jewel> MarkedJewel)
        {

        }

        private static async Task RemoveMarkedJewel(List<List<Jewel>> board, List<Jewel> MarkedJewel)
        {
            List <Jewel> NextMarkedJewel = new List<Jewel>();
            CalculateScore(MarkedJewel.Count);
            List <Task> tasks = new List<Task>();

            foreach (Jewel jewel in MarkedJewel)
            {
                Task? task = null;
                switch (jewel.jewelStyle)
                {
                    case Style.Bomb:
                        jewel.ActivateOneBomb(board, jewel, NextMarkedJewel);
                        task = jewel.ChangeOrRemoveJewel();
                        break;
                    case Style.Cross:
                        jewel.ActivateOneCross(board, jewel, NextMarkedJewel);
                        task = jewel.ChangeOrRemoveJewel();
                        break;
                    case Style.Star:
                        Random random = new Random();
                        Type jeweltype = (Type)(random.Next(0, COUNT));
                        jewel.ActivateOneStar(board, jewel, jeweltype, NextMarkedJewel);
                        task = jewel.ChangeOrRemoveJewel();
                        break;
                    default:
                        jewel.RemoveOneNormal(board, jewel, NextMarkedJewel);
                        task = jewel.ChangeOrRemoveJewel();
                        break;
                }
                if (task != null)
                {
                    tasks.Add(task);
                }
            }

            await Task.WhenAll(tasks);

            MarkedJewel.Clear();

            foreach (Jewel jewel in NextMarkedJewel)
            {
                MarkedJewel.Add(jewel);
            }
        }

        private void RemoveOneNormal(List<List<Jewel>> board, Jewel jewel, List<Jewel> NextMarkedJewel)
        {

        }

        private void RemoveOneJewelByBomb()
        {
            btn_Jewel.Dispose();
            Board[LocationUnit.Y][LocationUnit.X] = null;
        }

        private void RemoveOneJewelByCross()
        {
            btn_Jewel.Dispose();
            Board[LocationUnit.Y][LocationUnit.X] = null;
        }

        private Task? ChangeOrRemoveJewel()
        {
            Task? task = null;
            if (toStyle == Style.Normal)
            {
                switch (isMarked)
                {
                    case IsMarkedBy.Bomb:
                        RemoveOneJewelByBomb();
                        break;
                    case IsMarkedBy.Cross:
                        RemoveOneJewelByCross();
                        break;
                    case IsMarkedBy.Star:
                        task = ShrinkJewel();
                        break;
                    default:
                        task = ShrinkJewel();
                        break;
                }
            }
            else
            {
                MakeOneSpecialJewel(toStyle);
            }
            return task;
        }

        private void ActivateOneBomb(List<List<Jewel>> board, Jewel jewel, List<Jewel> NextMarkedJewel)
        {
            CalculateScore(5);
            BombAnimation(pnl_Board);
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if ((i == 0 && j == 0) 
                     || jewel.LocationUnit.Y + i < 1 
                     || jewel.LocationUnit.Y + i > HEIGHT 
                     || jewel.LocationUnit.X + j < 1 
                     || jewel.LocationUnit.X + j > WIDTH)
                    {
                        continue;
                    }
                    MarkOneJewel(board, jewel.LocationUnit.Y + i, jewel.LocationUnit.X + j, NextMarkedJewel, IsMarkedBy.Bomb);
                }
            }
        }

        private void ActivateOneCross(List<List<Jewel>> board, Jewel jewel, List<Jewel> NextMarkedJewel)
        {
            CalculateScore(8);
            CrossAnimation(pnl_Board);

            for (int i = 1; i <= HEIGHT; i++)
            {
                if (i == jewel.LocationUnit.Y)
                {
                    continue;
                }
                MarkOneJewel(board, i, jewel.LocationUnit.X, NextMarkedJewel, IsMarkedBy.Cross);
            }

            for (int j = 1; j <= WIDTH; j++)
            {
                if (j == jewel.LocationUnit.X)
                {
                    continue;
                }
                MarkOneJewel(board, jewel.LocationUnit.Y, j, NextMarkedJewel, IsMarkedBy.Cross);
            }
        }

        private void ActivateOneStar(List<List<Jewel>> board, Jewel jewel, Type jeweltype, List <Jewel> NextMarkedJewel)
        {
            CalculateScore(12);

            for (int i = 1; i <= HEIGHT; i++)
            {
                for (int j = 1; j <= WIDTH; j++)
                {
                    if (board[i][j] == null)
                    {
                        continue;
                    }
                    Jewel target = board[i][j];
                    if (target.toStyle != Style.Normal)
                    {
                        continue;
                    }
                    if (target.jewelType == jeweltype)
                    {
                        target.isMarked = IsMarkedBy.Star;
                        NextMarkedJewel.Add(target);
                    }
                }
            }
        }

        private void MarkOneJewel(List<List<Jewel>> board, int i, int j, List<Jewel> NextMarkedJewel, IsMarkedBy ismarkedby)
        {
            if (board[i][j] == null)
            {
                return;
            }
            Jewel target = board[i][j];
            if (target.toStyle != Style.Normal)
            {
                return;
            }
            if (target.isMarked == IsMarkedBy.None)
            {
                target.isMarked = ismarkedby;
                NextMarkedJewel.Add(target);
            }
        }

        private void Button_MouseEnter(object? sender, EventArgs e)
        {
            if (GlobalFlags.isAnimating)
            {
                return;
            }
            btn_Jewel.FlatAppearance.BorderSize = BorderWidth;
        }

        private void Button_MouseLeave(object? sender, EventArgs e)
        {
            if (GlobalFlags.isAnimating)
            {
                return;
            }
            if (Jewel1st != this)
            {
                btn_Jewel.FlatAppearance.BorderSize = 0;
            }
        }

        /////////////////////////////////////////////
        /// Logic
        /////////////////////////////////////////////
        
        public static void InitializeBoard
        (
            Panel pnl_board,
            int width,
            int height,
            int count,
            int timersessionduration,
            int timersessionbonus,
            int scoremultiplier
        ){
            Score = 0;
            Form1.Instance?.SetScore(Score);
            pnl_Board = pnl_board;
            WIDTH = width;
            HEIGHT = height;
            COUNT = count;
            timerSessionDuration = timersessionduration;
            timeBonus = timersessionbonus;
            scoreMultiplier = scoremultiplier;

            int boardmargin = boardMargin; 
            int jewelspacing = jewelSpacing;

            timerSessionValue = timerSessionDuration;
            timerSession.Dispose();
            timerSession = new WinFormsTimer();
            timerSession.Interval = 100;
            timerSession.Tick += TimerSession_Tick;
            Form1.Instance?.SetTimer(timerSessionValue, timerSessionDuration);

            int jewelwidth = Math.Min
            (
                (Form1.Instance.BoardSize.Width - boardmargin * 2 + jewelspacing) / width - jewelspacing, 
                (Form1.Instance.BoardSize.Height - boardmargin * 2 + jewelspacing) / height - jewelspacing
            );
            Form1.Instance?.SetBoardSize(new Size
            (
                boardmargin * 2 + (jewelwidth + jewelspacing) * width - jewelspacing, 
                boardmargin * 2 + (jewelwidth + jewelspacing) * height - jewelspacing
            ));

            jewelWidth = jewelwidth;
            BorderWidth = jewelwidth / 30;

            stepTile = jewelWidth / (stepTotal*4/5);

            Point firstlocation = new Point
            (
                boardmargin + (pnl_board.Width - boardmargin * 2 - (jewelspacing + jewelwidth) * (width + 2) + jewelspacing ) / 2,
                boardmargin + (pnl_board.Height - boardmargin * 2 - (jewelspacing + jewelwidth) * (height + 2) + jewelspacing ) / 2
            );

            firstLocation = firstlocation;

            int seed = (int)DateTime.Now.Ticks;
            Random random = new Random(seed);

            darkenAreaForm.Dispose();

            foreach (Control control in pnl_board.Controls)
            {
                control.Dispose();
            }
            pnl_board.Controls.Clear();

            darkenAreaForm = CreateDarkenAreaForm(pnl_board.Size);

            Board = new List<List<Jewel>>();
            for (int i = 0; i <= height + 1; i++)
            {
                Board.Add(new List<Jewel>());
                for (int j = 0; j <= width + 1; j++)
                {
                    Board[i].Add(null);
                }
            }

            for (int i = 1; i <= height; i++)
            {
                for (int j = 1; j <= width; j++)
                {
                    Point location = new Point(firstlocation.X + j * (jewelwidth + jewelspacing), firstlocation.Y + i * (jewelwidth + jewelspacing));
                    Type jeweltype = (Type)(random.Next(0, count));

                    Board[i][j] = new Jewel(location, jeweltype, new Point(j, i));
                }
            }

            UpdateBoardNoAnimation();
            Form1.Instance?.Newgame();
            GlobalFlags.isGameOver = false;
            timerSession.Start();
        }

        private static Form CreateDarkenAreaForm(Size size)
        {
            Form darkenareaform = new Form();

            darkenareaform.Opacity = 0.5;
            darkenareaform.FormBorderStyle = FormBorderStyle.None;
            darkenareaform.StartPosition = FormStartPosition.Manual;
            darkenareaform.ShowInTaskbar = false;
            darkenareaform.Size = size;
            darkenareaform.BackColor = Color.Black;

            Label gameover = new Label();

            darkenareaform.Controls.Add(gameover);

            gameover.Text = "Game Over";
            gameover.Font = new Font("Nunito", 70, FontStyle.Bold);
            gameover.ForeColor = Color.White;
            gameover.AutoSize = true;
            gameover.TextAlign = ContentAlignment.MiddleCenter;
            gameover.Location = new Point((darkenareaform.Width - gameover.Width) / 2, (darkenareaform.Height - gameover.Height) / 2);
            if (gameover.Location.X < 100)
            {
                gameover.Text = "Game\nOver";
                gameover.Location = new Point((darkenareaform.Width - gameover.Width) / 2, (darkenareaform.Height - gameover.Height) / 2);
            }

            return darkenareaform;
        }

        private static void TimerSession_Tick(object? sender, EventArgs e)
        {
            if (timerSessionValue >= 0)
            {
                timerSessionValue -= timerSession.Interval;
                Form1.Instance?.SetTimer(timerSessionValue, timerSessionDuration);
            }
            else
            {
                timerSession.Stop();
                SetGameOver();
            }
        }

        public static void SetGameOver()
        {
            timerSession.Stop();
            timerSessionValue = 0;
            Form1.Instance?.SetTimer(timerSessionValue, timerSessionDuration);
            GlobalFlags.isGameOver = true;
            darkenAreaForm.Location = pnl_Board.PointToScreen(Point.Empty);
            darkenAreaForm.Size = pnl_Board.ClientSize;
            if (darkenAreaForm.Visible == false)
            {
                darkenAreaForm.Show(Form1.Instance);
            }
            Form1.Instance?.Endgame();
        }

        public static void ReduceTimer()
        {
            timerSessionValue -= 10000;
            if (timerSessionValue < 0)
            {
                timerSessionValue = 0;
            }
            Form1.Instance?.SetTimer(timerSessionValue, timerSessionDuration);
        }

        private static void UpdateBoardNoAnimation()
        {
            List<List<bool>> isMatched = new List<List<bool>>();
            bool hasMatch = true;

            while (hasMatch)
            {
                isMatched.Clear();
                hasMatch = false;
                CalculateBoardSimple(isMatched, out hasMatch);
                RemoveMatchBoardNoAnimation(isMatched, hasMatch);
                FillBoardNoAnimation(isMatched, hasMatch);
            }
        }

        private static void CalculateBoardSimple(List<List<bool>> isMatched, out bool hasMatch)
        {
            ClearMarkedJewel(Board);
            isMatched.Clear();
            hasMatch = false;

            List<List<bool>> isMatchedVertical = new List<List<bool>>();
            List<List<bool>> isMatchedHorizontal = new List<List<bool>>();

            for (int i = 0; i <= HEIGHT + 1; i++)
            {
                isMatched.Add(new List<bool>());
                isMatchedVertical.Add(new List<bool>());
                isMatchedHorizontal.Add(new List<bool>());

                for (int j = 0; j <= WIDTH + 1; j++)
                {
                    isMatched[i].Add(false);
                    isMatchedVertical[i].Add(false);
                    isMatchedHorizontal[i].Add(false);
                }
            }

            for (int i = 1; i <= HEIGHT; i++)
            {
                for (int j = 1; j <= WIDTH; j++)
                {
                    if (isMatchedVertical[i][j] == false)
                    {
                        List<Jewel> match = new List<Jewel>();
                        Type jeweltype = Board[i][j].jewelType;
                        int ii = i;
                        while (ii <= HEIGHT && Board[ii][j].jewelType == jeweltype)
                        {
                            match.Add(Board[ii][j]);
                            ii++;
                        }
                        if (match.Count >= 3)
                        {
                            hasMatch = true;
                            foreach (Jewel jewel in match)
                            {
                                isMatchedVertical[jewel.LocationUnit.Y][jewel.LocationUnit.X] = true;
                                isMatched[jewel.LocationUnit.Y][jewel.LocationUnit.X] = true;
                            }
                        }
                    }

                    ///////////

                    if (isMatchedHorizontal[i][j] == false)
                    {
                        List<Jewel> match = new List<Jewel>();
                        Type jeweltype = Board[i][j].jewelType;
                        int jj = j;
                        while (jj <= WIDTH && Board[i][jj].jewelType == jeweltype)
                        {
                            match.Add(Board[i][jj]);
                            jj++;
                        }
                        if (match.Count >= 3)
                        {
                            hasMatch = true;
                            foreach (Jewel jewel in match)
                            {
                                isMatchedHorizontal[jewel.LocationUnit.Y][jewel.LocationUnit.X] = true;
                                isMatched[jewel.LocationUnit.Y][jewel.LocationUnit.X] = true;
                            }
                        }
                    }
                }
            }
        }

        private static void  RemoveMatchBoardNoAnimation(List<List<bool>> isMatch, bool hasMatch)
        {
            if (hasMatch == false)
            {
                return;
            }
            for (int i = 1; i <= HEIGHT; i++)
            {
                for (int j = 1; j <= WIDTH; j++)
                {
                    if (isMatch[i][j])
                    {
                        Board[i][j].btn_Jewel.Dispose();
                        Board[i][j] = null;
                    }
                }
            }
        }

        private static void FillBoardNoAnimation(List<List<bool>> isMatch, bool hasMatch)
        {
            if (hasMatch == false)
            {
                return;
            }
            for (int i = 1; i <= HEIGHT; i++)
            {
                for (int j = 1; j <= WIDTH; j++)
                {
                    if (isMatch[i][j] == true || Board[i][j] == null)
                    {
                        int X = firstLocation.X + j * (jewelWidth + jewelSpacing);
                        int Y = firstLocation.Y + i * (jewelWidth + jewelSpacing);
                        Type jeweltype = (Type)(new Random().Next(0, COUNT));
                        Board[i][j] = new Jewel(new Point(X, Y), jeweltype, new Point(j, i));
                    }
                }
            }
        }

        private static void CalculateScore(int num)
        {
            int score = num;
            CalculateTimer(score);
            if (score == -1)
            {
                score = -20;
            }
            Score += score * scoreMultiplier;
            Form1.Instance?.SetScore(Score);
        }

        private static void CalculateTimer(int score)
        {
            if (score == -1)
            {
                timerSessionValue += Penalty*200;
            }
            else if (score == 0)
            {
                
            }
            else if (score <= 3)
            {
                timerSessionValue += timeBonus * 3;
            }
            else if (score <= 6)
            {
                timerSessionValue += timeBonus * 4;
            }
            else if (score <= 9)
            {
                timerSessionValue += timeBonus * 6;
            }


            if (timerSessionValue > timerSessionDuration)
            {
                timerSessionValue = timerSessionDuration;
            }
            Form1.Instance?.SetTimer(timerSessionValue, timerSessionDuration);
        }

        private async Task ShrinkJewel()
        {
            timerRemoveTileCompleteCheck = new TaskCompletionSource<bool>();
            timerRemoveTile.Start();

            await timerRemoveTileCompleteCheck.Task;
        }
        
        private void TimerRemoveTile_Tick(object? sender, EventArgs e)
        {
            if (btn_Jewel.Height > 10)
            {
                btn_Jewel.Size = new Size(btn_Jewel.Width - stepTile, btn_Jewel.Height - stepTile);
                btn_Jewel.Location = new Point(LocationPixel.X + (jewelWidth - btn_Jewel.Width) / 2, LocationPixel.Y + (jewelWidth - btn_Jewel.Height) / 2);
            }
            else
            {
                timerRemoveTile.Stop();
                btn_Jewel.Dispose();
                Board[LocationUnit.Y][LocationUnit.X] = null;

                timerRemoveTileCompleteCheck.TrySetResult(true);
            }
        }

        private static async Task FillBoardDrop(List<List<Jewel>> board)
        {
            GlobalFlags.isAnimating = true;

            List<Task> tasks = new List<Task>();
            List<Jewel> MovingJewel = new List<Jewel>();

            while (true)
            {
                MovingJewel.Clear();
                tasks.Clear();
                for (int j = 1; j <= WIDTH; j++)
                {
                    int i = HEIGHT;
                    while (i >= 1)
                    {
                        if (board[i][j] == null)
                        {
                            board[0][j] = CreateJewel(0, j);
                            i--;
                            break;
                        }
                        i--;
                    }
                    while (i >= 0)
                    {
                        if (board[i][j] != null)
                        {
                            MovingJewel.Add(board[i][j]);
                        }
                        i--;
                    }
                }

                if (MovingJewel.Count == 0)
                {
                    break;
                }

                foreach (Jewel jewel in MovingJewel)
                {
                    Point destinationunit = new Point(jewel.LocationUnit.X, jewel.LocationUnit.Y + 1);
                    Task task = jewel.GoToLocation(destinationunit);
                    tasks.Add(task);
                }

                await Task.WhenAll(tasks);

            }

            await Task.Delay(200);
            GlobalFlags.isAnimating = false;
        }

        private static async Task FillBoard(List<List<bool>> isMatch)
        {
            GlobalFlags.isAnimating = true;
            List<Task> tasks = new List<Task>();

            for (int i = 1; i <= HEIGHT; i++ )
            {
                for (int j = 1; j <= WIDTH; j++)
                {
                    if (Board[i][j] == null)
                    {
                        Board[i][j] = CreateJewel(i, j);
                        Task task = Board[i][j].ExpandJewel();
                        tasks.Add(task);
                    }
                }
            }

            await Task.WhenAll(tasks);

            await Task.Delay(200);
            GlobalFlags.isAnimating = false;
        }

        private static Jewel CreateJewel(int i, int j)
        {
            int X = firstLocation.X + j * (jewelWidth + jewelSpacing);
            int Y = firstLocation.Y + i * (jewelWidth + jewelSpacing);
            Type jeweltype = (Type)(new Random().Next(0, COUNT));
            return new Jewel(new Point(X, Y), jeweltype, new Point(j, i));
        }

        private async Task ExpandJewel()
        {
            btn_Jewel.Size = new Size(10, 10);

            timerMakeTileCompleteCheck = new TaskCompletionSource<bool>(); // Ensure a fresh instance
            timerMakeTile.Start();

            await timerMakeTileCompleteCheck.Task;
        }

        private void TimerMakeTile_Tick(object? sender, EventArgs e)
        {
            if (btn_Jewel.Height < jewelWidth - 10)
            {
                btn_Jewel.Size = new Size(btn_Jewel.Width + stepTile, btn_Jewel.Height + stepTile);
                btn_Jewel.Location = new Point(LocationPixel.X + (jewelWidth - btn_Jewel.Width) / 2, LocationPixel.Y + (jewelWidth - btn_Jewel.Height) / 2);
            }
            else
            {
                timerMakeTile.Stop();
                btn_Jewel.Size = new Size(jewelWidth, jewelWidth);
                btn_Jewel.Location = LocationPixel;

                timerMakeTileCompleteCheck.TrySetResult(true);
            }
        }

        private void MarkOneSpecialJewel(Style style)
        {
            toStyle = style;
        }

        private void MakeOneSpecialJewel(Style style)
        {            
            //UpdateInfo();
            if (jewelStyle == Style.Normal)
            {
                if (style == Style.Bomb)
                {
                    jewelStyle = Style.Bomb;
                    btn_Jewel.BackColor = Color.Yellow;
                }
                else if (style == Style.Cross)
                {
                    jewelStyle = Style.Cross;
                    btn_Jewel.BackColor = Color.Red;
                }
                else if (style == Style.Star)
                {
                    jewelStyle = Style.Star;
                    btn_Jewel.BackgroundImage = Asset.ResourceManager.GetObject("Star") as Image;
                }
            }
        }

        private void BombAnimation(Panel pnl_board)
        {
            int width = jewelWidth / 6;

            btn1_BombAnimation = CreateButtonAnimation();
            btn2_BombAnimation = CreateButtonAnimation();
            btn3_BombAnimation = CreateButtonAnimation();
            btn4_BombAnimation = CreateButtonAnimation();

            btn1_BombAnimation.Height = width;
            btn2_BombAnimation.Width = width;
            btn3_BombAnimation.Height = width;
            btn4_BombAnimation.Width = width;

            btn1_BombAnimation.Width = width;
            btn2_BombAnimation.Height = width;
            btn3_BombAnimation.Width = width;
            btn4_BombAnimation.Height = width;

            CenterLocation(btn1_BombAnimation, btn_Jewel);
            CenterLocation(btn2_BombAnimation, btn_Jewel);
            CenterLocation(btn3_BombAnimation, btn_Jewel);
            CenterLocation(btn4_BombAnimation, btn_Jewel);

            timerBombAnimation.Tick += TimerBombAnimation_Tick;
            timerBombAnimation.Start();
        }

        private void TimerBombAnimation_Tick(object? sender, EventArgs e)
        {
            if (btn1_BombAnimation.Width < jewelWidth * 3 + jewelSpacing * 2 - 20)
            {
                btn1_BombAnimation.Location = new Point(btn1_BombAnimation.Location.X - 10, btn1_BombAnimation.Location.Y - 10);
                btn1_BombAnimation.Width += 20;

                btn2_BombAnimation.Location = new Point(btn2_BombAnimation.Location.X + 10, btn2_BombAnimation.Location.Y - 10);
                btn2_BombAnimation.Height += 20;

                btn3_BombAnimation.Location = new Point(btn3_BombAnimation.Location.X - 10, btn3_BombAnimation.Location.Y + 10);
                btn3_BombAnimation.Width += 20;

                btn4_BombAnimation.Location = new Point(btn4_BombAnimation.Location.X - 10, btn4_BombAnimation.Location.Y - 10);
                btn4_BombAnimation.Height += 20;
            }
            else
            {
                btn1_BombAnimation.Width = jewelWidth * 3 + jewelSpacing * 2;
                btn2_BombAnimation.Height = jewelWidth * 3 + jewelSpacing * 2;
                btn3_BombAnimation.Width = jewelWidth * 3 + jewelSpacing * 2;
                btn4_BombAnimation.Height = jewelWidth * 3 + jewelSpacing * 2;

                isBombAnimationFinished = true;
            }
            if (isBombAnimationFinished == true)
            {
                timerBombAnimation.Stop();
                btn1_BombAnimation.Dispose();
                btn2_BombAnimation.Dispose();
                btn3_BombAnimation.Dispose();
                btn4_BombAnimation.Dispose();
            }    
        }

        private void CrossAnimation(Panel pnl_board)
        {
            int width = jewelWidth / 2;

            btn1_CrossAnimation = CreateButtonAnimation();
            btn2_CrossAnimation = CreateButtonAnimation();
            btn3_CrossAnimation = CreateButtonAnimation();
            btn4_CrossAnimation = CreateButtonAnimation();

            btn1_CrossAnimation.Width = width;
            btn2_CrossAnimation.Height = width;
            btn3_CrossAnimation.Width = width;
            btn4_CrossAnimation.Height = width;

            btn1_CrossAnimation.Height = width;
            btn2_CrossAnimation.Width = width;
            btn3_CrossAnimation.Height = width;
            btn4_CrossAnimation.Width = width;

            CenterLocation(btn1_CrossAnimation, btn_Jewel);
            CenterLocation(btn2_CrossAnimation, btn_Jewel);
            CenterLocation(btn3_CrossAnimation, btn_Jewel);
            CenterLocation(btn4_CrossAnimation, btn_Jewel);

            timerCrossAnimation.Tick += TimerCrossAnimation_Tick;
            timerCrossAnimation.Start();
        }

        private void TimerCrossAnimation_Tick(object? sender, EventArgs e)
        {
            int step = 60;

            if (isCrossAnimationFinished == false)
            {
                bool flag = true;
                if (btn1_CrossAnimation.Location.Y > step)
                {
                    btn1_CrossAnimation.Location =
                    new Point(
                        btn1_CrossAnimation.Location.X, 
                        btn1_CrossAnimation.Location.Y - step
                    );
                    btn1_CrossAnimation.Height += step;
                    flag = false;
                }
                if (btn2_CrossAnimation.Location.X + btn2_CrossAnimation.Width < pnl_Board.Width - step)
                {
                    btn2_CrossAnimation.Width += step;
                    flag = false;
                }
                if (btn3_CrossAnimation.Location.Y + btn3_CrossAnimation.Height < pnl_Board.Height - step)
                {
                    btn3_CrossAnimation.Height += step;
                    flag = false;
                }
                if (btn4_CrossAnimation.Location.X > step)
                {
                    btn4_CrossAnimation.Location =
                    new Point(
                        btn4_CrossAnimation.Location.X - step,
                        btn4_CrossAnimation.Location.Y
                    );
                    btn4_CrossAnimation.Width += step;
                    flag = false;
                }
                if (flag == true)
                {
                    isCrossAnimationFinished = true;
                }
            }
            else
            {
                timerCrossAnimation.Stop();
                btn1_CrossAnimation.Dispose();
                btn2_CrossAnimation.Dispose();
                btn3_CrossAnimation.Dispose();
                btn4_CrossAnimation.Dispose();
            }
        }

        private void CenterLocation(Control control1, Control control2)
        {
            int X = control2.Location.X + control2.Width / 2 - control1.Width / 2;
            int Y = control2.Location.Y + control2.Height / 2 - control1.Height / 2;
            control1.Location = new Point(X, Y);
        }

        private Button CreateButtonAnimation()
        {
            Button button = new Button();
            button.BackColor = Color.Black;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Visible = true;
            button.Enabled = false;
            pnl_Board.Controls.Add(button);
            button.BringToFront();

            return button;
        }
    }
}
