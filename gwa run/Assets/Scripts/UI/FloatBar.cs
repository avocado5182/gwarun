using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using UnityEditor;
using UnityEngine.UI;

public class FloatBar : MonoBehaviour {
	public Image bar;
	public Gradient colorGradient;
	
	#region actual inspector stuff
	public bool runAtAwake = true;
	public bool runAtStart = true;
	public bool runAtRuntime = true;
	public float activeValue;
	public GameObject obj;
	[HideInInspector] public int componentSelected;
	[HideInInspector] public int propertyOrFieldSelected;
	[HideInInspector] public string[] components;
	[HideInInspector] public string[] validPropertiesNFields;
	Component[] componentArray;
	int propertyCount;
	List<PropertyInfo> validProperties;
	List<FieldInfo> validFields;
	PropertyInfo[] propertyInfoArray;
	#endregion

	void Awake() {
		if (runAtAwake) {
			GetPropsNFields();
		}
	}

	void Start() {
		if (runAtStart) {
			GetPropsNFields();
		}

		bar = GetComponent<Image>();
	}

	/// TODO: This could run more efficiently in a coroutine
	void Update() {
		if (runAtRuntime) {
			if (obj != null) {
				GetPropsNFields();
				if (GameManager.Instance.currPowerup != null) {
					// set bar based on active value
					float amt = activeValue / GameManager.Instance.currPowerup.duration;
					bar.fillAmount = amt;
					bar.color = colorGradient.Evaluate(amt);
				}
			}
		}
		else {
			Destroy(this);
		}
	}

	#region actual inspector stuff
	[ExecuteInEditMode]
	public void GetPropsNFields() {
		if (obj != null) { // If the developer has dragged in an object,
			// Get the list of components on the object
			componentArray = obj.GetComponents(typeof(Component));
			// Get the type of each component
			string[] componentTypeStrings = new string[componentArray.Length];
			for (int i = 0; i < componentArray.Length; i++) {
				componentTypeStrings[i] = componentArray[i].GetType().ToString();
			}

			// Display the components to the developer
			components = componentTypeStrings;
			// Get all public, non-inherited properties and fields from the component that the developer has selected
			PropertyInfo[] propertyInfoArray = componentArray[componentSelected].GetType()
				.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
			FieldInfo[] fieldInfoArray = componentArray[componentSelected].GetType()
				.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
			// Check each property and field on the component to see if it is "valid". 
			List<string> validPropertiesNFieldsNameList = new List<string>();
			validProperties = new List<PropertyInfo>();
			for (int i = 0; i < propertyInfoArray.Length; i++) { // Check each property
				if (propertyInfoArray[i].PropertyType.Equals(activeValue.GetType())) {
					// If the current property matches the type of activeValue, 
					// Store the property in the list of valid properties and store its name in a grand list of valid properties AND fields
					validProperties.Add(propertyInfoArray[i]);
					validPropertiesNFieldsNameList.Add(propertyInfoArray[i].Name + ": <" +
					                                   propertyInfoArray[i].PropertyType + ">");
				}
			} // finally, store the number of valid properties so that we can differentiate between properties and fields later on

			propertyCount = validPropertiesNFieldsNameList.Count;
			validFields = new List<FieldInfo>();
			for (int i = 0; i < fieldInfoArray.Length; i++) { // Check each field
				if (fieldInfoArray[i].FieldType.Equals(activeValue.GetType())) {
					// If the current field matches the type of activeValue, 
					// Store the field in the list of valid fields and store its name in a grand list of valid properties AND fields
					validPropertiesNFieldsNameList.Add(fieldInfoArray[i].Name + ": <" +
					                                   fieldInfoArray[i - propertyInfoArray.Length].FieldType + ">");
					validFields.Add(fieldInfoArray[i]);
				}
			}

			// Expose the valid properties and fields to be displayed to the developer
			validPropertiesNFields = validPropertiesNFieldsNameList.ToArray();
			// Store the property/field value locally
			GetSelectedValue();
		}
	}

	private void GetSelectedValue() {
		if (validPropertiesNFields.Length == 0) { // If we have NO properties OR fields, set float to default value
			activeValue = 0f;
		}
		else { // If there ARE some valid properties or fields
			if (componentSelected > (componentArray.Length - 1)) { // Prevent out of bounds
				componentSelected = 0;
			}

			if (propertyOrFieldSelected < propertyCount) { // If the developer has selected a property,
				if (validProperties.Count > 0) { // If there are any valid properties,
					// Store the value of the selected property
					if (propertyOrFieldSelected > validProperties.Count) { // Prevent out of bounds
						propertyOrFieldSelected = 0;
					}

					activeValue = (float) validProperties[propertyOrFieldSelected]
						.GetValue(componentArray[componentSelected]);
				}
			}
			else { // Otherwise the developer has selected a field, 
				if (validFields.Count > 0) { // So if there are any valid fields,
					// Store the value of the selected field
					int fieldSelected = propertyOrFieldSelected - propertyCount;
					if (fieldSelected > (validFields.Count - 1) || fieldSelected < 0) { // Prevent out of bounds
						fieldSelected = 0;
					}

					activeValue = (float) validFields[fieldSelected].GetValue(componentArray[componentSelected]);
				}
			}
		}
	}
	#endregion
}

#region actual inspector stuff
#if UNITY_EDITOR
[CustomEditor(typeof(FloatBar))]
public class FloatHolderEditor : Editor {
	public override void OnInspectorGUI() {
		// var offsetProperty = serializedObject.FindProperty("Offset");
		//    EditorGUILayout.PropertyField(offsetProperty);
		//  serializedObject.ApplyModifiedProperties();
		// Get the current script
		FloatBar script = (FloatBar) target;
		// Draw the rest standard variables
		DrawDefaultInspector();
		// Collect the valid properties and fields
		script.GetPropsNFields();
		// Draw custom drop-down menus for the selected object's components and the fields/properties of that selected component
		if (script.components.Length > 0) { // If we have any components,
			// Draw the drop-down menu for them
			var componentSelectedProperty = serializedObject.FindProperty("componentSelected");
			//EditorGUILayout.PropertyField(componentSelectedProperty);
			componentSelectedProperty.intValue =
				EditorGUILayout.Popup("Components", componentSelectedProperty.intValue, script.components);
		}

		if (script.validPropertiesNFields.Length > 0) { // if the selected component has any valid properties/fields,
			// Draw the drop-down menu for them
			var propertyOrFieldSelectedProperty = serializedObject.FindProperty("propertyOrFieldSelected");
			propertyOrFieldSelectedProperty.intValue = EditorGUILayout.Popup("Members",
				propertyOrFieldSelectedProperty.intValue, script.validPropertiesNFields);
		}

		serializedObject.ApplyModifiedProperties();
	}
}
#endif
#endregion