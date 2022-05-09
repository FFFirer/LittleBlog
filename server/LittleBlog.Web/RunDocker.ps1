docker run -d --name=littleblog \
-p 43165:80 \
-v /backup/littleblogweb/logs:/app/logs \
-e "LittleBlog_ConnectionStrings__LittleBlog=Host=192.168.31.167;Port=54321;Database=littleblog;Username=postgres;Password=1234567890;" \
-e "LittleBlog_AllowedOrigins__0=http://192.168.31.167:9000" \
littleblog:0.0.3