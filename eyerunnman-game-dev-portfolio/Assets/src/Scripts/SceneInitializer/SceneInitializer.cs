using System.Collections.Generic;
using UnityEngine;
using com.portfolio.interfaces;
using System.Threading.Tasks;
using System;
using com.portfolio.patterns;

public class SceneInitializer : GameService
{
    [Header("Services : keep the order in mind")]
    public List<GameService> Services;

    public bool IsSceneInitialized
    {
        get {
            return ServiceState == ServiceState.Registered;
        }
    }

    private void Awake()
    {
        _ = Initialize(overrideService: true);
    }

    public override async Task Initialize(bool overrideService = false)
    {
        await base.Initialize(overrideService);

        await InilizeServices();

        ServiceLocator.Current.Register<SceneInitializer>(this, overrideService);
    }

    private async Task InilizeServices()
    {
        foreach (GameService service in Services)
        {
            await service.Initialize(overrideService:true);

            while (service.ServiceState != ServiceState.Registered)
            {
                await Task.Yield();
            }
        }

    }
}
