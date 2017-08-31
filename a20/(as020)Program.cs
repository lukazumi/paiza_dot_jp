using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace a020
{
    [Serializable]
    public class Program 
    {
        List<string> input1;
        private List<List<char>> board;
        int boardSize;
        int checkStrCnt;
        List<Point> visited;
        Point currentPosition;
        string currentStr;

        public Program()
        {
            currentPosition = new Point();
            currentPosition.X = 0;
            currentPosition.Y = 0;
            currentStr="";
            visited = new List<Point>();
        }

        public bool generateBoard()
        {
            //input1 = new List<string>() { "3", "abc", "cab", "bca", "3", "aca", "bca", "bcc" };
            input1 = new List<string>() { "4", "abcd", "efgh", "hgfe", "dcba", "5", "abfgf", "bfgc", "abfga", "hdc", "fghde" }; 
            boardSize = Int32.Parse(input1[0]);
            //var line = System.Console.ReadLine();
            //boardSize = Int32.Parse(line);

            List<string> faces = new List<string>();
            for (int x = 0; x < boardSize; x++)
            {
                faces.Add(input1[x + 1]);
                //faces.Add(System.Console.ReadLine());
            }

            board = new List<List<char>>();
            for (int x = 0; x < boardSize; x++)
            {
                board.Add(new List<char>());
            }
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    board[x].Add(faces[x][y]);
                }
            }

            checkStrCnt = Int32.Parse(input1[boardSize + 1]);
            //line = System.Console.ReadLine();
            //checkStrCnt = Int32.Parse(line);
            return true;
        }

        public string findString(string findStr, Program p)
        {
            if (p.currentStr == "")//find the first letter
            {
                for (int y = 0; y < boardSize; y++)
                {
                    for (int x = 0; x < boardSize; x++)
                    {
                        if (board[y][x] == findStr[0])
                        {
                            Program p2 = DeepClone(p);
                            p2.currentPosition = new Point(y, x);
                            p2.visited.Add(currentPosition);
                            if (p2.explore(findStr, p2) == "yes")
                                return "yes";
                        }
                    }
                }
                return "no";
            }
            else
            {
                return p.explore(findStr, p);
            }
        }

        public string explore(string findStr, Program p)
        {
            p.visited.Add(currentPosition);

            bool validSquare = true;

            //check if this completes the string
            try
            {
                p.currentStr += board[p.currentPosition.Y][p.currentPosition.X];
            }
            catch (ArgumentOutOfRangeException e)
            {
                validSquare = false;
            }

            if (validSquare)
            {
                if (p.currentStr == findStr)
                    return "yes";
                if (p.currentStr.Length >= findStr.Length)
                    return "no";

                //go to next box

                //up
                Point nextPoint = new Point(p.currentPosition.Y - 1, p.currentPosition.X);
                if (!p.visited.Contains(nextPoint))
                {
                    Program np = DeepClone(this);
                    np.currentPosition = nextPoint;
                    if (np.findString(findStr, np) == "yes")
                        return "yes";
                }

                if (p.visited.Count == 1)//debug
                {

                }

                //down
                nextPoint = new Point(p.currentPosition.Y + 1, p.currentPosition.X);
                if (!p.visited.Contains(nextPoint))
                {
                    Program np = DeepClone(this);
                    np.currentPosition = nextPoint;
                    if (np.findString(findStr, np) == "yes")
                        return "yes";
                }

                //left
                nextPoint = new Point(p.currentPosition.Y, p.currentPosition.X - 1);
                if (!p.visited.Contains(nextPoint))
                {
                    Program np = DeepClone(this);
                    np.currentPosition = nextPoint;
                    if (np.findString(findStr, np) == "yes")
                        return "yes";
                }

                //right
                nextPoint = new Point(p.currentPosition.Y, p.currentPosition.X + 1);
                if (!p.visited.Contains(nextPoint))
                {
                    Program np = DeepClone(this);
                    np.currentPosition = nextPoint;
                    if (np.findString(findStr, np) == "yes")
                        return "yes";
                }
            }
            return "no";
        }

        [Serializable]
        public struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Point(int _x, int _y)
            {
                X = _y;
                Y = _x;
            }
        }


        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.generateBoard();
            for (int x = 0; x < p.checkStrCnt; x++)
            {
                var line = p.input1[(p.boardSize + 2) + x];
                //var line = System.Console.ReadLine();
                string result = p.findString(line, DeepClone(p));
                System.Console.WriteLine(result);
            }
            System.Console.ReadLine();
        }


    }
}
