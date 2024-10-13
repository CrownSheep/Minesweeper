using Microsoft.Xna.Framework.Input;

namespace Minesweeper.System.Input.Keyboard;

public record MultiKey(params Keys[] Keys);