

/// <summary>
/// Represents a two-dimensional vector using axial coordinates (q, r), commonly used for hexagonal grid systems.
/// Traditionally, Vector2, or Vector2Int woudl use x and y, but here we use q and r to align with hex grid conventions.
/// Also this remains parallel with the Python version.
/// </summary>

class Vector2IntQR
{
    public int q { get; }
    public int r { get; }
    public Vector2IntQR(int q, int r)
    {
        this.q = q;
        this.r = r;
    }
    public float Magnitude => (float)System.Math.Sqrt(q * q + r * r);
    public void Deconstruct(out int q, out int r) { q = this.q; r = this.r; }

}