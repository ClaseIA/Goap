using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM  {

    private Stack<FSMState> pilaEstados = new Stack<FSMState>();
    public void pushState(FSMState estado)
    {
        pilaEstados.Push(estado);
    }


        public void popState(){
            pilaEstados.Pop();
        }

    public void UpdateFSM(GameObject gameObject){
        if(pilaEstados.Peek()!= null){
            //ejecuto el estado actual
            pilaEstados.Peek().Invoke(this, gameObject);
        }
    }

    public delegate void FSMState(FSM fsm, GameObject gameobject);
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
