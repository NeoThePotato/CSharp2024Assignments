using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class StateMachine : IState
{
	private StateWrapper _currentState;
	private HashSet<StateWrapper> _states;

	private IState CurrentState 
	{ set 
		{ 
			_currentState.state?.OnExit();
			_currentState = _states.Single(s => s.state == value);
			_currentState.state.OnEnter();
			UnityEngine.Debug.Log(value); 
		} 
	}

	public StateMachine(int capacity = 10)
	{
		_states = new HashSet<StateWrapper>(capacity);
	}

	public void Update()
	{
		if (_currentState.TryTransition(out IState state))
		{
			CurrentState = state;
			return;
		}
		_currentState.state.Update();
	}

	public void AddState(IState state)
	{
		if (state == this)
			throw new InvalidOperationException("Can't feed this state machine to itself.");
		if (_states.Any(s => s.state == state))
			return;
		var newState = new StateWrapper(state);
		_states.Add(newState);
		if (_currentState.Equals(null))
			_currentState = newState;
	}

	public void AddTransition(IState from, IState to, Func<bool> predicate)
	{
		var stateWrapper = _states.Where(s => s.state == from).Single();
		stateWrapper.transitions.Add(new Transition(to, predicate));
	}

	public void AddTwoWayTransition(IState from, IState to, Func<bool> predicate)
	{
		AddTransition(from, to, predicate);
		AddTransition(to, from, () => !predicate());
	}

	public void TriggerTransition(IState from, IState to)
	{
		if (_currentState.Equals(from))
			CurrentState = to;
	}

    public void OnEnter()
    {
		_currentState.state.OnEnter();
    }

    public void OnExit()
    {
        _currentState.state.OnExit();
    }

    private struct Transition : IEquatable<Transition>
	{
		public static Transition Empty = new();

		public IState to;
		public Func<bool> predicate;

		public Transition(IState to, Func<bool> predicate)
		{
			this.to = to;
			this.predicate = predicate;
		}

		public readonly bool Equals(Transition other) => to == other.to;
	}

	private struct StateWrapper : IEquatable<StateWrapper>, IEquatable<IState>, IEnumerable<Transition>
	{
		public IState state;
		public List<Transition> transitions;

		public StateWrapper(IState state)
		{
			this.state = state;
			transitions = new();
		}

		public readonly bool TryTransition(out IState state)
		{
			Transition transition = transitions.FirstOrDefault(t => t.predicate());
			if (transition.Equals(Transition.Empty))
			{
				state = null;
				return false;
			}
			state = transition.to;
			return true;
		}

		public readonly IEnumerator<Transition> GetEnumerator() => ((IEnumerable<Transition>)transitions).GetEnumerator();

		readonly IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)transitions).GetEnumerator();

		public readonly bool Equals(StateWrapper other) => Equals(other.state);

		public readonly bool Equals(IState other) => state == other;
	}
}

public interface IState
{
	public abstract void Update();

	public abstract void OnEnter();

	public abstract void OnExit();
}