using Minesweeper.NineSliceSystem;

namespace Minesweeper.Entities;

public class GridFrame : Frame
{

    public GridFrame(int x, int y, int width, int height) : base(x, y, width, height, 
        new FrameElement(40, 83, 10, 10),
        new FrameElement(60, 83, 18, 10, width - 20),
        new FrameElement(50, 83, 10, 10), 
        new FrameElement(146, 23, 10, 18, 10, height - 20),
        new FrameElement(146, 23, 10, 18, 10, height - 20),
        new FrameElement(20, 83, 10, 10),
        new FrameElement(60, 83, 18, 10, width - 20),
        new FrameElement(30, 83, 10, 10))
    {
    }
}