using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accion_TomarMineral : GoapAction {

    public Accion_TomarMineral()
    {
        //donde vamos a poner precondiciones y efectos
        AddPrecondition("hayMineral", false);


        AddEffect("hayMineral", true);

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

        GameObject[] Forjas = GameObject.FindGameObjectsWithTag("Almacen");
        GameObject Forjacercan = null;
        float distanciaMenor = 0;
        foreach (GameObject Forja in Forjas)
        {
            if (Forja == null)
            {
                Forjacercan = Forja;
                distanciaMenor = Vector3.Distance(transform.position, Forja.transform.position);
            }
            else
            {
                float dist = Vector3.Distance(Forja.transform.position, transform.position);
                if (dist < distanciaMenor)
                {
                    // encontramos uno mas cercano
                    Forjacercan = Forja;
                    distanciaMenor = dist;
                }
            }
        }
        if (Forjacercan == null)
        
            return false;
        
        Target = Forjacercan;
        return true;
    }

    public override bool Perform(GameObject gameobject)
    {
        // el agente se tardara un poco en realizar esta opcion
        if (tiempoInicio == 0)
         tiempoInicio = Time.timeSinceLevelLoad;
            
        if (Time.timeSinceLevelLoad > tiempoInicio + duracionAccion)
            {

         //verificar si hay mineral en el almacen
            if(Target.GetComponent<Inventario>().mineral>3)// 3 es el mineral que necesito para tabalar
            {
                Target.GetComponent<Inventario>().mineral -= 3;
                GetComponent<Inventario>().mineral += 3;
                terminado = true;
                return true;
            }
            else
            {
                return false;
            }
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
