using Microsoft.Xna.Framework;
using Minesweeper.System.Input.Global;
using Newtonsoft.Json;

namespace Minesweeper.System.Input.Pointer.Remote;

public struct PointerSnapshot(Vector2 position, PointerAction action, PointerState state)
{
    [JsonConverter(typeof(Vector2StringConverter))]
    public readonly Vector2 Position = position;
    
    public readonly PointerAction Action = action;
    public readonly PointerState State = state;
}