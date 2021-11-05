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
    static class Minimax
    {
        static int GetMobilityCoefficient(int stepNumber)
        {
            return stepNumber < 20 ? 3 : stepNumber < 40 ? 2 : 1;
        }

        static int GetStabilityCoefficient(int stepNumber)
        {
            return stepNumber < 20 ? 1 : stepNumber < 40 ? 2 : 3;
        }
        
        
    

        static int GetTerminalScore(GamePosition position, Player maxPlayer)
        {
            var blackCount = position.BlackCount;
            var whiteCount = position.WhiteCount;
            var score = blackCount > whiteCount ? int.MaxValue : blackCount == whiteCount ? 0 : int.MinValue;
            return maxPlayer == Player.Black ? score : -score; 
            
        }

        public static double MinimaxFun(GamePosition position, Player maxPlayer, int depth, int maxDepth, HeuristicsEnum heuristics)
        {
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
                foreach (var p in position.GetAllPossibleSteps())
                {
                    var s = MinimaxFun(p, maxPlayer, depth + 1, maxDepth, heuristics);
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
                foreach (var p in position.GetAllPossibleSteps())
                {
                    var s = MinimaxFun(p, maxPlayer, depth + 1, maxDepth, heuristics);
                    if (s < score)
                    {
                        score = s;
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
