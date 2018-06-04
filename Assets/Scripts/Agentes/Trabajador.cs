using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trabajador : MonoBehaviour, IGOAP {

 
    protected float velocidad;

    public Inventario inventario;

    public GameObject herramienta;


    public abstract Dictionary<string, object> CreateGoalState();

    public void ActionsFinished()
    {

    }

    public void PlanAborted(GoapAction abortedAction)
    {

    }

    public void PlanFailed(Dictionary<string, object> failedGoal)
    {

    }

    public void PlanFound(Dictionary<string,object> goal, Queue<GoapAction>Actions){

    }

    //informacion que usan las acciones y el planeador

    public Dictionary<string, object> GetWorldState()
    {
        Dictionary<string, object> datos = new Dictionary<string, object>();
        datos.Add("hayMineral", inventario.mineral>0);
        datos.Add("hayMadera", inventario.madera > 0);
        datos.Add("hayHerramienta", inventario.herramienta.activeInHierarchy);

        return datos;
    }
	// Use this for initialization
	void Start () {
        if (inventario == null)
        {
            inventario = GetComponent<Inventario>();
            inventario.numeroHerramientas++;
            inventario.herramienta = herramienta;

        }
        inventario.herramienta = Instantiate(herramienta);
        inventario.herramienta.transform.parent = transform;
        inventario.herramienta.transform.localPosition = new Vector3(1f, 2f, 0f);
		
	}

    public bool MoveAgent(GoapAction nextAction)
    {
        //mover el agente a su siquiente objeto
        float avance = velocidad * Time.deltaTime;
        transform.position = Vector3.MoveTowards
            (transform.position, nextAction.transform.position, avance);
        if (Vector3.Distance(transform.position, nextAction.Target.transform.position) < 2f)
        {
            //estamos en el target de la accion
            nextAction.SetInRange(true);
            return true;
        }
        else
            return false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}


}
