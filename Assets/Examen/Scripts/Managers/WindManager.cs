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
            return "Noord";
        }
        else if (directionInt >= 11 && directionInt < 33)
        {
            return "Noord Noord Oost";
        }
        else if (directionInt >= 33 && directionInt < 56)
        {
            return "Noord Oost";
        }
        else if (directionInt >= 56 && directionInt < 78)
        {
            return "Oost Noord Oost";
        }
        else if (directionInt >= 78 && directionInt < 101)
        {
            return "Oost";
        }
        else if (directionInt >= 101 && directionInt < 123)
        {
            return "Oost Zuid Oost";
        }
        else if (directionInt >= 123 && directionInt < 146)
        {
            return "Zuid Oost";
        }
        else if (directionInt >= 146 && directionInt < 168)
        {
            return "Zuid Zuid Oost";
        }
        else if (directionInt >= 168 && directionInt < 191)
        {
            return "Zuid";
        }
        else if (directionInt >= 191 && directionInt < 213)
        {
            return "Zuid Zuid West";
        }
        else if (directionInt >= 213 && directionInt < 236)
        {
            return "Zuid West";
        }
        else if (directionInt >= 236 && directionInt < 258)
        {
            return "West Zuid West";
        }
        else if (directionInt >= 258 && directionInt < 281)
        {
            return "West";
        }
        else if (directionInt >= 281 && directionInt < 303)
        {
            return "West Noord West";
        }
        else if (directionInt >= 303 && directionInt < 326)
        {
            return "Noord West";
        }
        else if (directionInt >= 326 && directionInt < 348)
        {
            return "Noord Noord West";
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
