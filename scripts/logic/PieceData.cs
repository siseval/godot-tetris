namespace Tetris.scripts.logic;

public static class PieceData
{
    public static int[,,] getRotations(int type)
    {
        int[,,] matrices = new int[4, 4, 4];

        for (int r = 0; r < _MATRICES.GetLength(1); r++)
        {
            for (int i = 0; i < _MATRICES.GetLength(2); i++)
            {
                for (int j = 0; j < _MATRICES.GetLength(3); j++)
                {
                    matrices[r, j, i] = _MATRICES[type, r, j, i];
                }
            }
        }
        
        return matrices;
    }

    public static int[,,] getCollisionCoordinates(int type)
    {
        int collision_coordinate = 0;
        int[,,] collision_coordinates = new int[4, 4, 2];
        
        for (int r = 0; r < _MATRICES.GetLength(1); r++)
        {
            for (int i = 0; i < _MATRICES.GetLength(2); i++)
            {
                for (int j = 0; j < _MATRICES.GetLength(3); j++)
                {
                    if (_MATRICES[type, r, i, j] == 0) continue;
                    collision_coordinates[r, collision_coordinate, 0] = i; 
                    collision_coordinates[r, collision_coordinate, 1] = j; 
                    collision_coordinate += 1;
                }
            }

            collision_coordinate = 0;
        }
              
        return collision_coordinates;
    }
    
    public static int[,] getCollisionCoordinates(int type, int rotation)
    {
        int collision_coordinate = 0;
        int[,] collision_coordinates = new int[4, 2];

        for (int i = 0; i < _MATRICES.GetLength(2); i++)
        {
            for (int j = 0; j < _MATRICES.GetLength(3); j++)
            {
                if (_MATRICES[type, rotation, i, j] == 0) continue;
                collision_coordinates[collision_coordinate, 0] = i; 
                collision_coordinates[collision_coordinate, 1] = j; 
                collision_coordinate += 1;
            }
        }
              
        return collision_coordinates;
    }

    private static readonly byte[,,,] _MATRICES = {
        // I PIECE:
        {
            // ROTATION 0:
            {
                { 0, 0, 0, 0 },
                { 1, 1, 1, 1 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 1:
            {
                { 0, 0, 1, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 1, 0 }
            },
            // ROTATION 2:
            {
                { 0, 0, 0, 0 },
                { 1, 1, 1, 1 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 3:
            {
                { 0, 0, 1, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 1, 0 }
            }
        },
        // O PIECE:
        {
            // ROTATION 0:
            {
                { 0, 0, 0, 0 },
                { 0, 1, 1, 0 },
                { 0, 1, 1, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 1:
            {
                { 0, 0, 0, 0 },
                { 0, 1, 1, 0 },
                { 0, 1, 1, 0 },
                { 0, 0, 0, 0 }

            },
            // ROTATION 2:
            {
                { 0, 0, 0, 0 },
                { 0, 1, 1, 0 },
                { 0, 1, 1, 0 },
                { 0, 0, 0, 0 }

            },
            // ROTATION 3:
            {
                { 0, 0, 0, 0 },
                { 0, 1, 1, 0 },
                { 0, 1, 1, 0 },
                { 0, 0, 0, 0 }

            } 
        }, 
        // S PIECE:
        {
            // ROTATION 0:
            {
                { 0, 0, 0, 0 },
                { 0, 1, 1, 0 },
                { 1, 1, 0, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 1:
            {
                { 1, 0, 0, 0 },
                { 1, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 0, 0 }

            },
            // ROTATION 2:
            {
                { 0, 0, 0, 0 },
                { 0, 1, 1, 0 },
                { 1, 1, 0, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 3:
            {
                { 1, 0, 0, 0 },
                { 1, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 0, 0 }

            }
        }, 
        // Z PIECE:
        {
            // ROTATION 0:
            {
                { 0, 0, 0, 0 },
                { 1, 1, 0, 0 },
                { 0, 1, 1, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 1:
            {
                { 0, 0, 1, 0 },
                { 0, 1, 1, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 0, 0 }

            },
            // ROTATION 2:
            {
                { 0, 0, 0, 0 },
                { 1, 1, 0, 0 },
                { 0, 1, 1, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 3:
            {
                { 0, 0, 1, 0 },
                { 0, 1, 1, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 0, 0 }
            }
        }, 
        // L PIECE:
        {
            // ROTATION 0:
            {
                { 0, 0, 0, 0 },
                { 1, 1, 1, 0 },
                { 1, 0, 0, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 1:
            {
                { 1, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 2:
            {
                { 0, 0, 0, 0 },
                { 0, 0, 1, 0 },
                { 1, 1, 1, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 3:
            {
                { 0, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 1, 1, 0 },
                { 0, 0, 0, 0 }
            }
        }, 
        // J PIECE:
        {
            // ROTATION 0:
            {
                { 0, 0, 0, 0 },
                { 1, 1, 1, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 1:
            {
                { 0, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 1, 1, 0, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 2:
            {
                { 0, 0, 0, 0 },
                { 1, 0, 0, 0 },
                { 1, 1, 1, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 3:
            {
                { 0, 1, 1, 0 },
                { 0, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 0, 0 }
            }
        }, 

        // T PIECE:
        {
            
            // ROTATION 0:
            {
                { 0, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 1, 1, 1, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 1:
            {
                { 0, 1, 0, 0 },
                { 0, 1, 1, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 2:
            {
                { 0, 0, 0, 0 },
                { 1, 1, 1, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 3:
            {
                { 0, 1, 0, 0 },
                { 1, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 0, 0 }
            }
        } 
    };
}