using Minesweeper.NineSliceSystem;

namespace Minesweeper.Entities;

public class TopSectionFrame(int x, int y, int width, int height)
    : Frame(x, y, width, height,
        new FrameElement(0, 83, 10, 10),
        new FrameElement(60, 83, 18, 10, width - 10),
        new FrameElement(10, 83, 10, 10),
        new FrameElement(146, 23, 10, 18, null, 36),
        new FrameElement(146, 23, 10, 18, null, 36),
        new FrameElement(40, 83, 10, 10),
        new FrameElement(60, 83, 18, 10, width - 10),
        new(50, 83, 10, 10));