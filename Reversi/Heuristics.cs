using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    public enum HeuristicsEnum
    {
        CoinPartly,
        CornersCount,
        Mobility,
        Stability,
        StaticWeights,
        MobilityAndStability,
    }

    public static class HeuristicsExtension
    {
        public static Func<GamePosition, Player, double> Func(this HeuristicsEnum h)
        {
            switch (h)
            {
                case HeuristicsEnum.CoinPartly:
                    return Heuristics.CoinParity;
                case HeuristicsEnum.CornersCount:
                    return Heuristics.CornersCount;
                case HeuristicsEnum.Stability:
                    return Heuristics.Stability;
                case HeuristicsEnum.StaticWeights:
                    return Heuristics.StaticWeights;
                case HeuristicsEnum.Mobility:
                    return Heuristics.Mobility;
                case HeuristicsEnum.MobilityAndStability:
                    return Heuristics.MobilityAndStability;
                default:
                    throw new ArgumentException();
            }
        }
    }


    public static class Heuristics
    {
        static int[,] Weights = new int[8, 8]
        {
            {4,-3,2,2,2,2,-3,4 },
            {-3,-4,-1,-1,-1,-1,-4,-3 },
            {2,-1,1,0,0,1,-1,2 },
            {2,-1,0,1,1,0,-1,2 },
            {2,-1,0,1,1,0,-1,2 },
            {2,-1,1,0,0,1,-1,2 },
            {-3,-4,-1,-1,-1,-1,-4,-3 },
            {4,-3,2,2,2,2,-3,4 }
        };


        public static double CoinParity(GamePosition position, Player maxPlayer)
        {
            var minPlayer = maxPlayer.Opponent();
            var maxPlayerCoins = position.GetDisksCount(maxPlayer);
            var minPlayerCoins = position.GetDisksCount(minPlayer);
            return 100 * (maxPlayerCoins - minPlayerCoins) * 1.0 / (minPlayerCoins + maxPlayerCoins);
        }


        public static double Mobility(GamePosition position, Player maxPlayer)
        {
            var minPlayer = maxPlayer.Opponent();
            var maxPlayerValue = position.GetPotentialMobility(maxPlayer);
            var minPlayerValue = position.GetPotentialMobility(minPlayer);
            return minPlayerValue+maxPlayerValue==0? 0:
                100 * (maxPlayerValue - minPlayerValue) * 1.0 / (minPlayerValue + maxPlayerValue);
        }

        public static double CornersCount(GamePosition position, Player maxPlayer)
        {
            var minPlayer = maxPlayer.Opponent();
            var maxPlayerValue = position.GetCornersCount(maxPlayer);
            var minPlayerValue = position.GetCornersCount(minPlayer);
            return minPlayerValue + maxPlayerValue == 0 ? 0 :
                100 * (maxPlayerValue - minPlayerValue) * 1.0 / (minPlayerValue + maxPlayerValue);
        }

        public static double Stability(GamePosition position, Player maxPlayer)
        {
            var minPlayer = maxPlayer.Opponent();
            var maxPlayerValue = position.GetStability(maxPlayer);
            var minPlayerValue = position.GetStability(minPlayer);
            return minPlayerValue + maxPlayerValue == 0 ? 0 :
                100 * (maxPlayerValue - minPlayerValue) * 1.0 / (minPlayerValue + maxPlayerValue);
        }

        public static double StaticWeights(GamePosition position, Player maxPlayer)
        {
            var isBlack = maxPlayer == Player.Black;
            var maxSquares = isBlack ? position.BlackPieces : position.WhitePieces;
            var minSquares = isBlack ? position.WhitePieces : position.BlackPieces;
            var maxPlayerValue = 0;
            var minPlayerValue = 0;
            foreach(var s in minSquares)
            {
                minPlayerValue += Weights[s.X, s.Y];
            }
            foreach (var s in maxSquares)
            {
                maxPlayerValue += Weights[s.X, s.Y];
            }
            return minPlayerValue + maxPlayerValue == 0 ? 0 :
                100 * (maxPlayerValue - minPlayerValue) * 1.0 / (minPlayerValue + maxPlayerValue);
        }

        public static double GetMobilityCoefficient(int depth)
        {
            //if (depth < 5)
            //{
            //    return 5;
            //}
            //else if (depth < 10)
            //{
            //    return 4;
            //}
            //else if (depth < 15)
            //{
            //    return 3;
            //}
            //else if (depth < 20)
            //{
            //    return 2;
            //}
            //return 1;
            if (depth < 20)
            {
                return 3;
            }
            else if (depth < 40)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        public static double GetStabilityCoefficient(int depth)
        {
            //if (depth < 5)
            //{
            //    return 1;
            //}
            //else if (depth < 10)
            //{
            //    return 2;
            //}
            //else if (depth < 15)
            //{
            //    return 3;
            //}
            //else if (depth < 20)
            //{
            //    return 4;
            //}
            //return 5;
            if (depth < 20)
            {
                return 1;
            }
            else if (depth < 40)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        public static double MobilityAndStability(GamePosition position, Player maxPlayer)
        {
            //var isBlack = maxPlayer == Player.Black;
            //var maxSquares = isBlack ? position.BlackPieces : position.WhitePieces;
            //var minSquares = isBlack ? position.WhitePieces : position.BlackPieces;
            //var maxPlayerWeight = 0;
            //var minPlayerWeight= 0;
            //foreach (var s in minSquares)
            //{
            //    minPlayerWeight += Weights[s.X, s.Y];
            //}
            //foreach (var s in maxSquares)
            //{
            //    minPlayerWeight += Weights[s.X, s.Y];
            //}
            var minPlayer = maxPlayer.Opponent();
            var maxPlayerValue =
                ( maxPlayer ==Player.Black? position.WhiteCount : position.BlackCount +
                GetStabilityCoefficient(position.Depth) * position.GetStability(maxPlayer) +
                GetMobilityCoefficient(position.Depth) * position.GetMobility(maxPlayer));
            var minPlayerValue =
                 (minPlayer == Player.Black ? position.WhiteCount : position.BlackCount +
                 GetStabilityCoefficient(position.Depth) * position.GetStability(minPlayer) +
                 GetMobilityCoefficient(position.Depth) * position.GetMobility(minPlayer));
            return minPlayerValue + maxPlayerValue == 0 ? 0 :
                100 * (maxPlayerValue - minPlayerValue) * 1.0 / (minPlayerValue + maxPlayerValue);
        }
    }
}
