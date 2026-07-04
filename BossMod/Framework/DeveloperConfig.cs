namespace BossMod;

[ConfigDisplay(Name = "开发者设置", Order = 9)]
public sealed class DeveloperConfig : ConfigNode
{
    [PropertyDisplay("模块包：源目录")]
    public string ModulePackDirectory = "";

    [PropertyDisplay("障碍物地图：源路径", tooltip: "应为 <repo root>/BossMod/Pathfinding/ObstacleMaps")]
    public string MapSourcePath = "";

    [PropertyDisplay("自动生成障碍物地图")]
    public bool AutoBitmaps = true;
}
