using System.Collections;
using System.Collections.Generic;
using CircuitSim;
using CircuitSim.Components;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CircuitSolver
{
    public const float FLOAT_DELTA = 0.01f;
    
    [Test]
    public void TestExampleCircuit()
    {
        /* Build the circuit */
        Node A = new Node();
        Node B = new Node();
        Node C = new Node();
        Node D = new Node();
        List<Node> nodes = new List<Node>() {A, B, C, D};

        SourceDC src = new SourceDC(10.0f);
        Resistor r1 = new Resistor(2);
        Resistor r2 = new Resistor(2);
        Resistor r3 = new Resistor(10);
        Resistor r4 = new Resistor(20);
        Resistor r5 = new Resistor(10);
        Resistor r6 = new Resistor(5);

        src.LinkT1(A);
        src.LinkT2(D);

        r1.LinkT1(A);
        r2.LinkT1(A);
        r1.LinkT2(B);
        r2.LinkT2(B);

        r3.LinkT1(B);
        r4.LinkT1(B);
        r3.LinkT2(C);
        r4.LinkT2(C);

        r5.LinkT1(B);
        r5.LinkT2(D);

        r6.LinkT1(C);
        r6.LinkT2(D);
        
        /* Solve for voltages */
        var ret = NodeVoltageSolver.Solve(nodes, D);

        Assert.AreEqual(10.0f,ret[0],  FLOAT_DELTA);
        Assert.AreEqual(8.434f,ret[1],  FLOAT_DELTA);
        Assert.AreEqual(3.614f,ret[2],  FLOAT_DELTA);
        Assert.AreEqual(0.0f,ret[3],  FLOAT_DELTA);
    }
    
    [Test]
    public void TestExampleCircuit2()
    {
        /* Build the circuit */
        Node A = new Node();
        Node B = new Node();
        Node C = new Node();
        List<Node> nodes = new List<Node>() {A, B, C};

        SourceDC src = new SourceDC(140.0f);
        Resistor r1 = new Resistor(20);
        Resistor r2 = new Resistor(6);
        Resistor r3 = new Resistor(5);

        src.LinkT1(A);
        src.LinkT2(C);

        r1.LinkT1(A);
        r1.LinkT2(B);

        r2.LinkT1(B);
        r2.LinkT2(C);

        r3.LinkT1(B);
        r3.LinkT2(C);
        
        /* Solve for voltages */
        var ret = NodeVoltageSolver.Solve(nodes, C);

        Assert.AreEqual(140.0f,ret[0],  FLOAT_DELTA);
        Assert.AreEqual(16.8f,ret[1],  FLOAT_DELTA);
        Assert.AreEqual(0,ret[2],  FLOAT_DELTA);
    }
}
