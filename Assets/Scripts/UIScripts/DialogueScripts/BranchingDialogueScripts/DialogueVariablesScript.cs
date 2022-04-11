using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;

public class DialogueVariablesScript
{
	public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }

	/// <summary>
	///	Compiles the story
	/// </summary>
	/// <param name="a_globalsFilePath"></param>
	public DialogueVariablesScript(TextAsset a_loadGlobalsJSON)
	{
		//string inkFileContents = File.ReadAllText(a_globalsFilePath);
		//Ink.Compiler compiler = new Ink.Compiler(inkFileContents);
		//Story globalVariablesStory = compiler.Compile();
		Story globalVariablesStory = new Story(a_loadGlobalsJSON.text);

		variables = new Dictionary<string, Ink.Runtime.Object>();
		foreach (string name in globalVariablesStory.variablesState)
		{
			Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
			variables.Add(name, value);
			Debug.Log("Initialised global dialogue variable: " + name + "=" + value);
		}
	}

	public void StartListening(Story a_story)
	{
		VariablesToStory(a_story);
		a_story.variablesState.variableChangedEvent += VariableChanged;
	}

	public void StopListening(Story a_story)
	{
		a_story.variablesState.variableChangedEvent -= VariableChanged;
	}

	private void VariableChanged(string a_name, Ink.Runtime.Object a_value)
	{
		if (variables.ContainsKey(a_name))
		{
			variables.Remove(a_name);
			variables.Add(a_name, a_value);
		}
	}

	private void VariablesToStory(Story a_story)
	{
		foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variables)
		{
			a_story.variablesState.SetGlobal(variable.Key, variable.Value);
		}
	}
}
