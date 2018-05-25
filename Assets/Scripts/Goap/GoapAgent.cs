using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapAgent : MonoBehaviour {

    private List<GoapAction> AccionesDisponibles;
    private Queue<GoapAction> AccionesActuales;


    //para conocer el estado del muno y actualizarlo
    private IGOAP goapData;

    // necesita su planeador de acciones
    private GoapPlanner Planeador;

    private FSM maquinaDeEstados;
    private FSM.FSMState idleState;
    private FSM.FSMState ActState;
    private FSM.FSMState MoveState;







	// Use this for initialization
	void Start () {
        maquinaDeEstados = new FSM();
        AccionesActuales = new Queue<GoapAction>();
        AccionesDisponibles = new List<GoapAction>();

        //provedor de datos del mundo
        goapData = GetComponent<IGOAP>();
        // Crear nuestros estados
	}

    public void CrearEstadoIdle()
    {
        //Este estado lo usa el agente para calcular un plan
        idleState = (fsm, gameObj) =>
            {
                //planeacion goap
                //obtener el estado del mundo
                Dictionary<string, object> worldState = goapData.GetWorldState();
                //obtener la meta dle agente
                Dictionary<string, object> goal = goapData.CreateGoalState();

                //crear un plan
                Queue<GoapAction> plan;

                //logramos tener un plan?
                if (plan != null)
                {
                    Debug.Log("tengo un plan");
                    AccionesActuales = plan;
                    goapData.PlanFound(goal, plan);

                    maquinaDeEstados.popState();//saca el estado idle
                    maquinaDeEstados.pushState(ActState);//pasa al estado de actuar


                }
                else
                {
                    Debug.Log("no tengo plan");
                    goapData.PlanFailed(goal);
                    maquinaDeEstados.popState();
                    maquinaDeEstados.pushState(idleState);// para que vuelva a intentar calcular el plan

                }

            };
    }



	
	// Update is called once per frame
	void Update () {
		
	}
}
