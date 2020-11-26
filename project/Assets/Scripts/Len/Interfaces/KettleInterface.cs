using UnityEngine;

public class KettleInterface : Interface
{
    public Kettle kettle;

    private void Update()
    {
        kettle.Simulate(Time.deltaTime);
    }

    public KettleInterface()
    {
        interfaceType = InterfaceType.KETTLE_INTERFACE;
    }
}
