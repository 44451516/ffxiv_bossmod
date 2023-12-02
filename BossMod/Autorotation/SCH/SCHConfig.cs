namespace BossMod
{
    [ConfigDisplay(Parent = typeof(AutorotationConfig))]
    class SCHConfig : ConfigNode
    {
        //[PropertyDisplay("Execute optimal rotations on Ruin/Broil (ST damage), Art of War (AOE damage), Physick (ST heal) and Succor (AOE heal)")]
        //public bool FullRotation = true;

   
        [PropertyDisplay("对友方法术使用鼠标悬停定位")]
        public bool MouseoverFriendly = true;

        [PropertyDisplay("优先选择Selene而不是Eos")]
        public bool PreferSelene = false;
    }
}
