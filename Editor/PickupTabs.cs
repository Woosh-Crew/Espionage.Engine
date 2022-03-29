using System.Collections;
using System.Collections.Generic;
 using UnityEngine;
 using UnityEditor;
 using System.Reflection;
 using System;
 using System.Linq;



namespace Espionage.Engine.Editor
{

	[CustomEditor(typeof(Pickup_SO),true)]
    public class PickupTabs : UnityEditor.Editor
    {
		/// <summary>Our Target</summary>
		private Pickup_SO _myTarget;

		/// <summary>Serialized Object</summary>
		private SerializedObject _soTarget;

		/// <summary>The Fields for our given object</summary>
		FieldInfo[] Fields;

		/// <summary>The Current Tab</summary>
		private int _currentTab;

		/// <summary>List of our tabs</summary>
		private List<Tab> _tabs;
		
		/// <summary>List of tab names, used for tool bar</summary>
		private List<string> _tabNames;

		private void OnEnable(){
			//Get our target and cast it to a pickup
			_myTarget = (Pickup_SO)target;
			//Get te type
			Type t = target.GetType();
			//Extract the fields
			Fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public);
			
			//Set the serializedo bject
			_soTarget = new SerializedObject(_myTarget);

			//Create the list of tabs and tab names
			_tabNames = new();
			_tabs = new();

			//Create the default tab
			_tabNames.Add("Default");
			_tabs.Add(new Tab("Default"));
			
			//Initialze the tabs
			InitializeTabs(Fields);
		}

		/// <summary>Initializes the tabs and gives them their relevant attributes</summary>
		public void InitializeTabs(FieldInfo[] f){
			//Initialize a list of strings
			List<string> l = new List<string>();

			//Go over each field
			for(int i = 0; i < f.Length; i++){
				//Get the tab attribute
				TabAttribute tab = Attribute.GetCustomAttribute(f[i], typeof(TabAttribute)) as TabAttribute;
				
				//If it has the attribute
				if(tab != null){
					//If we don't have this tab yet
					if(!_tabNames.Contains(tab.Name)){
						//Create a new tab
						Tab newTab = new Tab();
						//Set its name
						newTab.Name = tab.Name;
						//Add the field to the tabs fields
						newTab.Fields.Add(_soTarget.FindProperty(f[i].Name));

						//Add this tab to our tabs
						_tabNames.Add(tab.Name);
						_tabs.Add(newTab);
					}else{

						//Otherwise find this tab if it exists
						Tab existingTab = _tabs.Where(x => x.Name == tab.Name).FirstOrDefault();
						//And if it does, add this field to its fields
						if(existingTab != null){
							existingTab.Fields.Add(_soTarget.FindProperty(f[i].Name));
						}
					}
				}else{
					//If a field doesn't have a tab, chuck it in the default tabs fields
					Tab existingTab = _tabs.Where(x => x.Name == "Default").FirstOrDefault();
					if(existingTab != null){
							existingTab.Fields.Add(_soTarget.FindProperty(f[i].Name));
					}
				}
			}

		}

        public override void OnInspectorGUI(){

			//Check for changes
			EditorGUI.BeginChangeCheck();

			//Draw the tool bar
			_currentTab = GUILayout.Toolbar(_currentTab, _tabNames.ToArray());
			EditorGUILayout.Space(10f);

			if(EditorGUI.EndChangeCheck()){
				_soTarget.ApplyModifiedProperties();
				GUI.FocusControl(null);
			}


			EditorGUI.BeginChangeCheck();

			//If we have tabs
			if(_tabs.Count > 0){
				//Draw each property field
				Tab current = _tabs[_currentTab];
				foreach(SerializedProperty field in current.Fields){

					EditorGUILayout.PropertyField(field);
				}
			}

			if(EditorGUI.EndChangeCheck()){
				_soTarget.ApplyModifiedProperties();
			}

		}
    }

	/// <summary>A class representing tabs in an inspector</summary>
	public class Tab{
		public string Name;
		public List<SerializedProperty> Fields = new List<SerializedProperty>();

		public Tab(){
			Name = "";
		}

		public Tab(string s){
			Name = s;
		}
	}

	
}
