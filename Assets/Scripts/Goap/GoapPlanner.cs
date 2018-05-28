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

        bool exito= ConstruyeGrafo(inicial,arbol,AccionesUsables,Goal);

        if (!exito)
        {
            Debug.Log("no encontro una solucion :(");
            return null;
        }

        // si llega aqui encontre al menos una solucion
        //tengo que ver que solucion es la demmenor costo

        Node masBarato = null;

        foreach (Node hoja in arbol)
        {
            if (masBarato == null)
            
                masBarato = hoja;
            
            else
                if(hoja.costo < masBarato.costo)
               masBarato=hoja;
            
        }

        //como ya enconter el nodo mas barato hago un back trackin para regresar la lista de acciones a seguir

        List<GoapAction> resultado = new List<GoapAction>();
        Node temp = masBarato;
        while (temp != null)
        {
            if (temp.accion != null)
            {
                //agrego la accion al frente de la lista de acciones
                resultado.Insert(0,temp.accion);
            }
            temp = temp.padre;
        }
        Queue<GoapAction> colaAcciones = new Queue<GoapAction>();
        foreach (GoapAction a in resultado)
            colaAcciones.Enqueue(a);

        //lo logramos ten tu plan

        return colaAcciones;
    }

    private bool ConstruyeGrafo(Node inicial, List<Node> arbol, List<GoapAction> acciones, Dictionary<string, object> goal)
    {
        bool encontreSolucion = false;
        //ver cada accion disponible y ver si puede usarse
        foreach (GoapAction accion in acciones)
        {
            //si el estado tiene las condiciones para cumplir las precondiciones de est5a accion
            if (CheckState(accion.GetPrecondiciones, inicial.estado))
            {
                //los efectos de la accion se realizan en el nodo
                Dictionary<string, object> estadoActual = ActualizaEstado(inicial.estado, accion.GetEfectos);

                //con el estado actualizado, creamos su nodo
                Node nodo = new Node(inicial,inicial.costo + accion.cost, accion, estadoActual);
            
                //verifico si este nuevo nodo cumple con la meta
                if (CheckState(goal, estadoActual))
                {
                    //encon tro una solucion
                    arbol.Add(nodo);
                    encontreSolucion = true;
                }

                else
                {
                    //esta no es una solucion, tiene que seguir contruyendo   el arbo en busca de opciones


                    //como estoy en un nuevo nodo estado del mundo ), los acciones que puede hacer ya noson las mismas entonces le paso un nuevo subconjunto de acciones.

                    List<GoapAction> subConjunto = subconjuntoAcciones(acciones, accion);
                    bool nuevaSolucion =
                        ConstruyeGrafo(nodo, arbol, subConjunto, goal);


                    if (nuevaSolucion)
                        encontreSolucion = true;
                    


                }
            }


        }
        return encontreSolucion;
    }
    private List<GoapAction> subconjuntoAcciones (
        List<GoapAction> acciones, GoapAction accionActual)
    {
        List<GoapAction> subconjunto = new List<GoapAction>();
        foreach (GoapAction a in acciones)
        {
            if (!a.Equals(accionActual))
                subconjunto.Add(a);
        }
        return subconjunto;
    }

    //actualizar el estado actual con los efectos que tenga funa accion
    private Dictionary<string, object> ActualizaEstado(
        Dictionary<string, object> estadoActual,
        Dictionary<string,object>efectoDeAccion
        )
    {
        //primero copio la informacion del estado actual
        Dictionary<string, object> estadoTem = new Dictionary<string, object>();
        foreach (KeyValuePair<string, object> valor in estadoActual)
        {
            estadoTem.Add(valor.Key, valor.Value);

            //ahora voy a agregar los efectos al estado si es que no existen
            foreach (KeyValuePair<string, object> efecto in efectoDeAccion)
            {
                bool yaExiste = false;
                    //puede que el dato ya exista pero que su valor haya cambiado
                foreach (KeyValuePair<string, object> dato in estadoTem)
                {
                    if (dato.Equals(efecto))
                    {
                        yaExiste = true;
                        break;
                    }
                }
                if (yaExiste)
                {
                    //puedo actualizar el dato, primero quitandolo del diccionario
                    estadoTem.Remove(efecto.Key);
                    //lo vuelo a poner en el estado pero con su valor nuevo
                    estadoTem.Add(efecto.Key, efecto.Value);

                }
                else
                {
                    //si no existe solo lo agrego
                    estadoTem.Add(efecto.Key, efecto.Value);
                }

            }

            return estadoTem;
        }
        return estadoTem;
    }

    //verificar que las precondiciones estan en el estado del mundo en el nodo actual basta que una precondicion no este en el estado del mundo
    //para que no se cumpla la condicion
    private bool CheckState(Dictionary<string,object> precondiciones,Dictionary<string,object> estado)
    {
//        bool sonIguales = true;

        //para cada precondicion tengo que checar que este en el estado del mundo

        foreach (KeyValuePair<string, object> precondicion in precondiciones)
        {
            // preguntar si esta precondicion esta en el estado del mundo
            if (!estado.ContainsKey(precondicion.Key))
            {
                //regreso falso porque no se cumplio la recondicion
                return false; 
            }
        }

        return true;
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
