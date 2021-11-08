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
        static void Main(string[] args)
        {
            var level = 5;
            var positions = new GamePosition[level+1];
            for(int i=0; i<positions.Length; i++)
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
    }
}
