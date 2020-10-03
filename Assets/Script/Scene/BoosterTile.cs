using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BoosterTile : Tile
{
    protected override void CheckMatches()
    {
        //Check horizontal matching
        if (column > 0 && column < Grid.gridSizeX - 1)
        {
            //Check samping kiri dan kanan nya
            GameObject leftTile = Grid.tiles[column - 1, row];
            GameObject rightTile = Grid.tiles[column + 1, row];
            if (leftTile != null && rightTile != null)
            {
                if (IsTagBoosterOrEqual(leftTile) && IsTagBoosterOrEqual(rightTile))
                {
                    isMatched = true;
                    rightTile.GetComponent<Tile>().isMatched = true;
                    leftTile.GetComponent<Tile>().isMatched = true;
                }
            }
        }
        //Check vertical matching
        if (row > 0 && row < Grid.gridSizeY - 1)
        {
            //Check samping atas dan bawahnya
            GameObject upTile = Grid.tiles[column, row + 1];
            GameObject downTile = Grid.tiles[column, row - 1];
            if (upTile != null && downTile != null)
            {
                if (IsTagBoosterOrEqual(upTile) && IsTagBoosterOrEqual(downTile))
                {
                    isMatched = true;
                    downTile.GetComponent<Tile>().isMatched = true;
                    upTile.GetComponent<Tile>().isMatched = true;
                }
            }
        }
    }

    private bool IsTagBoosterOrEqual(GameObject tile)
    {
        if(tile.CompareTag(gameObject.tag) || tile.tag.Equals("Booster") || tag.Equals("Booster"))
        {
            return true;
        }

        return false;
    }
}
