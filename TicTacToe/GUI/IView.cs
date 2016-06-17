using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.GUI
{
    public interface IView
    {
        int GetSize();
        int GetWinning();
        bool GetUserSign();

        void SetSize(int size);
        void SetWinning(int winning);
        void SetUserSign(bool userSign);

        void SetProperties(IView sourceObject);
    }
}
