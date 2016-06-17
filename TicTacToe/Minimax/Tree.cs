using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe.Minimax
{
    public class Tree
    {
        private Node root;
        private int depth;

        public int Size { get; }
        public int Winning { get; }

        public int Depth
        {
            get { return depth; }
            set { depth = (value > 0) ? value : Int32.MaxValue; }
        }


        public Tree(int size, int winning, int maxDepth = Int32.MaxValue)
        {
            if (size < 3) throw new BoardSizeException();
            if (winning < 3 || winning > size) throw new WinningNumberException();

            Size = size;
            Winning = winning;
            Depth = maxDepth;

            root = new Node(Size, Winning);
        }


        public Move OpponentsMove(Move move)
        {
            if (move == null || !move.Validate(Size)) throw new MoveException();

            root = new Node(root, move);
            BuildTree();
            CalculateScores();

            List<Node> possibleMoves = root.Children.Where(c => c.Score == root.Score).ToList();
            if (possibleMoves == null || possibleMoves.Count == 0) return null;

            Node next = possibleMoves.First();
            Move myMove = next.GameBoard.Change;
            MyMove(myMove);

            return myMove;
        }

        private void MyMove(Move move)
        {
            root = new Node(root, move);
        }

        private void BuildTree()
        {
            BuildTree(root, Depth);
        }

        private void BuildTree(Node node, int depth)
        {
            if (depth > 0 && !node.IsTerminal)
            {
                bool childrenAdded = false;
                node.Children = new List<Node>();

                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        if (node.GameBoard.GameMatrix[i, j] == 0)
                        {
                            var child = new Node(node, new Move(i, j, node.IsOTurn ? 1 : -1));
                            child.Depth = node.Depth + 1;
                            node.Children.Add(child);
                            childrenAdded = true;
                        }
                    }
                }

                if (childrenAdded) foreach (var c in node.Children) BuildTree(c, depth - 1);
            }
            else
            {
                node.IsTerminal = true;
            }
        }

        private void CalculateScores()
        {
            root.Score = CalculateScores(root, Depth);
        }

        private int CalculateScores(Node node, int depth)
        {
            if (depth <= 0 || node.IsTerminal) return node.GameBoard.Score;

            foreach (var child in node.Children)
                child.Score = CalculateScores(child, depth - 1);

            if (node.Children == null || node.Children.Count == 0)
            {
                return node.GameBoard.Score;
            }

            return (node.Depth % 2 == 0) ? 
                node.Children.Max(c => c.Score) : 
                node.Children.Min(c => c.Score);
        }
        
        public void PrintFirstScores()
        {
            Console.WriteLine("0: {0}", root.Score);
            Console.Write("1: ");

            for (int i = 0; i < root.Children.Count; i++)
            {
                Console.Write(root.Children[i].Score + " ");
            }
        }

    }
}
