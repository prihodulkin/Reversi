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
        public int Score;
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

        static int Heuristic(GamePosition position, Player maxPlayer, int stepNumber)
        {
            var opponent = maxPlayer.Opponent();
            var mCoeff = GetMobilityCoefficient(stepNumber);
            var sCoeff = GetStabilityCoefficient(stepNumber);
            var result = (position.GetDisksCount(maxPlayer) - position.GetDisksCount(opponent))+
                       2* position.GetMobility(maxPlayer)+
                       2* (position.GetPotintialMobility(maxPlayer) - position.GetPotintialMobility(opponent)) +
                       // (position.GetCornersCount(maxPlayer) - position.GetCornersCount(opponent)) +
                       // (position.GetEdgesSquaresCount(maxPlayer) - position.GetEdgesSquaresCount(opponent)) +
                       // (position.GetEdgesStability(maxPlayer) - position.GetEdgesSquaresCount(opponent))
                       +0;
            return result;
        }

        static int GetTerminalScore(GamePosition position, Player maxPlayer)
        {
            var blackCount = position.BlackCount;
            var whiteCount = position.WhiteCount;
            var score = blackCount > whiteCount ? int.MaxValue : blackCount == whiteCount ? 0 : int.MinValue;
            return maxPlayer == Player.Black ? score : -score; 
            
        }

        public static int MinimaxFun(GamePosition position, Player maxPlayer, int depth, int maxDepth)
        {
            if (position.IsTerminal())
            {
                return GetTerminalScore(position, maxPlayer);
            }
            if (depth == maxDepth)
            {
                return Heuristic(position, maxPlayer, depth);
            }
            if (position.Player == maxPlayer)
            {
                int score = int.MinValue;
                foreach (var p in position.GetAllPossibleSteps())
                {
                    var s = MinimaxFun(p, maxPlayer, depth + 1, maxDepth);
                    if (s > score)
                    {
                        score = s;
                    }
                }
                return score;
            }
            else
            {
                int score = int.MaxValue;
                foreach (var p in position.GetAllPossibleSteps())
                {
                    var s = MinimaxFun(p, maxPlayer, depth + 1, maxDepth);
                    if (s < score)
                    {
                        score = s;
                    }
                }
                return score;
            }
           
        }

        public static void MinimaxProcedure(Node node, Player maxPlayer, int depth)
        {
            var position = node.Position;
            if (position.IsTerminal())
            {
                node.Score = GetTerminalScore(position, maxPlayer);
                return;
            }
            if (depth == 0)
            {
                node.Score = Heuristic(position, maxPlayer, node.AbsDepth);
                return;
            }
            if (position.Player == maxPlayer)
            {
                int score = int.MinValue;
                foreach (var n in node.Descendants)
                {
                    MinimaxProcedure(n, maxPlayer, depth - 1);
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
                int score = int.MaxValue;
                foreach (var n in node.Descendants)
                {
                    MinimaxProcedure(n, maxPlayer, depth - 1);
                    var s = n.Score;
                    if (s  < score)
                    {
                        score = s;
                    }
                }
                node.Score = score;
            }
        }


        public static void MinimaxProcedureWithAlphaBetaPruning(Node node, Player maxPlayer, int depth, int alpha, int beta)
        {
            var position = node.Position;
            if (position.IsTerminal())
            {
                node.Score = GetTerminalScore(position, maxPlayer);
                return;
            }
            if (depth == 0)
            {
                node.Score = Heuristic(position, maxPlayer, node.AbsDepth);
                return;
            }

            if (position.Player == maxPlayer)
            {
                alpha = int.MinValue;
                int score = int.MinValue;
                foreach (var n in node.Descendants)
                {
                    MinimaxProcedureWithAlphaBetaPruning(n, maxPlayer, depth - 1, alpha, beta);
                    score = Math.Max(score, n.Score);
                    alpha = Math.Max(alpha, n.Score);
                    if (alpha <= beta)
                    {
                        break;
                    }
                }
                node.Score = score;
            }
            else
            {
                int score = int.MaxValue;
                foreach (var n in node.Descendants)
                {
                    MinimaxProcedureWithAlphaBetaPruning(n, maxPlayer, depth - 1, alpha, beta);
                    var s = n.Score;
                    score = Math.Min(score, n.Score);
                    beta = Math.Min(score, n.Score);
                    if (alpha <= beta)
                    {
                        break;
                    }
                }
                node.Score = score;
            }
        }
    }
}
