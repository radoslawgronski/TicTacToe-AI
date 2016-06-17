using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Minimax
{
    /// <summary>
    /// Represents game board in a certain state.
    /// </summary>
    public class Board
    {
        private class Row
        {
            public int[] Array { get; }
            public int Size { get { return Array.Length; } }

            public int this[int key]
            {
                get { return Array[key]; }
            }

            public Row(int[,] matrix, int startRow, int startColumn, int endRow, int endColumn, bool direction = true)
            {
                if (startRow == endRow)
                {
                    Array = new int[endColumn - startColumn + 1];
                    for (int i = startColumn; i <= endColumn; i++) Array[i] = matrix[startRow, i];
                }
                else if (startColumn == endColumn)
                {
                    Array = new int[endRow - startRow + 1];
                    for (int i = startRow; i <= endRow; i++) Array[i] = matrix[i, startColumn];
                }
                else
                {
                    if (direction)
                    {
                        Array = new int[endColumn - startColumn + 1];
                        for (int i = startColumn; i <= endColumn; i++) Array[i - startColumn] = matrix[startRow + i - startColumn, i];
                    }
                    else
                    {
                        Array = new int[startColumn - endColumn + 1];
                        for (int i = startColumn; i >= endColumn; i--) Array[startColumn - i] = (startRow == 0) ? 
                                matrix[endRow - i, i] : matrix[endRow + startRow - i, i];
                    }
                }
            }
        }

        public int[,] GameMatrix { get; }           // NxN matrix; values: 1 (X), -1 (O), 0 (empty)
        public Move Change { get; }                 // change in relation to previous Board
        public int Score { get; }                   // calculated heuristic score for AI player
        public int Winning { get; }                 // how many in a row to win
        public int Size { get;  }                   // game board size
        private int winningScore;

        /// <summary>
        /// Creates new empty game board.
        /// </summary>
        /// <param name="size">Size of board.</param>
        public Board(int size, int winning)
        {
            if (size < 3) throw new BoardSizeException();
            if (winning < 3 || winning > size) throw new WinningNumberException();

            GameMatrix = new int[size, size];
            Size = size;
            Winning = winning;
            winningScore = (int) Math.Pow(10, Winning);
            Change = null;
        }

        /// <summary>
        /// Creates new game board on base of previousBoard matrix and change in relation to the previous board.
        /// </summary>
        /// <param name="previousBoard">Board object representing game board from previous state.</param>
        /// <param name="change">Change in relation to previous board (row, column, value: 1 or -1).</param>
        public Board(Board previousBoard, Move change)
        {
            if (previousBoard == null || previousBoard.Size < 3) throw new BoardSizeException();
            if (change == null || !change.Validate(previousBoard.Size)) throw new MoveException();

            Size = previousBoard.Size;
            Winning = previousBoard.Winning;
            winningScore = (int)Math.Pow(10, Winning);
            Change = new Move(change);
            GameMatrix = (int[,])previousBoard.GameMatrix.Clone();
            GameMatrix[Change.Row, Change.Column] = Change.Value;
            Score = CalculateScore();
        }

        /// <summary>
        /// Calculate heuristic score for current board.
        /// </summary>
        /// <returns>Heuristic score for current board.</returns>
        public int CalculateScore()
        {
            List<int> scoresX = new List<int>();
            List<int> scoresO = new List<int>();
            int scoreO, scoreX;

            for (int i = 0; i < Size; i++)
            {
                scoreO = RowScore(i, -1);
                if (scoreO == winningScore) return -scoreO;

                scoreX = RowScore(i, 1);
                if (scoreX == winningScore) return scoreX;

                scoresO.Add(scoreO);
                scoresX.Add(scoreX);

                scoreO = ColumnScore(i, -1);
                if (scoreO == winningScore) return -scoreO;

                scoreX = ColumnScore(i, 1);
                if (scoreX == winningScore) return scoreX;

                scoresO.Add(scoreO);
                scoresX.Add(scoreX);
            }

            for (int i = 0; i <= Size - Winning; i++)
            {
                scoreO = DiagonalScore(0, i, true, -1);
                if (scoreO == winningScore) return -scoreO;

                scoreX = DiagonalScore(0, i, true, 1);
                if (scoreX == winningScore) return scoreX;

                scoresO.Add(scoreO);
                scoresX.Add(scoreX);

                if (i != 0)
                {
                    scoreO = DiagonalScore(i, 0, true, -1);
                    if (scoreO == winningScore) return -scoreO;

                    scoreX = DiagonalScore(i, 0, true, 1);
                    if (scoreX == winningScore) return scoreX;

                    scoresO.Add(scoreO);
                    scoresX.Add(scoreX);
                }                
            }
            
            for (int i = Size - Winning + ((Size == Winning) ? 1 : 0); i < Size; i++)
            {
                scoreO = DiagonalScore(0, i, false, -1);
                if (scoreO == winningScore) return -scoreO;

                scoreX = DiagonalScore(0, i, false, 1);
                if (scoreX == winningScore) return scoreX;

                scoresO.Add(scoreO);
                scoresX.Add(scoreX);

                if (i != Size - 1)
                {
                    scoreO = DiagonalScore(Size - i - 1, Size - 1, false, -1);
                    if (scoreO == winningScore) return -scoreO;

                    scoreX = DiagonalScore(Size - i - 1, Size - 1, false, 1);
                    if (scoreX == winningScore) return scoreX;

                    scoresO.Add(scoreO);
                    scoresX.Add(scoreX);
                }
            }
            
            int maxO = scoresO.Count > 0 ? -scoresO.Max() : -1;
            int maxX = scoresX.Count > 0 ? scoresX.Max() : 1;

            return (Math.Abs(maxO) >= maxX) ? maxO : maxX;
        }

        private int RowScore(int row, int ox)
        {
            Row rowObject = new Row(GameMatrix, row, 0, row, Size - 1);
            return AnalyzePartials(rowObject, CreatePartials(rowObject, ox));
        }

        private int ColumnScore(int column, int ox)
        {
            Row rowObject = new Row(GameMatrix, 0, column, Size - 1, column);
            return AnalyzePartials(rowObject, CreatePartials(rowObject, ox));
        }

        private int DiagonalScore(int startRow, int startColumn, bool direction, int ox)
        {
            int difference = (startRow != 0) ? 
                Size - startRow : direction ? Size - startColumn : startColumn + 1;
            int endRow = startRow + difference - 1;
            int endColumn = startColumn + (direction ? difference - 1 : -difference + 1);

            Row rowObject = new Row(GameMatrix, startRow, startColumn, endRow, endColumn, direction);
            return AnalyzePartials(rowObject, CreatePartials(rowObject, ox));
        }

        private List<int[]> CreatePartials(Row row, int ox)
        {
            List<int[]> partialsList = new List<int[]>();
            int[] partials = new int[row.Size];

            for (int i = 0; i < row.Size; i++)
            {
                if (row[i] == ox) partials[i] = (i == 0) ? 1 : partials[i - 1] + 1;
            }

            for (int i = row.Size - 1; i >= 0; i--)
            {
                if (partials[i] != 0)
                {
                    partialsList.Add(new int[] { i - partials[i] + 1, partials[i] });
                    i = i - partials[i] + 1;
                }
            }

            return partialsList;
        }

        private int AnalyzePartials(Row row, List<int[]> partialsList)
        {
            List<int> scores = new List<int>();

            foreach (var partial in partialsList)
            {
                if (partial[1] >= Winning) return winningScore;
                int emptyFields = 0;

                for (int i = partial[0] - 1; i >= 0; i--)
                {
                    if (row[i] == 0) emptyFields++; else break;
                }

                for (int i = partial[0] + partial[1]; i < row.Size; i++)
                {
                    if (row[i] == 0) emptyFields++; else break;
                }

                scores.Add((emptyFields >= Winning - partial[1]) ? (int)Math.Pow(10, partial[1]) : 0);
            }

            return (scores.Count == 0) ? 0 : scores.Max();
        }

    }
}
