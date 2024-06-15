namespace Engine.Maths;

public readonly partial struct AngleF
{
    private const float DegreesToRadians = MathF.PI / 180.0f;
    private const float RadiansToDegrees = 180.0f / MathF.PI;

    public AngleF(float radians)
    {
        Radians = radians;
    }

    public readonly float Radians { get; init; }
    public readonly float Degrees => Radians * RadiansToDegrees;

    public static AngleF Zero => new(0f);
    public static AngleF North => new(0.0f);
    public static AngleF NorthEast => new(MathF.PI * 0.25f);
    public static AngleF East => new(MathF.PI * 0.5f);
    public static AngleF SouthEast => new(MathF.PI * 0.75f);
    public static AngleF South => new(MathF.PI);
    public static AngleF SouthWest => new(MathF.PI * 1.25f);
    public static AngleF West => new(MathF.PI * 1.5f);
    public static AngleF NorthWest => new(MathF.PI * 1.75f);

    public static AngleF FromRadians(float radians)
    {
        return new AngleF(radians);
    }

    public static AngleF FromDegrees(float degrees)
    {
        var radians = degrees * DegreesToRadians;
        return new AngleF(radians);
    }
}