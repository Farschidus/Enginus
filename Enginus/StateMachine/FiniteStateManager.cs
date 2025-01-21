using System;
using System.Collections.Generic;

namespace Enginus.StateMachine
{
	/// <summary>
	///  FSMSystem class represents the Finite State Machine class.
	///  It has a List with the States the NPC has and methods to add,
	///  delete a state, and to change the current state the Machine is on.
	/// </summary>
	public class FiniteStateManager
    {
        private readonly List<FiniteState> states;
		public StateID CurrentStateID { get; private set; }
		public FiniteState CurrentState { get; private set; }

		public FiniteStateManager()
        {
            states = new List<FiniteState>();
        }
        /// <summary>
        /// This method places new states inside the FSM,
        /// or prints an ERROR message if the state was already inside the List.
        /// First state added is also the initial state.
        /// </summary>
        public void AddState(FiniteState state)
        {
            // Check for Null reference before deleting
            if (state == null)
            {
                throw new Exception("FSM ERROR: Null reference is not allowed");
            }

            // First State inserted is also the Initial state, the state the machine is in when the simulation begins
            if (states.Count == 0)
            {
                states.Add(state);
                CurrentState = state;
                CurrentStateID = state.ID;
                return;
            }

            // Add the state to the List if it's not inside it
            foreach (FiniteState finiteState in states)
            {
                if (finiteState.ID == state.ID)
                {
                    throw new Exception("FSM ERROR: Impossible to add state " + state.ID.ToString() + " because state has already been added");
                }
            }
            states.Add(state);
        }
        /// <summary>
        /// This method delete a state from the FSM List if it exists, 
        ///   or prints an ERROR message if the state was not on the List.
        /// </summary>
        public void DeleteState(StateID id)
        {
            // Check for NullState before deleting
            if (id == StateID.NullStateID)
            {
                throw new Exception("FSM ERROR: NullStateID is not allowed for a real state");
            }

            // Search the List and delete the state if it's inside it
            foreach (FiniteState state in states)
            {
                if (state.ID == id)
                {
                    states.Remove(state);
                    return;
                }
            }
            throw new Exception("FSM ERROR: Impossible to delete state " + id.ToString() + ". It was not on the list of states");
        }
        /// <summary>
        /// This method tries to change the state the FSM is in based on
        /// the current state and the transition passed. If current state
        ///  doesn't have a target state for the transition passed, 
        /// an ERROR message is printed.
        /// </summary>
        public void PerformTransition(Transition trans)
        {
            // Check for NullTransition before changing the current state
            if (trans == Transition.NullTransition)
            {
                throw new Exception("FSM ERROR: NullTransition is not allowed for a real transition");
            }

            // Check if the currentState has the transition passed as argument
            StateID id = CurrentState.GetOutputState(trans);
            if (id == StateID.NullStateID)
            {
                throw new Exception("FSM ERROR: State " + CurrentStateID.ToString() + " does not have a target state " + id + " for transition " + trans.ToString());
            }

            // Update the currentStateID and currentState
            CurrentStateID = id;
            foreach (FiniteState state in states)
            {
                if (state.ID == CurrentStateID)
                {
                    // Do the post processing of the state before setting the new one
                    CurrentState.DoBeforeLeaving();

                    CurrentState = state;

                    // Reset the state to its desired condition before it can reason or act
                    CurrentState.DoBeforeEntering();
                    break;
                }
            }
        }
    }
}
