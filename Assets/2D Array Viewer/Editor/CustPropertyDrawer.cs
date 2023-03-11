using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(ArrayLayout))]
public class CustPropertyDrawer : PropertyDrawer {


	public override void OnGUI(Rect position,SerializedProperty property,GUIContent label){
		EditorGUI.PrefixLabel(position,label);
		Rect newposition = position;
		newposition.y += 15;
		SerializedProperty data = property.FindPropertyRelative("rows");
		//data.rows[0][]
		for(int j=0;j<16;j++){
			SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
			newposition.height = 15;
			if(row.arraySize != 16)
				row.arraySize = 16;
			newposition.width = position.width/16;
			for(int i=0;i<16;i++){
				EditorGUI.PropertyField(newposition,row.GetArrayElementAtIndex(i),GUIContent.none);
				newposition.x += newposition.width;
			}

			newposition.x = position.x;
			newposition.y += 15;
		}
	}

	public override float GetPropertyHeight(SerializedProperty property,GUIContent label){
		return 15 * 17;
	}
}
