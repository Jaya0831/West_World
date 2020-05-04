using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanTheHouse : State<Elsa>
{
    public override StateName stateName
    {
        get
        {
            return State<Elsa>.StateName.CleanTheHouse;
        }
    }
    public override void Enter(Elsa elsa)
    {
        if (elsa.GetComponent<Elsa>().path.Count == 0)
        {
            //Debug.Log("CleanTheHouse Enter path: " + elsa.GetComponent<Elsa>().path.Count);
            elsa.GoTo(elsa.grid.GetComponent<Grid>().objectInf[(int)Node.Location_Type.Floor][Random.Range(0, elsa.grid.GetComponent<Grid>().objectInf[(int)Node.Location_Type.Floor].Count)]);
        }
    }
    public override void Execute(Elsa elsa)
    {
        if (elsa.GetComponent<Elsa>().path.Count == 0)
        {
            //Debug.Log("CleanTheHouse Execute path: " + elsa.GetComponent<Elsa>().path.Count);
            elsa.GoTo(elsa.grid.GetComponent<Grid>().objectInf[(int)Node.Location_Type.Floor][Random.Range(0, elsa.grid.GetComponent<Grid>().objectInf[(int)Node.Location_Type.Floor].Count)]);
        }
    }
    public override void Exit(Elsa miner)
    {
        Debug.Log("Exit CleanTheHouse!");
    }
}
