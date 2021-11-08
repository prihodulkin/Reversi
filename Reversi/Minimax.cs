using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{

    class Node
    {
        public GamePosition Position;
        public double Score=-1;
        public int AbsDepth { get; private set; }
        Dictionary<Square, Node> descendants;

        public Node()
        {
            Position = new GamePosition();
        }
        Node(GamePosition position, int depth)
        {
            Position = position;
            AbsDepth = depth;
        }

        public IEnumerable<Node> Descendants
        {
            get
            {
                return DescendantsDict.Values;
            }
        }

        public Dictionary<Square, Node> DescendantsDict
        {
            get
            {
                if (descendants == null)
                {
                    descendants = new Dictionary<Square, Node>();
                    foreach(var p in Position.GetAllPossibleStepsWithSquares())
                    {
                        descendants[p.Item1] = new Node(p.Item2, AbsDepth+1);
                    }
                }
                return descendants;
            }
        }
        
    }

    enum MinimaxEnum
    {
        Simple,
        AlphaBeta,
    }

    static class Minimax
    {
    
        




        static int GetTerminalScore(GamePosition position, Player maxPlayer)
        {
            var blackCount = position.BlackCount;
            var whiteCount = position.WhiteCount;
            var score = blackCount > whiteCount ? int.MaxValue : blackCount == whiteCount ? 0 : int.MinValue;
            return maxPlayer == Player.Black ? score : -score; 
            
        }

        public static Square Find(GamePosition[] positions, Player maxPlayer, int depth, int maxDepth, HeuristicsEnum heuristics, MinimaxEnum fun)
        {
            var position = positions[depth];
            if (position.Player == maxPlayer)
            {
                double score = double.MinValue;
                Square result = position.PossibleStepsSquares.First();
                foreach (var square in position.PossibleStepsSquares)
                {
                    position.MakeStep(square, positions[depth + 1]);
                    var s = fun==MinimaxEnum.Simple?
                        MinimaxFun(positions, maxPlayer, depth + 1, maxDepth, heuristics): 
                        MinimaxFunWithAlphaBetaPruning(positions, maxPlayer, depth + 1, maxDepth, heuristics, double.MinValue, double.MaxValue);
                    if (s > score)
                    {
                        score = s;
                        result = square;
                    }
                }
                return result;
            }
            else
            {
                double score = double.MaxValue;
                Square result = position.PossibleStepsSquares.First();
                foreach (var square in position.PossibleStepsSquares)
                {
                    position.MakeStep(square, positions[depth + 1]);
                    var s = fun == MinimaxEnum.Simple ?
                      MinimaxFun(positions, maxPlayer, depth + 1, maxDepth, heuristics) :
                      MinimaxFunWithAlphaBetaPruning(positions, maxPlayer, depth + 1, maxDepth, heuristics, double.MinValue, double.MaxValue);
                    if (s < score)
                    {
                        score = s;
                        result = square;
                    }
                }
                return result;
            }

        }

        public static double MinimaxFun(GamePosition[] positions, Player maxPlayer, int depth, int maxDepth,  HeuristicsEnum heuristics)
        {
            var position = positions[depth];
            var Heuristic = heuristics.Func();
            if (position.IsTerminal())
            {
                return GetTerminalScore(position, maxPlayer);
            }
            if (depth == maxDepth)
            {
                return Heuristic(position, maxPlayer);
            }
            if (position.Player == maxPlayer)
            {
                double score = double.MinValue;
                foreach (var square in position.PossibleStepsSquares)
                {
                    position.MakeStep(square, positions[depth + 1]);
                    var s =  MinimaxFun(positions, maxPlayer, depth + 1, maxDepth,  heuristics);
                    if (s > score)
                    {
                        score = s;
                    }
                }
                return score;
            }
            else
            {
                double score = double.MaxValue;
                foreach (var square in position.PossibleStepsSquares)
                {
                    position.MakeStep(square, positions[depth + 1]);
                    var s = MinimaxFun(positions, maxPlayer, depth + 1, maxDepth, heuristics);
                    if (s < score)
                    {
                        score = s;
                    }
                }
                return score;
            }
           
        }

        public static double MinimaxFunWithAlphaBetaPruning(GamePosition[] positions, Player maxPlayer, int depth, int maxDepth, HeuristicsEnum heuristics, double alpha, double beta)
        {
            var position = positions[depth];
            var Heuristic = heuristics.Func();
            if (position.IsTerminal())
            {
                return GetTerminalScore(position, maxPlayer);
            }
            if (depth == maxDepth)
            {
                return Heuristic(position, maxPlayer);
            }
            if (position.Player == maxPlayer)
            {
                double score = double.MinValue;
                alpha = double.MinValue;
                foreach (var square in position.PossibleStepsSquares)
                {
                    position.MakeStep(square, positions[depth + 1]);
                    var s = MinimaxFunWithAlphaBetaPruning(positions, maxPlayer, depth + 1, maxDepth, heuristics, alpha, beta);
                    score = Math.Max(score, s);
                    alpha = Math.Max(alpha, score);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                return score;
            }
            else
            {
                double score = double.MaxValue;
                beta = double.MaxValue;
                foreach (var square in position.PossibleStepsSquares)
                {
                    position.MakeStep(square, positions[depth + 1]);
                    var s = MinimaxFunWithAlphaBetaPruning(positions, maxPlayer, depth + 1, maxDepth, heuristics, alpha, beta);
                    score = Math.Min(score, s);
                    beta = Math.Min(score, beta);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                return score;
            }

        }

        public static void MinimaxProcedure(Node node, Player maxPlayer, int depth, HeuristicsEnum heuristics)
        {
            var Heuristic = heuristics.Func();
            var position = node.Position;
            if (position.IsTerminal())
            {
                node.Score = GetTerminalScore(position, maxPlayer);
                return;
            }
            if (depth == 0)
            {
                node.Score = Heuristic(position, maxPlayer);
                return;
            }
            if (position.Player == maxPlayer)
            {
                double score = double.MinValue;
                foreach (var n in node.Descendants)
                {
                    MinimaxProcedure(n, maxPlayer, depth - 1, heuristics);
                    var s = n.Score;
                    if (s > score)
                    {
                        score = s;
                    }
                }
                node.Score = score;
            }
            else
            {
                double score = double.MaxValue;
                foreach (var n in node.Descendants)
                {
                    MinimaxProcedure(n, maxPlayer, depth - 1, heuristics);
                    var s = n.Score;
                    if (s  < score)
                    {
                        score = s;
                    }
                }
                node.Score = score;
            }
        }


        public static void MinimaxProcedureWithAlphaBetaPruning(Node node, Player maxPlayer, int depth, double alpha, double beta, HeuristicsEnum heuristics)
        {
            var Heuristic = heuristics.Func();
            var position = node.Position;
            if (position.IsTerminal())
            {
                node.Score = GetTerminalScore(position, maxPlayer);
                return;
            }
            if (depth == 0)
            {
                node.Score = Heuristic(position, maxPlayer);
                return;
            }

            if (position.Player == maxPlayer)
            {
                alpha = double.MinValue;
                double score = double.MinValue;
                foreach (var n in node.Descendants)
                {
                    MinimaxProcedureWithAlphaBetaPruning(n, maxPlayer, depth - 1, alpha, beta, heuristics);
                    score = Math.Max(score, n.Score);
                    alpha = Math.Max(alpha, score);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                node.Score = score;
            }
            else
            {
                double score = double.MaxValue;
                foreach (var n in node.Descendants)
                {
                    MinimaxProcedureWithAlphaBetaPruning(n, maxPlayer, depth - 1, alpha, beta, heuristics);
                    var s = n.Score;
                    score = Math.Min(score, n.Score);
                    beta = Math.Min(score, score);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                node.Score = score;
            }
        }
    }
}
