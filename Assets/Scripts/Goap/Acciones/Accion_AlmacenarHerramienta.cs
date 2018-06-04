using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accion_AlmacenarHerramienta : GoapAction {

    public Accion_AlmacenarHerramienta()
    {
        //donde vamos a poner precondiciones y efectos
        AddPrecondition("hayHerramienta", true);
        
        AddEffect("almacenarHerramienta", true);

        AddEffect("hayHerramienta", false);

    }

    //variables de la accion
    private bool terminado = false;
    private float tiempoInicio = 0;
    public float duracionAccion = 1;

    public override bool isDone()
    {
        return terminado;

    }

    public override bool requiresInRange()
    {
       // si, necesita estar cerca de un almacen para depositar la herramienta
        return true;

    }

    public override void reset()
    {
        terminado = false;
        Target = null;
        tiempoInicio = 0;
    }

    public override bool checkPreconditions(GameObject go)
    {
        // esto se refiere a precondiciones procedurales es decir que conllevan otras operaciones

        //tengo que estar cerca d eun almacen

        GameObject[] almacenes = GameObject.FindGameObjectsWithTag("Almacen");
        GameObject almacenCercano = null;
        float distanciaMenor = 0;
        foreach (GameObject almacen in almacenes)
        {
            if (almacen == null)
            {
                almacenCercano = almacen;
                distanciaMenor = Vector3.Distance(transform.position, almacen.transform.position);
            }
            else
            {
                float dist = Vector3.Distance(almacen.transform.position, transform.position);
                if (dist < distanciaMenor)
                {
                    // encontramos uno mas cercano
                    almacenCercano = almacen;
                    distanciaMenor = dist;
                }
            }
        }
        if (almacenCercano == null)
        {
            return false;
        }
        Target = almacenCercano;
        return true;
    }

    public override bool Perform(GameObject gameobject)
    {
        // el agente se tardara un poco en realizar esta opcion
        if (tiempoInicio == 0)
         tiempoInicio = Time.timeSinceLevelLoad;
            
        if (Time.timeSinceLevelLoad > tiempoInicio + duracionAccion)
            {
                //dejo la herramient en el deposito
                Target.GetComponent<Inventario>().numeroHerramientas++;

            //quito la herramienta que traia
                GetComponent<Inventario>().herramienta.SetActive(false);
                GetComponent<Inventario>().herramienta = null;
                terminado = true;
            }
            return true;
        }
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
