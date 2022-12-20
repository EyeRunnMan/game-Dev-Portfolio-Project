using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using com.package.scriptableObjects;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;
using System;

namespace com.package.gridSystem
{
    public class GridEditor : EditorWindow
    {
        private GridDataSO SourceGridDataSO;
        private GridEditorConfigDataSO SourceGridEditorDebugDataSO;

        static GameGrid debugGrid;
        static GridTileObject lastClickedDebugTile;
        static EditorGridTile lastClickedEditorGridTile;
        static GridTileData debugGridTileData = new();
        static bool resetToggleValue = false;

        static bool updateNorthTile = false;
        static bool updateSouthTile = false;
        static bool updateEastTile = false;
        static bool updateWestTile = false;

        static bool resetTile = false;

        [MenuItem("Portfolio/GridSystem/GridEditor")]
        static void OpenWindow()
        {
            GridEditor window = (GridEditor)EditorWindow.GetWindow(typeof(GridEditor));
            window.Show();
        }

        void OnGUI()
        {
            HashSet <GridTileObject> selectedTiles= new();
            if (Selection.activeGameObject!=null && Selection.activeGameObject.GetComponent<GridTileObject>())
            {
                lastClickedDebugTile = Selection.activeGameObject.GetComponent<GridTileObject>();

                foreach (GameObject selectedGameObjects in Selection.gameObjects)
                {
                    if (selectedGameObjects.GetComponent<GridTileObject>())
                    {
                        selectedTiles.Add(selectedGameObjects.GetComponent<GridTileObject>());
                    }
                }

                lastClickedEditorGridTile = Selection.activeGameObject.GetComponent<EditorGridTile>();
                debugGridTileData = new();
                debugGridTileData = SourceGridDataSO.EditorGetTileData(tileNumber:lastClickedDebugTile.Data.TileId);
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
                SourceGridEditorDebugDataSO = (GridEditorConfigDataSO)EditorGUILayout.ObjectField(SourceGridEditorDebugDataSO, typeof(GridEditorConfigDataSO), true);
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
                UtilityMenu();
            }

            if (debugButtonValue && debugGrid == null && SourceGridEditorDebugDataSO != null)
            {
                EditorSceneManager.OpenScene("Assets/packages/Grid System Package/EditorScenes/GridEditorScene.unity");
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
                UpdateSelectedTiles(selectedTiles);
            }

            updateNorthTile = false;
            updateSouthTile = false;
            updateEastTile = false;
            updateWestTile = false;
            resetTile = false;
        }

        private void UpdateSelectedTiles(HashSet<GridTileObject> tileObjects)
        {

            foreach (GridTileObject gridTileObject in tileObjects)
            {
                GridTileData tileObjectData = new(gridTileObject.Data,debugGridTileData);

                if (tileObjects.Count == 1)
                {
                    foreach (GridEnums.Direction direction in Enum.GetValues(typeof(GridEnums.Direction)))
                    {
                        GridTileData adjecentTileData = debugGrid.GetTileInDirection(tileObjectData.TileId, direction);

                        float heightAmount = debugGridTileData.Height - adjecentTileData.Height;

                        if (debugGridTileData.SlantDirection == direction)
                        {
                            heightAmount += debugGridTileData.LeadingEdgeHeight;
                        }

                        switch (direction)
                        {
                            case GridEnums.Direction.North:
                                if (!updateNorthTile) continue;
                                adjecentTileData = new(adjecentTileData, heightAmount, GridEnums.Direction.South);
                                break;
                            case GridEnums.Direction.South:
                                if (!updateSouthTile) continue;
                                adjecentTileData = new(adjecentTileData, heightAmount, GridEnums.Direction.North);
                                break;
                            case GridEnums.Direction.East:
                                if (!updateEastTile) continue;
                                adjecentTileData = new(adjecentTileData, heightAmount, GridEnums.Direction.West);
                                break;
                            case GridEnums.Direction.West:
                                if (!updateWestTile) continue;
                                adjecentTileData = new(adjecentTileData, heightAmount, GridEnums.Direction.East);
                                break;
                            case GridEnums.Direction.Undefined:
                                break;
                        }

                        SourceGridDataSO.EditorSetTileData(debugGrid, adjecentTileData);
                    }
                    if (resetTile)
                    {
                        debugGridTileData = new(debugGridTileData.TileId, debugGridTileData.Coordinates);
                    }
                }

                SourceGridDataSO.EditorSetTileData(debugGrid, tileObjectData);
            }

        }

        void UtilityMenu()
        {
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                EditorGUILayout.LabelField("Utility Menu", EditorStyles.boldLabel);
            });
            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();

                EditorGUILayout.BeginVertical();
                updateNorthTile = GUILayout.Button("Snap North Tile");
                updateSouthTile = GUILayout.Button("Snap South Tile");
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical();
                updateEastTile = GUILayout.Button("Snap East Tile");
                updateWestTile = GUILayout.Button("Snap West Tile");
                EditorGUILayout.EndVertical();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();
                EditorGUILayout.Separator();

            });

            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                resetTile = GUILayout.Button("Reset Tile");
            });
        }

        private void GridEditorFields()
        {
            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                EditorGUILayout.LabelField("Grid Info", EditorStyles.boldLabel);
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

            int tileNumber=tileData.TileId;
            Vector2Int coordinates = tileData.Coordinates;
            float height = tileData.Height;
            GridEnums.Direction slantDirection=tileData.SlantDirection;
            float slantAngle = tileData.SlantAngle;
            float leadingEdgeHeight = tileData.LeadingEdgeHeight;
            GridEnums.Tile.Type type = tileData.Type;


            EditorGUILayout.BeginVertical();

            EditorGUI.BeginDisabledGroup(true);

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                tileNumber = EditorGUILayout.IntField("Tile Number", inTileData.TileId);
            });

            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                EditorGUILayout.Vector2IntField("Tile Coordinates", new UnityEngine.Vector2Int(inTileData.Coordinates.x, inTileData.Coordinates.y));
            });

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                height = EditorGUILayout.Slider("Height", inTileData.Height, 0, 10);
                height -= height%SourceGridEditorDebugDataSO.TileHeightSnap;
            });

            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                slantDirection = (GridEnums.Direction)EditorGUILayout.EnumPopup("Slant Direction", inTileData.SlantDirection);
            });

            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                leadingEdgeHeight = EditorGUILayout.Slider("Leading Edge Height", outTileData.LeadingEdgeHeight, 0, 5);
            });

            EditorGUILayout.Separator();

            SetupHorizontalLayout(GUIElementCallback: () =>
            {
                type = (GridEnums.Tile.Type)EditorGUILayout.EnumPopup("Tile Type", inTileData.Type);
            });

            EditorGUILayout.EndVertical();

            return new(new (tileNumber, coordinates, height, slantDirection, slantAngle, type),leadingEdgeHeight,slantDirection);
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
            gridObjectPrefab.AddComponent(typeof(GameGrid));
            debugGrid = gridObjectPrefab.GetComponent<GameGrid>();

            GameObject gridTileObjectPrefab = Instantiate(SourceGridEditorDebugDataSO.DebugTileObject.gameObject, debugGrid.transform);
            gridTileObjectPrefab.AddComponent(typeof(GridTileObject));
            GridTileObject debugTileObject = gridTileObjectPrefab.GetComponent<GridTileObject>();


            debugGrid.GenerateGrid(SourceGridDataSO.GridData, debugTileObject);
            DestroyImmediate(gridTileObjectPrefab);
        }

        private void ResetGrid()
        {
            GenerateDebugGrid();
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