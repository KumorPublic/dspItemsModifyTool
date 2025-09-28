using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CommonAPI;
using CommonAPI.Systems;
using HarmonyLib;


namespace ItemsManage
{

    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInProcess(GAME_PROCESS)]
    [BepInDependency(CommonAPIPlugin.GUID)]
    [CommonAPISubmoduleDependency(nameof(ProtoRegistry), nameof(CustomDescSystem))]

    public class ItemManagePlugin : BaseUnityPlugin
    {
        public const string GUID = "kumor.plugin.ItemManage";
        public const string NAME = "DSP_Kumor_ItemManage";
        public const string VERSION = "1.0.3.0";
        public const string GAME_VERSION = "0.10.28.20856";
        public const string GAME_PROCESS = "DSPGAME.exe";
        public static ManualLogSource logger;
        private static Harmony harmony;
        public static int frame = 0;

        ///<summary>
        /// 屏蔽检测
        ///</summary>
        public static ConfigEntry<bool> 屏蔽检测;
        ///<summary>
        /// 行星视图下解锁右键移动
        ///</summary>
        public static ConfigEntry<bool> 更好的行星视图;
        ///<summary>
        /// 修改储液罐容量
        ///</summary>
        public static ConfigEntry<bool> 储液罐;
        ///<summary>
        /// 充电塔链接距离
        ///</summary>
        public static ConfigEntry<bool> 充电塔;
        ///<summary>
        /// 显示恒星戴森半径
        ///</summary>
        public static ConfigEntry<bool> 显示恒星戴森半径;

        ///<summary>
        /// 自动填充翘曲器
        ///</summary>
        public static ConfigEntry<bool> 自动填充翘曲器;

        ///<summary>
        /// 轨道开采站
        ///</summary>
        public static ConfigEntry<bool> 轨道开采站;

        ///<summary>
        /// 垃圾换沙土
        ///</summary>
        public static ConfigEntry<bool> 垃圾换沙土;


        private void LoadConfig()
        {
            
           
            //ItemManagePlugin.模拟帧数量 = this.Config.Bind<int>(
            //    "模拟帧兼容",
            //    "模拟帧数量",
             //   1,
             //   new ConfigDescription(
             //       "1 = 正常，5 = 5倍速运行",
             //       new AcceptableValueRange<int>(1, 10),
             //       new object[0]));

            // 通用修改
            const string 通用 = "1.通用";
            ItemManagePlugin.屏蔽检测 = this.Config.Bind<bool>(
                通用,
                "【启用后不可关闭】屏蔽游戏异常检测",
                true,
                new ConfigDescription("屏蔽游戏异常检测；同个存档下启用后如果关闭，会造成坏档。关闭后重启，下个存档生效；", null, new object[0]));

            ItemManagePlugin.更好的行星视图 = this.Config.Bind<bool>(
                通用,
                "【重启后生效】更好的行星视图",
                true,
                new ConfigDescription("行星视图下解锁右键移动", null, new object[0]));


            // 物品参数修改
            const string 物品修改 = "2.物品修改";
            ItemManagePlugin.储液罐 = this.Config.Bind<bool>(
                物品修改,
                "【重启后生效】修改储液罐容量",
                true,
                new ConfigDescription("修改储液罐容量：" + ItemConfig.ModifyCfg.储液罐.容量.ToString(), null, new object[0]));

            ItemManagePlugin.充电塔 = this.Config.Bind<bool>(
                物品修改,
                "【重启后生效】修改充电塔链接距离",
                true,
                new ConfigDescription("修改充电塔链接距离：" + ItemConfig.ModifyCfg.充电塔.链接距离.ToString(), null, new object[0]));

            ItemManagePlugin.垃圾换沙土 = this.Config.Bind<bool>(
                物品修改,
                "【重启后生效】垃圾换沙土",
                true,
                new ConfigDescription("清理垃圾时，石头，硅，分形硅，金伯利可变成沙土", null, new object[0]));
            

            // 更多信息
            const string 更多信息 = "3.显示更多信息";
            ItemManagePlugin.显示恒星戴森半径 = this.Config.Bind<bool>(
                更多信息,
                "显示恒星戴森半径",
                true,
                new ConfigDescription("在恒星界面显示该恒星可建造的戴森球最大半径", null, new object[0]));

            // 星际物流塔
            const string 星际物流塔 = "4.星际物流塔";
            ItemManagePlugin.自动填充翘曲器 = this.Config.Bind<bool>(
                星际物流塔,
                "【重启后生效】自动填充翘曲器",
                true,
                new ConfigDescription("将自动填充物流塔的翘曲器槽。需要星区内有物流塔提供翘曲器，且运输本身会消耗翘曲器。", null, new object[0]));

            // 轨道开采站
            const string 轨道开采站 = "5.轨道开采站";
            ItemManagePlugin.轨道开采站 = this.Config.Bind<bool>(
                轨道开采站,
                "【启用后不可关闭】增加轨道开采站",
                true,
                new ConfigDescription("增加轨道开采站，采集行星上所有矿物，油，海洋。\r\n可以调整开采速度，全球总矿簇数达到500时，达到采矿速度最大值，矿簇较少时，效率远不如大采矿机。由于是轨道作业，会损失20%的矿。\r\n采集油井时，速度与油井数量正相关，油井枯竭后无法采集。采集海洋时，速度与海洋面积正相关。\r\n受采矿速度科技加成(但有上限)，且速度越高，能耗越高\r\n如果开采站能量不足一半，且开采的矿物里有油或煤，且库存大于50个，会消耗它们获取能量，获取的能量多少与燃料的热值正相关\r\n可以与黑雾物流塔科技兼容，同时采集5个相同的矿；\r\n同个存档下启用后如果关闭，会造成坏档。关闭后重启，下个存档生效；", null, new object[0]));

        }



        private void Awake()
        {
            this.LoadConfig();
            ItemManagePlugin.logger = this.Logger;
            ItemManagePlugin.harmony = new Harmony(GUID);

            if (ItemManagePlugin.屏蔽检测.Value)
            {
                ItemManagePlugin.harmony.PatchAll(typeof(NoAbnormalLogic));
            }
            if (ItemManagePlugin.更好的行星视图.Value)
            {
                ItemManagePlugin.harmony.PatchAll(typeof(PlayerControllerGlobeMap));
            }

            ItemManagePlugin.harmony.PatchAll(typeof(ItemModiy));
            ItemManagePlugin.harmony.PatchAll(typeof(ShowMe));

            if (ItemManagePlugin.自动填充翘曲器.Value)
            {
                ItemManagePlugin.harmony.PatchAll(typeof(InterstellarLogisticsTower));
            }


            ItemManagePlugin.harmony.PatchAll(typeof(ItemManageTest));

            if (ItemManagePlugin.轨道开采站.Value)
            {
                ItemCollector.AddItem();
                ItemManagePlugin.harmony.PatchAll(typeof(ItemCollector));
            }

            if (ItemManagePlugin.垃圾换沙土.Value)
            {
                ItemManagePlugin.harmony.PatchAll(typeof(Trash2Sand));
            }

        }

        private void Start()
        {

        }

        private void Update()
        {
           ++frame;
        }

        private void OnDestroy()
        {

        }

       












    }







}



