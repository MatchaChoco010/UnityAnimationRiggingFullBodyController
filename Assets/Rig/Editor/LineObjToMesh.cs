using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LineObjToMesh : EditorWindow {
    [MenuItem ("Animation Rigging/Utils/Line Obj To Mesh")]
    static void Open () {
        GetWindow<LineObjToMesh> ("Line Obj To Mesh");
    }

    Object obj;

    void OnGUI () {
        obj = EditorGUILayout.ObjectField (obj, typeof (Object), false);
        if (GUILayout.Button ("Create Line Mesh")) {
            CreateLineMesh (obj);
        }
    }

    void CreateLineMesh (Object obj) {
        var path = AssetDatabase.GetAssetPath (obj);

        var vertices = new List<Vector3> ();
        var indices = new List<int> ();

        using (var reader = new StreamReader (path)) {
            string line;
            while ((line = reader.ReadLine ()) != null) {
                if (line.Length <= 0) continue;
                var l = line.Split (' ');
                switch (l[0]) {
                    case "v":
                        vertices.Add (new Vector3 (
                            float.Parse (l[1]),
                            float.Parse (l[2]),
                            float.Parse (l[3])
                        ));
                        break;
                    case "l":
                        indices.Add (int.Parse (l[1]) - 1);
                        indices.Add (int.Parse (l[2]) - 1);
                        break;
                    default:
                        break;
                }

            }
        }

        var mesh = new Mesh ();
        mesh.SetVertices (vertices);
        mesh.SetIndices (indices, MeshTopology.Lines, 0);

        var assetPath = Path.ChangeExtension (path, "asset");
        AssetDatabase.CreateAsset (mesh, assetPath);
    }
}
