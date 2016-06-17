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
using TicTacToe.Minimax;

namespace TicTacToe.GUI
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : UserControl
    {
        private Tree AIPlayer;
        private Board board;
        private int size;
        private int winning;
        private bool ox;
        private GameView parentView;
        private Label turnInfo;
        private Dictionary<String, Button> buttonsDictionary;
        private bool turn;

        public Game(int size, int winning, bool ox, GameView parentView)
        {
            if (size < 3 || winning < 3 || winning > size)
            {
                throw new IndexOutOfRangeException();
            }

            this.size = size;
            this.winning = winning;
            this.ox = ox;
            this.parentView = parentView;
            this.turnInfo = parentView.lblTurn;

            InitializeComponent();
            buttonsDictionary = new Dictionary<string, Button>();

            CreateGrid();
            AddButtons();

            turn = true;
            AIPlayer = new Tree(size, winning, 3);
            board = new Board(size, winning);
        }

        private void CreateGrid()
        {
            for (int i = 0; i < size; i++)
            {
                grdMainGrid.RowDefinitions.Add(new RowDefinition());
                grdMainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private void AddButtons()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Button button = new Button();

                    button.Name = "x" + i + "x" + j;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);

                    button.Click += (sender, e) => ButtonClicked(button.Name);

                    grdMainGrid.Children.Add(button);
                    buttonsDictionary.Add(button.Name, button);
                }
            }
        }

        private void ButtonClicked(String name)
        {
            if (turn)
            {
                Button b = buttonsDictionary[name];

                if (b.IsEnabled)
                {
                    b.Content = ox ? "O" : "X";
                    b.IsEnabled = false;
                    board = new Board(board, ButtonToMove(b.Name, -1));

                    if (!ChangeTurn()) // lock player's move
                    {
                        SetComputersMove(PerformComputerAction(name));
                        ChangeTurn(); // unlock player's move
                    }
                }
            }
        }

        private bool ChangeTurn()
        {
            if (CheckIfWin() != 0)
            {
                string winner = CheckIfWin() == 1 ? "You" : "Computer";
                var result = MessageBox.Show(winner + " won!");
                if (result == MessageBoxResult.OK || result == MessageBoxResult.Cancel)
                {
                    parentView.window.SwitchContent();
                    return true;
                }
            }

            if (turn)
            {
                turnInfo.Content = "computer's";
            }
            else
            {
                turnInfo.Content = "your";
            }

            turn = !turn;
            return false;
        }

        private Move PerformComputerAction(string name)
        {
            return AIPlayer.OpponentsMove(ButtonToMove(name, -1));
        }

        private void SetComputersMove(Move computersMove)
        {
            if (computersMove != null && computersMove.Validate(size))
            {
                Button b = buttonsDictionary["x" + computersMove.Row + "x" + computersMove.Column];
                b.Content = ox ? "X" : "O";
                b.IsEnabled = false;

                board = new Board(board, ButtonToMove(b.Name, 1));
            }            
        }

        private Move ButtonToMove(string buttonName, int ox)
        {
            string[] buttonInfo = buttonName.Split(new char[] { 'x' });
            int row = Int32.Parse(buttonInfo[1]);
            int column = Int32.Parse(buttonInfo[2]);

            return new Move(row, column, ox);
        }

        private int CheckIfWin()
        {
            if (Math.Abs(board.Score) == Math.Pow(10, winning))
            {
                if (board.Score > 0) return -1;
                return 1;
            }

            return 0;
        }

    }
}
