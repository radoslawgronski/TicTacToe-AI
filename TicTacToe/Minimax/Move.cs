using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Minimax
{
    public class Move
    {
        private int row;
        private int column;
        private int value;

        public Move(int row, int column, int value = 0)
        {
            Row = row;
            Column = column;
            Value = value;
        }

        public Move(Move move)
        {
            Row = move.Row;
            Column = move.Column;
            Value = move.Value;
        }

        public int Row
        {
            get { return row; }
            set { row = value < 0 ? 0 : value; }
        }

        public int Column
        {
            get { return column; }
            set { column = value < 0 ? 0 : value; }
        }

        public int Value
        {
            get { return this.value; }
            set { this.value = value*value == 1 ? value : 0; }
        }

        public bool Validate(int boardSize)
        {
            if (row >= boardSize || column >= boardSize) return false;
            return true;
        }
    }
}
