using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*cual quier agente que ahiera usar goap tiene que implementar 
     * esta interfaz
     * ayudara al planeador de acciones para decidir que acciones tomar.
     * lo uaremos tambien para ocmunicarnos ocn el agente y 
     * hacerle saber si  yuna accion falla o se cumple*/

	// Use this for initialization

public interface IGOAP {
    //crear una informacion del estado del juego o del mundo

   // HashSet<KeyValuePair<string, object>> getworldState();

    Dictionary<string, object> GetWorldState();
    //promoprcionar al planeador una meta para que pueda construir la secuencia de acciones a seguir, el plan

    Dictionary<string, object> CreateGoalState();
    //
    void PlanFound(Dictionary<string,object>Goal,
                    Queue<GoapAction>Actions);

    // si el plan falla, podriamos tratar de encotnrar uno nuevoj
    void PlanFailed(Dictionary<string, object> FailedGoal);

    void ActionsFinished();

    void PlanAborted(GoapAction abortedAction);

    bool MoveAgent(GoapAction nextAction);
	
	
}
