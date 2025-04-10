using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoltObject : MonoBehaviour
{
    VoltComponent vComp;

    public int vIn;
    public int vOut;

    // Start is called before the first frame update
    void Start()
    {
        vComp = new(vIn, vOut);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public VoltComponent GetVoltComponent()
    {
        return vComp;
    }
}
