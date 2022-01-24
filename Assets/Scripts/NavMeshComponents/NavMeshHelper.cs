using UnityEngine;
using UnityEngine.AI;

public class NavMeshHelper
{
    /**

        return a random coordinate in NavMesh thats in the NavMesh area provided
        origin  => center point from where to find the random coordinate
        range   => diameter from the origin determining the range of the available coordinates for the random selection
        area    => On Which area the coorinate must be available

    */
    public static Vector3 RandomCoordinateInRange(Vector3 origin, float range, int area){
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * range;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, range, 1 << area);
        return navHit.position;
    }

    /*
        Same as RandomCoordinateInRange except the area can be specified by it's name
        be aware it is less efficient
    */
    public static Vector3 RandomCoordinateInRange(Vector3 origin, float range, string area){
        int a = NavMesh.GetAreaFromName(area);
        return RandomCoordinateInRange(origin, range, a);
    }

    public static Vector3 GetCloseCoordinate(Vector3 origin, int area){
        NavMeshHit navHit;
        NavMesh.SamplePosition(origin, out navHit, 5f, 1 << area);
        return navHit.position;
    }

}
