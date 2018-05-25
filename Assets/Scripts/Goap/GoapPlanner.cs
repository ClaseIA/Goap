using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//planear las acciones que pueden ser ejecutadas para dcumplir una meta
public class GoapPlanner {

    //la secuencia de acciones para cumplir una meta
    public Queue<GoapAction> ElPlan(
        GameObject agent,
        List<GoapAction> AccionesDisponibles,
        Dictionary<string, object> WorldState,
        Dictionary<string,object> Goal
        )
    {
        //limpiar las acciones
        foreach (GoapAction accion in AccionesDisponibles)
            accion.Reset();
        // de las acciones disponibles vamos a checar cuales se pueden
        //ejecutar a traves de sus precondiciones.
        List<GoapAction> AccionesUsables = new List<GoapAction>();

        foreach (GoapAction accion in AccionesDisponibles)
        {
            if (accion.checkPreconditions(agent))
                AccionesUsables.Add(accion);
        }

        //una vez que sabemos cuales acciones se pueden llevar acabo, construimos el arbol de accions para encontrar la meta

        List<Node> arbol = new List<Node>();
        //contruir el grafo o arbol
        Node inicial = new Node(null, 0, null, WorldState);

        ConstruyeGrafo();


    }

    private bool ContruyeGrafo(Node inicial, List<Node> arbol, List<GoapAction> acciones, Dictionary<string, object> goal)
    {
        bool encontreSolucion = false;
        //ver cada accion disponible y ver si puede usarse
        foreach (GoapAction accion in acciones)
        {
            //si el estado tiene las condiciones para cumplir las precondiciones de est5a accion
            if (CheckState(accion.GetPrecondiciones, inicial.estado))
            {

            }


        }
    }

    //verificar que las precondiciones estan en el estado del mundo en el nodo actual basta que una precondicion no este en el estado del mundo
    //para que no se cumpla la condicion
    private bool CheckState(Dictionary<string,object> precondiciones,Dictionary<string,object> estado)
    {
        bool sonIguales = true;

        //para cada precondicion tengo que checar que este en el estado del mundo


    }
    
    private class Node
    {
        public Node padre;
        public float costo;
        public GoapAction accion;
        public Dictionary<string, object> estado;

        public Node(Node padre, float costo, GoapAction accion,
            Dictionary<string, object> estado)
        {
            this.padre = padre;
            this.costo = costo;
            this.accion = accion;
            this.estado = estado;
        }
    }





	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
