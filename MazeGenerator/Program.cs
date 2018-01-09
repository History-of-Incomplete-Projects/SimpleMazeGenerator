using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGenerator
{
    public class GridProperties
    {
        private Int32 xLength;
        private Int32 yLength;
        public GridProperties(Int32 xlength, Int32 ylength)
        {
            xLength = xlength;
            yLength = ylength;
        }
        public Int32 getXLength()
        {
            return this.xLength;
        }
        public Int32 getYLength()
        {
            return this.yLength;
        }
    }
    public class CellProperties
    {
        private GridProperties Grid;
        private Boolean topWall, bottomWall, rightWall, leftWall;
        private Int32? xCoord, yCoord;
        private CellProperties topCell, bottomCell, rightCell, leftCell;
        private CellProperties topParentCell, bottomParentCell, rightParentCell, leftParentCell;
        private HashSet<String> passageID = new HashSet<String>();
        public CellProperties(GridProperties grid = null, Int32? targetXCoord = null, Int32? targetYCoord = null,
            CellProperties tpCell = null, CellProperties bpCell = null, 
            CellProperties rpCell = null, CellProperties lpCell = null)
        {
            this.Grid = grid;

            this.xCoord = targetXCoord;
            this.yCoord = targetYCoord;
            
            this.updatePassageID(this.xCoord.ToString() + "|" + this.yCoord.ToString());

            this.topWall = true;
            this.bottomWall = true;
            this.rightWall = true;
            this.leftWall = true;

            this.topParentCell = tpCell;
            this.bottomParentCell = bpCell;
            this.rightParentCell = rpCell;
            this.leftParentCell = lpCell;

            this.topCell = this.setTopCell();
            this.bottomCell = this.setBottomCell();
            if (this.yCoord == 0)
            {
                this.rightCell = this.setRightCell();
            }
            this.leftCell = this.setLeftCell();
        }
        
        public CellProperties setTopCell()
        {
            if (this.Grid == null)
            {
                return null;
            }
            if (this.yCoord == this.Grid.getYLength() - 1)
            {
                return new CellProperties();
            }
            else
            {
                return new CellProperties(this.Grid, this.xCoord, this.yCoord + 1, null, this, null, null);
            }
        }
        public CellProperties setBottomCell()
        {
            if (this.yCoord == 0)
            {
                return new CellProperties();
            }
            else
            {
                return this.bottomParentCell;
            }
        }
        public CellProperties setRightCell()
        {
            if (this.Grid == null)
            {
                return null;
            }
            if (this.xCoord == this.Grid.getXLength() - 1)
            {
                return new CellProperties();
            }
            else
            {
                return new CellProperties(this.Grid, this.xCoord + 1, this.yCoord, null, null, null, this);
            }
        }
        public CellProperties setLeftCell( )
        {
            if (this.xCoord == 0)
            {
                return new CellProperties();
            }
            else
            {
                if (this.Grid == null)
                {
                    return null;
                }
                if (this.yCoord == 0)
                {
                    return this.leftParentCell;
                }
                else
                {
                    CellProperties buffer = this.bottomCell;
                    for (Int32 depth = 0; depth < this.yCoord - 1; depth++)
                    {
                        buffer = buffer.bottomParentCell;
                    }
                    buffer = buffer.leftParentCell.topCell;
                    for (Int32 depth = 0; depth < this.yCoord - 1; depth++)
                    {
                        buffer = buffer.topCell;
                    }
                    buffer.rightCell = this;
                    return buffer;
                }
            }
        }
        
        public GridProperties getGrid()
        {
            return this.Grid;
        }
        public CellProperties getBottomCell()
        {
            return this.bottomCell;
        }
        public CellProperties getTopCell()
        {
            return this.topCell;
        }
        public CellProperties getLeftCell()
        {
            return this.leftCell;
        }
        public CellProperties getRightCell()
        {
            return this.rightCell;
        }

        public void setBottomWall(Boolean newBottomWall)
        {
            this.bottomWall = newBottomWall;
        }
        public void setTopWall(Boolean newTopWall)
        {
            this.topWall = newTopWall;
        }
        public void setLeftWall(Boolean newLeftWall)
        {
            this.leftWall = newLeftWall;
        }
        public void setRightWall(Boolean newRightWall)
        {
            this.rightWall = newRightWall;
        }
        public Boolean getBottomWall()
        {
            return this.bottomWall;
        }
        public Boolean getTopWall()
        {
            return this.topWall;
        }
        public Boolean getLeftWall()
        {
            return this.leftWall;
        }
        public Boolean getRightWall()
        {
            return this.rightWall;
        }

        public void updatePassageID(String newPassageID)
        { 
            this.passageID.Add(newPassageID);
        }
        public void setPassageID(HashSet<String> newPassageID)
        {
            this.passageID = new HashSet<string>(newPassageID);
        }
        public HashSet<String> getPassageID()
        {
            return this.passageID;
        }
    }
    public class drawGrid : CellProperties
    {
        private CellProperties Cell0x0 = new CellProperties();
        private CellProperties yWalkBuffer = new CellProperties();
        private CellProperties xWalkBuffer = new CellProperties();
        private String bottomWall = "";
        private String topWall = "";
        private String leftAndrightWalls = "";

        public drawGrid(CellProperties cell0x0)
        {
            Cell0x0 = cell0x0;
        }

        private void WallDrawingReset()
        {
            this.bottomWall = "\n";
            this.topWall = "\n";
            this.leftAndrightWalls = "\n";
        }
        private void Draw()
        {
            // draw bottom wall
            {
                if (this.bottomWall == "\n")
                {
                    Console.Write("");
                }
                else
                {

                    Console.Write(this.bottomWall);
                }
            }
            Console.Write(this.leftAndrightWalls);
            // draw top wall
            {
                if (topWall == "\n")
                {
                    Console.Write("");
                }
                else
                {

                    Console.Write(this.topWall);
                }
            }
        }
        public void yWalk()
        {
            for (Int32 yWalk = 0; yWalk < this.Cell0x0.getGrid().getYLength(); yWalk++)
            {
                this.yWalkBuffer = yWalk == 0 ? this.Cell0x0 : this.yWalkBuffer.getTopCell();

                this.WallDrawingReset();
                this.xWalk(yWalk);
                this.Draw();
            }
        }
        private void xWalk(Int32 yWalk)
        {
            for (Int32 xWalk = 0; xWalk < this.Cell0x0.getGrid().getXLength(); xWalk++)
            {
                this.xWalkBuffer = xWalk == 0 ? this.yWalkBuffer : this.xWalkBuffer.getRightCell();

                if (yWalk == 0)
                {
                    this.bottomWall = xWalkBuffer.getBottomWall() ? this.bottomWall + "----" : this.bottomWall + "    ";
                    this.topWall = xWalkBuffer.getTopWall() ? this.topWall + "----" : this.topWall + "    ";
                }
                else
                {
                    this.topWall = this.xWalkBuffer.getTopWall() ? this.topWall + "----" : this.topWall + "    ";
                }
                if (xWalk == 0)
                {
                    leftAndrightWalls = this.xWalkBuffer.getLeftWall() ? this.leftAndrightWalls + "|   " : this.leftAndrightWalls + "    ";
                    leftAndrightWalls = this.xWalkBuffer.getRightWall() ? this.leftAndrightWalls + "|   " : this.leftAndrightWalls + "    ";
                }
                else
                {
                    leftAndrightWalls = this.xWalkBuffer.getRightWall() ? this.leftAndrightWalls + "|   " : this.leftAndrightWalls + "    ";
                }


            }
        }
    }
    public class KruskalMazeGenerator
    {
        private CellProperties Cell0x0;
        private CellProperties CurrentCell;
        private CellProperties NeighbourCell;
        private Int32 WallsDown;
        private Int32 TotalNumberOfCells;
        private Random rnd = new Random();
        private Int32 rndXCoord, rndYCoord;
        private String rndSide;
        public KruskalMazeGenerator(CellProperties cell0x0)
        {
            Cell0x0 = cell0x0;
            WallsDown = 0;
            TotalNumberOfCells = Cell0x0.getGrid().getXLength() * Cell0x0.getGrid().getYLength();
        }
        public void selectRandomCellCoords()
        {
            this.rndXCoord = rnd.Next(0, this.Cell0x0.getGrid().getXLength());
            this.rndYCoord = rnd.Next(0, this.Cell0x0.getGrid().getYLength());
        }
        public void selectRandomSide(String[] possibleSides)
        {
            if (possibleSides.Length != 0)
            {
                this.rndSide = possibleSides[rnd.Next(0, possibleSides.Length)];
            }
        }
        public void selectRandomCurrentCell()
        {
            this.selectRandomCellCoords();
            this.CurrentCell = this.Cell0x0;
            for (Int32 xWalk = 0; xWalk < this.rndXCoord; xWalk++)
            {
                this.CurrentCell = this.CurrentCell.getRightCell();
            }
            for (Int32 xWalk = 0; xWalk < this.rndYCoord; xWalk++)
            {
                this.CurrentCell = this.CurrentCell.getTopCell();
            }
        }
        public CellProperties checkWallBetweenCurrentAndNeighbour(List<String> possibleSides)
        {
            if (this.rndSide == "top")
            {
                if (this.CurrentCell.getTopCell() == null || this.CurrentCell.getTopCell().getGrid() == null)
                {
                    possibleSides.Remove("top");
                    this.selectRandomSide(possibleSides.ToArray());
                    return this.checkWallBetweenCurrentAndNeighbour(possibleSides);
                }
                return this.CurrentCell.getTopCell();
            }
            else if (this.rndSide == "bottom")
            {
                if (this.CurrentCell.getBottomCell() == null || this.CurrentCell.getBottomCell().getGrid() == null)
                {
                    possibleSides.Remove("bottom");
                    this.selectRandomSide(possibleSides.ToArray());
                    return this.checkWallBetweenCurrentAndNeighbour(possibleSides);
                }
                return this.CurrentCell.getBottomCell();
            }
            else if (this.rndSide == "left")
            {
                if (this.CurrentCell.getLeftCell() == null || this.CurrentCell.getLeftCell().getGrid() == null)
                {
                    possibleSides.Remove("left");
                    this.selectRandomSide(possibleSides.ToArray());
                    return this.checkWallBetweenCurrentAndNeighbour(possibleSides);
                }
                return this.CurrentCell.getLeftCell();
            }
            else if (this.rndSide == "right")
            {
                if (this.CurrentCell.getRightCell() == null || this.CurrentCell.getRightCell().getGrid() == null)
                {
                    possibleSides.Remove("right");
                    this.selectRandomSide(possibleSides.ToArray());
                    return this.checkWallBetweenCurrentAndNeighbour(possibleSides);
                }
                return this.CurrentCell.getRightCell();
            }
            return null;
        }
        public void selectRandomNeigbhourCell()
        {
            this.selectRandomSide(new String[4] { "top", "bottom", "left", "right" });
            this.NeighbourCell = this.checkWallBetweenCurrentAndNeighbour(new List<String>(new String[4] { "top", "bottom", "left", "right" }));
        }
        public void checkForDifferentPassageID()
        {
            if (!this.CurrentCell.getPassageID().SetEquals(this.NeighbourCell.getPassageID()))
            {
                if (this.rndSide == "top")
                {
                    this.CurrentCell.setTopWall(false);
                    this.NeighbourCell.setBottomWall(false);
                    this.unionAndResetPassageID();
                    this.WallsDown += 1;
                }
                else if (this.rndSide == "bottom")
                {
                    this.CurrentCell.setBottomWall(false);
                    this.NeighbourCell.setTopWall(false);
                    this.unionAndResetPassageID();
                    this.WallsDown += 1;
                }
                else if (this.rndSide == "left")
                {
                    this.CurrentCell.setLeftWall(false);
                    this.NeighbourCell.setRightWall(false);
                    this.unionAndResetPassageID();
                    this.WallsDown += 1;
                }
                else if (this.rndSide == "right")
                {
                    this.CurrentCell.setRightWall(false);
                    this.NeighbourCell.setLeftWall(false);
                    this.unionAndResetPassageID();
                    this.WallsDown += 1;
                }
            }
        }
        public void unionAndResetPassageID()
        {
            HashSet<String> oldCurrentPassageID = new HashSet<String>(this.CurrentCell.getPassageID());
            HashSet<String> oldNeighbourPassageID = new HashSet<String>(this.NeighbourCell.getPassageID());

            HashSet <String> newPassageID = new HashSet<String>();
            newPassageID = this.CurrentCell.getPassageID();
            newPassageID.UnionWith(this.NeighbourCell.getPassageID());
            
            CellProperties xwalkCell = new CellProperties();
            CellProperties ywalkCell = new CellProperties();
            for (Int32 xWalk = 0; xWalk < this.Cell0x0.getGrid().getXLength(); xWalk++)
            {
                xwalkCell = xWalk == 0 ? this.Cell0x0 : xwalkCell.getRightCell();
                for (Int32 yWalk = 0; yWalk < this.Cell0x0.getGrid().getYLength(); yWalk++)
                {
                    xwalkCell.setBottomWall(xWalk == 0 && yWalk == 0 ? false : xwalkCell.getBottomWall());
                    xwalkCell.setBottomWall(xWalk == this.Cell0x0.getGrid().getXLength() - 1 && yWalk == this.Cell0x0.getGrid().getYLength() - 1 ? false : xwalkCell.getBottomWall());
                    ywalkCell = yWalk == 0 ? xwalkCell : ywalkCell.getTopCell();

                    if (ywalkCell.getPassageID().SetEquals(oldCurrentPassageID) || 
                        ywalkCell.getPassageID().SetEquals(oldNeighbourPassageID))
                    {
                        ywalkCell.setPassageID(newPassageID);
                    }
                }
            }
        }
        public CellProperties createMaze()
        {
            while (this.WallsDown < this.TotalNumberOfCells - 1)
            {
                this.selectRandomCurrentCell();
                this.selectRandomNeigbhourCell();
                if (this.NeighbourCell != null)
                {
                    this.checkForDifferentPassageID();
                }
                
            }
            return this.Cell0x0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            {
                CellProperties cell = new CellProperties(new GridProperties(10, 10), 0, 0, null, null, null, null);
                drawGrid draw = new drawGrid(cell);
                draw.yWalk();
                KruskalMazeGenerator kmaze = new KruskalMazeGenerator(cell);
                cell = kmaze.createMaze();
                Console.WriteLine("Final");
                draw = new drawGrid(cell);
                draw.yWalk();
            }

            Console.ReadKey();
        }
    }
}
