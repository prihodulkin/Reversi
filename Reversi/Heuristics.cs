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
        EdgeStability,
        StaticWeights
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
                case HeuristicsEnum.EdgeStability:
                    return Heuristics.EdgesStability;
                case HeuristicsEnum.StaticWeights:
                    return Heuristics.StaticWeights;
                case HeuristicsEnum.Mobility:
                    return Heuristics.Mobility;
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

        public static double EdgesStability(GamePosition position, Player maxPlayer)
        {
            var minPlayer = maxPlayer.Opponent();
            var maxPlayerValue = position.GetEdgesStability(maxPlayer);
            var minPlayerValue = position.GetEdgesStability(minPlayer);
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
    }
}
