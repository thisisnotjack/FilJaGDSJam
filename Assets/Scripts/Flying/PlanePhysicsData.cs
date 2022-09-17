
using UnityEngine;

public class PlanePhysicsData 
{
    public Vector3 centerOfGravity;
    public float totalWeight;
    public Vector3 centerOfResistance;
    public float totalResistance;
    public Vector3 totalEngineThrustVector;
    public Vector3 totalEngineOrigin;
    

    public PlanePhysicsData(PhysicsAffector[] physicsAffectors)
    {
        float cummulatedWeight = 0;
        float cummulatedResistance = 0;
        Vector3 cummulatedCenterOfGravity = Vector3.zero;
        Vector3 cummulatedCenterOfResistance = Vector3.zero;
        
        foreach(PhysicsAffector affector in physicsAffectors)
        {
            cummulatedWeight += affector.weight;
            cummulatedResistance += affector.resistance;
            cummulatedCenterOfGravity += affector.weight * affector.position;
            cummulatedCenterOfResistance += affector.resistance * affector.position;
        }

        totalWeight = cummulatedWeight;
        totalResistance = cummulatedResistance;
        centerOfGravity = cummulatedCenterOfGravity / totalWeight;
        centerOfResistance = cummulatedCenterOfResistance / totalResistance;
    }
}
