using UnityEditor;
using UnityEngine;
using com.portfolio.scriptableObjects;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;
using System;

namespace com.portfolio.gridSystem
{
    public class GridEditor : EditorWindow
    {
        private GridDataSO SourceGridDataSO;
        private GridEditorDebugDataSO SourceGridEditorDebugDataSO;

        static Grid debugGrid;
        static GridTileObject lastClickedDebugTile;
        static EditorGridTile lastClickedEditorGridTile;
        static GridTileData debugGridTileData = new();
        static bool resetToggleValue = false;

        [MenuItem("Portfolio/GridSystem/GridEditor")]
        static void OpenWindow()
        {
            GridEditor window = (GridEditor)EditorWindow.GetWindow(typeof(GridEditor));
            window.Show();
        }

        void OnGUI()
        {
            if (Selection.activeGameObject!=null && Selection.activeGameObject.GetComponent<GridTileObject>())
            {
                lastClickedDebugTile = Selection.activeGameObject.GetComponent<GridTileObject>();
                lastClickedEditorGridTile = Selection.activeGameObject.GetComponent<EditorGridTile>();
                debugGridTileData = new();
                debugGridTileData = SourceGridDataSO.EditorGetTileData(tileNumber:lastClickedDebugTile.Data.TileNumber);
                lastClickedEditorGridTile.UpdateDebugData();
            }

            bool debugButtonValue = false;
            bool resetGridSOButtonValue = false;
            bool regenerateDebugGridButtonValue = false;


            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                EditorGUILayout.LabelField("Grid Data SO");
                SourceGridDataSO = (GridDataSO)EditorGUILayout.ObjectField(SourceGridDataSO, typeof(GridDataSO), true);
            });

            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                EditorGUILayout.LabelField("Editor Debug SO");
                SourceGridEditorDebugDataSO = (GridEditorDebugDataSO)EditorGUILayout.ObjectField(SourceGridEditorDebugDataSO, typeof(GridEditorDebugDataSO), true);
            });

            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                debugButtonValue = GUILayout.Button("Debug");
                regenerateDebugGridButtonValue = GUILayout.Button("Regenerate Debug Grid");

            });

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                resetToggleValue = EditorGUILayout.BeginToggleGroup("Reset SO", resetToggleValue);
                resetGridSOButtonValue = GUILayout.Button("⚠ Reset SO");
                EditorGUILayout.EndToggleGroup();
            });

            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            if (SourceGridDataSO != null)
            {
                GridEditorFields();
            }

            if (debugButtonValue && debugGrid == null && SourceGridEditorDebugDataSO != null)
            {
                EditorSceneManager.OpenScene("Assets/src/Scripts/GridSystem/GridEditor/GridEditorScene.unity");
                GenerateDebugGrid();
            }

            if (regenerateDebugGridButtonValue && SourceGridEditorDebugDataSO != null)
            {
                GenerateDebugGrid();
            }


            if (resetGridSOButtonValue && debugGrid != null && SourceGridEditorDebugDataSO != null)
            {
                resetToggleValue = false;
                ResetGridSO();
            }

            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<GridTileObject>())
            {
                lastClickedDebugTile = Selection.activeGameObject.GetComponent<GridTileObject>();
                SourceGridDataSO.EditorSetTileData(lastClickedDebugTile, debugGridTileData);
            }

        }

        private void GridEditorFields()
        {
            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.LabelField("Grid Info", EditorStyles.boldLabel);
                EditorGUI.EndDisabledGroup();
            });

            EditorGUILayout.Separator();


            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.Vector2IntField("Grid Dimension", SourceGridDataSO.GridData.GridDimension);
                EditorGUI.EndDisabledGroup();
            });


            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                EditorGUILayout.LabelField("Last Clicked Tile Info", EditorStyles.boldLabel);
            });

            EditorGUILayout.Separator();

            debugGridTileData = GridTileDataField(debugGridTileData);
        }

        private GridTileData GridTileDataField(in GridTileData tileData )
        {
            GridTileData inTileData = tileData;
            GridTileData outTileData = tileData;

            EditorGUILayout.BeginVertical();

            EditorGUI.BeginDisabledGroup(true);

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                outTileData.TileNumber = EditorGUILayout.IntField("Tile Number", inTileData.TileNumber);
            });

            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                EditorGUILayout.Vector2IntField("Tile Coordinates", new Vector2Int(inTileData.Coordinates.X, inTileData.Coordinates.Y));
            });

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                outTileData.Height = EditorGUILayout.Slider("Height", inTileData.Height, -1f, 2f);
            });

            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                outTileData.SlantDirection = (GridEnums.Navigation)EditorGUILayout.EnumPopup("Slant Direction", inTileData.SlantDirection);
            });

            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                outTileData.SlantAngle = EditorGUILayout.IntSlider("Slant Angle",outTileData.SlantAngle,0,90);
            });

            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                outTileData.Type = (GridEnums.Tile.Type)EditorGUILayout.EnumPopup("Tile Type", inTileData.Type);
            });

            EditorGUILayout.EndVertical();

            return outTileData;
        }

        private void ResetGridSO()
        {
            SourceGridDataSO.GridData = new(SourceGridDataSO.GridData.GridDimension);
            ResetGrid();
        }

        private void GenerateDebugGrid()
        {
            if(debugGrid && debugGrid.gameObject)
            {
                DestroyImmediate(debugGrid.gameObject);
            }

            GameObject gridObjectPrefab = new("GRID_DEBUG_GO");
            gridObjectPrefab.AddComponent(typeof(Grid));
            debugGrid = gridObjectPrefab.GetComponent<Grid>();

            GameObject gridTileObjectPrefab = Instantiate(SourceGridEditorDebugDataSO.DebugTileObject.gameObject, debugGrid.transform);
            gridTileObjectPrefab.AddComponent(typeof(GridTileObject));
            GridTileObject debugTileObject = gridTileObjectPrefab.GetComponent<GridTileObject>();


            debugGrid.GenerateGrid(SourceGridDataSO.GridData, debugTileObject);
            UpdateEditorTilesInfo(debugGrid);
            DestroyImmediate(gridTileObjectPrefab);
        }

        private void ResetGrid()
        {
            GenerateDebugGrid();
            UpdateEditorTilesInfo(debugGrid);
        }

        private void UpdateEditorTilesInfo(Grid grid)
        {
            foreach (GridTileObject gridTileObject in grid.GridTileObjects)
            {
                EditorGridTile EditorTile = gridTileObject.GetComponent<EditorGridTile>();

                if (EditorTile != null)
                {
                    EditorTile.UpdateDebugData();
                }

            }
        }

        private void SetupHorizontalLayout(Action GUIElementCallback)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Separator();

            GUIElementCallback();

            EditorGUILayout.Separator();
            EditorGUILayout.EndHorizontal();
        }
    }
}