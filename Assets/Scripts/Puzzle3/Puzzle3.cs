using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

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

    public override string ToString()
    {
        return $"VoltComponent({voltIn}:{voltOut})";
    }
}

public class Puzzle3 : MonoBehaviour
{
    List<VoltSlot> slots = new();
    List<VoltSlot> endSlots = new();

    Dictionary<string, VoltSlot> namedSlots = new();

    VoltSlot pIn;
    VoltSlot pOut;
    VoltSlot secIn;
    VoltSlot secOut;
    VoltSlotMulti shared;

    public XRSocketInteractor ASocket;
    public XRSocketInteractor BSocket;
    public XRSocketInteractor CSocket;
    public XRSocketInteractor DSocket;
    public XRSocketInteractor ESocket;

    public DoorController doorController;

    public int sharedIn = 30;
    public int sharedOut = 30;

    public int vPOut = 30;
    public int vSecOut = 12;

    public void ChangeTest()
    {
        bool didSucceed = Test();
        print($"Did the change work? {didSucceed}");

        if (didSucceed)
        {
            doorController?.RaiseDoor();
        }
    }

    public void ChangeSlot(string slot, SelectEnterEventArgs args)
    {
        namedSlots[slot].component = args.interactableObject.transform.GetComponent<VoltObject>().GetVoltComponent();
    }

    public void SocketA(SelectEnterEventArgs args)
    {
        namedSlots["A"].component = args.interactableObject.transform.GetComponent<VoltObject>().GetVoltComponent();
        print($"Changing A {namedSlots["A"].component}");
        ChangeTest();
    }

    public void SocketB(SelectEnterEventArgs args)
    {
        namedSlots["B"].component = args.interactableObject.transform.GetComponent<VoltObject>().GetVoltComponent();
        print($"Changing B {namedSlots["B"].component}");
        ChangeTest();
    }

    public void SocketC(SelectEnterEventArgs args)
    {
        namedSlots["C"].component = args.interactableObject.transform.GetComponent<VoltObject>().GetVoltComponent();
        print($"Changing C {namedSlots["C"].component}");
        ChangeTest();
    }
    public void SocketD(SelectEnterEventArgs args)
    {
        namedSlots["D"].component = args.interactableObject.transform.GetComponent<VoltObject>().GetVoltComponent();
        print($"Changing D {namedSlots["D"].component}");
        ChangeTest();
    }
    public void SocketE(SelectEnterEventArgs args)
    {
        namedSlots["E"].component = args.interactableObject.transform.GetComponent<VoltObject>().GetVoltComponent();
        print($"Changing E {namedSlots["E"].component}");
        ChangeTest();
    }

    public void ClearSlot(string slot)
    {
        namedSlots[slot].component = null;
        ChangeTest();
    }

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

    private void OnValidate()
    {
        if (shared != null)
        {
            shared.component.voltIn = sharedIn;
            shared.component.voltOut = sharedOut;

            pOut.component.voltIn = vPOut;
            secOut.component.voltIn = vSecOut;

            Test();
        }
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

        shared = new();

        namedSlots.Add("A", new());
        namedSlots.Add("B", new());
        namedSlots.Add("C", shared);
        namedSlots.Add("D", new());
        namedSlots.Add("E", new());

        ASocket.selectEntered.AddListener(SocketA);
        BSocket.selectEntered.AddListener(SocketB);
        CSocket.selectEntered.AddListener(SocketC);
        DSocket.selectEntered.AddListener(SocketD);
        ESocket.selectEntered.AddListener(SocketE);

        pIn.NextSlot(namedSlots["A"]).NextSlot(namedSlots["B"]).NextSlot(namedSlots["C"]).NextSlot(pOut);

        secIn.NextSlot(namedSlots["E"]).NextSlot(namedSlots["C"]).NextSlot(namedSlots["D"]).NextSlot(secOut);

        //pIn.NextSlot(625, 30).NextSlot(30, 30).NextSlot(shared).NextSlot(pOut); // shared = 3rd nextslot

        //secIn.NextSlot(12, 30).NextSlot(shared).NextSlot(30, 12).NextSlot(secOut); // shared = 2rd nextslot

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
            VoltSlot nextSlot = slot.next;
            if (slot == shared) nextSlot = pOut;

            if (slot.component.voltOut == nextSlot.component.voltIn) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;

            Handles.Label(transform.position + Vector3.right * i, $"VIn:{slot.component.voltIn}\nVOut:{slot.component.voltOut}\nID:{slot.ID}");

            Gizmos.DrawLine(transform.position + Vector3.right * i, transform.position + Vector3.right * (i+1));

            if (slot == shared)
            {
                slot = pOut;
            }
            else slot = slot.next;

            //slot = slot.next;
            i++;
        }

        Handles.Label(transform.position + Vector3.right * i, $"VIn:{slot.component.voltIn}\nVOut:{slot.component.voltOut}\nID:{slot.ID}");

        slot = secIn;

        i = 0;

        while (slot.next != null)
        {
            if (slot.component.voltOut == slot.next.component.voltIn) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;

            Handles.Label(transform.position + Vector3.up * 1 + Vector3.right * i, $"VIn:{slot.component.voltIn}\nVOut:{slot.component.voltOut}\nID:{slot.ID}");

            Gizmos.DrawLine(transform.position + Vector3.up * 1 + Vector3.right * i, transform.position + Vector3.up * 1 + Vector3.right * (i + 1));

            slot = slot.next;

            i++;
        }

        Handles.Label(transform.position + Vector3.up * 1 + Vector3.right * i, $"VIn:{slot.component.voltIn}\nVOut:{slot.component.voltOut}\nID:{slot.ID}");
    }
}
