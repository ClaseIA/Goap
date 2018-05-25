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
    }

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
