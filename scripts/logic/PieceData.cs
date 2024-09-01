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

    public static int[,,,] getCollisionChecks(int type)
    {
        int[,,,] collision_checks = new int[4, 2, 4, 2];
        
        for (int i = 0; i < _COLLISION_CHECKS.GetLength(1); i++)
        {
            for (int j = 0; j < _COLLISION_CHECKS.GetLength(2); j++)
            {
                for (int l = 0; l < _COLLISION_CHECKS.GetLength(3); l++)
                {
                    collision_checks[i, j, l, 0] = _COLLISION_CHECKS[type, i, j, l, 0];
                    collision_checks[i, j, l, 1] = _COLLISION_CHECKS[type, i, j, l, 1];
                }
            }
        }

        return collision_checks;
    }
    
    private static readonly byte[,,,] _MATRICES = 
    {
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
                { 0, 0, 0, 0 },
                { 1, 1, 1, 1 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 3:
            {
                { 0, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 1, 0, 0 }
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
                { 0, 1, 1, 0 },
                { 1, 1, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 1:
            {
                { 0, 1, 0, 0 },
                { 0, 1, 1, 0 },
                { 0, 0, 1, 0 },
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
                { 1, 1, 0, 0 },
                { 0, 1, 1, 0 },
                { 0, 0, 0, 0 },
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
                { 0, 1, 0, 0 },
                { 1, 1, 0, 0 },
                { 1, 0, 0, 0 },
                { 0, 0, 0, 0 }
            }
        }, 
        // L PIECE:
        {
            // ROTATION 0:
            {
                { 0, 0, 1, 0 },
                { 1, 1, 1, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 1:
            {
                { 0, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 1, 1, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 2:
            {
                { 0, 0, 0, 0 },
                { 1, 1, 1, 0 },
                { 1, 0, 0, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 3:
            {
                { 1, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 0, 0 }
            }
        }, 
        // J PIECE:
        {
            
            // ROTATION 0:
            {
                { 1, 0, 0, 0 },
                { 1, 1, 1, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 1:
            {
                { 0, 1, 1, 0 },
                { 0, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 2:
            {
                { 0, 0, 0, 0 },
                { 1, 1, 1, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 0 }
            },
            // ROTATION 2:
            {
                { 0, 1, 0, 0 },
                { 0, 1, 0, 0 },
                { 1, 1, 0, 0 },
                { 0, 0, 0, 0 }
            }
        }, 

        // T PIECE:
        {
            
            // ROTATION 0:
            {
                { 0, 1, 0, 0 },
                { 1, 1, 1, 0 },
                { 0, 0, 0, 0 },
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
    
    private static readonly int[,,,,] _COLLISION_CHECKS =
    {
        // I PIECE:
        {
            // FROM 0:
            {
                // LEFT:
                {
                    { 0, -1 },
                    { 0, 2 },
                    { -2, -1 },
                    { 1, 2 }
                },
                // RIGHT:
                {
                    { 0, -2 },
                    { 0, 1 },
                    { 1, -2 },
                    { -2, 1 }
                }
            },
            // FROM 1:
            {
                // LEFT:
                {
                    { 0, 2 },
                    { 0, -1 },
                    { -1, 2 },
                    { 2, -1 }
                },
                // RIGHT:
                {
                    { 0, -1 },
                    { 0, 2 },
                    { -2, -1 },
                    { 1, 2 }
                }
            },
            // FROM 2:
            {
                // LEFT:
                {
                    { 0, 1 },
                    { 0, -2 },
                    { 2, 1 },
                    { -1, -2 }
                },
                // RIGHT:
                {
                    { 0, 2 },
                    { 0, -1 },
                    { -1, 2 },
                    { 2, -1 }
                }
            },
            // FROM 3:
            {
                // LEFT:
                {
                    { 0, -2 },
                    { 0, 1 },
                    { 1, -2 },
                    { -2, 1 }
                },
                // RIGHT: 
                {
                    { 0, 1 },
                    { 0, -2 },
                    { 2, 1 },
                    { -1, -2 }
                }
            }
        },
        // O PIECE:
        {
            // FROM 0:
            {
                // LEFT:
                {
                    { 0, 1 },
                    { -1, 1 },
                    { 2, 0 },
                    { 2, 1 }
                }, 
                // RIGHT:
                {
                    { 0, -1 },
                    { -1, -1 },
                    { 2, 0 },
                    { 2, -1 }
                }
            }, 
            // FROM 1:
            {
                // LEFT:
                {
                    { 0, 1 },
                    { 1, 1 },
                    { -2, 0 },
                    { -2, 1 }
                },
                // RIGHT:
                {
                    { 0, 1 },
                    { 1, 1 },
                    { -2, 0 },
                    { -2, 1 }
                }
            },
            // FROM 2:
            {
                // LEFT:
                {
                    { 0, -1 },
                    { -1, -1 },
                    { 2, 0 },
                    { 2, -1 }
                },
                // RIGHT:
                {
                    { 0, 1 },
                    { -1, 1 },
                    { 2, 0 },
                    { 2, 1 }
                }
            },
            // FROM 3:
            {
                // LEFT:
                {
                    { 0, -1 },
                    { 1, -1 },
                    { -2, 0 },
                    { -2, -1 }
                },
                // RIGHT:
                {
                    { 0, -1 },
                    { 1, -1 },
                    { -2, 0 },
                    { -2, -1 }
                }
            }
        },
        // S PIECE:
        {
            // FROM 0:
            {
                // LEFT:
                {
                    { 0, 1 },
                    { -1, 1 },
                    { 2, 0 },
                    { 2, 1 }
                }, 
                // RIGHT:
                {
                    { 0, -1 },
                    { -1, -1 },
                    { 2, 0 },
                    { 2, -1 }
                }
            }, 
            // FROM 1:
            {
                // LEFT:
                {
                    { 0, 1 },
                    { 1, 1 },
                    { -2, 0 },
                    { -2, 1 }
                },
                // RIGHT:
                {
                    { 0, 1 },
                    { 1, 1 },
                    { -2, 0 },
                    { -2, 1 }
                }
            },
            // FROM 2:
            {
                // LEFT:
                {
                    { 0, -1 },
                    { -1, -1 },
                    { 2, 0 },
                    { 2, -1 }
                },
                // RIGHT:
                {
                    { 0, 1 },
                    { -1, 1 },
                    { 2, 0 },
                    { 2, 1 }
                }
            },
            // FROM 3:
            {
                // LEFT:
                {
                    { 0, -1 },
                    { 1, -1 },
                    { -2, 0 },
                    { -2, -1 }
                },
                // RIGHT:
                {
                    { 0, -1 },
                    { 1, -1 },
                    { -2, 0 },
                    { -2, -1 }
                }
            }
        },
        // Z PIECE:
        {
            // FROM 0:
            {
                // LEFT:
                {
                    { 0, 1 },
                    { -1, 1 },
                    { 2, 0 },
                    { 2, 1 }
                }, 
                // RIGHT:
                {
                    { 0, -1 },
                    { -1, -1 },
                    { 2, 0 },
                    { 2, -1 }
                }
            }, 
            // FROM 1:
            {
                // LEFT:
                {
                    { 0, 1 },
                    { 1, 1 },
                    { -2, 0 },
                    { -2, 1 }
                },
                // RIGHT:
                {
                    { 0, 1 },
                    { 1, 1 },
                    { -2, 0 },
                    { -2, 1 }
                }
            },
            // FROM 2:
            {
                // LEFT:
                {
                    { 0, -1 },
                    { -1, -1 },
                    { 2, 0 },
                    { 2, -1 }
                },
                // RIGHT:
                {
                    { 0, 1 },
                    { -1, 1 },
                    { 2, 0 },
                    { 2, 1 }
                }
            },
            // FROM 3:
            {
                // LEFT:
                {
                    { 0, -1 },
                    { 1, -1 },
                    { -2, 0 },
                    { -2, -1 }
                },
                // RIGHT:
                {
                    { 0, -1 },
                    { 1, -1 },
                    { -2, 0 },
                    { -2, -1 }
                }
            }
        }, 
        // L PIECE:
        {
            // FROM 0:
            {
                // LEFT:
                {
                    { 0, 1 },
                    { -1, 1 },
                    { 2, 0 },
                    { 2, 1 }
                }, 
                // RIGHT:
                {
                    { 0, -1 },
                    { -1, -1 },
                    { 2, 0 },
                    { 2, -1 }
                }
            }, 
            // FROM 1:
            {
                // LEFT:
                {
                    { 0, 1 },
                    { 1, 1 },
                    { -2, 0 },
                    { -2, 1 }
                },
                // RIGHT:
                {
                    { 0, 1 },
                    { 1, 1 },
                    { -2, 0 },
                    { -2, 1 }
                }
            },
            // FROM 2:
            {
                // LEFT:
                {
                    { 0, -1 },
                    { -1, -1 },
                    { 2, 0 },
                    { 2, -1 }
                },
                // RIGHT:
                {
                    { 0, 1 },
                    { -1, 1 },
                    { 2, 0 },
                    { 2, 1 }
                }
            },
            // FROM 3:
            {
                // LEFT:
                {
                    { 0, -1 },
                    { 1, -1 },
                    { -2, 0 },
                    { -2, -1 }
                },
                // RIGHT:
                {
                    { 0, -1 },
                    { 1, -1 },
                    { -2, 0 },
                    { -2, -1 }
                }
            }
        },
        
        // J PIECE:
        {
            // FROM 0:
            {
                // LEFT:
                {
                    { 0, 1 },
                    { -1, 1 },
                    { 2, 0 },
                    { 2, 1 }
                }, 
                // RIGHT:
                {
                    { 0, -1 },
                    { -1, -1 },
                    { 2, 0 },
                    { 2, -1 }
                }
            }, 
            // FROM 1:
            {
                // LEFT:
                {
                    { 0, 1 },
                    { 1, 1 },
                    { -2, 0 },
                    { -2, 1 }
                },
                // RIGHT:
                {
                    { 0, 1 },
                    { 1, 1 },
                    { -2, 0 },
                    { -2, 1 }
                }
            },
            // FROM 2:
            {
                // LEFT:
                {
                    { 0, -1 },
                    { -1, -1 },
                    { 2, 0 },
                    { 2, -1 }
                },
                // RIGHT:
                {
                    { 0, 1 },
                    { -1, 1 },
                    { 2, 0 },
                    { 2, 1 }
                }
            },
            // FROM 3:
            {
                // LEFT:
                {
                    { 0, -1 },
                    { 1, -1 },
                    { -2, 0 },
                    { -2, -1 }
                },
                // RIGHT:
                {
                    { 0, -1 },
                    { 1, -1 },
                    { -2, 0 },
                    { -2, -1 }
                }
            }
        },
        // T PIECE:
        {
            // FROM 0:
            {
                // LEFT:
                {
                    { 0, 1 },
                    { -1, 1 },
                    { 2, 0 },
                    { 2, 1 }
                }, 
                // RIGHT:
                {
                    { 0, -1 },
                    { -1, -1 },
                    { 2, 0 },
                    { 2, -1 }
                }
            }, 
            // FROM 1:
            {
                // LEFT:
                {
                    { 0, 1 },
                    { 1, 1 },
                    { -2, 0 },
                    { -2, 1 }
                },
                // RIGHT:
                {
                    { 0, 1 },
                    { 1, 1 },
                    { -2, 0 },
                    { -2, 1 }
                }
            },
            // FROM 2:
            {
                // LEFT:
                {
                    { 0, -1 },
                    { -1, -1 },
                    { 2, 0 },
                    { 2, -1 }
                },
                // RIGHT:
                {
                    { 0, 1 },
                    { -1, 1 },
                    { 2, 0 },
                    { 2, 1 }
                }
            },
            // FROM 3:
            {
                // LEFT:
                {
                    { 0, -1 },
                    { 1, -1 },
                    { -2, 0 },
                    { -2, -1 }
                },
                // RIGHT:
                {
                    { 0, -1 },
                    { 1, -1 },
                    { -2, 0 },
                    { -2, -1 }
                }
            }
        } 
    };
}