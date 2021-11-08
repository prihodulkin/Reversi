using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    class Program
    {
        static void SelfPlaying()
        {
            var level = 7;
            var positions = new GamePosition[level + 1];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new GamePosition();
            }
            Stopwatch stopwatchAll = new Stopwatch();
            stopwatchAll.Start();
            while (!positions[0].IsTerminal())
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var step = Minimax.Find(positions, positions[0].Player, 0, level, HeuristicsEnum.Mobility, MinimaxEnum.AlphaBeta);
                stopwatch.Stop();
                Console.WriteLine(positions[0]);
                Console.WriteLine("Step Time is {0} ms\n", stopwatch.ElapsedMilliseconds);
                positions[0].MakeStep(step, positions[0]);
            }
            stopwatchAll.Stop();
            Console.WriteLine("GAME Time is {0} ms\n", stopwatchAll.ElapsedMilliseconds);
            Console.ReadKey();
        }

        static int Play(string[] args)
        {
            
            var c = int.Parse(args.Last());
            var player = c == 0 ? Player.Black : Player.White;
            var opponent = player.Opponent();
            var level = args.Count()==2? int.Parse(args[0]): 6;
            Console.WriteLine(level);
            var positions = new GamePosition[level + 1];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new GamePosition();
            }
            if (player == Player.Black)
            {
                var step = Minimax.Find(positions, positions[0].Player, 0, level, 
                    HeuristicsEnum.MobilityAndStability, MinimaxEnum.AlphaBeta);
                positions[0].MakeStep(step, positions[0]);
                Console.Error.WriteLine(step);
            }
            while (true)
            {
                while (positions[0].Player != player)
                {
                    var s = Console.ReadLine();
                    var step = new Square(s);
                    positions[0].MakeStep(step, positions[0]);
                    if (positions[0].IsTerminal())
                    {
                        var playerScore = positions[0].GetCount(player);
                        var opponentScore = positions[0].GetCount(opponent);
                        return playerScore > opponentScore ? 0 :
                            playerScore == opponentScore ? 4 : 3;
                    }
                }

                while (positions[0].Player == player)
                {
                    var step = Minimax.Find(positions, positions[0].Player, 0, level,
                    HeuristicsEnum.MobilityAndStability, MinimaxEnum.AlphaBeta);
                    positions[0].MakeStep(step, positions[0]);
                    Console.Error.WriteLine(step);
                    if (positions[0].IsTerminal())
                    {
                        var playerScore = positions[0].GetCount(player);
                        var opponentScore = positions[0].GetCount(opponent);
                        return playerScore > opponentScore ? 0 :
                            playerScore == opponentScore ? 4 : 3;
                    }
                }
            } 
        }


        static int Main(string[] args)
        {
            //var s = new Square("a5"); 
            //Console.WriteLine(s);
            //Console.ReadKey();
           return Play(args);
            
        }
    }
}
