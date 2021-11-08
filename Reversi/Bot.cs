using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    public interface Bot
    {
        int Level { get; }

        Player Player { get; }
        Player Opponent { get; }
        HeuristicsEnum Heuristics { get; }

       void OpponentStep(Square square);

        IEnumerable<Square> BotSteps();
    }
    public class BotWithNodes : Bot
    {
        /// <summary>
        /// Уровень - глубина алгоритма
        /// </summary>
        public int Level { get;  }

        /// <summary>
        /// За кого бот - за чёрных или за белых 
        /// </summary>
        public Player Player { get; }
        public Player Opponent { get; }

        public HeuristicsEnum Heuristics { get;  }

        Node node = new Node();

        List<Square> steps;

        GamePosition position
        {
            get
            {
                return node.Position;
            }
        }

        public BotWithNodes(Player player, int level, HeuristicsEnum heuristics)
        {
            Player = player;
            Opponent = player.Opponent();
            Level = level;
            steps = new List<Square>();
            Heuristics = heuristics;
        }

        void checkStepCorrectness(Player player)
        {
            if (position.Player != player)
            {
                throw new InvalidOperationException("Попытка хода вне очереди ");
            }
            if (position.IsTerminal())
            {
                throw new InvalidOperationException("Попытка хода после окончания игры");
            }
        }

        public void OpponentStep(Square square)
        {
            checkStepCorrectness(Opponent);
            node = node.DescendantsDict[square];
            steps.Clear();
        }

      

        public IEnumerable<Square> BotSteps()
        {
            checkStepCorrectness(Player);
            if (steps.Count > 0)
            {
                return steps;
            }
            while (position.Player == Player)
            {
                Minimax.MinimaxProcedureWithAlphaBetaPruning(node, Player, Level, double.MinValue, double.MaxValue, Heuristics);

                var descDict = node.DescendantsDict;
                var maxP = descDict.First();
                foreach(var p in descDict)
                {
                    if (p.Value.Score > p.Value.Score)
                    {
                        maxP = p;
                    }
                }
                node = maxP.Value;
                steps.Add(maxP.Key);
            }
            return steps;
        }
    }


    public class EffectiveBot : Bot
    {
        public int Level { get; }

        public Player Player  { get; }

        public Player Opponent { get; }

        public HeuristicsEnum Heuristics { get; }

        List<Square> steps;

        GamePosition[] positions;

        public EffectiveBot(Player player, int level, HeuristicsEnum heuristics)
        {
            Player = player;
            Opponent = player.Opponent();
            Level = level;
            Heuristics = heuristics;
            positions = new GamePosition[Level+1];
            for(int i = 0; i <= Level; i++)
            {
                positions[i] = new GamePosition();
            }
            steps = new List<Square>();
        }

        public IEnumerable<Square> BotSteps()
        {
            checkStepCorrectness(Player);
            if (steps.Count > 0)
            {
                return steps;
            }
            while (positions[0].Player == Player)
            {
                var step = Minimax.Find(positions, positions[0].Player, 0, Level,
                    Heuristics, MinimaxEnum.AlphaBeta);
                positions[0].MakeStep(step, positions[0]);
                steps.Add(step);
            }
            return steps;
        }
    

        void checkStepCorrectness(Player player)
        {
            if (positions[0].Player != player)
            {
                throw new InvalidOperationException("Попытка хода вне очереди ");
            }
            if (positions[0].IsTerminal())
            {
                throw new InvalidOperationException("Попытка хода после окончания игры");
            }
        }

        public void OpponentStep(Square square)
        {
            checkStepCorrectness(Opponent);
            positions[0].MakeStep(square, positions[0]);
            steps.Clear();
        }
    }
}
