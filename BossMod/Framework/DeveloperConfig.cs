namespace BossMod;

[ConfigDisplay(Name = "Developer settings", Order = 9)]
public sealed class DeveloperConfig : ConfigNode
{
    [PropertyDisplay("模块包：源目录")]
    public string ModulePackDirectory = "";

    [PropertyDisplay("障碍物地图：从源加载")]
    public bool MapLoadFromSource;

    [PropertyDisplay("障碍物地图：源路径", tooltip: "应为 <repo root>/BossMod/Pathfinding/ObstacleMaps/maplist.json")]
    public string MapSourcePath = "";
}
