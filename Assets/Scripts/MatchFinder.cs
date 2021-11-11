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

    public List<Gem> letterGemMatches = new List<Gem>();

    public List<int> horizontalPieces = new List<int>();

    public List<int> verticalPieces = new List<int>();

    private void Awake()
    {
        board = FindObjectOfType<Board>();
    }
    
    public void FindAllMatches(int xIndex = -1, int yIndex = -1)
    {
        currentMatches.Clear();

        // check horizontally
        for (int y = 0; y < board.height; y++)
        {
            Gem tempGem = null;

            for (int x = 0; x < board.width; x++)
            {
                Gem currentGem = board.allGems[x, y];
                if (tempGem == null || (currentGem != null && currentGem.type != tempGem.type))
                {
                    tempGem = currentGem;
                    horizontalPieces.Add(x);
                    for (int dir = 0; dir <= 1; dir++)
                    {
                        for (int xOffset = 1; xOffset < board.width; xOffset++)
                        {
                            int newX;

                            if (dir == 0)
                            {
                                newX = x - xOffset;
                            } else
                            {
                                newX = x + xOffset;
                            }

                            if (newX < 0 || newX > board.width -1)
                            {
                                break;
                            }

                            if (board.allGems[newX, y] != null && board.allGems[newX, y].type == tempGem.type)
                            {
                                horizontalPieces.Add(newX);
                            } else
                            {
                                break;
                            }
                        }
                    }

                    if (horizontalPieces.Count >= 3)
                    {
                        for (int i = 0; i < horizontalPieces.Count; i++)
                        {
                            currentMatches.Add(board.allGems[horizontalPieces[i], y]);
                            board.allGems[horizontalPieces[i], y].isMatched = true;
                        }

                        if (horizontalPieces.Count >= 5)
                        {
                            if (xIndex == -1)
                            {
                                fiveGemMatches.Add(board.allGems[horizontalPieces[2], y]);
                            } else
                            {
                                fiveGemMatches.Add(board.allGems[xIndex, yIndex]);
                            }
                        } else
                        {
                            bool isLetterAppeared = false;

                            for (int i = 0; i < horizontalPieces.Count; i++)
                            {
                                for (int dir = 0; dir <= 1; dir++)
                                {
                                    for (int yOffset = 1; yOffset < board.height; yOffset++)
                                    {
                                        int newY;

                                        if (dir == 0)
                                        {
                                            newY = y - yOffset;
                                        } else
                                        {
                                            newY = y + yOffset;
                                        }

                                        if (newY < 0 || newY > board.height -1)
                                        {
                                            break;
                                        }

                                        if (board.allGems[horizontalPieces[i], newY] != null && board.allGems[horizontalPieces[i], newY].type == tempGem.type)
                                        {
                                            verticalPieces.Add(newY);
                                        } else
                                        {
                                            break;
                                        }
                                    }
                                }

                                if (verticalPieces.Count > 1)
                                {
                                    for (int j = 0; j < verticalPieces.Count; j++)
                                    {
                                        currentMatches.Add(board.allGems[horizontalPieces[i], verticalPieces[j]]);
                                        board.allGems[horizontalPieces[i], verticalPieces[j]].isMatched = true;
                                    }

                                    if (!letterGemMatches.Contains(board.allGems[horizontalPieces[i], y]))
                                    {
                                        letterGemMatches.Add(board.allGems[horizontalPieces[i], y]);
                                    }
                                    isLetterAppeared = true;
                                }

                                verticalPieces.Clear();
                            }

                            if (!isLetterAppeared && horizontalPieces.Count == 4)
                            {
                                if (xIndex == -1)
                                {
                                    fourGemMatches.Add(board.allGems[horizontalPieces[1], y]);
                                } else
                                {
                                    fourGemMatches.Add(board.allGems[xIndex, yIndex]);
                                }
                            }
                        }
                    }
                    horizontalPieces.Clear();
                }
            }
        }

        // check vertically
        for (int x = 0; x < board.width; x++)
        {
            Gem tempGem = null;

            for (int y = 0; y < board.height; y++)
            {
                Gem currentGem = board.allGems[x, y];
                if (tempGem == null || (currentGem != null && currentGem.type != tempGem.type))
                {
                    tempGem = currentGem;
                    verticalPieces.Add(y);
                    for (int dir = 0; dir <= 1; dir++)
                    {
                        for (int yOffset = 1; yOffset < board.height; yOffset++)
                        {
                            int newY;

                            if (dir == 0)
                            {
                                newY = y - yOffset;
                            } else
                            {
                                newY = y + yOffset;
                            }

                            if (newY < 0 || newY > board.height - 1)
                            {
                                break;
                            }

                            if (board.allGems[x, newY] != null && board.allGems[x, newY].type == tempGem.type)
                            {
                                verticalPieces.Add(newY);
                            } else
                            {
                                break;
                            }
                        }
                    }

                    if (verticalPieces.Count >= 3)
                    {
                        for (int i = 0; i < verticalPieces.Count; i++)
                        {
                            currentMatches.Add(board.allGems[x, verticalPieces[i]]);
                            board.allGems[x, verticalPieces[i]].isMatched = true;
                        }

                        if (verticalPieces.Count >= 5)
                        {
                            if (xIndex == -1)
                            {
                                fiveGemMatches.Add(board.allGems[x, verticalPieces[2]]);
                            } else
                            {
                                fiveGemMatches.Add(board.allGems[xIndex, yIndex]);
                            }
                        } else
                        {
                            bool isLetterAppeared = false;

                            for (int i = 0; i < verticalPieces.Count; i++)
                            {
                                for (int dir = 0; dir <= 1; dir++)
                                {
                                    for (int xOffset = 1; xOffset < board.width; xOffset++)
                                    {
                                        int newX;

                                        if (dir == 0)
                                        {
                                            newX = x - xOffset;
                                        } else
                                        {
                                            newX = x + xOffset;
                                        }

                                        if (newX < 0 || newX > board.width - 1)
                                        {
                                            break;
                                        }

                                        if (board.allGems[newX, verticalPieces[i]] != null && board.allGems[newX, verticalPieces[i]].type == tempGem.type)
                                        {
                                            horizontalPieces.Add(newX);
                                        } else
                                        {
                                            break;
                                        }
                                    }
                                }

                                if (horizontalPieces.Count > 1)
                                {
                                    for (int j = 0; j < horizontalPieces.Count; j++)
                                    {
                                        currentMatches.Add(board.allGems[horizontalPieces[j], verticalPieces[i]]);
                                        board.allGems[horizontalPieces[j], verticalPieces[i]].isMatched = true;
                                    }

                                    if (!letterGemMatches.Contains(board.allGems[x, verticalPieces[i]]))
                                    {
                                        letterGemMatches.Add(board.allGems[x, verticalPieces[i]]);
                                    }
                                    isLetterAppeared = true;
                                }

                                horizontalPieces.Clear();
                            }

                            if (!isLetterAppeared && verticalPieces.Count == 4)
                            {
                                if (xIndex == -1)
                                {
                                    fourGemMatches.Add(board.allGems[x, verticalPieces[1]]);
                                } else
                                {
                                    fourGemMatches.Add(board.allGems[xIndex, yIndex]);
                                }
                            }
                        }
                    }
                    verticalPieces.Clear();
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
