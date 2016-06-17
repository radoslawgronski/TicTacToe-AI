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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserControl[] contents;

        public MainWindow()
        {
            InitializeComponent();

            contents = new UserControl[] { new StartView(this), new GameView(this) };
            ContentHolder.Content = contents[0];
        }

        public void SwitchContent()
        {
            if (ContentHolder.Content.Equals(contents[0]))
            {
                ((IView)contents[1]).SetProperties(((IView)contents[0]));
                ContentHolder.Content = contents[1];
                this.ResizeMode = ResizeMode.CanResize;
            }
            else
            {
                ContentHolder.Content = contents[0];
                WindowState = WindowState.Normal;
                this.Width = 450;
                this.Height = 638;
                this.ResizeMode = ResizeMode.NoResize;
            }
        }
    }
}
