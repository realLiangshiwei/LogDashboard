# LogDashboard

![](https://blobscdn.gitbook.com/v0/b/gitbook-28427.appspot.com/o/assets%2F-LVN1HYgo-k1Fbhi8zVY%2F-LuXVi0e8IztQlB2CZ7M%2F-LuXVw5g8qiMEmJiMBTg%2Fimage.png?alt=media&token=8285b592-f36a-443b-a3ac-da686488fe8e)

 [![NuGet](https://camo.githubusercontent.com/bc8dec0292ca0c47891f945a1b635973c59944b0/68747470733a2f2f696d672e736869656c64732e696f2f6e756765742f762f4c6f6744617368626f6172642e737667)](https://www.nuget.org/packages/LogDashboard/) [![NuGet](https://camo.githubusercontent.com/b25e4e88cb2f008b50ba357d1de414277ea87018/68747470733a2f2f696d672e736869656c64732e696f2f6e756765742f64742f4c6f6744617368626f6172642e737667)](https://www.nuget.org/packages/LogDashboard/)

logdashboard是在github上开源的aspnetcore项目, 它旨在帮助开发人员排查项目运行中出现错误时快速查看日志排查问题

通常我们会在项目中使用nlog、log4net等日志组件,它们用于记录日志的功能非常强大和完整,常见情况会将日志写到`txt`或`数据库`中, 但通过记事本和sql查看日志并不简单方便. LogDashboard提供了一个可以简单快速查看日志的面板.

LogDashboard适用于aspnetcore 2.x - aspnetcore3.x 项目, 采用aspnetcore`中间件`技术开发. 轻量快速

### [快速开始](https://doc.logdashboard.net/ru-men/quickstart)

### 页面预览

#### 首页 <a id="esa_index_2"></a>

![](https://blobscdn.gitbook.com/v0/b/gitbook-28427.appspot.com/o/assets%2F-LVN1HYgo-k1Fbhi8zVY%2F-LuVw_UzlCi5nVCnBqjV%2F-LuVxUCqidH8V6C0nAmm%2Fimage.png?alt=media&token=490e4fa8-2c1a-470e-b008-98780f1f2f9b)

实时查看应用程序运行中产生的日志

* 日志聚合
* 趋势图表
* 最近十条日志

#### 列表

![](https://blobscdn.gitbook.com/v0/b/gitbook-28427.appspot.com/o/assets%2F-LVN1HYgo-k1Fbhi8zVY%2F-LuVw_UzlCi5nVCnBqjV%2F-LuVyq_ZFHzenIDg97Oz%2Fimage.png?alt=media&token=d66da484-12a3-44f9-abfd-f00a3586be03)

复合检索所有日志并查看详情等操作

### 特性

* 授权访问
* 自定义日志模型
* 日志追踪
* 堆栈查看

### 支持的日志组件

* nlog
* log4net
* serilog

### 支持的数据源

* txt
* 数据库

## 线路图

* 日志中心


## 交流群

![logdashboardqrcode](https://user-images.githubusercontent.com/16813853/51227366-df111580-198e-11e9-9e0c-f7b077e63fe7.png)
