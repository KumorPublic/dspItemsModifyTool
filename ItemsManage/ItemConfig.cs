namespace ItemsManage
{
    class ItemConfig
    {


        public class CollectorCfg
        {
            ///<summary>
            ///每次挖矿使用能量，默认16MJ*5格
            ///</summary>
            public const long UseEnergy = 16000000L;
            ///<summary>
            ///最大储存能量，默认100GJ
            ///</summary>
            public const long MaxEnergyAcc = 90000000000L;
            ///<summary>
            ///最大充能功率，默认1GW
            ///</summary>
            public const long MaxEnergyPerTick = 900000000L / 300;
            ///<summary>
            ///最大充能功率，内部计算的功率值
            ///</summary>
            public const long MaxEnergyPerTick1 = MaxEnergyPerTick * 5;
            ///<summary>
            ///燃料充能上限
            ///</summary>
            public const long MaxFuelEnergyAcc = MaxEnergyAcc / 2;
            ///<summary>
            ///每次作业采油数量
            ///</summary>
            public const int CollectorOilNum = 2;
            ///<summary>
            ///每次作业采矿数量
            ///</summary>
            public const int CollectorMineNum = 1;
            ///<summary>
            ///每次作业抽水数量
            ///</summary>
            public const int CollectorWaterNum = 333;

        }


        public class ModifyCfg
        {
            public class TankStorage
            {
                ///<summary>
                ///储液罐容量
                ///</summary>
                public const int fluidStorageCount = 50000;
            }

            public class WarpStorage
            {
                ///<summary>
                ///机甲翘曲器堆叠
                ///</summary>
                public const int StackSize = 500;
            }
        }




    }



}
