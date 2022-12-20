using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.portfolio.patterns;
using com.package.scriptableObjects;
using com.package.gridSystem;
using com.portfolio.interfaces;
using System.Threading.Tasks;

public class SceneDataContainer : GameService
{
    #region Grid System Data Container
    [Header("Grid System Data ")]
    [SerializeField] GridDataSO gridDataSO;
    public GridData GridData
    {
        get { return gridDataSO.GridData; }
    }

    #endregion


    #region GameServiceCallbacks

    public override async Task Initialize(bool overrideService = false)
    {
        await base.Initialize(overrideService);
        ServiceLocator.Current.Register<SceneDataContainer>(this);
    }

    #endregion

}
