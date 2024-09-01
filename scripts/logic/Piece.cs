using System.Diagnostics.CodeAnalysis;

namespace Tetris.scripts.logic;

public class Piece
{
    public PieceType Type { get; }
    public int[,] Matrix { get; } = new int[4, 4];
    public int[,,] CollisionCoordinates { get; }
    public int[,,,] CollisionChecks { get; }
    
    private readonly int[,,] _matrices;
    public int _rotation;
    
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum PieceType
    {
        NULL = -1,
        I = 0,
        O = 1,
        Z = 2,
        S = 3,
        L = 4,
        J = 5, 
        T = 6,
        NUM_TYPES = 7
    }

    public Piece(PieceType type)
    {   
       Type = type;

       _matrices = PieceData.getRotations((int)type);
       CollisionCoordinates = PieceData.getCollisionCoordinates((int)type);
       CollisionChecks = PieceData.getCollisionChecks((int)type);
       updateRotation();
    }

    public int[,] getCollisionChecks(int rotation, int direction)
    {
        int[,] collision_checks = new int[4, 2];
        
        for (int i = 0; i < 4; i++)
        {
            collision_checks[i, 0] = CollisionChecks[rotation, direction, i, 0];         
            collision_checks[i, 1] = CollisionChecks[rotation, direction, i, 1];         
        }
        
        return collision_checks;
    }

    public void setRotation(int rotation)
    {
        _rotation = rotation;
        
        updateRotation();
    }

    public void rotateLeft()
    {
        setRotation(getPreviousRotation());
    }
    public void rotateRight()
    {
        setRotation(getNextRotation());
    }

    public int getNextRotation()
    {
        if (_rotation + 1 > 3)
        {
            return 0;
        }
        return _rotation + 1;
    }
    
    public int getPreviousRotation()
    {
        if (_rotation - 1 < 0)
        {
            return 3;
        }
        return _rotation - 1;
    }
    
    private void updateRotation()
    {
        for (int i = 0; i < _matrices.GetLength(1); i++)
        {
            for (int j = 0; j < _matrices.GetLength(2); j++)
            {
                Matrix[i, j] = _matrices[_rotation, i, j];
            }
        }
    }
}