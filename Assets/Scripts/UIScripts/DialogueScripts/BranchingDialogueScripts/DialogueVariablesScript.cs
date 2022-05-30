using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

/// <summary>
/// Contains variables using values collected from JSON dialogue file.
/// Created by: Kane Adams
/// </summary>
public class DialogueVariablesScript
{
	public Dictionary<string, Ink.Runtime.Object> Variables { get; private set; }

	/// <summary>
	/// Collects all the variables from the JSON files
	/// </summary>
	/// <param name="a_loadGlobalsJSON">Where all the variables can be referenced from</param>
	public DialogueVariablesScript(TextAsset a_loadGlobalsJSON)
	{
		//string inkFileContents = File.ReadAllText(a_globalsFilePath);
		//Ink.Compiler compiler = new Ink.Compiler(inkFileContents);
		//Story globalVariablesStory = compiler.Compile();
		Story globalVariablesStory = new Story(a_loadGlobalsJSON.text);

		Variables = new Dictionary<string, Ink.Runtime.Object>();
		foreach (string name in globalVariablesStory.variablesState)
		{
			Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
			Variables.Add(name, value);
			//Debug.Log("Initialised global dialogue variable: " + name + "=" + value);
		}
	}

	/// <summary>
	/// Starts collecting variables looking for value changes
	/// </summary>
	/// <param name="a_story">Current conversation</param>
	public void StartListening(Story a_story)
	{
		VariablesToStory(a_story);
		a_story.variablesState.variableChangedEvent += VariableChanged;
	}

	/// <summary>
	/// Stops altering variables when story complete
	/// </summary>
	/// <param name="a_story">Current conversation</param>
	public void StopListening(Story a_story)
	{
		a_story.variablesState.variableChangedEvent -= VariableChanged;
	}

	/// <summary>
	/// Changes the value of a variable from JSON file
	/// </summary>
	/// <param name="a_name">Variable name</param>
	/// <param name="a_value">New value for variable</param>
	private void VariableChanged(string a_name, Ink.Runtime.Object a_value)
	{
		if (Variables.ContainsKey(a_name))
		{
			Variables.Remove(a_name);
			Variables.Add(a_name, a_value);
		}
	}

	/// <summary>
	/// Collects variables from JSON file
	/// </summary>
	/// <param name="a_story">Current conversation</param>
	private void VariablesToStory(Story a_story)
	{
		foreach (KeyValuePair<string, Ink.Runtime.Object> variable in Variables)
		{
			a_story.variablesState.SetGlobal(variable.Key, variable.Value);
		}
	}
}
