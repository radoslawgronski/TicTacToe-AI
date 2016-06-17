using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToe.GUI
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl, IView
    {
        public MainWindow window;
        private int boardSize;
        private int winning;
        private bool ox;

        public GameView(MainWindow window)
        {
            InitializeComponent();
            this.window = window;

            boardSize = 3;
            winning = 3;
            ox = true;

            LoadGame();
        }

        public GameView(MainWindow window, int boardSize, int winning, bool userSign)
        {
            InitializeComponent();
            this.window = window;

            BoardSize = boardSize;
            Winning = winning;
            UserSign = userSign;

            LoadGame();
        }

        public int BoardSize
        {
            get { return boardSize; }
            set { if (value >= 3) boardSize = value; }
        }

        public int Winning
        {
            get { return winning; }
            set { if (value >= 3 && value <= BoardSize) winning = value; }
        }

        public bool UserSign
        {
            get { return ox; }
            set { ox = value; }
        }

        public int GetSize()
        {
            return BoardSize;
        }

        public int GetWinning()
        {
            return Winning;
        }

        public bool GetUserSign()
        {
            return UserSign;
        }

        public void SetSize(int size)
        {
            BoardSize = size;
        }

        public void SetWinning(int winning)
        {
            Winning = winning;
        }

        public void SetUserSign(bool userSign)
        {
            UserSign = userSign;
        }

        public void SetProperties(IView sourceObject)
        {
            SetSize(sourceObject.GetSize());
            SetWinning(sourceObject.GetWinning());
            SetUserSign(sourceObject.GetUserSign());

            LoadGame();
        }

        public void LoadGame()
        {
            try
            {
                scvGame.Content = new Game(BoardSize, Winning, UserSign, this);
            }
            catch (IndexOutOfRangeException e) { e.ToString(); }
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            window.SwitchContent();
        }

    }
}
