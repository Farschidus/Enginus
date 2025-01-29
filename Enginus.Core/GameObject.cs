using System.Collections.Generic;
using System;

namespace Enginus.Core;

public class GameObject
{
    /// <summary>
    /// Stores a dictionary where the key is the Type of the Component and the value is the Component instance.
    /// </summary>
    private readonly Dictionary<Type, IComponent> _components;

    /// <summary>
    /// Tells if the GameObject is active.
    /// </summary>
    public bool IsActive;

    /// <summary>
    /// Initializes a new instance of the GameObject class.
    /// </summary>
    /// <param name="isActive">Determines if the GameObject is active or not. Default is true.</param>
    public GameObject(bool isActive = true)
    {
        _components = [];
        IsActive = isActive;
    }

    /// <summary>
    /// Adds a component to the GameObject.
    /// </summary>
    /// <param name="component">The component to add.</param>
    public void AddComponent(IComponent component)
    {
        Type type = component.GetType();
        if (!_components.ContainsKey(type))
        {
            _components.Add(type, component);
        }
        else if (Constants.EntityDebugMessages)
        {
            Console.WriteLine($"Component of type {type} already exists!");
        }
    }

    /// <summary>
    /// Removes a component from the GameObject.
    /// </summary>
    /// <typeparam name="T">The type of the component to remove.</typeparam>
    public void RemoveComponent<T>() where T : IComponent
    {
        Type type = typeof(T);
        if (_components.ContainsKey(type))
        {
            _components.Remove(type);
        }
        else if (Constants.EntityDebugMessages)
        {
            Console.WriteLine("Tried to remove a component that doesn't exist!");
        }
    }

    /// <summary>
    /// Gets a component of a specified type from the GameObject.
    /// </summary>
    /// <typeparam name="T">The type of the component to get.</typeparam>
    /// <returns>The component of the specified type, or null if it doesn't exist.</returns>
    public T GetComponent<T>() where T : IComponent
    {
        Type type = typeof(T);
        if (_components.TryGetValue(type, out IComponent component) && component is T tComponent)
        {
            return tComponent;
        }
        else if (Constants.EntityDebugMessages)
        {
            Console.WriteLine("Tried to get a component that doesn't exist!");
        }
        return default;
    }

    /// <summary>
    /// Gets all the components of the GameObject.
    /// </summary>
    /// <returns>A list of all the components of the GameObject.</returns>
    public List<IComponent> GetAllComponents()
    {
        List<IComponent> componentList = new(_components.Values);
        return componentList;
    }
}