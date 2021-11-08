using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    public struct Shift
    {
        public int Item1;
        public int Item2;
        public Shift(int i1, int i2)
        {
            Item1 = i1;
            Item2 = i2;
        }
    }
    public struct Square
    {
        static Shift[] shiftDirections = new Shift[8]
        {
           new Shift(0, 1),
           new Shift(0, -1),
           new Shift(1, 0),
           new Shift(-1, 0),
           new Shift(-1, -1),
           new Shift(1,  1),
           new Shift(1, -1),
           new Shift(-1, 1) 
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

        public List<Shift> GetNeightbourDirections()
        {
            List<Shift> result = new List<Shift>(8);
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

        public Square Next(Shift direction) {
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

        public static string SquareToString(this SquareState square)
        {
            switch (square)
            {
                case SquareState.Black:
                    return "B";
                case SquareState.White:
                    return "W";
                case SquareState.Empty:
                    return "_";
                default: throw new ArgumentException();
            }
        }
    }


    public class GamePosition
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

        public HashSet<Square> WhitePieces { get; private set; }
        public HashSet<Square> BlackPieces { get; private set; }

        public int Depth { get; private set; }

        SquareState[,] field;
        Player player;
        public Dictionary<Square, Dictionary<Shift, Square>> PossibleSteps;

        public SquareState[,] Field
        {
            get
            {
                return (SquareState[,])field.Clone();
            }
        }


        public Player Player
        {
            get
            {
                return player;
            }

            private set
            {
                player = value;
            }
        }

        List<Square> possibleStepSquares = new List<Square>();
        public IEnumerable<Square> PossibleStepsSquares
        {
            get 
            {
                //if (possibleStepSquares.Count == 0)
                //{
                //    foreach(var s in PossibleSteps.Keys)
                //    {
                //        possibleStepSquares.Add(s);
                //    }
                //    possibleStepSquares.Sort((s1, s2) => Weights[s1.X, s1.Y].CompareTo(Weights[s2.X, s2.Y]));

                //}
                //return possibleStepSquares;
                return PossibleSteps.Keys;
            }
        }
        public int WhiteCount => WhitePieces.Count;
        public int BlackCount => BlackPieces.Count;

        public double Score = -1;

        void initField()
        {
            field = new SquareState[8, 8];
            foreach(var p in WhitePieces)
            {
                field[p.X, p.Y] = SquareState.White;
            }
            foreach (var p in BlackPieces)
            {
                field[p.X, p.Y] = SquareState.Black;
            }
        }

        void findPossibleStep(Square square, Shift direction)
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
                    if (PossibleSteps.ContainsKey(square))
                    {
                        PossibleSteps[square][direction] =start;
                    }
                    else
                    {
                        PossibleSteps[square] = new Dictionary<Shift, Square>() { {direction, start } };
                    }
                    break;
                }
                square = square.Next(direction);
            }
        }

        void findPossibleSteps()
        {
            this.PossibleSteps = new Dictionary<Square, Dictionary<Shift, Square>>();
            var pieces = player == Player.Black ? BlackPieces : WhitePieces;
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
                foreach (var p1 in PossibleSteps[to])
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
            WhitePieces = wPieces;
            BlackPieces = bPieces;
            this.player = player;
            initField();
            findPossibleSteps();
       }

        public GamePosition()
        {
            WhitePieces = new HashSet<Square>() {new Square(3,3), new Square(4,4) };
            BlackPieces = new HashSet<Square>() { new Square(3, 4), new Square(4, 3) };
            initField();
            findPossibleSteps();
        }

        public bool IsTerminal()
        {
            return BlackPieces.Count + WhitePieces.Count == 64||PossibleSteps.Count==0;
        }

        public GamePosition MakeStep(Square square)
        {
            if (!PossibleSteps.ContainsKey(square))
            {
                throw new ArgumentException("Incorrect square for step!");
            }
            var wPieces = new HashSet<Square>(WhitePieces);
            var bPieces = new HashSet<Square>(BlackPieces);
            var playerPieces = player == Player.Black ? bPieces:wPieces;
            var opponentPieces = player == Player.Black ? wPieces : bPieces;
            
            var newPlayer = player;
            if (PossibleSteps.Count > 0)
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

        public void MakeStep(Square square, GamePosition position)
        {
            if (!PossibleSteps.ContainsKey(square))
            {
                throw new ArgumentException("Incorrect square for step!");
            }
            position.WhitePieces = new HashSet<Square>(WhitePieces);
            position.BlackPieces = new HashSet<Square>(BlackPieces);
            var playerPieces = player == Player.Black ? position.BlackPieces : position.WhitePieces;
            var opponentPieces = player == Player.Black ? position.WhitePieces : position.BlackPieces;

            var newPlayer = player;
            if (PossibleSteps.Count > 0)
            {
                newPlayer = player.Opponent();
                var changedSquares = getChangedSquares(square);
                foreach (var s in changedSquares)
                {
                    playerPieces.Add(s);
                    opponentPieces.Remove(s);
                }
            }

            position.Player = newPlayer;
            position.initField();
            position.possibleStepSquares.Clear();
            position.findPossibleSteps();
            position.Depth = Depth + 1;
        }

        public IEnumerable<GamePosition> GetAllPossibleSteps()
        {
            foreach(var p in PossibleSteps)
            {
               yield return MakeStep(p.Key);
            }   
        }

        public List<(Square, GamePosition)> GetAllPossibleStepsWithSquares()
        {
            List<(Square, GamePosition)> result = new List<(Square, GamePosition)>(PossibleSteps.Count);
            foreach (var p in PossibleSteps)
            {
                result.Add((p.Key, MakeStep(p.Key)));
            }
            return result;
        }

        public int GetMobility(Player maxPlayer)
        {
            return maxPlayer==Player? PossibleSteps.Count:-PossibleSteps.Count;
        }

        public int GetPotentialMobility(Player player)
        {

            HashSet<Square> emptySquares = new HashSet<Square>();
            foreach(var s in player == Player.White ? BlackPieces : WhitePieces)
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

        public int GetStability(Player player)
        {
            int result = 0;
            var playerState = player.PlayerState();
            for(int i = 0; i < 8; i++)
            {
               for(int j=0; j<8; j++)
               {
                    if (field[i, j] == playerState)
                    {
                        result++;
                    }
                    else
                    {
                        break;
                    }
               }

                for (int j = 0; j < 8; j++)
                {
                    if (field[j, i] == playerState)
                    {
                        result++;
                    }
                    else
                    {
                        break;
                    }
                }

                for (int j = 0; j < 8; j++)
                {
                    if (field[i, 7-j] == playerState)
                    {
                        result++;
                    }
                    else
                    {
                        break;
                    }
                }

                for (int j = 0; j < 8; j++)
                {
                    if (field[7 - i,  j] == playerState)
                    {
                        result++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return result;
        }

        public int GetDisksCount(Player player)
        {
            return player == Player.Black ? BlackCount : WhiteCount;
        }


        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for(int i = 0; i < 8; i++)
            {
                for(int j=0; j<8; j++)
                {
                    stringBuilder.Append(field[i,j].SquareToString());
                }
                stringBuilder.Append("\n");
            }
            return stringBuilder.ToString();
        }
    }
}
