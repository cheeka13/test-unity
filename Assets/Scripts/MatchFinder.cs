using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MatchFinder : MonoBehaviour
{
    private Board board;
    public List<Gem> currentMatches = new List<Gem>();

    public List<Gem> fiveGemMatches = new List<Gem>();

    public List<Gem> fourGemMatches = new List<Gem>();

    public List<int> horizontalPieces = new List<int>();

    public List<int> verticalPieces = new List<int>();

    private void Awake()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMatches()
    {
        currentMatches.Clear();

        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                Gem currentGem = board.allGems[x, y];
                if (currentGem != null)
                {
                    if (x > 0 && x < board.width - 1)
                    {                    
                        horizontalPieces.Add(x);
                        for (int dir = 0; dir <= 1; dir++)
                        {
                            for (int xOffset = 1; xOffset <= board.width; xOffset++)
                            {
                                int newX;

                                if (dir == 0)
                                {
                                    newX = x - xOffset;
                                } else
                                {
                                    newX = x + xOffset;
                                }
                                
                                if (newX < 0 || newX >= board.width)
                                {
                                    break;
                                }

                                if (board.allGems[newX, y] != null && board.allGems[newX, y].type == currentGem.type)
                                {
                                    horizontalPieces.Add(newX);
                                } else
                                {
                                    break;
                                }
                            }
                        }

                        if (horizontalPieces.Count >= 5)
                        {
                            if (!currentGem.isMatched)
                            {
                                for (int i = 0; i < horizontalPieces.Count; i++)
                                {
                                    currentMatches.Add(board.allGems[horizontalPieces[i], y]);
                                    board.allGems[horizontalPieces[i], y].isMatched = true;

                                    fiveGemMatches.Add(board.allGems[horizontalPieces[2], y]);
                                }
                            }
                        } else
                        {
                            if (horizontalPieces.Count >= 3)
                            {
                                for (int i = 0; i < horizontalPieces.Count; i++)
                                {
                                    currentMatches.Add(board.allGems[horizontalPieces[i], y]);
                                    board.allGems[horizontalPieces[i], y].isMatched = true;
                                }
                            }
                            if (horizontalPieces.Count >= 3)
                            {
                                for (int i = 0; i < horizontalPieces.Count; i++)
                                {
                                    for (int dir = 0; dir <= 1; dir++)
                                    {
                                        for (int yOffset = 1; yOffset <= board.height; yOffset++)
                                        {
                                            int newY;

                                            if (dir == 0)
                                            {
                                                newY = y - yOffset;
                                            } else
                                            {
                                                newY = y + yOffset;
                                            }

                                            if (newY < 0 || newY >= board.height)
                                            {
                                                break;
                                            }

                                            if (board.allGems[horizontalPieces[i], newY] != null && board.allGems[horizontalPieces[i], newY].type == currentGem.type)
                                            {
                                                verticalPieces.Add(newY);
                                            } else
                                            {
                                                break;
                                            }

                                            if (verticalPieces.Count >= 2)
                                            {
                                                for (int j = 0; j < verticalPieces.Count; j++)
                                                {
                                                    currentMatches.Add(board.allGems[horizontalPieces[i], verticalPieces[j]]);
                                                    board.allGems[horizontalPieces[i], verticalPieces[j]].isMatched = true;
                                                }
                                            }
                                            verticalPieces.Clear();
                                        }
                                    }
                                }
                            }
                        }

                            /*if (horizontalPieces.Count == 4)
                            {
                                fourGemMatches.Add(currentGem);
                            }

                            if (horizontalPieces.Count >= 5)
                            {
                                fiveGemMatches.Add(board.allGems[horizontalPieces[2], y]);
                            }*/
                        horizontalPieces.Clear();
                    }
                    
                    if (y > 0 && y < board.height - 1)
                    {
                        verticalPieces.Add(y);
                        for (int dir = 0; dir <= 1; dir++)
                        {
                            for (int yOffset = 1; yOffset <= board.height; yOffset++)
                            {
                                int newY;

                                if (dir == 0)
                                {
                                    newY = y - yOffset;
                                } else
                                {
                                    newY = y + yOffset;
                                }
                                
                                if (newY < 0 || newY >= board.height)
                                {
                                    break;
                                }

                                if (board.allGems[x, newY] != null && board.allGems[x, newY].type == currentGem.type)
                                {
                                    verticalPieces.Add(newY);
                                } else
                                {
                                    break;
                                }
                            }
                        }
                        if (verticalPieces.Count >= 3 && !currentGem.isMatched)
                        {
                            for (int i = 0; i < verticalPieces.Count; i++)
                            {
                                currentMatches.Add(board.allGems[x, verticalPieces[i]]);
                                board.allGems[x, verticalPieces[i]].isMatched = true;
                            }

                            if (verticalPieces.Count >= 5)
                            {
                                fiveGemMatches.Add(board.allGems[x, verticalPieces[2]]);
                            }
                        }
                        verticalPieces.Clear();
                    }
                }
            }
        }

        if (currentMatches.Count > 0)
        {
            currentMatches = currentMatches.Distinct().ToList();
        }

        // CheckForBombs();
    }

    /*
    public void CheckForBombs()
    {
        for (int i = 0; i < currentMatches.Count; i++)
        {
            Gem gem = currentMatches[i];

            int x = gem.posIndex.x;
            int y = gem.posIndex.y;

            if (gem.posIndex.x > 0)
            {
                if (board.allGems[x-1, y] != null)
                {
                    if (board.allGems[x-1, y].type == Gem.GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x-1, y), board.allGems[x-1, y]);
                    }
                }
            }

            if (gem.posIndex.x < board.width -1)
            {
                if (board.allGems[x+1, y] != null)
                {
                    if (board.allGems[x+1, y].type == Gem.GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x+1, y), board.allGems[x+1, y]);
                    }
                }
            }

            if (gem.posIndex.y > 0)
            {
                if (board.allGems[x, y-1] != null)
                {
                    if (board.allGems[x, y-1].type == Gem.GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x, y-1), board.allGems[x, y-1]);
                    }
                }
            }

            if (gem.posIndex.y < board.height -1)
            {
                if (board.allGems[x, y+1] != null)
                {
                    if (board.allGems[x, y+1].type == Gem.GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x, y+1), board.allGems[x, y+1]);
                    }
                }
            }
        }
    }

    public void MarkBombArea(Vector2Int bombPos, Gem theBomb)
    {
        for (int x = bombPos.x - theBomb.blastSize; x <= bombPos.x + theBomb.blastSize; x++)
        {
            for (int y = bombPos.y - theBomb.blastSize; y <= bombPos.y + theBomb.blastSize; y++)
            {
                if (x >= 0 && x < board.width && y >= 0 && y < board.height)
                {
                    if (board.allGems[x, y] != null)
                    {
                        board.allGems[x, y].isMatched = true;
                        currentMatches.Add(board.allGems[x, y]);
                    }
                }
            }
        }

        currentMatches = currentMatches.Distinct().ToList();
    }
    */
}
