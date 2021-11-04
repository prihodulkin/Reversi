using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    public struct Square
    {
        static Tuple<int, int>[] shiftDirections = new Tuple<int, int>[8]
        {
           new Tuple<int,int>(0, 1),
           new Tuple<int,int>(0, -1),
           new Tuple<int,int>(1, 0),
           new Tuple<int,int>(-1, 0),
           new Tuple<int,int>(-1, -1),
           new Tuple<int,int>(1,  1),
           new Tuple<int,int>(1, -1),
           new Tuple<int,int>(-1, 1) 
        };

        int x;
        int y;

        public Square(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || !(obj is Square))
                return false;
            else
                return x == ((Square)obj).x && y ==((Square)obj).y;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + X.GetHashCode();
            hash = hash * 23 + Y.GetHashCode();
            return hash;
        }

        public List<Tuple<int,int>> GetNeightbourDirections()
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>(8);
            foreach(var s in shiftDirections)
            {
                var newX = x + s.Item1;
                var newY = y + s.Item2;
                if (newX >= 0 && newX <= 7 && newY >= 0 && newY <= 7)
                {
                    result.Add(s);
                }
            }
            return result;
        }

        public List<Square> GetNeightbours()
        {
            var x1 = x;
            var y1 = y;
            return GetNeightbourDirections().Select(d => new Square(x1 + d.Item1, y1 + d.Item2)).ToList();
        }

        public Square Next(Tuple<int,int> direction) {
            return new Square(x + direction.Item1, y + direction.Item2);
        }

        public bool InField => x >= 0 && x <= 7 && y >= 0 && y <= 7;

        public int X => x;
        public int Y => y;
    }

    public enum SquareState
    {
        Empty,
        Black,
        White,    
    }

    //избегает нетолерантного использования bool
    public enum Player
    {
        Black,
        White,
    }

    public static class PlayerExtension
    {
        public static Player Opponent(this Player player)
        {
            return player==Player.Black? Player.White:Player.Black;
        }

        public static SquareState PlayerState(this Player player)
        {
            return player == Player.Black ? SquareState.Black: SquareState.White;
        }

        public static SquareState OpponentState(this Player player)
        {
            return player == Player.Black ? SquareState.White : SquareState.Black;
        }
    }


    public class GamePosition
    {
        HashSet<Square> whitePieces;
        HashSet<Square> blackPieces;
        SquareState[,] field;
        SquareState[,] edges;
        Player player;
        Dictionary<Square, Dictionary<Tuple<int, int>, Square>> possibleSteps;

        public SquareState[,] Field
        {
            get
            {
                return (SquareState[,])field.Clone();
            }
        }


        public Player Player => player;
        public List<Square> PossibleStepsSquares => possibleSteps.Keys.ToList();
        public HashSet<Square> WhitePieces => whitePieces;
        public HashSet<Square> BlackPieces => blackPieces;
        public int WhiteCount => whitePieces.Count;
        public int BlackCount => blackPieces.Count;



        void initEdgesWithPieces(SquareState state)
        {
            if (state == SquareState.Empty)
            {
                throw new ArgumentException("State shouldn't be empty!");
            }
            var pieces = state == SquareState.Black ? blackPieces : whitePieces;
            foreach (var p in pieces)
            {
                if (p.X == 0)
                {
                    edges[0, p.Y] = state;
                }
                if (p.X == 7)
                {
                    edges[1, p.Y] = state;
                }
                if (p.Y == 0)
                {
                    edges[2, p.X] = state;
                }
                if (p.Y == 7)
                {
                    edges[3, p.X] = state;
                }
            }
        }

        void initEdges()
        {
            edges = new SquareState[4,8];
            initEdgesWithPieces(SquareState.Black);
            initEdgesWithPieces(SquareState.White);
        }

        void initField()
        {
            field = new SquareState[8, 8];
            foreach(var p in whitePieces)
            {
                field[p.X, p.Y] = SquareState.White;
            }
            foreach (var p in blackPieces)
            {
                field[p.X, p.Y] = SquareState.Black;
            }
        }

        void findPossibleStep(Square square, Tuple<int,int> direction)
        {
            var start = square;
            square = square.Next(direction);
            var playerState = player.PlayerState();
            while (square.InField)
            {
                var squareState = field[square.X, square.Y];
                if (squareState == playerState)
                {
                    return;
                }
                if (squareState == SquareState.Empty)
                {
                    if (possibleSteps.ContainsKey(square))
                    {
                        possibleSteps[square][direction] =start;
                    }
                    else
                    {
                        possibleSteps[square] = new Dictionary<Tuple<int, int>, Square>() { {direction, start } };
                    }
                    break;
                }
                square = square.Next(direction);
            }
        }

        void findPossibleSteps()
        {
            this.possibleSteps = new Dictionary<Square, Dictionary<Tuple<int, int>, Square>>();
            var pieces = player == Player.Black ? blackPieces : whitePieces;
            var opponentState = player.OpponentState();
            foreach(var p in pieces)
            {
                foreach(var dir in p.GetNeightbourDirections())
                {
                    var next = p.Next(dir);
                    if(field[next.X, next.Y] == opponentState)
                    {
                        findPossibleStep(next, dir);
                    }
                }
            }

        }

        IEnumerable<Square> getChangedSquares(Square to)
        {
                foreach (var p1 in possibleSteps[to])
                { 
                    var dir = p1.Key;
                    var from = p1.Value;
                    while (!from.Equals(to))
                    {
                        yield return from;
                        from = from.Next(dir);
                    }
                    yield return to;
                }
            
        }

       GamePosition(HashSet<Square> wPieces, HashSet<Square> bPieces, Player player)
       {
            whitePieces = wPieces;
            blackPieces = bPieces;
            this.player = player;
            initField();
            initEdges();
            findPossibleSteps();
       }

        public GamePosition()
        {
            whitePieces = new HashSet<Square>() {new Square(3,3), new Square(4,4) };
            blackPieces = new HashSet<Square>() { new Square(3, 4), new Square(4, 3) };
            initField();
            initEdges();
            findPossibleSteps();
        }

        public bool IsTerminal()
        {
            return blackPieces.Count + whitePieces.Count == 64||possibleSteps.Count==0;
        }

        public GamePosition MakeStep(Square square)
        {
            if (!possibleSteps.ContainsKey(square))
            {
                throw new ArgumentException("Incorrect square for step!");
            }
            var wPieces = new HashSet<Square>(whitePieces);
            var bPieces = new HashSet<Square>(blackPieces);
            var playerPieces = player == Player.Black ? bPieces:wPieces;
            var opponentPieces = player == Player.Black ? wPieces : bPieces;
            
            var newPlayer = player;
            if (possibleSteps.Count > 0)
            {
                newPlayer = player.Opponent();
                var changedSquares = getChangedSquares(square).ToList();
                foreach (var s in changedSquares)
                {
                    playerPieces.Add(s);
                    opponentPieces.Remove(s);
                }
            }
           
            return new GamePosition(wPieces, bPieces, newPlayer);
        }

        public List<GamePosition> GetAllPossibleSteps()
        {
            List<GamePosition> result = new List<GamePosition>(possibleSteps.Count);
            foreach(var p in possibleSteps)
            {
                result.Add(MakeStep(p.Key));
            }
            return result;
        }

        public List<(Square, GamePosition)> GetAllPossibleStepsWithSquares()
        {
            List<(Square, GamePosition)> result = new List<(Square, GamePosition)>(possibleSteps.Count);
            foreach (var p in possibleSteps)
            {
                result.Add((p.Key, MakeStep(p.Key)));
            }
            return result;
        }

        public int GetMobility()
        {
            return possibleSteps.Count;
        }

        public int GetPotintialMobility(Player player)
        {

            HashSet<Square> emptySquares = new HashSet<Square>();
            foreach(var s in player == Player.White ? blackPieces : whitePieces)
            {
                foreach(var n in s.GetNeightbours())
                {
                    emptySquares.Add(n);
                }
            }
            return emptySquares.Count;
        }

        public int GetCornersCount(Player player)
        {
            var playerState = player.PlayerState();
            int result = 0;
            foreach(var s in new SquareState[4]{ field[0,0], field[0,7], field[7,0], field[7,7] })
            {
                if (s == playerState)
                {
                    result++;
                }
            }
            return result;
        }

        public int GetEdgesSquaresCount(Player player)
        {
            var playerState = player.PlayerState();
            int result = 0;
            foreach(var s in edges)
            {
                if (s == playerState)
                {
                    result++;
                }
            }
            return result;
        }

        public int GetEdgesStability(Player player)
        {
            var playerState = player.PlayerState();
            int result = 0;
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (field[i, j] != playerState)
                    {
                        break;
                    }
                    result++;
                }
                for (int j = 7; j >=0; j--)
                {
                    if (field[i, j] != playerState)
                    {
                        break;
                    }
                    result++;
                }
            }
            return result - GetCornersCount(player);
        }
    }
}
