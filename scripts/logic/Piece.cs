namespace Tetris.scripts.logic;

public class Piece
{
    public PieceType Type { get; set; }
    public int[,] Matrix { get; set; } = new int[4, 4];
    public int[,,] CollisionCoordinates { get; set; } = new int[4, 4, 2];
    
    private readonly int[,,] _matrices;
    public int _rotation;
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
        NUM_TYPES = 7,
    }

    public Piece(PieceType type)
    {   
       Type = type;

       _matrices = PieceData.getRotations((int)type);
       CollisionCoordinates = PieceData.getCollisionCoordinates((int)type);
       updateRotation();
    }

    public void rotateLeft()
    {
        _rotation = getPreviousRotation();

        updateRotation();
    }
    public void rotateRight()
    {
        _rotation = getNextRotation(); 

        updateRotation();
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