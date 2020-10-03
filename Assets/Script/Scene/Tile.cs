using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public float xPosition;
    public float yPosition;
    public int column;
    public int row;
    public bool isMatched = false;

    private void Start()
    {
        Grid = FindObjectOfType<Grid>();
        xPosition = transform.position.x;
        yPosition = transform.position.y;
        column = Mathf.RoundToInt((xPosition - Grid.startPos.x) / Grid.offset.x);
        row = Mathf.RoundToInt((yPosition - Grid.startPos.y) / Grid.offset.x);
    }

    private void Update()
    {
        CheckMatches();
        if (isMatched)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.grey;
        }

        xPosition = (column * Grid.offset.x) + Grid.startPos.x;
        yPosition = (row * Grid.offset.y) + Grid.startPos.y;
        SwipeTile();
    }

    private void OnMouseDown()
    {
        firstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        finalPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    private void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalPosition.y - firstPosition.y, finalPosition.x - firstPosition.x) * 180 / Mathf.PI;
        MoveTile();
    }

    private void SwipeTile()
    {
        if (Mathf.Abs(xPosition - transform.position.x) > .1)
        {
            tempPosition = new Vector2(xPosition, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(xPosition, transform.position.y);
            transform.position = tempPosition;
            Grid.tiles[column, row] = this.gameObject;
        }
        if (Mathf.Abs(yPosition - transform.position.y) > .1)
        {
            //Move towards the target
            tempPosition = new Vector2(transform.position.x, yPosition);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(transform.position.x, yPosition);
            transform.position = tempPosition;
            Grid.tiles[column, row] = this.gameObject;
        }
    }

    private void MoveTile()
    {
        previousRow = row;
        previousColumn = column;
        if (swipeAngle > -45 && swipeAngle <= 45)
            //Right swipe
            SwipeRightMove();
        else if (swipeAngle > 45 && swipeAngle <= 135)
            //Up swipe
            SwipeUpMove();
        else if (swipeAngle > 135 || swipeAngle <= -135)
            //Left swipe
            SwipeLeftMove();
        else if (swipeAngle < -45 && swipeAngle >= -135)
            //Down swipe
            SwipeDownMove();
        StartCoroutine(checkMove());
    }

    IEnumerator checkMove()
    {
        yield return new WaitForSeconds(.5f);
        //Cek jika tile nya tidak sama kembalikan, jika ada yang sama panggil DestroyMatches
        if (otherTile != null)
        {
            if (!isMatched && !otherTile.GetComponent<Tile>().isMatched)
            {
                otherTile.GetComponent<Tile>().row = row;
                otherTile.GetComponent<Tile>().column = column;
                row = previousRow;
                column = previousColumn;

                ScoreManager.instance.ResetStreak();
            }
            else
            {
                Grid.DestroyMatches();
            }
        }
        otherTile = null;
    }

    //Method untuk menentukan arah dari swipe
    private void SwipeRightMove()
    {
        if (column + 1 < Grid.gridSizeX)
        {
            //Menukar posisi tile dengan sebelah kanan nya
            otherTile = Grid.tiles[column + 1, row];
            otherTile.GetComponent<Tile>().column -= 1;
            column += 1;
        }
    }

    private void SwipeUpMove()
    {
        if (row + 1 < Grid.gridSizeY)
        {
            //Menukar posisi tile dengan sebelah atasnya
            otherTile = Grid.tiles[column, row + 1];
            otherTile.GetComponent<Tile>().row -= 1;
            row += 1;
        }
    }

    private void SwipeLeftMove()
    {
        if (column - 1 >= 0)
        {
            //Menukar posisi tile dengan sebelah kiri nya
            otherTile = Grid.tiles[column - 1, row];
            otherTile.GetComponent<Tile>().column += 1;
            column -= 1;
        }
    }

    private void SwipeDownMove()
    {
        if (row - 1 >= 0)
        {
            //Menukar posisi tile dengan sebelah bawahnya
            otherTile = Grid.tiles[column, row - 1];
            otherTile.GetComponent<Tile>().row += 1;
            row -= 1;
        }
    }

    protected virtual void CheckMatches()
    {
        //Check horizontal matching
        if (column > 0 && column < Grid.gridSizeX - 1)
        {
            //Check samping kiri dan kanan nya
            GameObject leftTile = Grid.tiles[column - 1, row];
            GameObject rightTile = Grid.tiles[column + 1, row];
            if (leftTile != null && rightTile != null)
            {
                if (leftTile.CompareTag(gameObject.tag) && rightTile.CompareTag(gameObject.tag))
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
                if (upTile.CompareTag(gameObject.tag) && downTile.CompareTag(gameObject.tag))
                {
                    isMatched = true;
                    downTile.GetComponent<Tile>().isMatched = true;
                    upTile.GetComponent<Tile>().isMatched = true;
                }
            }
        }
    }

    protected Grid Grid { get; private set; }

    private int previousColumn;
    private int previousRow;

    private Vector3 firstPosition;
    private Vector3 finalPosition;
    private Vector3 tempPosition;
    private float swipeAngle;
    private GameObject otherTile;
}