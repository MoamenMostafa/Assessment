docker-compose up --build -d
Start-Sleep -Seconds 10
Start-Process "http://localhost:8081/swagger/index.html"