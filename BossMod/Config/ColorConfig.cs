namespace BossMod;

[ConfigDisplay(Name = "配色方案", Order = -1)]
public sealed class ColorConfig : ConfigNode
{
    [PropertyDisplay("竞技场：背景")]
    public Color ArenaBackground = new(0xc00f0f0f);

    [PropertyDisplay("竞技场：边框")]
    public Color ArenaBorder = new(0xffffffff);

    [PropertyDisplay("竞技场：典型危险区域（AOE）")]
    public Color ArenaAOE = new(0x80008080);

    [PropertyDisplay("竞技场：典型安全区域")]
    public Color ArenaSafeFromAOE = new(0x80008000);

    // TODO: imminent aoes and dangerous players should use separate color
    [PropertyDisplay("竞技场：典型危险前景元素（连线等）")]
    public Color ArenaDanger = new(0xff00ffff);

    [PropertyDisplay("竞技场：典型安全前景元素（连线等）")]
    public Color ArenaSafe = new(0xff00ff00);

    [PropertyDisplay("竞技场：敌人")]
    public Color ArenaEnemy = new(0xff0000ff);

    [PropertyDisplay("竞技场：非敌人重要对象（不可选中的连线来源、可交互对象等）")]
    public Color ArenaObject = new(0xff0080ff);

    [PropertyDisplay("竞技场：玩家角色")]
    public Color ArenaPC = new(0xff00ff00);

    [PropertyDisplay("竞技场：重要玩家，对机制很重要")]
    public Color ArenaPlayerInteresting = new(0xffc0c0c0);

    [PropertyDisplay("竞技场：脆弱玩家，需要特别关注")]
    public Color ArenaPlayerVulnerable = new(0xffff00ff);

    [PropertyDisplay("竞技场：通用/无关玩家（可根据设置被职责特定颜色覆盖）")]
    public Color ArenaPlayerGeneric = new(0xff808080);

    [PropertyDisplay("竞技场：不在当前队伍/联盟中的玩家（通常不在地图上绘制）")]
    public Color ArenaPlayerReallyGeneric = new(0x80808080);

    [PropertyDisplay("竞技场：通用/无关坦克")]
    public Color ArenaPlayerGenericTank = Color.FromComponents(30, 50, 110);

    [PropertyDisplay("竞技场：通用/无关治疗")]
    public Color ArenaPlayerGenericHealer = Color.FromComponents(30, 110, 50);

    [PropertyDisplay("竞技场：通用/无关近战")]
    public Color ArenaPlayerGenericMelee = Color.FromComponents(110, 30, 30);

    [PropertyDisplay("竞技场：通用/无关法系")]
    public Color ArenaPlayerGenericCaster = Color.FromComponents(70, 30, 110);

    [PropertyDisplay("竞技场：通用/无关物理远程")]
    public Color ArenaPlayerGenericPhysRanged = Color.FromComponents(110, 90, 30);

    [PropertyDisplay("竞技场：通用/无关焦点目标")]
    public Color ArenaPlayerGenericFocus = Color.FromComponents(0, 255, 255);

    [PropertyDisplay("规划器：背景")]
    public Color PlannerBackground = new(0x80362b00); // solarized base03; old: 0x40404040

    [PropertyDisplay("规划器：背景高亮")]
    public Color PlannerBackgroundHighlight = new(0x80423607); // solarized base02; old: n/a

    [PropertyDisplay("规划器：冷却时间")]
    public Color PlannerCooldown = new(0x80756e58); // solarized base01; old: 0x80808080

    [PropertyDisplay("规划器：选项的备用颜色")]
    public Color PlannerFallback = new(0x80969483); // solarized base0; old: ???

    [PropertyDisplay("规划器：效果")]
    public Color PlannerEffect = new(0x8000ff00); // TODO: solarized base1 (0x80a1a193) ???

    [PropertyDisplay("规划器：窗口")]
    public Color[] PlannerWindow = [new(0x800089b5), new(0x80164bcb), new(0x802f32dc), new(0x808236d3), new(0x80c4716c), new(0x80d28b26), new(0x8098a12a), new(0x80009985)]; // solarized accents
}
