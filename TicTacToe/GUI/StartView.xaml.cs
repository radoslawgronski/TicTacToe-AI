using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for StartView.xaml
    /// </summary>
    public partial class StartView : UserControl, IView
    {
        private MainWindow window;
        private int boardSize;
        private int winning;
        private bool ox;

        public int BoardSize
        {
            get { return boardSize; }
            set
            {
                if (value >= 3) boardSize = value;
                if (!txtBoardSize.Text.Equals(BoardSize.ToString())) txtBoardSize.Text = BoardSize.ToString();
            }
        }

        public int Winning
        {
            get { return winning; }
            set
            {
                if (value >= 3 && value <= BoardSize) winning = value;
                if (!txtWinning.Text.Equals(Winning.ToString())) txtWinning.Text = Winning.ToString();
            }
        }

        public bool UserSignO
        {
            get { return ox; }
            set
            {
                ox = value;
                if (rbtnO.IsChecked != UserSignO) rbtnO.IsChecked = UserSignO;
            }
        }

        public bool UserSignX
        {
            get { return !ox; }
            set
            {
                ox = !value;
                if (rbtnX.IsChecked != UserSignX) rbtnX.IsChecked = UserSignX;
            }
        }

        public StartView(MainWindow window)
        {
            InitializeComponent();
            this.window = window;

            BoardSize = 3;
            Winning = 3;
            UserSignO = true;
        }

        public StartView(MainWindow window, int boardSize, int winning, bool ox) : this(window)
        {
            BoardSize = boardSize;
            Winning = winning;
            UserSignO = ox;
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
            return UserSignO;
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
            UserSignO = userSign;
        }

        public void SetProperties(IView sourceObject)
        {
            SetSize(sourceObject.GetSize());
            SetWinning(sourceObject.GetWinning());
            SetUserSign(sourceObject.GetUserSign());
        }

        private void txtBoardSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            InputChanged(txtBoardSize, BoardSize);
        }

        private void txtWinning_TextChanged(object sender, TextChangedEventArgs e)
        {
            InputChanged(txtWinning, Winning);
        }

        private void rbtnO_Checked(object sender, RoutedEventArgs e)
        {
            UserSignO = true;
        }

        private void rbtnX_Checked(object sender, RoutedEventArgs e)
        {
            UserSignX = true;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            int size = -1, win = -1;
            bool result = true;

            if (!int.TryParse(txtBoardSize.Text, out size) || size < 3) result = false;
            if (!result || !int.TryParse(txtWinning.Text, out win) || win < 3 || win > size) result = false;

            if (result)
            {
                BoardSize = size;
                Winning = win;
                window.SwitchContent();
            }
            else
            {
                MessageBox.Show("Size must be at least 3. Winning number must be at least 3 and no larger than size.");
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void InputChanged(TextBox inputBox, int previousValue)
        {
            int result;
            
            if (!int.TryParse(inputBox.Text, out result) || result < 1)
            {
                inputBox.Text = previousValue.ToString();
            }
        }

    }
}
