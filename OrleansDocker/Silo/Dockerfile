FROM microsoft/aspnetcore:2.0

COPY bin/Release/PublishOutput  /app/

WORKDIR /app

EXPOSE 40000

ENTRYPOINT ["dotnet", "/app/Silo.dll"]