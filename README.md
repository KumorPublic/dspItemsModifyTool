# Mod_DysonSphereProgram

戴森球计划mods


# AutoBuild 自动建造模式（公开，前往[https://dsp.thunderstore.io/] 搜索下载）
 1. 放好蓝图后按Z键自动建造
 2. 允许设置飞行建造模式或者步行建造模式(需要对应科技)  
 3. 允许设置电量低于$%时，自动寻找无线输电塔充电  
 4. ~~允许开启或关闭建造无人机，防止在规划阶段无人机乱造(需要Configuration插件才能在游戏中实时控制)~~   
 5. 再按一次Z、移动、进入建造、拆除、蓝图模式可以退出功能  

# CloseDraw 帧数拯救者
 1. 自定义开启关闭一些游戏渲染以提高性能
 2. 不渲染飞机飞船
 3. 不渲染戴森壳
 4. 不渲染戴森云
 5. 不渲染电网
 6. 不渲染研究站
 7. 不渲染建筑
 8. 戴森球层级

# CustomCreateBirthStar 自定义星区（公开，前往[https://dsp.thunderstore.io/] 搜索下载）
查看介绍 - > https://dsp.thunderstore.io/package/kumor/CustomCreateBirthStar/
   
   
# ItemsManage 轨道开采站/储液罐容量++
 1. 轨道开采站（星球矿机修改版），较大的合成代价（星际物流运输站 + 能量核心满 + 10大型采矿机 + 20抽水站 + 20原油萃取站），开采全球矿物/液体，关联采矿科技/生产统计，并删除面板上无关组件（套用的物流塔模型）
 2. 可通过滑块自定义采矿倍率（类似大矿机，但能耗指数级上涨）
 3. 能耗：16MJ x 5格 x 采矿能耗倍率 x 采矿科技加成（能耗会随着采矿科技升级而增加）。如果开采的矿物中有石油和煤炭，则会消耗它们补充能量（仅在剩余能量不到最大值的一半时启用，且燃料储量多于50个，有外部充电就不会消耗燃料）
 4. 开采速度：油，2 x 全球油井数 x 采矿倍率 x 采矿科技加成。油（枯竭），0.5 x 全球油井数 x 采矿科技加成
 5. 开采速度：矿，1 x 全球矿簇数 x 采矿科技加成，其中全球矿簇数大于100则取100（当矿簇较少时，效率小于大矿机）
 6. 开采速度：海洋液体，3333 x 海洋面积比例 x 采矿科技加成
 7. 修改了储液罐容量50000

# ShowMe 自用信息显示小工具
1. 显示矿物采集速度
2. 在恒星界面显示戴森半径
   
# Trash2Sand 沙土转换器（公开，前往[https://dsp.thunderstore.io/] 搜索下载）
1. 将地上的垃圾按不同比例转换成沙土
2. 可配置是否开启白嫖模式，该模式下可以将任何在地上的垃圾转化为沙土，比如抽海水抽氢气。关闭后只允许转化石头，硅头，金伯利矿，分形硅矿。
3. 可配置沙土兑换倍率

# DSPAutoNavigation 自用星际导航
1. 可用性修复