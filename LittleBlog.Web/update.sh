systemctl stop blog
cd /root/codes/LittleBlog/
git checkout master
git pull
dotnet restore
dotnet publish -c Release -o /root/services/blog/
systemctl start blog
systemctl status blog