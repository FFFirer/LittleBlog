# LittleBlog

AspNet Core 博客网站练习

# 描述

使用 AspNet Core 框架,以博客网站为需求,完成网站的设计实现

# 使用到

AspNet Core,Javascript,css,html,C#,Razor
数据库:EF Core,Mysql

# 数据库迁移

## 开发时的迁移

开发时的修改比较频繁，与运行项目时的要应用的迁移区分开。所有开发时的数据库变更都依赖于此迁移。

开发时的所有迁移及数据库上下文放在LittleBlog/LittleBlog.DbMigrations项目中。
数据上下文为`DevPgsqlContext.cs`
数据上下文工厂为`DevPgsqlContextFactory.cs`
开发时的数据库连接填写在`DevPgsqlContextFactory.cs`中
当有新的实体类变动需要创建迁移时：
```sh
# 进入该项目目录下执行命令
PS: D:\Playground\repos\LittleBlog\LittleBlog.DbMigrations :: git(master) 
➜ dotnet ef migrations add Update_Article -c DevpgsqlContext -p .\LittleBlog.DbMigrations.csproj -o DevPgsql
```

## 实际运行时的迁移

实际运行时的所有迁移及数据库上下文放在LittleBlog/LittleBlog.DbMigrations项目中。

```sh
# 进入该项目目录下执行命令
PS: D:\Playground\repos\LittleBlog\LittleBlog.DbMigrations :: git(master)
➜  dotnet ef migrations add AddLogInDb -c LittleBlogContext -o Migrations --project .\LittleBlog.Core.csproj -s ..\LittleBlog.Web\LittleBlog.Web.csproj
```

撤销迁移

```sh
# 进入该项目目录下执行命令
PS: D:\Playground\repos\LittleBlog\LittleBlog.DbMigrations :: git(master)
➜ dotnet ef migrations remove -c LittleBlogContext --project .\LittleBlog.Core.csproj -s ..\LittleBlog.Web\LittleBlog.Web.csproj
```

# 待实现功能

| 名称                  | 状态 |
| --------------------- | ---- |
| 日志存入数据库        | 🚧    |
| 日志管理              | 🚧    |
| 日志级别动态过滤      | 🚧    |
| 日志范围动态过滤      | 🚧    |
| 标签管理              | 🚧    |
| 友情链接模块          | 🚧    |
| Markdown渲染css自定义 |      |
|                       |      |
|                       |      |

