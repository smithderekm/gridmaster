#nullable enable
namespace GridMaster.App.Models
{
    public class Box
    {
        public Side Top { get; set; } = new Side{Name="Top", IsMarked = false};
        public Side Bottom { get; set; } = new Side { Name = "Bottom", IsMarked = false };
        public Side Left { get; set; } = new Side { Name = "Left", IsMarked = false };
        public Side Right { get; set; } = new Side { Name = "Right", IsMarked = false };

        public int X { get; set; }
        public int Y { get; set; }

        public Player? WonBy { get; set; }

        public bool IsSideMarked(string moveSide)
        {
            switch (moveSide.ToUpper())
            {
                case "T":
                    return this.Top.IsMarked;
                case "B":
                    return this.Bottom.IsMarked;
                case "L":
                    return this.Left.IsMarked;
                case "R":
                    return this.Right.IsMarked;
            }

            return false;
        }

        public bool Enclosed()
        {
            return this.Top.IsMarked & this.Bottom.IsMarked & this.Left.IsMarked & this.Right.IsMarked;
        }
    }
}
