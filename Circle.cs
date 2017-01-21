using UnityEngine;
using System.Collections;

public class Circle
{
    public float X;
    public float Y;

    public float Radius;

    public Vector3 m_position;

    public Vector3 Position { get { return m_position; } set { m_position = value; SetPosition(value); } }

    public Circle(float x, float y, float z, float radius)
    {
        X = x;
        Y = y;

        Radius = radius;

        m_position = new Vector3(x, y, 1);
    }

    private void SetPosition(Vector3 pos)
    {
        X = pos.x;
        Y = pos.y;
    }

    public bool Contains(Vector3 position)
    {
        return (Vector3.Distance(m_position, position) < Radius);
    }
    public bool Contains(Circle circle)
    {
        return (Vector3.Distance(m_position, circle.Position) + circle.Radius < Radius);
    }

    public bool Intersects(Circle circle)
    {
        return (Vector3.Distance(m_position, circle.Position) < circle.Radius + Radius);
    }
}
