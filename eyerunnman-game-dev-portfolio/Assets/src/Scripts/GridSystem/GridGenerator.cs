using com.portfolio.interfaces;
using com.portfolio.patterns;
using System.Threading.Tasks;
using UnityEngine;

namespace com.portfolio.gridSystem
{
    public class GridGenerator : GameService
    {
        [Header("Initialization Variables")]
        [SerializeField] Grid gridPrefab;
        [SerializeField] GridTileObject gridTileObjectPrefab;

        [Space(20)]
        [Header("Local Variables")]
        Grid Grid;


        #region Required Data from Services

        private GridData GridData
        {
            get
            {
                SceneDataContainer sceneDataContainer = ServiceLocator.Current.Get<SceneDataContainer>();

                return sceneDataContainer.GridData;
            }
        }

        #endregion
        //refrence to grid prefab

        public override async Task Initialize(bool overrideService = false)
        {
            await base.Initialize(overrideService);

            await GridGeneration(GridData,gridTileObjectPrefab);

            ServiceLocator.Current.Register<GridGenerator>(this, overrideService);
        }

        private async Task GridGeneration(GridData gridData, GridTileObject gridTileObjectPrefab)
        {
            // check if grid is already there or not
            if(Grid!=null)
            {
                return;
            }

            //instantiate local grid game object
            Grid = Instantiate(gridPrefab, transform);

            //generate grid
            await Grid.GenerateGridAsync(gridData, gridTileObjectPrefab);

        }
    }
}

