using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GoapAction : MonoBehaviour {

    //una accion puede tener un costo para realizarse
    //dependiendo del costo sera la secuencia que tome el planeador
    public float cost;
    
    //precondiciones 
    private Dictionary<string, object> Precondiciones;
    
    //efectos
    private Dictionary<string, object> Efectos;
	
    //una accion por lo general se ejecuta sobre un objeto
    public GameObject Target;

    public GoapAction()
    {
        Precondiciones = new Dictionary<string, object>();
        Efectos = new Dictionary<string, object>();
    }

    //limpiar las variables de la accion
    private void Reset()
    {
        Target = null;
        reset();
    }
    //para que cada accjion resetee sus variables
    public abstract void reset();

    //cada accion debe decir cuano ya termino
    public abstract void isDone();

    //cada accion debe checar si sus precondiciones se cumplen
    public abstract bool checkPreconditions(GameObject go);
    //cada accion debe ejecutar sus tareas
    public abstract bool Perform(GameObject gameobject);

    public bool inRange;
    // si la accion necesita estar cerca de un objetivo
    public abstract bool requiresInRange();
    public bool IsInRange()
    {
        return inRange;
    }

    //agregar precondiciones
    public void AddPrecondition(string key, object value)
    {
        Precondiciones.Add(key, value);
    }

    //agregar efectos
    public void AddEffect(string key, object value)
    {
        Efectos.Add(key, value);
    }

    //revisar las precondiciones de la accion
    public Dictionary<string, object> GetPrecondiciones
    {
        get { return Precondiciones; }
    }
    //revisar los efectos de la accion
    public Dictionary<string, object> GetEfectos
    {
        get { return Efectos; }
    }

   
}
