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

        CrearEstadoActuar();
        CrearEstadoIdle();
        CrearEstadoMoverse();

        //empezamos pensando
        maquinaDeEstados.pushState(idleState);


        //cargar las accions que puede hacer el agente

        GoapAction[] acciones = GetComponents<GoapAction>();
        foreach(GoapAction a in acciones)
            AccionesDisponibles.Add(a);


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
                Queue<GoapAction> plan= Planeador.ElPlan(
                    gameObject,AccionesDisponibles,worldState,goal);

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


    public void CrearEstadoActuar()


    {
        ActState = (fsm, gameObj) =>
            {
                //ejecutar la accion
                if (AccionesActuales.Count <= 0)// no tengo un plan
                {
                    fsm.popState();
                    fsm.pushState(idleState);
                    goapData.ActionsFinished();
                    return;
                }
                //si si tengo acciones entronces objetco la primeroa

                GoapAction accion = AccionesActuales.Peek();
                if (accion.isDone())
                {

                    //si ya se termino la accion la lquito
                    AccionesActuales.Dequeue();


                }
                //si no ha terminad oy quedan acciones hay que ejecutrarl
                if (AccionesActuales.Count > 0)
                {
                    accion = AccionesActuales.Peek();
                    //verifico si requiere estar en un rango}(cerca de su objetivo)
                    bool enRango = accion.requiresInRange() ? accion.IsInRange() : true;
                    if (enRango)
                    {
                        bool exito = accion.Perform(gameObj);
                        // sil a accion no se ejecuto
                        if (!exito)
                        {
                            //planeo otra vez
                            fsm.popState();
                            //sjalgo de acturar y vuelvo a idle
                            fsm.pushState(idleState);

                            goapData.PlanAborted(accion);
                        }
                    }

                    else
                    {
                        //no esta en donde deberia estar, no esta en rango
                        Debug.Log("estoy lejos del objetivo");
                        fsm.pushState(MoveState);
                    }
                }
                else
                {
                    //no quedan acciones, entonces puedo volver a planear
                    fsm.popState();
                    fsm.pushState(idleState);

                    goapData.ActionsFinished();
                }
            };
        
    }


    public void CrearEstadoMoverse()
    {
        MoveState=(fsm,gameObj)=>{
            GoapAction accion= AccionesActuales.Peek();
            // mover el agenbte hacia el objetivo de la accion si es que tiene
            if(accion.requiresInRange()&& accion.Target==null){
                Debug.Log("la accion drequere un targetm pero no tiene");
                fsm.popState();// salri de actuar
                fsm.popState();//salir de movers
                fsm.pushState(idleState);
                return;
            }

            //que se mueva
            if(goapData.MoveAgent(accion)){
                fsm.popState();
            }

            ////////////////////////////////////////////////////////////script de movimiento o de lo que quieras/////////////////////////////////////////
            gameObj.transform.position=Vector3.MoveTowards(
                gameObj.transform.position,
                accion.Target.transform.position,
                Time.deltaTime*5
                );

            if(Vector3.Distance(gameObj.transform.position,accion.Target.transform.position)<1f){
                //llega al objetivo
                accion.SetInRange(true);
                fsm.popState();//salirde moverse
            }
        };
    }

	
	// Update is called once per frame
	void Update () {

        maquinaDeEstados.UpdateFSM(gameObject);
		
	}
}
