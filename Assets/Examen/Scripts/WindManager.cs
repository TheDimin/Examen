using Examen.Managers;
using UnityEngine.Events;

public class WindEvent : UnityEvent<int>
{
};

public static class IntHelpers
{
    public static string GetDirection(this int directionInt)
    {
        //http://snowfence.umn.edu/Components/winddirectionanddegrees.htm

        if (directionInt > 348 || directionInt < 11) // this wont work
        {
            return "N";
        }
        else if (directionInt >= 11 && directionInt < 33)
        {
            return "NNE";
        }
        else if (directionInt >= 33 && directionInt < 56)
        {
            return "NE";
        }
        else if (directionInt >= 56 && directionInt < 78)
        {
            return "ENE";
        }
        else if (directionInt >= 78 && directionInt < 101)
        {
            return "E";
        }
        else if (directionInt >= 101 && directionInt < 123)
        {
            return "ESE";
        }
        else if (directionInt >= 123 && directionInt < 146)
        {
            return "SE";
        }
        else if (directionInt >= 146 && directionInt < 168)
        {
            return "SSE";
        }
        else if (directionInt >= 168 && directionInt < 191)
        {
            return "S";
        }
        else if (directionInt >= 191 && directionInt < 213)
        {
            return "SSW";
        }
        else if (directionInt >= 213 && directionInt < 236)
        {
            return "SW";
        }
        else if (directionInt >= 236 && directionInt < 258)
        {
            return "WSW";
        }
        else if (directionInt >= 258 && directionInt < 281)
        {
            return "W";
        }
        else if (directionInt >= 281 && directionInt < 303)
        {
            return "WNW";
        }
        else if (directionInt >= 303 && directionInt < 326)
        {
            return "NW";
        }
        else if (directionInt >= 326 && directionInt < 348)
        {
            return "NNW";
        }

        return "UNKOWN";
    }

}

public class WindManager : Singelton<WindManager>
{
    public WindEvent OnDirectionChange = new WindEvent();
    public WindEvent OnSpeedChange = new WindEvent();

    private int direction = 0;
    private int speed = 0;

    public int GetWindDirection() => direction;

    public void SetWindDirection(int newDirection)
    {
        direction = newDirection;
        OnDirectionChange.Invoke(newDirection);
    }
    public int GetWindSpeed() => speed;

    public void SetWindSpeed(int newSpeed)
    {
        speed = newSpeed;
        OnSpeedChange.Invoke(newSpeed);
    }

    public override void Awake()
    {
    }
}
