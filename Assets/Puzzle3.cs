using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public interface IVoltSlot
{
    public bool TestSlot();
}

public class VoltSlot : IVoltSlot
{
    public VoltComponent component;
    public VoltSlot next;
    public VoltSlot previous;
    public int ID = 0;

    public static int curID = 0;

    public VoltSlot(int vIn, int vOut)
    {
        component = new(vIn, vOut);
        ID = curID;
        curID++;
    }

    public VoltSlot()
    {
        ID = curID;
        curID++;
    }

    public override string ToString()
    {
        return $"VoltSlot[{ID}:{curID}]({component?.voltIn}:{component?.voltOut})";
    }

    public virtual VoltSlot NextSlot(int vIn, int vOut)
    {
        var slot = new VoltSlot(vIn, vOut);
        next = slot;
        slot.previous = this;

        return slot;
    }

    public virtual VoltSlot NextSlot(VoltSlot slot)
    {
        next = slot;
        slot.previous = this;

        return slot;
    }

    public virtual VoltSlot NextSlot()
    {
        var slot = new VoltSlot();
        next = slot;
        slot.previous = this;

        return slot;
    }

    public virtual bool TestSlot()
    {
        //var nextSlot = testSlot.next;

        Debug.Log($"Testing slots {this}:{next}");

        if (next == null || next.component == null) return false;

        if (component.voltOut == next.component.voltIn)
        {
            //print("Testing next slot " + testSlot + " : " + nextSlot);
            //print($"WHAT IS NEXT SLOT? {nextSlot.next} {nextSlot.next == null}");
            if (next.next == null) return true;
            else return next.TestSlot();
        }

        return false;
    }
}

public class VoltSlotMulti : VoltSlot
{
    public List<VoltSlot> slotsIn;
    public List<VoltSlot> slotsOut;

    public VoltSlotMulti(int vIn, int vOut)
    {
        component = new(vIn, vOut);
        ID = curID;
        curID++;

        slotsIn = new List<VoltSlot>();
        slotsOut = new List<VoltSlot>();
    }

    public VoltSlotMulti()
    {
        ID = curID;
        curID++;

        slotsIn = new List<VoltSlot>();
        slotsOut = new List<VoltSlot>();
    }

    public override VoltSlot NextSlot(int vIn, int vOut)
    {
        var slot = new VoltSlot(vIn, vOut);
        next = slot;
        slot.previous = this;

        slotsOut.Add(slot);

        return slot;
    }

    public override VoltSlot NextSlot(VoltSlot slot)
    {
        next = slot;
        slot.previous = this;

        slotsOut.Add(slot);

        return slot;
    }

    public override VoltSlot NextSlot()
    {
        var slot = new VoltSlot();
        next = slot;
        slot.previous = this;

        slotsOut.Add(slot);

        return slot;
    }

    public override bool TestSlot()
    {
        Debug.Log($"Testing slots {this}:{next}");

        if (slotsOut.Count == 0) return false;

        //bool pathFailed = false;
        foreach (var slot in slotsOut)
        {
            if (component.voltOut == slot.component.voltIn)
            {
                if (slot.next != null)
                {
                    if (slot.TestSlot() == false) return false;
                }
            }
            else return false;
        }

        return true;
    }
}

public class VoltComponent
{
    public int voltIn;
    public int voltOut;

    public VoltComponent(int vIn, int vOut)
    {
        voltIn = vIn;
        voltOut = vOut;
    }
}

public class Puzzle3 : MonoBehaviour
{
    List<VoltSlot> slots = new();
    List<VoltSlot> endSlots = new();

    VoltSlot pIn;
    VoltSlot pOut;
    VoltSlot secIn;
    VoltSlot secOut;

    bool TestSlot(VoltSlot testSlot)
    {
        var nextSlot = testSlot.next;

        print($"Testing slots {testSlot}:{nextSlot}");

        if (nextSlot == null || nextSlot.component == null) return false;

        if (testSlot.component.voltOut == nextSlot.component.voltIn)
        {
            //print("Testing next slot " + testSlot + " : " + nextSlot);
            //print($"WHAT IS NEXT SLOT? {nextSlot.next} {nextSlot.next == null}");
            if (nextSlot.next == null) return true;
            else return TestSlot(nextSlot);
        }

        return false;

    }

    bool Test()
    {
        bool powerSuccess = pIn.TestSlot();//TestSlot(pIn);
        bool securitySuccess = secIn.TestSlot();//TestSlot(secIn);

        print($"TEST RESULTS: POWER {powerSuccess}, SEC {securitySuccess}");

        if (powerSuccess && securitySuccess) return true;
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        pIn = new VoltSlot(625, 625);
        pOut = new VoltSlot(30, 30);
        secIn = new VoltSlot(12, 12);
        secOut = new VoltSlot(12, 12);

        endSlots.Add(pOut);
        endSlots.Add(secOut);

        VoltSlotMulti shared = new(30, 30);

        pIn.NextSlot(625, 30).NextSlot(30, 30).NextSlot(shared).NextSlot(pOut); // shared = 3rd nextslot

        secIn.NextSlot(12, 30).NextSlot(shared).NextSlot(30, 12).NextSlot(secOut); // shared = 2rd nextslot

        print("Did we succeed? " + Test());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        VoltSlot slot = pIn;

        if (slot == null) return;

        int i = 0;

        while (slot.next != null)
        {
            Gizmos.DrawLine(transform.position + Vector3.right * i, transform.position + Vector3.right * (i+1));
            slot = slot.next;
            i++;
        }

        slot = secIn;

        i = 0;

        while (slot.next != null)
        {
            Gizmos.DrawLine(transform.position + Vector3.up * 1 + Vector3.right * i, transform.position + Vector3.up * 1 + Vector3.right * (i + 1));
            slot = slot.next;
            i++;
        }
    }
}
