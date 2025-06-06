﻿using Microsoft.Xna.Framework;
using Minesweeper.DataHolders;

namespace Minesweeper.Particles;

public class TileParticle : PhysicsParticle
{
    public TileParticle(GridTile tile, bool useTransparent = true, float lifespan = 2.5f)
    {
        Position = tile.Position;
        Sprite = GridTile.GetSpriteByTile(tile);
        Initialize(useTransparent, lifespan);
    }

    public TileParticle(int index = 0, bool hidden = false, bool flagged = false, bool clickedBomb = false,
        bool badFlagged = false, bool showHeld = false, bool useTransparent = true, float lifespan = 2.5f)
    {
        Sprite = GridTile.GetSpriteByTileData(index, hidden, flagged, clickedBomb, badFlagged, showHeld);
        Initialize(useTransparent, lifespan);
    }

    private void Initialize(bool useTransparent, float lifespan)
    {
        float speed = Globals.Random.Next(30, 35);
        float horizontalVariation = Globals.Random.Next(-50, 50);

        Velocity = new Vector2(
            horizontalVariation is > -20 and < 20
                ? horizontalVariation < 0 ? -20 : 20
                : horizontalVariation, -speed);
        Lifespan = lifespan;
        CurrentLife = Lifespan;
        SpriteSheet = useTransparent ? Globals.TransparentSpriteSheet : Globals.MainSpriteSheet;
    }

}