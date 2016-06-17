using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Minimax
{
    public class Node
    {
        public Board GameBoard { get; }
        public List<Node> Children { get; set; }
        public int Size { get; }
        public int Winning { get; }
        public int Score { get; set; }
        public int Depth { get; set; }
        public bool IsTerminal { get; set; }
        public bool IsOTurn { get { return Depth % 2 == 0; } }
        public bool IsXTurn { get { return !IsOTurn; } }

        public Node(int size, int winning)
        {
            Size = size;
            Winning = winning;

            GameBoard = new Board(Size, Winning);
            IsTerminal = ChceckIfIsTerminal();
        }

        public Node(Node previous, Move change)
        {
            Size = previous.Size;
            Winning = previous.Winning;

            GameBoard = new Board(previous.GameBoard, change);
            IsTerminal = ChceckIfIsTerminal();
        }

        private bool ChceckIfIsTerminal()
        {
            return Math.Abs(GameBoard.Score) == Math.Pow(10, Winning);
        }

    }
}
