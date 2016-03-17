/*###################################################################
#########  2014  BATTLESHIP, JAVIER AVILES FOR BEDE GAMING  #########
###################################################################*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleShip
{
    public class Ship
    {
        public string Name { get; set; }
        public int KindOfShip { get; set; }
        public int ShipSquares { get; set; }
        public int ShipLifes { get; set; }
        //OBJECT SHIP
        public Ship(string name, int kindOfShip, int squares)
        {
            Name = name;
            KindOfShip = kindOfShip;
            ShipSquares = squares;
            ShipLifes = squares;
        }
    }
    public class Player
    {
        public int[,] Grid { get; set; }
        public Ship[] Navy { get; set; }
        public int Lifes { get; set; }
        //OBJECT PLAYER
        public Player(int[,] grid, Ship[] navy)
        {
            Grid = grid;
            Navy = navy;
            Lifes = 0;
        }
        //PLAYER'S METHOD FOR BUILDING HIS NAVY
        public void BuildNavy ()
        {
            Navy[0] = new Ship("Destroyer1", 2, 4);
            Navy[1] = new Ship("Destroyer2", 3, 4);
            Navy[2] = new Ship("Battleship", 4, 5);
            for (int i = 0; i < Navy.Length;i++ )
            {
                Lifes = Lifes + Navy[i].ShipLifes;
            }
        }
        //PLAYER'S METHOD WHEN HIS NAVY IS HIT
        public void hit(int kindOfShip)
        {
            Console.WriteLine("{0} hit!", Navy[kindOfShip - 2].Name);
            Navy[kindOfShip - 2].ShipLifes--;
            if (Navy[kindOfShip - 2].ShipLifes == 0)
                Console.WriteLine("{0} sunk!", Navy[kindOfShip - 2].Name);
            Lifes--;
        }
    }
    class Battle
    {
        //FUNCTION FOR FILLING A MATRIX WITH ZEROS
        static void initializeMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = 0;
                }
            }
        }

        //SIMPLE FUNCTION FOR PRINTING A MATRIX
        static void printMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write("{0}\t", matrix[i, j]);
                }
                Console.Write("\v");
            }
            Console.Write("\n");
        }

        //FUNCTION FOR GENERATING A RANDOM NUMBER
        private static readonly Random getrandom = new Random();
        public static int GetRandomNumber(int min, int max)
        {
                return getrandom.Next(min, max);
        }

        //FUNCTION FOR PLACING THE SHIPS OF A NAVY IN A GRID
        static void constructGrid(int[,] grid , Ship[] navy)
        {
            initializeMatrix(grid);
            int gridCell;
            int ShipInitialPositionX;
            int ShipInitialPositionY;
            int direction;
            int pathX;
            int pathY;
            bool coordsOk;
            //Array with all the possible directions
            //A random number will decide which one the ship will take
            int[][] directions = new int[8][];
            for (int i = 0; i < directions.Length; i++)
            {
                directions[i] = new int[2];
            }
         
            
int i,j;

if((i==0 && j==1) || (i==1 && j==0) || (i==1 && j==1) || (i==2 && j==0) || (i==3 && j==0)|| (i==7 && j==1))

      directions[i][j]=1;

else if ((i==0 && j==0)|| (i==2 && j==1)||(i==4 && j==0) || (i==6 && j==1))
     
      directions[i][j]=0;
else if ((i==3 && j==1)|| (i==4 && j==1)|| (i==5 && j==0)|| (i==5 && j==1)|| (i==6 && j==0)|| (i==7 && j==0))
      directions[i][j]=-1;
            for (int i = 0; i < navy.Length; i++)
            {
                do
                {
                    coordsOk = true;
                    //Two random numbers will decide the possible initial coordinate of the boat
                    ShipInitialPositionX = GetRandomNumber(0, 9);
                    ShipInitialPositionY = GetRandomNumber(0, 9);
                    //random number for choosing the direction
                    direction = GetRandomNumber(0, 7);
                    pathX = directions[direction][0];
                    pathY = directions[direction][1];
                    //If the last coordinates of the ship are outside the grid
                    if ((((ShipInitialPositionX + navy[i].ShipSquares * pathX) < 0) || ((ShipInitialPositionX + navy[i].ShipSquares * pathX) > grid.GetLength(0))) || (((ShipInitialPositionY - navy[i].ShipSquares * pathY) < 0) || ((ShipInitialPositionY - navy[i].ShipSquares * pathY) > grid.GetLength(1))))
                        coordsOk = false;
                    else
                    {
                        //checking if there is already a ship in the coordinates where the new ship could be placed
                        for (int j = 0; j < navy[i].ShipSquares; j++)
                        {
                            gridCell = grid[ShipInitialPositionY - (pathY * j), ShipInitialPositionX + (pathX * j)];
                            if ((gridCell == navy[0].KindOfShip) || (gridCell == navy[1].KindOfShip))
                                coordsOk = false;
                        }
                    }
                }
                while (!coordsOk);

                //Placing the ship in the valid coordinates
                for (int j = 0; j < navy[i].ShipSquares; j++)
                {
                    grid[ShipInitialPositionY - (pathY * j), ShipInitialPositionX + (pathX * j)] = navy[i].KindOfShip;
                }
                
            }
        }
        
        //+++MAIN+++
        static void Main(string[] args)
        {
            //CONSTRUCTION PHASE
            int navySize = 3;
            int GridSize = 10;
            Player Human = new Player(new int[GridSize, GridSize], new Ship[navySize]);
            Player Computer = new Player(new int[GridSize, GridSize], new Ship[navySize]);
            Human.BuildNavy();
            Computer.BuildNavy();
            constructGrid(Human.Grid, Human.Navy);
            constructGrid(Computer.Grid,Computer.Navy);

            //PLAYING PHASE
            string coords;
            int xShoot, yShoot;
            bool wrongCoordinates,alreadyFired;
            Console.WriteLine("WELCOME TO THE BATTLESHIP");
            do
            {//do until the player or the computer looses

                //PLAYER'S TURN
                Console.WriteLine("\nYour turn:");
                Console.WriteLine("You still have {0} lifes", Human.Lifes);
                Console.WriteLine("Enter coordinates to Attack (A-J),(0-9), e.g. A5");
                alreadyFired = false;
                do
                {//do until get valid non-fired coordinates
                    if (alreadyFired) Console.WriteLine("You have already fired those coordinates");
                    wrongCoordinates = false;
                    do
                    {//do until valid coordinates
                        if (wrongCoordinates) Console.WriteLine("Please follow the syntaxis \'A5\'");
                        coords = Console.ReadLine();
                        wrongCoordinates=true;
                    }
                    while ((coords.Length!=2)||((((coords[1] - '0') < 0) || ((coords[1] - '0') >= GridSize)) || (((coords[0] - 'A') < 0) || ((coords[0] - 'A') >= GridSize))));
                    xShoot = coords[0] - 'A';//char to int (ascii), A-J = 0-10
                    yShoot = coords[1] - '0';//char to int (ascii)
                    alreadyFired=true;
                } while (Computer.Grid[xShoot, yShoot] == 1);//already fired coordinate
                if (Computer.Grid[xShoot, yShoot] == 0) Console.WriteLine("Water!");
                else
                {//Hit any Ship
                    Computer.hit(Computer.Grid[xShoot, yShoot]);
                }
                //Mark cell as "already fired"
                Computer.Grid[xShoot, yShoot] = 1;
                //printMatrix(Computer.Grid);

                //COMPUTER'S TURN
                do{//do until get a non-fired coordinate
                    xShoot = GetRandomNumber(0, GridSize-1);
                    yShoot = GetRandomNumber(0, GridSize-1);
                } while (Human.Grid[xShoot, yShoot] == 1);//already fired coordinate
                Console.WriteLine("\nComputer's turn:");
                Console.WriteLine("The computer still has {0} lifes", Computer.Lifes);
                if (Human.Grid[xShoot, yShoot] == 0)
                    Console.WriteLine("Water!");
                else
                {//Hit any Ship
                    Human.hit(Human.Grid[xShoot, yShoot]);
                }
                //Mark cell as "already fired"
                Human.Grid[xShoot, yShoot] = 1;
                //printMatrix(Human.Grid);
            } while (Computer.Lifes>0 && Human.Lifes>0);
            if (Computer.Lifes == 0) Console.WriteLine("You won! :)");
            else Console.WriteLine("Sorry, you Lost :(");
            Console.ReadKey();
        }

    }
}
