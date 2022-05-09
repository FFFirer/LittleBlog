$ContainerName = "littleblog"

# 发布站点
dotnet publish ./LittleBlog.Web/LittleBlog.Web.csproj -c Release -o published

# 构建镜像
cd ./published
docker build -t littleblogweb:dev .